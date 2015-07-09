using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public interface IHandleAsync<TEvent>
	{
		Task Handle(TEvent @event);
	}
}
