using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;

namespace DistributedComputingSystem.Shared
{
	/// <summary>
	/// Безпосередній виконавець задач.
	/// </summary>
	public static class CSharpScriptExecutor
	{
		/// <summary>
		/// Виконує задачу.
		/// </summary>
		/// <param name="code">Код задачі.</param>
		/// <returns>Об'єкт <see cref="System.Threading.Tasks.Task"/>, що містить інформацію про результат.</returns>
		public static async Task<string> Execute(string code) {
			string result = null;
			ScriptState<object> _scriptState = await CSharpScript.RunAsync(code);
			result = _scriptState.ReturnValue?.ToString();
			return result;
		}
	}
}
