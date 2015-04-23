using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class EventBus : IEventBus
	{
		private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

		public void Publish<TEvent>(TEvent message)
			where TEvent : IEvent
		{
			if (message == null)
				throw new ArgumentNullException("message", "message is null.");

			object subject;
			if (_subjects.TryGetValue(typeof(TEvent), out subject))
			{
				((ISubject<TEvent>)subject).OnNext(message);
			}
		}

		public IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : IEvent
		{
			var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());

			return subject.AsObservable();
		}
	}
}