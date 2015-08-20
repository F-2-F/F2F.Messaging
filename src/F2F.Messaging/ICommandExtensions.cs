using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public static class ICommandExtensions
	{
		public static IExecute<TCommand> CreateEmptyHandler<TCommand>(this TCommand self)
			where TCommand : ICommand
		{
			return new ExecuteDelegateCommand<TCommand>(c => { return Task.FromResult(true); });
		}

		public static IExecute<TCommand> CreateHandler<TCommand>(this TCommand self, Action<TCommand> handler)
			where TCommand : ICommand
		{
			return new ExecuteDelegateCommand<TCommand>(c => { handler(c); return Task.FromResult(true); });
		}

		public static IExecute<TCommand> CreateHandlerAsync<TCommand>(this TCommand self, Action<TCommand> handler)
			where TCommand : ICommand
		{
			return new ExecuteDelegateCommand<TCommand>(c => Task.Run(() => handler(c)));
		}

		public static IExecute<TCommand> CreateHandler<TCommand>(this TCommand self, Func<TCommand, Task> handler)
			where TCommand : ICommand
		{
			return new ExecuteDelegateCommand<TCommand>(handler);
		}
	}
}