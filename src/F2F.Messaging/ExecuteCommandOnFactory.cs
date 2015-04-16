using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Messaging
{
	public abstract class ExecuteCommandOnFactory<TCommand> : IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		private readonly IStartTask _factory;

		protected ExecuteCommandOnFactory(IStartTask factory)
		{
			dbc.Contract.Requires<ArgumentNullException>(factory != null, "factory must not be null");

			_factory = factory;
		}

		public Task ExecuteAsync(TCommand command)
		{
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
			dbc.Contract.Requires<ArgumentNullException>(factory != null, "factory must not be null");

			_factory = factory;
		}

		public Task<TResult> ExecuteAsync(TCommand command)
		{
			return _factory.Start(() => Execute(command));
		}

		public abstract TResult Execute(TCommand command);
	}
}