using System;
using Akka.Actor;
using Akka.Configuration;
using DistributedComputingSystem.Shared;

namespace DistributedComputingSystem.WorkerSystem
{
	/// <summary>
	/// Клас робочої системи - системи, що виконує передані їй задачі.
	/// </summary>
	public class WorkerSystem : IDisposable
	{
		private static WorkerSystem _instance = null;

		private ActorSystem _actorSystem;

		private WorkerSystem() { }

		/// <summary>
		/// Інстанціює робочу систему, а якщо вона уже інстанійована - повертає її екземпляр.
		/// </summary>
		/// <param name="ip">IP-адреса для розгортування.</param>
		/// <returns>Екземпляр системи.</returns>
		public static WorkerSystem DelpoyInstance(string ip) {
			if (_instance == null) {
				_instance = new WorkerSystem();
				_instance.Deploy(ip);
			}
			return _instance;
		}

		private void Deploy(string ip) {
			_actorSystem = ActorSystem.Create(Constants.WorkersSystemName, ConfigurationFactory.ParseString(@"
				akka {  
					actor.provider = remote
					remote {
						dot-netty.tcp {
							port = " + Constants.WorkersPortNumber.ToString() + @"
							hostname = " + ip + @"
						}
					}
				}"));
			Console.WriteLine($"Worker system has been deployed successfully.");
		}

		/// <summary>
		/// Вимикає інстанційовану систему.
		/// </summary>
		public void Dispose() {
			_actorSystem.Dispose();
			_instance = null;
		}
	}
}
