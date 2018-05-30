using System.Threading.Tasks;

namespace DistributedComputingSystem.Shared
{
	public class CSharpScriptExecutionContext
	{
		public CSharpScriptExecutionContext(CSharpScriptTask scriptTask, 
				TaskCompletionSource<CSharpTaskCompletionResult> taskCompletionSource) {
			CSharpScriptTask = scriptTask;
			TaskCompletionSource = taskCompletionSource;
		}

		public CSharpScriptTask CSharpScriptTask { get; set; }

		public TaskCompletionSource<CSharpTaskCompletionResult> TaskCompletionSource { get; set; }
	}
}
