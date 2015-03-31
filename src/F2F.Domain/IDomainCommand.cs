using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	/// <summary>
	/// Tagging interface for domain commands.
	/// An <see cref="IDomainCommand"/> is a command that must be executed within the domain model.
	/// The direction of a command is opposite to the direction of an event. A command always flows
	/// from the outside into the domain and triggers an action within the domain.
	/// </summary>
	/// <remarks>
	/// Domain Commands are implemented as immutable value objects and just carry around
	/// necessary command related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		"CA1040:AvoidEmptyInterfaces", Justification = "Tagging interface is prefered solution here.")]
	public interface IDomainCommand
	{
	}

	/// <summary>
	/// Tagging interface for domain commands with return value.
	/// An <see cref="IDomainCommand"/> is a command that must be executed within the domain model.
	/// The direction of a command is opposite to the direction of an event. A command always flows
	/// from the outside into the domain and triggers an action within the domain.
	/// </summary>
	/// <remarks>
	/// Domain Commands are implemented as immutable value objects and just carry around
	/// necessary command related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		"CA1040:AvoidEmptyInterfaces", Justification = "Tagging interface is prefered solution here.")]
	public interface IDomainCommand<TResult>
	{
	}
}