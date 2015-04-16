using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// Tagging interface for commands.
	/// </summary>
	/// <remarks>
	/// Commands are implemented as immutable value objects and just carry around
	/// necessary command related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		"CA1040:AvoidEmptyInterfaces", Justification = "Tagging interface is prefered solution here.")]
	public interface ICommand
	{
	}

	/// <summary>
	/// Tagging interface for commands with return value.
	/// </summary>
	/// <remarks>
	/// Commands are implemented as immutable value objects and just carry around
	/// necessary command related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		"CA1040:AvoidEmptyInterfaces", Justification = "Tagging interface is prefered solution here.")]
	public interface ICommand<TResult>
	{
	}
}