namespace DistributedComputingSystem.MainSystem
{
	public class WorkerDeploymentInfo
	{
		public WorkerDeploymentInfo(string ip, double tasksCountRatio) {
			IP = ip;
			TasksCountRatio = tasksCountRatio;
		}

		public string IP { get; set; }

		public double TasksCountRatio { get; set; }

		public override string ToString() {
			return $"{nameof(IP)}: {IP}; {nameof(TasksCountRatio)}: {TasksCountRatio}";
		}
	}
}
