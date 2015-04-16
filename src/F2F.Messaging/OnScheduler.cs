using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	public class OnScheduler : IStartTask
	{
		private readonly IScheduler _scheduler;

		public OnScheduler(IScheduler scheduler)
		{
			dbc.Contract.Requires<ArgumentNullException>(scheduler != null, "scheduler must not be null");

			_scheduler = scheduler;
		}

		public Task Start(Action work)
		{
			return Observable.Start(work, _scheduler).ToTask();
		}

		public Task<TResult> Start<TResult>(Func<TResult> work)
		{
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