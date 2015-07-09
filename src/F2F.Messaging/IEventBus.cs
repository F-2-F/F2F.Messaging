using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		void Register<TEvent>(Func<IEnumerable<IHandle<TEvent>>> resolveHandlers)
			where TEvent : class, IEvent;
	}
}