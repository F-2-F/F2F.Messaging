using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	/// <summary>
	/// A message bus for asynchronous execution of <see cref="ICommand"/>.
	/// </summary>
	public interface ICommandBus
	{
		Task Execute<TCommand>(TCommand command)
			where TCommand : class, ICommand;

		Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>;

		void Register<TCommand>(Func<IEnumerable<IExecute<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand;

		void Register<TCommand, TResult>(Func<IExecute<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>;

		void Register<TCommand>(Func<IEnumerable<IExecuteAsync<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand;

		void Register<TCommand, TResult>(Func<IExecuteAsync<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>;
	}
}