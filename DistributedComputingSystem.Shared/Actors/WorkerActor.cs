using System;
using System.Text.RegularExpressions;
using Akka.Actor;

namespace DistributedComputingSystem.Shared
{
	/// <summary>
	/// Актор, що виконує задачі.
	/// </summary>
	public class WorkerActor : ReceiveActor
	{
		public WorkerActor(IActorRef resultDispatchActorRef) {
			Receive<CSharpScriptTask>(async scriptTask => {
				CSharpTaskCompletionResult completionResult = new CSharpTaskCompletionResult();
				try {
					string scriptContent = PreprocessTaskContent(scriptTask.Content);
					completionResult.Message = await CSharpScriptExecutor.Execute(scriptContent);
					completionResult.Success = true;
				} catch (Exception e) {
					completionResult.Message = "Compilation error." + e.ToString();
					completionResult.Success = false;
				} finally {
					scriptTask.CompletionResult = completionResult;
					resultDispatchActorRef.Tell(scriptTask);
					Console.WriteLine(completionResult.Message);
				}
			});
		}

		private static string PreprocessTaskContent(string taskContent) {
			if (string.IsNullOrWhiteSpace(taskContent)) {
				return string.Empty;
			}
			string processedContent = taskContent;
			processedContent = Regex.Replace(processedContent, "//.*", "", RegexOptions.Singleline);
			if (Regex.IsMatch(taskContent, "Environment[^;]*Exit")) {
				throw new ArgumentException("Task content can not contain terminating command.");
			}
			processedContent = taskContent.Replace("using System;", "");
			return $"using System;{Environment.NewLine}{processedContent}";
		}
	}
}
