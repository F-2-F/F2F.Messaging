using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class OnTaskPool : IStartTask
	{
		private readonly TaskFactory _taskFactory;

		public OnTaskPool()
		{
			_taskFactory = new TaskFactory(TaskScheduler.Default);
		}

		public OnTaskPool(TaskFactory taskFactory)
		{
			if (taskFactory == null)
				throw new ArgumentNullException("taskFactory", "taskFactory is null.");

			_taskFactory = taskFactory;
		}

		public Task Start(Action work)
		{
			if (work == null)
				throw new ArgumentNullException("work", "work is null.");

			return _taskFactory.StartNew(work);
		}

		public Task<TResult> Start<TResult>(Func<TResult> work)
		{
			if (work == null)
				throw new ArgumentNullException("work", "work is null.");

			return _taskFactory.StartNew(work);
		}
	}
}