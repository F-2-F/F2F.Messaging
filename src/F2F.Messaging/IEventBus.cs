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
			where TEvent : IEvent;

		IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : IEvent;
	}
}