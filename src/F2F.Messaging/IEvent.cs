using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// Tagging interface for events.
	/// </summary>
	/// <remarks>
	/// Events are implemented as immutable value objects and just carry around
	/// necessary event related data. They do not provide any functionality.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
		"CA1040:AvoidEmptyInterfaces", Justification = "Tagging interface is perfered solution here.")]
	public interface IEvent
	{
	}
}