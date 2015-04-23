using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class OnScheduler : IStartTask
	{
		private readonly IScheduler _scheduler;

		public OnScheduler(IScheduler scheduler)
		{
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");

			_scheduler = scheduler;
		}

		public Task Start(Action work)
		{
			if (work == null)
				throw new ArgumentNullException("work", "work is null.");

			return Observable.Start(work, _scheduler).ToTask();
		}

		public Task<TResult> Start<TResult>(Func<TResult> work)
		{
			if (work == null)
				throw new ArgumentNullException("work", "work is null.");

			return Observable.Start(work, _scheduler).ToTask();
		}
	}

	public class OnScheduler<TScheduler> : OnScheduler
		where TScheduler : IScheduler, new()
	{
		public OnScheduler()
			: base(new TScheduler())
		{
		}
	}
}