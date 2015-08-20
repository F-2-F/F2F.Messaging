using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace F2F.Messaging
{
	internal class ScopedEventBus : IEventBus
	{
		private readonly IEventBus _outerScopeBus;
		private readonly IEventBus _scopedBus;

		public ScopedEventBus(IEventBus outerScopeBus, IScheduler scheduler)
		{
			if (outerScopeBus == null)
				throw new ArgumentNullException("outerScopeBus", "outerScopeBus is null.");
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");

			_outerScopeBus = outerScopeBus;

			_scopedBus = new EventBus(scheduler);
		}

		public void Publish<TEvent>(TEvent message) where TEvent : class, IEvent
		{
			// only publish to current scope
			_scopedBus.Publish(message);
		}

		public IObservable<TEvent> ListenTo<TEvent>() where TEvent : class, IEvent
		{
			// Listen to events from outerScope as well as current scope
			return _scopedBus.ListenTo<TEvent>().Merge(_outerScopeBus.ListenTo<TEvent>());
		}

		public void RegisterHandlers(Func<Type, IEnumerable<IHandle>> resolveHandlers)
		{
			// TODO: Think about also allowing external handlers for outer scope
			_scopedBus.RegisterHandlers(resolveHandlers);
		}

		public void RegisterAsyncHandlers(Func<Type, IEnumerable<IHandleAsync>> resolveHandlers)
		{
			// TODO: Think about also allowing external handlers for outer scope
			_scopedBus.RegisterAsyncHandlers(resolveHandlers);
		}
	}
}
