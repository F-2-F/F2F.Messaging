using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	/// <summary>
	/// A asynchronous command handler which accepts <see cref="ICommand"/>.
	/// </summary>
	[dbc.ContractClass(typeof(IExecuteCommandContract<>))]
	public interface IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		Task ExecuteAsync(TCommand command);
	}

	/// <summary>
	/// A asynchronous command handler which accepts <see cref="ICommand"/> and returns a result.
	/// </summary>
	[dbc.ContractClass(typeof(IExecuteCommandContract<,>))]
	public interface IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		Task<TResult> ExecuteAsync(TCommand command);
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="IExecuteCommand<>"/></summary>
	[dbc.ContractClassFor(typeof(IExecuteCommand<>))]
	internal abstract class IExecuteCommandContract<TCommand> : IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		public Task ExecuteAsync(TCommand command)
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task);
		}
	}

	/// <summary>Contract for <see cref="IExecuteCommand<,>"/></summary>
	[dbc.ContractClassFor(typeof(IExecuteCommand<,>))]
	internal abstract class IExecuteCommandContract<TCommand, TResult> : IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		public Task<TResult> ExecuteAsync(TCommand command)
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task<TResult>);
		}
	}

#pragma warning restore
}