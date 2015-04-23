using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// A asynchronous command handler which accepts <see cref="ICommand"/>.
	/// </summary>
	public interface IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		Task ExecuteAsync(TCommand command);
	}

	/// <summary>
	/// A asynchronous command handler which accepts <see cref="ICommand"/> and returns a result.
	/// </summary>
	public interface IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		Task<TResult> ExecuteAsync(TCommand command);
	}
}