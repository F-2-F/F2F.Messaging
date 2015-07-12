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

		private Func<Type, IEnumerable<IHandle>> _resolveHandlers;
		private Func<Type, IEnumerable<IHandleAsync>> _resolveAsyncHandlers;

		private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

		public EventBus(IScheduler scheduler)
		{
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");
			
			_scheduler = scheduler;

			_resolveHandlers = _ => Enumerable.Empty<IHandle>();
			_resolveAsyncHandlers = _ => Enumerable.Empty<IHandleAsync>();
		}

		public void Publish<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			if (message == null)
				throw new ArgumentNullException("message", "message is null.");

			NotifySubjects<TEvent>(message);

			NotifyAsyncHandlers<TEvent>(message);

			NotifyHandlers<TEvent>(message);
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

		private IEnumerable<IHandle<TEvent>> ResolveHandlers<TEvent>()
		{
			var handlers = _resolveHandlers(typeof(TEvent));

			foreach (var handler in handlers)
			{
				yield return handler as IHandle<TEvent>;
			}
		}

		private void NotifyHandlers<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			foreach (var handler in ResolveHandlers<TEvent>())
			{
				Schedule(handler, message);
			}			
		}

		private IEnumerable<IHandleAsync<TEvent>> ResolveAsyncHandlers<TEvent>()
		{
			var handlers = _resolveAsyncHandlers(typeof(TEvent));

			foreach (var handler in handlers)
			{
				yield return handler as IHandleAsync<TEvent>;
			}
		}
		
		private void NotifyAsyncHandlers<TEvent>(TEvent message)
			where TEvent : class, IEvent
		{
			foreach (var handler in ResolveAsyncHandlers<TEvent>())
			{
				Schedule(handler, message);
			}			
		}

		public IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : class, IEvent
		{
			var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), _ => new Subject<TEvent>());

			return subject.AsObservable();
		}

		public void RegisterHandlers(Func<Type, IEnumerable<IHandle>> resolveHandlers)
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_resolveHandlers = resolveHandlers;
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

		public void RegisterAsyncHandlers(Func<Type, IEnumerable<IHandleAsync>> resolveAsyncHandlers)
		{
			if (resolveAsyncHandlers == null)
				throw new ArgumentNullException("resolveAsyncHandlers", "resolveAsyncHandlers is null.");

			_resolveAsyncHandlers = resolveAsyncHandlers;
		}
	}
}