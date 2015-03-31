using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Domain
{
	public abstract class ExecuteDomainCommand<TDomainCommand, TTaskFactory> : IExecuteDomainCommand<TDomainCommand>
		where TDomainCommand : IDomainCommand
		where TTaskFactory : IStartTask, new()
	{
		private readonly TTaskFactory _factory;

		public ExecuteDomainCommand()
		{
			_factory = new TTaskFactory();
		}

		public ExecuteDomainCommand(TTaskFactory factory)
		{
			_factory = factory;
		}

		public Task ExecuteAsync(TDomainCommand command)
		{
			return _factory.Start(() => Execute(command));
		}

		protected abstract void Execute(TDomainCommand command);
	}

	public abstract class ExecuteDomainCommand<TDomainCommand, TResult, TTaskFactory> : IExecuteDomainCommand<TDomainCommand, TResult>
		where TDomainCommand : IDomainCommand<TResult>
		where TTaskFactory : IStartTask, new()
	{
		private readonly TTaskFactory _factory;

		public ExecuteDomainCommand()
		{
			_factory = new TTaskFactory();
		}

		public ExecuteDomainCommand(TTaskFactory factory)
		{
			_factory = factory;
		}

		public Task<TResult> ExecuteAsync(TDomainCommand command)
		{
			return _factory.Start(() => Execute(command));
		}

		protected abstract TResult Execute(TDomainCommand command);
	}
}