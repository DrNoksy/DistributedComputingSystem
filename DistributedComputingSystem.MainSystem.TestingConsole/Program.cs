using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedComputingSystem.Shared;

namespace DistributedComputingSystem.MainSystem.TestingConsole
{
	class Program
	{
		static void Main(string[] args) {
			var ipAddresses = new List<string>();
			Console.Write($"Enter at least one IP-address of deployed worker system: ");
			string enteredIp = null;
			do {
				enteredIp = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(enteredIp)) {
					ipAddresses.Add(enteredIp);
					Console.Write("Enter one more IP-address or empty string for deploying main system: ");
				}
			} while (!string.IsNullOrWhiteSpace(enteredIp));

			var workerDeploymentInfos = ipAddresses.Select(ip => new WorkerDeploymentInfo(ip, 1)).ToList();

			using (var mainSystem = MainSystem.DelpoyInstance(workerDeploymentInfos)) {
				Parallel.ForEach(ipAddresses, ip => {
					CSharpTaskCompletionResult result = mainSystem.RunTask("return new Random().NextDouble();").Result;
					Console.WriteLine($"Random result of {ip}: {(result.Success ? "succes" : "failure")} - {result.Message}");
				});
				Console.WriteLine($"Press any key to STOP main system...");
				Console.ReadKey();
			}
		}
	}
}
