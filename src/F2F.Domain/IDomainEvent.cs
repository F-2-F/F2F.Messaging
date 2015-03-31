using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	/// <summary>
	/// Tagging interface for domain events.
	/// An <see cref="IDomainEvent"/> is an object that is raised from the domain model in case it has
	/// to communicate some domain relevant fact, e.g. data has changed.
	/// The direction of an event is opposite to the direction of a command. An event always flows
	/// from the inside of the domain to the outside to inform interested parties of domain relevant facts.
	/// </summary>
	/// <remarks>
	/// Domain Events are implemented as immutable value objects and just carry around
	/// necessary event related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces",
		Justification = "Tagging interface is perfered solution here.")]
	public interface IDomainEvent
	{
	}
}