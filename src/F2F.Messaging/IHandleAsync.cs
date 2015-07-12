using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public interface IHandleAsync { }
	
	public interface IHandleAsync<in TEvent> : IHandleAsync
	{
		Task Handle(TEvent @event);
	}
}
