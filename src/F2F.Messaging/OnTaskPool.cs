using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

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
			dbc.Contract.Requires<ArgumentNullException>(taskFactory != null, "taskFactory must not be null");

			_taskFactory = taskFactory;
		}

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