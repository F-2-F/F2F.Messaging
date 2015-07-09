using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// A command handler for a command.
	/// </summary>
	public interface IExecuteAsync<TCommand>
		where TCommand : ICommand
	{
		Task Execute(TCommand command);
	}

	/// <summary>
	/// A command handler for a command with result.
	/// </summary>
	public interface IExecuteAsync<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		Task<TResult> Execute(TCommand command);
	}

}
