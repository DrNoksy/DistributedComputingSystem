namespace DistributedComputingSystem.Shared
{
	/// <summary>
	/// Результат виконання задачі.
	/// </summary>
	public class CSharpTaskCompletionResult
	{
		public CSharpTaskCompletionResult() { }

		public CSharpTaskCompletionResult(bool success, string result) {
			Success = success;
			Message = result;
		}

		/// <summary>
		/// Показник, чи є виконання успішним.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Результат виконання.
		/// </summary>
		public string Message { get; set; }
	}
}
