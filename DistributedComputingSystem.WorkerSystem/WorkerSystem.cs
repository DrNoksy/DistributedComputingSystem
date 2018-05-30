using System;
using Akka.Actor;
using Akka.Configuration;
using DistributedComputingSystem.Shared;

namespace DistributedComputingSystem.WorkerSystem
{
	public class WorkerSystem : IDisposable
	{
		private static WorkerSystem _instance = null;

		private ActorSystem _actorSystem;

		private WorkerSystem() { }

		public static WorkerSystem DelpoyInstance(string ip) {
			if (_instance == null) {
				_instance = new WorkerSystem();
				_instance.Deploy(ip);
			}
			return _instance;
		}

		private void Deploy(string ip) {
			_actorSystem = ActorSystem.Create(Constants.WorkersSystemName, ConfigurationFactory.ParseString(@"
				akka {  
					actor.provider = remote
					remote {
						dot-netty.tcp {
							port = " + Constants.WorkersPortNumber.ToString() + @"
							hostname = " + ip + @"
						}
					}
				}"));
			Console.WriteLine($"Worker system has been deployed successfully.");
		}

		public void Dispose() {
			_actorSystem.Dispose();
		}
	}
}
