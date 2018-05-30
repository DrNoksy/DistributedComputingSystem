using System;

namespace DistributedComputingSystem.Shared
{
	public class CSharpScriptTask
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Content { get; set; }

		public CSharpTaskCompletionResult CompletionResult { get; set; }

		public CSharpScriptTask() { }

		public CSharpScriptTask(string content) : this() {
			Id = Guid.NewGuid();
			Name = Id.ToString();
			Content = content;
		}

		public CSharpScriptTask(string name, string content) : this() {
			Id = Guid.NewGuid();
			Name = name;
			Content = content;
		}
	}
}
