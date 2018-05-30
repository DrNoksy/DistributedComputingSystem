using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using DistributedComputingSystem.Shared;

namespace DistributedComputingSystem.MainSystem
{
	public class MainSystem : IDisposable
	{
		private const int InitialActorsCount = 10;

		private const string _mainSystemHoconConfig = @"
			akka {  
				actor.provider = remote
				remote {
					dot-netty.tcp {
						port = 0
						hostname = localhost
					}
				}
			}";

		private static MainSystem _instance = null;

		private ActorSystem _actorSystem;

		private WorkersBalancer _workersBalancer;

		private Dictionary<WorkerDeploymentInfo, Props> _workerRouters;

		private IActorRef _resultsDispatchActorRef;

		private MainSystem() { }

		public static MainSystem DelpoyInstance(IEnumerable<WorkerDeploymentInfo> workerDeploymentInfos) {
			if (_instance == null) {
				_instance = new MainSystem();
				_instance.Deploy(workerDeploymentInfos);
			}
			return _instance;
		}

		private void Deploy(IEnumerable<WorkerDeploymentInfo> workerDeploymentInfos) {
			if (workerDeploymentInfos == null || !workerDeploymentInfos.Any()) {
				throw new ArgumentNullException(nameof(workerDeploymentInfos));
			}
			_workersBalancer = new WorkersBalancer(workerDeploymentInfos);
			_actorSystem = ActorSystem.Create("MainSystem", ConfigurationFactory.ParseString(_mainSystemHoconConfig));
			_resultsDispatchActorRef = _actorSystem.ActorOf(Props.Create(() => new ResultsDispatchActor()));
			_workerRouters = workerDeploymentInfos.ToDictionary(i => i, i => {
				var router = new RoundRobinPool(InitialActorsCount,
					new DefaultResizer(InitialActorsCount, int.MaxValue, pressureThreshold: 0, messagesPerResize: 1));
				Address remoteAddress = Address.Parse($"akka.tcp://{Constants.WorkersSystemName}@{i.IP}:{Constants.WorkersPortNumber}");
				return Props.Create(() => new WorkerActor(_resultsDispatchActorRef))
					.WithDeploy(Akka.Actor.Deploy.None.WithScope(new RemoteScope(remoteAddress)))
					.WithRouter(router);
			});
			Console.WriteLine($"Main system has been deployed successfully.");
		}

		public async Task<CSharpTaskCompletionResult> RunTask(CSharpScriptTask scriptTask) {
			var taskCompletionSource = new TaskCompletionSource<CSharpTaskCompletionResult>();
			var executionContext = new CSharpScriptExecutionContext(scriptTask, taskCompletionSource);
			_resultsDispatchActorRef.Tell(executionContext);
			var worker = _workersBalancer.GetNextWorkerDeplInfo();
			var actor = _actorSystem.ActorOf(_workerRouters[worker]);
			actor.Tell(scriptTask);
			return await taskCompletionSource.Task;
		}

		public void Dispose() {
			_actorSystem.Dispose();
		}
	}
}
