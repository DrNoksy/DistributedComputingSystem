using System.Threading.Tasks;

namespace DistributedComputingSystem.Shared
{
	/// <summary>
	/// Контекст виконання задачі.
	/// </summary>
	public class CSharpScriptExecutionContext
	{
		public CSharpScriptExecutionContext(CSharpScriptTask scriptTask, 
				TaskCompletionSource<CSharpTaskCompletionResult> taskCompletionSource) {
			CSharpScriptTask = scriptTask;
			TaskCompletionSource = taskCompletionSource;
		}

		/// <summary>
		/// Об'єкт задачі.
		/// </summary>
		public CSharpScriptTask CSharpScriptTask { get; set; }

		/// <summary>
		/// Джерело визначення моменту завершення виконання задачі.
		/// </summary>
		public TaskCompletionSource<CSharpTaskCompletionResult> TaskCompletionSource { get; set; }
	}
}
