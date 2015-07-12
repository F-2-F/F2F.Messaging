using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.Messaging
{
	public interface IExecute { }

	/// <summary>
	/// A command handler for a command.
	/// </summary>
	public interface IExecute<TCommand> : IExecute
		where TCommand : ICommand
	{
		void Execute(TCommand command);
	}

	public interface IExecuteWithResult { }

	/// <summary>
	/// A command handler for a command with result.
	/// </summary>
	public interface IExecute<TCommand, TResult> : IExecuteWithResult
		where TCommand : ICommand<TResult>
	{
		TResult Execute(TCommand command);
	}
}