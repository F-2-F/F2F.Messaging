using System;
using System.Collections.Generic;
using System.Linq;
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

		void RegisterHandlers(Func<Type, IEnumerable<IExecute>> resolveHandlers);

		void RegisterAsyncHandlers(Func<Type, IEnumerable<IExecuteAsync>> resolveAsyncHandlers);

		void RegisterHandler(Func<Type, Type, IExecuteWithResult> resolveHandler);

		void RegisterAsyncHandler(Func<Type, Type, IExecuteAsyncWithResult> resolveAsyncHandler);
	}
}