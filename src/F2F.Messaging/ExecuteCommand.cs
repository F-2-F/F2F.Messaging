using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace F2F.Messaging
{
	public abstract class ExecuteCommand<TCommand, TTaskFactory> : ExecuteCommandOnFactory<TCommand>
		where TCommand : ICommand
		where TTaskFactory : IStartTask, new()
	{
		protected ExecuteCommand()
			: base(new TTaskFactory())
		{
		}
	}

	public abstract class ExecuteCommand<TCommand, TResult, TTaskFactory> : ExecuteCommandOnFactory<TCommand, TResult>
		where TCommand : ICommand<TResult>
		where TTaskFactory : IStartTask, new()
	{
		protected ExecuteCommand()
			: base(new TTaskFactory())
		{
		}
	}
}