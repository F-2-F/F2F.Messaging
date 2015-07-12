using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.Messaging
{
	/// <summary>
	/// A message bus for asynchronous execution of <see cref="IEvent"/>.
	/// </summary>
	public interface IEventBus
	{
		void Publish<TEvent>(TEvent message)
			where TEvent : class, IEvent;

		IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : class, IEvent;

		void RegisterHandlers(Func<Type, IEnumerable<IHandle>> resolveHandlers);

		void RegisterAsyncHandlers(Func<Type, IEnumerable<IHandleAsync>> resolveHandlers);
	}
}