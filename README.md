# DistributedComputingSystem (diploma project)

System for organization distributed scalable computing (with AKKA.NET). Allows using several computers (that supports .NET Core) to distribute incoming tasks (declared as a C# code) between them.

# How to

1. Specify in the appsettings.json file:
  - IP addresses of computers which you wanna use as workers,
  - the coefficient for each worker that adjusts the number of tasks that will be passed to this worker.

2. Start worker systems (DistributedComputingSystem.WorkerSystem) on computers you specified before and make sure this computer is available through the specified IP address.

3. Start the main system (DistributedComputingSystem.MainSystem.Web) on any computer that has access to all workers. The main system also can be run on a computer used as a worker.
