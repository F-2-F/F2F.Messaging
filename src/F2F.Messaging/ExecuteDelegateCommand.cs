using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class ExecuteDelegateCommand<TCommand> : IExecute<TCommand>
		where TCommand : ICommand
	{
		private readonly Func<TCommand, Task> _handler;

		public ExecuteDelegateCommand(Func<TCommand, Task> handler)
		{
			_handler = handler;
		}

		public void Execute(TCommand command)
		{
			_handler(command);
		}
	}

	public class ExecuteDelegateCommand<TCommand, TResult> : IExecute<TCommand, TResult>
		where TCommand : ICommand<TResult>
	{
		private readonly Func<TCommand, TResult> _handler;

		public ExecuteDelegateCommand(Func<TCommand, TResult> handler)
		{
			_handler = handler;
		}

		public TResult Execute(TCommand command)
		{
			return _handler(command);
		}
	}
}