namespace DistributedComputingSystem.Shared
{
	public class CSharpTaskCompletionResult
	{
		public CSharpTaskCompletionResult() { }

		public CSharpTaskCompletionResult(bool success, string result) {
			Success = success;
			Message = result;
		}

		public bool Success { get; set; }

		public string Message { get; set; }
	}
}
