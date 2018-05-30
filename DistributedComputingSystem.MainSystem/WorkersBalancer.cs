using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DistributedComputingSystem.MainSystem
{
	internal class WorkersBalancer
	{
		private Dictionary<double, WorkerDeploymentInfo> _normalizedDeploymentInfo;

		private Random _random = new Random();

		public WorkersBalancer(IEnumerable<WorkerDeploymentInfo> workerDeploymentInfos) {
			workerDeploymentInfos = workerDeploymentInfos.Where(i => i.TasksCountRatio > 0).ToList();
			if (!workerDeploymentInfos.Any()) {
				throw new ArgumentException(nameof(workerDeploymentInfos));
			}
			double sum = workerDeploymentInfos.Sum(i => i.TasksCountRatio);
			double commonRatio = 1 / sum;
			double previousNormilizedPercent = 0;
			_normalizedDeploymentInfo = workerDeploymentInfos.ToDictionary(i => {
				double normilizedPercent = i.TasksCountRatio * commonRatio + previousNormilizedPercent;
				previousNormilizedPercent = normilizedPercent;
				return normilizedPercent;
			}, i => i);
		}

		public WorkerDeploymentInfo GetNextWorkerDeplInfo() {
			double randomNumber = _random.NextDouble();
			var result = _normalizedDeploymentInfo.FirstOrDefault(i => randomNumber < i.Key);
			return default(KeyValuePair<double, WorkerDeploymentInfo>).Equals(result)
				? null
				: result.Value;
		}
	}
}
