using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Messaging
{
	public class CommandBus : ICommandBus
	{
		private readonly IScheduler _scheduler;
		private readonly ConcurrentDictionary<Type, object> _handlers = new ConcurrentDictionary<Type, object>();

		public CommandBus(IScheduler scheduler)
		{
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");

			_scheduler = scheduler;
		}

		public Task Execute<TCommand>(TCommand command)
			where TCommand : class, ICommand
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			object value;
			if (_handlers.TryGetValue(typeof(TCommand), out value)
				&& value is Func<IEnumerable<IExecuteCommand<TCommand>>>)
			{
				var resolver = value as Func<IEnumerable<IExecuteCommand<TCommand>>>;

				return Task.WhenAll(resolver().Select(h => Schedule(h, command)));
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TCommand)));
			}
		}

		private Task Schedule<TCommand>(IExecuteCommand<TCommand> handler, TCommand command)
			where TCommand : class, ICommand
		{
			return Observable.Start(() => handler.Execute(command), _scheduler).ToTask();
		}

		public Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			object value;
			if (_handlers.TryGetValue(typeof(TCommand), out value)
				&& value is Func<IExecuteCommand<TCommand, TResult>>)
			{
				var resolver = value as Func<IExecuteCommand<TCommand, TResult>>;

				return Schedule(resolver(), command);
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TCommand)));
			}
		}

		private Task<TResult> Schedule<TCommand, TResult>(IExecuteCommand<TCommand, TResult> handler, TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			return Observable.Start(() => handler.Execute(command), _scheduler).ToTask();
		}

		public void Register<TCommand>(Func<IEnumerable<IExecuteCommand<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_handlers.AddOrUpdate(typeof(TCommand), resolveHandlers, (t, h) => resolveHandlers);
		}

		public void Register<TCommand, TResult>(Func<IExecuteCommand<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>
		{
			if (resolveHandler == null)
				throw new ArgumentNullException("resolveHandler", "resolveHandler is null.");

			_handlers.AddOrUpdate(typeof(TCommand), resolveHandler, (t, h) => resolveHandler);
		}
	}
}