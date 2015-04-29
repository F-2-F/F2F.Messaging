using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// A command handler for a command.
	/// </summary>
	public interface IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		void Execute(TCommand command);
	}

	/// <summary>
	/// A command handler for a command with result.
	/// </summary>
	public interface IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		TResult Execute(TCommand command);
	}
}