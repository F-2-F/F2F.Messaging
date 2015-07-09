using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.Messaging
{
	public interface IHandle<TEvent>
	{
		void Handle(TEvent @event);
	}
}
