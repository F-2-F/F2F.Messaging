using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class EventBus : IEventBus
	{
		private readonly IScheduler _scheduler;
		
		private readonly ConcurrentDictionary<Type, object> _handlers = new ConcurrentDictionary<Type, object>();
		private readonly ConcurrentDictionary<Type, object> _asyncHandlers = new ConcurrentDictionary<Type, object>();

		private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

		public EventBus(IScheduler scheduler)
		{
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");
			
			_scheduler = scheduler;
		}

		public void Publish<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			if (message == null)
				throw new ArgumentNullException("message", "message is null.");

			NotifySubjects<TEvent>(message);

			NotifyHandlers<TEvent>(message);

			NotifyAsyncHandlers<TEvent>(message);
		}

		private void NotifySubjects<TEvent>(TEvent message)
				where TEvent : class, IEvent
		{
			object subject;
			if (_subjects.TryGetValue(typeof(TEvent), out subject))
			{
				((ISubject<TEvent>)subject).OnNext(message);
			}
		}

		private void NotifyHandlers<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			object handlerFactory;
			if (_handlers.TryGetValue(typeof(TEvent), out handlerFactory)
				&& handlerFactory is Func<IEnumerable<IHandle<TEvent>>>)
			{
				var resolver = handlerFactory as Func<IEnumerable<IHandle<TEvent>>>;

				foreach (var handler in resolver())
				{
					Schedule(handler, message);
				}
			}
		}

		private void NotifyAsyncHandlers<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			object handlerFactory;
			if (_asyncHandlers.TryGetValue(typeof(TEvent), out handlerFactory)
				&& handlerFactory is Func<IEnumerable<IHandleAsync<TEvent>>>)
			{
				var resolver = handlerFactory as Func<IEnumerable<IHandleAsync<TEvent>>>;

				foreach (var handler in resolver())
				{
					Schedule(handler, message);
				}
			}
		}

		public IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : class, IEvent
		{
			var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), _ => new Subject<TEvent>());

			return subject.AsObservable();
		}

		public void Register<TEvent>(Func<IEnumerable<IHandle<TEvent>>> resolveHandlers) where TEvent : class, IEvent
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_handlers.AddOrUpdate(typeof(TEvent), resolveHandlers, (_, __) => resolveHandlers);
		}

		private Task Schedule<TEvent>(IHandle<TEvent> handler, TEvent @event)
			where TEvent : class, IEvent
		{
			return Observable.Start(() => handler.Handle(@event), _scheduler).ToTask();
		}

		private Task Schedule<TEvent>(IHandleAsync<TEvent> handler, TEvent @event)
			where TEvent : class, IEvent
		{
			 return Observable.Start(async () => await handler.Handle(@event).ConfigureAwait(false), _scheduler).ToTask();
		}

		public void Register<TEvent>(Func<IEnumerable<IHandleAsync<TEvent>>> resolveHandlers) where TEvent : class, IEvent
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_asyncHandlers.AddOrUpdate(typeof(TEvent), resolveHandlers, (_, __) => resolveHandlers);
		}
	}
}