using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public interface IStartTask
	{
		Task Start(Action work);

		Task<TResult> Start<TResult>(Func<TResult> work);
	}
}