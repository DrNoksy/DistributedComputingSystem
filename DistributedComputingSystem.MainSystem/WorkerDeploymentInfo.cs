namespace DistributedComputingSystem.MainSystem
{
	/// <summary>
	/// Клас для зберігання інформації для розподілення задач на розгорнуту робочу систему.
	/// </summary>
	public class WorkerDeploymentInfo
	{
		public WorkerDeploymentInfo(string ip, double tasksCountRatio) {
			IP = ip;
			TasksCountRatio = tasksCountRatio;
		}

		/// <summary>
		/// IP-адреса хосту, де розгорнута робоча система.
		/// </summary>
		public string IP { get; set; }

		/// <summary>
		/// Коефіцієнт співвідношення кількості задач, які будуть розподілятись на дану систему.
		/// </summary>
		public double TasksCountRatio { get; set; }

		public override string ToString() {
			return $"{nameof(IP)}: {IP}; {nameof(TasksCountRatio)}: {TasksCountRatio}";
		}
	}
}
