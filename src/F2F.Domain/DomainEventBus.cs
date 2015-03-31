using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	public class DomainEventBus : IDomainEventBus
	{
		/// <summary>Thread-safe dictionary holding the subjects.</summary>
		private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

		public void Publish<TDomainEvent>(TDomainEvent message)
			where TDomainEvent : IDomainEvent
		{
			object subject;
			if (_subjects.TryGetValue(typeof(TDomainEvent), out subject))
			{
				((ISubject<TDomainEvent>)subject).OnNext(message);
			}
		}

		public IObservable<TDomainEvent> ListenTo<TDomainEvent>()
			where TDomainEvent : IDomainEvent
		{
			var subject = (ISubject<TDomainEvent>)_subjects.GetOrAdd(typeof(TDomainEvent), t => new Subject<TDomainEvent>());

			return subject.AsObservable();
		}
	}
}