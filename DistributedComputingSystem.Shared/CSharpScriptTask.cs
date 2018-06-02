using System;

namespace DistributedComputingSystem.Shared
{
	/// <summary>
	/// Клас-обгортка для задачі
	/// </summary>
	public class CSharpScriptTask
	{
		/// <summary>
		/// Унікальний ідентифікатор.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Назва.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Код задачі.
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Результат виконання (якщо задача ще не виконана - <code>null</code>).
		/// </summary>
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
