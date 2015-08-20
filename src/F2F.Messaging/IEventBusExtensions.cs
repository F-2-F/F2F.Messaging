using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;

namespace F2F.Messaging
{
	public static class IEventBusExtensions
	{
		public static IEventBus CreatedScopedEventBus(this IEventBus eventBus, IScheduler scheduler)
		{
			return new ScopedEventBus(eventBus, scheduler);
		}
	}
}
