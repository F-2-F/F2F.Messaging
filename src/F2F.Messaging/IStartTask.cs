using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	[dbc.ContractClass(typeof(IStartTaskContract))]
	public interface IStartTask
	{
		Task Start(Action work);

		Task<TResult> Start<TResult>(Func<TResult> work);
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	[dbc.ContractClassFor(typeof(IStartTask))]
	internal abstract class IStartTaskContract : IStartTask
	{
		public Task Start(Action work)
		{
			dbc.Contract.Requires<ArgumentNullException>(work != null, "work must not be null");

			return default(Task);
		}

		public Task<TResult> Start<TResult>(Func<TResult> work)
		{
			dbc.Contract.Requires<ArgumentNullException>(work != null, "work must not be null");

			return default(Task<TResult>);
		}
	}

#pragma warning restore
}