using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace DistributedComputingSystem.Shared
{
	public class ResultsDispatchActor : ReceiveActor
	{
		private Dictionary<Guid, TaskCompletionSource<CSharpTaskCompletionResult>> _taskCompletitionSources =
			new Dictionary<Guid, TaskCompletionSource<CSharpTaskCompletionResult>>();

		public ResultsDispatchActor() {
			Receive<CSharpScriptExecutionContext>(scriptExecutionContext => {
				Guid taskId = scriptExecutionContext.CSharpScriptTask.Id;
				if (_taskCompletitionSources.ContainsKey(taskId)) {
					throw new InvalidOperationException($"Task with identifier \"{taskId}\" is already executing.");
				}
				_taskCompletitionSources.Add(taskId, scriptExecutionContext.TaskCompletionSource);
			});

			Receive<CSharpScriptTask>(scriptTask => {
				var completionSource = _taskCompletitionSources[scriptTask.Id];
				completionSource.SetResult(scriptTask.CompletionResult);
				_taskCompletitionSources.Remove(scriptTask.Id);
			});
		}
	}
}
