using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DistributedComputingSystem.Shared;
using Newtonsoft.Json;

namespace DistributedComputingSystem.MainSystem.Web.Controllers
{
	public class TasksController : Controller
	{
		private static MainSystem _mainSystem;

		static TasksController() {
			AppDomain.CurrentDomain.ProcessExit += (e, args) => DisposeSystem();
		}

		public TasksController(IConfiguration config) {
			if (_mainSystem == null) {
				try {
					IConfigurationSection workers = config.GetSection("Workers");
					var workerDploymentInfos = workers.Get<string[]>()
						.Select(x => {
							string[] values = x.Split(",").Select(s => s.Trim()).Take(2).ToArray();
							return new WorkerDeploymentInfo(values[0], double.Parse(values[1]));
						})
						.ToArray();
					_mainSystem = MainSystem.DelpoyInstance(workerDploymentInfos);
				} catch (Exception e) {
					throw new ApplicationException("Main system deployment failed, check your configuration.", e);
				}
			}
		}

		private static void DisposeSystem() {
			if (_mainSystem != null) {
				_mainSystem.Dispose();
			}
		}

		[HttpGet]
		public ActionResult RunTask() {
			var scriptTask = new CSharpScriptTask();
			return View("RunTask", scriptTask);
		}

		[HttpPost]
		public async Task<ActionResult> RunTask(CSharpScriptTask scriptTask) {
			await _mainSystem.RunTask(scriptTask);
			return View("RunTask", scriptTask);
		}

		[HttpPost]
		public async Task<ActionResult> PostTask(string task) {
			var completionResult = await _mainSystem.RunTask(task);
			return Json(completionResult);
		}

		[HttpPost]
		public ActionResult PostTasks(string tasks) {
			CSharpTaskCompletionResult[] results = JsonConvert.DeserializeObject<string[]>(tasks)
				.Select(content => _mainSystem.RunTask(content))
				.ToArray()
				.Select(t => t.Result)
				.ToArray();
			return Json(results);
		}
	}
}
