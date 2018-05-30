using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DistributedComputingSystem.WorkerSystem
{
	class Program
	{
		private const int ErrorMilisecondsDelay = 2000;

		private static string GetLocalIPAddress() {
			var host = Dns.GetHostEntry(Dns.GetHostName());
			return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
		}

		private static void DeploySystem(string ip) {
			using (var workerSystem = WorkerSystem.DelpoyInstance(ip)) {
				Console.WriteLine($"Press any key to STOP worker system...");
				Console.ReadKey();
			}
		}

		static void Main(string[] args) {
			string ip = null;
			if ((ip = args.FirstOrDefault(arg => arg != null && arg.StartsWith("ip="))) != null) {
				ip = ip.Substring(3);
			}
			if (args.Contains("dontAskIp")) {
				if (ip == null && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
					ip = GetLocalIPAddress();
				}
				if (ip != null) {
					DeploySystem(ip);
					return;
				} else {
					throw new ApplicationException("IP-address detection failed");
				}
			} else {
				if (ip == null && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
					ip = GetLocalIPAddress();
				}
				if (ip != null) {
					Console.WriteLine($"Deploy on IP {ip} (y/n)?");
					string answer = Console.ReadLine();
					if (!string.IsNullOrWhiteSpace(answer) && answer.ToLower() != "y") {
						ip = null;
					}
				}
				bool deploymentFailed = true;
				while (deploymentFailed) {
					if (ip == null) {
						Console.WriteLine($"Enter the IP-address for deploying");
						ip = Console.ReadLine();
					}
					try {
						DeploySystem(ip);
						deploymentFailed = false;
					} catch (Exception) {
						Task.Delay(ErrorMilisecondsDelay).Wait();
						Console.WriteLine($"Deployment on IP {ip} failed.");
						ip = null;
					}
				}
			}
		}
	}
}
