using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	public static class IDomainCommandExtensions
	{
		public static IExecuteDomainCommand<TDomainCommand> CreateEmptyHandler<TDomainCommand>(this TDomainCommand self)
			where TDomainCommand : IDomainCommand
		{
			return new ExecuteDelegateDomainCommand<TDomainCommand>(c => { return Task.FromResult(true); });
		}

		public static IExecuteDomainCommand<TDomainCommand> CreateHandler<TDomainCommand>(this TDomainCommand self, Action<TDomainCommand> handler)
			where TDomainCommand : IDomainCommand
		{
			return new ExecuteDelegateDomainCommand<TDomainCommand>(c => { handler(c); return Task.FromResult(true); });
		}

		public static IExecuteDomainCommand<TDomainCommand> CreateHandlerAsync<TDomainCommand>(this TDomainCommand self, Action<TDomainCommand> handler)
			where TDomainCommand : IDomainCommand
		{
			return new ExecuteDelegateDomainCommand<TDomainCommand>(c => Task.Run(() => handler(c)));
		}

		public static IExecuteDomainCommand<TDomainCommand> CreateHandler<TDomainCommand>(this TDomainCommand self, Func<TDomainCommand, Task> handler)
			where TDomainCommand : IDomainCommand
		{
			return new ExecuteDelegateDomainCommand<TDomainCommand>(handler);
		}
	}
}