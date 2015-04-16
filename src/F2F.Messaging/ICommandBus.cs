using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	/// <summary>
	/// A message bus for asynchronous execution of <see cref="ICommand"/>.
	/// </summary>
	[dbc.ContractClass(typeof(ICommandBusContract))]
	public interface ICommandBus
	{
		Task Execute<TCommand>(TCommand command)
			where TCommand : class, ICommand;

		Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>;

		void Register<TCommand>(Func<IEnumerable<IExecuteCommand<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand;

		void Register<TCommand, TResult>(Func<IExecuteCommand<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>;
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="ICommandBus"/></summary>
	[dbc.ContractClassFor(typeof(ICommandBus))]
	internal abstract class ICommandBusContract : ICommandBus
	{
		public Task Execute<TCommand>(TCommand command)
			where TCommand : class, ICommand
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task);
		}

		public Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task<TResult>);
		}

		public void Register<TCommand>(Func<IEnumerable<IExecuteCommand<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand
		{
			dbc.Contract.Requires<ArgumentNullException>(resolveHandlers != null, "resolveHandlers is null");
		}

		public void Register<TCommand, TResult>(Func<IExecuteCommand<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>
		{
			dbc.Contract.Requires<ArgumentNullException>(resolveHandler != null, "resolveHandler is null");
		}
	}

#pragma warning restore
}