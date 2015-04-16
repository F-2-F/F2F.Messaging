using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	[dbc.ContractClass(typeof(IEventBusContract))]
	public interface IEventBus
	{
		void Publish<TEvent>(TEvent message)
			where TEvent : IEvent;

		IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : IEvent;
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="IEventBus"/></summary>
	[dbc.ContractClassFor(typeof(IEventBus))]
	internal abstract class IEventBusContract : IEventBus
	{
		public void Publish<TMessage>(TMessage message)
			where TMessage : IEvent
		{
			dbc.Contract.Requires<ArgumentNullException>(message != null, "message is null");
		}

		public IObservable<TEvent> ListenTo<TEvent>()
			where TEvent : IEvent
		{
			return default(IObservable<TEvent>);
		}
	}

#pragma warning restore
}