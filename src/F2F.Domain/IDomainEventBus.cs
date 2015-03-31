using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Domain
{
	[dbc.ContractClass(typeof(IDomainEventBusContract))]
	public interface IDomainEventBus
	{
		void Publish<TDomainEvent>(TDomainEvent message)
			where TDomainEvent : IDomainEvent;

		IObservable<TDomainEvent> ListenTo<TDomainEvent>()
			where TDomainEvent : IDomainEvent;
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="IDomainEventBus"/></summary>
	[dbc.ContractClassFor(typeof(IDomainEventBus))]
	internal abstract class IDomainEventBusContract : IDomainEventBus
	{
		public void Publish<TMessage>(TMessage message)
			where TMessage : IDomainEvent
		{
			dbc.Contract.Requires<ArgumentNullException>(message != null, "message is null");
		}

		public IObservable<TDomainEvent> ListenTo<TDomainEvent>()
			where TDomainEvent : IDomainEvent
		{
			return default(IObservable<TDomainEvent>);
		}
	}

#pragma warning restore
}