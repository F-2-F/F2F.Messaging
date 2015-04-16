using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class ExecuteDelegateCommand<TCommand> : IExecuteCommand<TCommand>
		where TCommand : ICommand
	{
		private readonly Func<TCommand, Task> _handler;

		public ExecuteDelegateCommand(Func<TCommand, Task> handler)
		{
			_handler = handler;
		}

		public Task ExecuteAsync(TCommand command)
		{
			return _handler(command);
		}
	}

	public class ExecuteDelegateCommand<TCommand, TResult> : IExecuteCommand<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		private readonly Func<TCommand, Task<TResult>> _handler;

		public ExecuteDelegateCommand(Func<TCommand, Task<TResult>> handler)
		{
			_handler = handler;
		}

		public Task<TResult> ExecuteAsync(TCommand command)
		{
			return _handler(command);
		}
	}
}