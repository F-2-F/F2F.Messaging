using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public abstract class ExecuteCommandOnFactory<TCommand> : IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		private readonly IStartTask _factory;

		protected ExecuteCommandOnFactory(IStartTask factory)
		{
			if (factory == null)
				throw new ArgumentNullException("factory", "factory is null.");

			_factory = factory;
		}

		public Task ExecuteAsync(TCommand command)
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			return _factory.Start(() => Execute(command));
		}

		public abstract void Execute(TCommand command);
	}

	public abstract class ExecuteCommandOnFactory<TCommand, TResult> : IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		private readonly IStartTask _factory;

		protected ExecuteCommandOnFactory(IStartTask factory)
		{
			if (factory == null)
				throw new ArgumentNullException("factory", "factory is null.");

			_factory = factory;
		}

		public Task<TResult> ExecuteAsync(TCommand command)
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			return _factory.Start(() => Execute(command));
		}

		public abstract TResult Execute(TCommand command);
	}
}