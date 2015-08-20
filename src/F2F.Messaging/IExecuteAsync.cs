using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public interface IExecuteAsync { }
	/// <summary>
	/// A command handler for a command.
	/// </summary>
	public interface IExecuteAsync<in TCommand> : IExecuteAsync
		where TCommand : ICommand
	{
		Task Execute(TCommand command);
	}

	public interface IExecuteAsyncWithResult { }

	/// <summary>
	/// A command handler for a command with result.
	/// </summary>
	public interface IExecuteAsync<in TCommand, TResult> : IExecuteAsyncWithResult
		where TCommand : ICommand<TResult>
	{
		Task<TResult> Execute(TCommand command);
	}

}
