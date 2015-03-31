using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	public class OnTaskPool : IStartTask
	{
		private readonly TaskFactory _taskFactory = new TaskFactory(TaskScheduler.Default);

		public Task Start(Action work)
		{
			return _taskFactory.StartNew(work);
		}

		public Task<TResult> Start<TResult>(Func<TResult> work)
		{
			return _taskFactory.StartNew(work);
		}
	}
}