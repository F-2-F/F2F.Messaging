using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	public class ExecuteDelegateDomainCommand<TDomainCommand> : IExecuteDomainCommand<TDomainCommand>
		where TDomainCommand : IDomainCommand
	{
		private readonly Func<TDomainCommand, Task> _handler;

		public ExecuteDelegateDomainCommand(Func<TDomainCommand, Task> handler)
		{
			_handler = handler;
		}

		public Task ExecuteAsync(TDomainCommand command)
		{
			return _handler(command);
		}
	}

	public class ExecuteDelegateDomainCommand<TDomainCommand, TResult> : IExecuteDomainCommand<TDomainCommand, TResult>
		where TDomainCommand : IDomainCommand<TResult>
	{
		private readonly Func<TDomainCommand, Task<TResult>> _handler;

		public ExecuteDelegateDomainCommand(Func<TDomainCommand, Task<TResult>> handler)
		{
			_handler = handler;
		}

		public Task<TResult> ExecuteAsync(TDomainCommand command)
		{
			return _handler(command);
		}
	}
}