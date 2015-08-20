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
		
		private Func<Type, IEnumerable<IExecute>> _resolveHandlers;
		private Func<Type, IEnumerable<IExecuteAsync>> _resolveAsyncHandlers;

		private Func<Type, Type, IExecuteWithResult> _resolveHandlerWithResult;
		private Func<Type, Type, IExecuteAsyncWithResult> _resolveAsyncHandlerWithResult;

		public CommandBus(IScheduler scheduler)
		{
			if (scheduler == null)
				throw new ArgumentNullException("scheduler", "scheduler is null.");

			_scheduler = scheduler;

			_resolveHandlers = _ => Enumerable.Empty<IExecute>();
			_resolveAsyncHandlers = _ => Enumerable.Empty<IExecuteAsync>();
		}

		public Task Execute<TCommand>(TCommand command)
			where TCommand : class, ICommand
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			var handlers =
				_resolveHandlers(typeof(TCommand)).OfType<IExecute<TCommand>>();

			var asyncHandlers = 
				_resolveAsyncHandlers(typeof(TCommand)).OfType<IExecuteAsync<TCommand>>();

			if (handlers.Any() || asyncHandlers.Any())
			{
				return Task.WhenAll(handlers.Select(h => Schedule(h, command))
							.Concat(asyncHandlers.Select(h => Schedule(h, command))));
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TCommand)));
			}
		}

		private Task Schedule<TCommand>(IExecute<TCommand> handler, TCommand command)
			where TCommand : class, ICommand
		{
			return Observable.Start(() => handler.Execute(command), _scheduler).ToTask();
		}

		private async Task Schedule<TCommand>(IExecuteAsync<TCommand> handler, TCommand command)
			where TCommand : class, ICommand
		{
			await Observable.Start(() => handler.Execute(command).Wait(), _scheduler).ToTask().ConfigureAwait(false);
		}

		public Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			var handler = 
				_resolveHandlerWithResult == null 
				? null
				: _resolveHandlerWithResult(typeof(TCommand), typeof(TResult)) as IExecute<TCommand, TResult>;

			var asyncHandler = 
				_resolveAsyncHandlerWithResult == null 
				? null
				: _resolveAsyncHandlerWithResult(typeof(TCommand), typeof(TResult)) as IExecuteAsync<TCommand, TResult>;

			if (handler != null)
			{
				return Schedule(handler, command);
			}
			else if (asyncHandler != null)
			{
				return Schedule(asyncHandler, command);
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TCommand)));
			}
		}

		private Task<TResult> Schedule<TCommand, TResult>(IExecute<TCommand, TResult> handler, TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			return Observable.Start(() => handler.Execute(command), _scheduler).ToTask();
		}

		private async Task<TResult> Schedule<TCommand, TResult>(IExecuteAsync<TCommand, TResult> handler, TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			return await Observable.Start(() => handler.Execute(command).Result, _scheduler);
		}

		public void RegisterHandlers(Func<Type, IEnumerable<IExecute>> resolveHandlers)
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_resolveHandlers = resolveHandlers;
		}

		public void RegisterAsyncHandlers(Func<Type, IEnumerable<IExecuteAsync>> resolveAsyncHandlers)
		{
			if (resolveAsyncHandlers == null)
				throw new ArgumentNullException("resolveAsyncHandlers", "resolveAsyncHandlers is null.");

			_resolveAsyncHandlers = resolveAsyncHandlers;
		}

		public void RegisterHandler(Func<Type, Type, IExecuteWithResult> resolveHandler)
		{
			if (resolveHandler == null)
				throw new ArgumentNullException("resolveHandler", "resolveHandler is null.");

			_resolveHandlerWithResult = resolveHandler;
		}

		public void RegisterAsyncHandler(Func<Type, Type, IExecuteAsyncWithResult> resolveAsyncHandler)
		{
			if (resolveAsyncHandler == null)
				throw new ArgumentNullException("resolveAsyncHandler", "resolveAsyncHandler is null.");

			_resolveAsyncHandlerWithResult = resolveAsyncHandler;
		}
	}
}