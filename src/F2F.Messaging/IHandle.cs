using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.Messaging
{
	public interface IHandle { }

	public interface IHandle<in TEvent> : IHandle
	{
		void Handle(TEvent @event);
	}
}