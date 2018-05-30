using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;

namespace DistributedComputingSystem.Shared
{
	public static class CSharpScriptExecutor
	{
		public static async Task<string> Execute(string code) {
			string result = null;
			ScriptState<object> _scriptState = await CSharpScript.RunAsync(code);
			result = _scriptState.ReturnValue?.ToString();
			return result;
		}
	}
}
