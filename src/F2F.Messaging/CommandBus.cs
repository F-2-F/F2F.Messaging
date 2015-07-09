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
		private readonly ConcurrentDictionary<Type, object> _asyncHandlers = new ConcurrentDictionary<Type, object>();

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
			
			object handlerFactory;
			var handlerFound = 
				_handlers.TryGetValue(typeof(TCommand), out handlerFactory)
				&& handlerFactory is Func<IEnumerable<IExecute<TCommand>>>;

			object asyncHandlerFactory;
			var asyncHandlerFound =
				_asyncHandlers.TryGetValue(typeof(TCommand), out asyncHandlerFactory)
				&& asyncHandlerFactory is Func<IEnumerable<IExecuteAsync<TCommand>>>;

			if (handlerFound || asyncHandlerFound)
			{
				var createHandlers = 
					handlerFactory as Func<IEnumerable<IExecute<TCommand>>>
					?? (() => Enumerable.Empty<IExecute<TCommand>>());

				var createAsyncHandlers = 
					asyncHandlerFactory as Func<IEnumerable<IExecuteAsync<TCommand>>> 
					?? (() => Enumerable.Empty<IExecuteAsync<TCommand>>());

				return Task.WhenAll(createHandlers().Select(h => Schedule(h, command))
							.Concat(createAsyncHandlers().Select(h => Schedule(h, command))));
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

		private Task Schedule<TCommand>(IExecuteAsync<TCommand> handler, TCommand command)
			where TCommand : class, ICommand
		{
			return Observable.Start(async () => await handler.Execute(command).ConfigureAwait(false), _scheduler).ToTask();
		}

		public Task<TResult> Execute<TCommand, TResult>(TCommand command)
			where TCommand : class, ICommand<TResult>
		{
			if (command == null)
				throw new ArgumentNullException("command", "command is null.");

			object handler;
			var handlerFound = 
				_handlers.TryGetValue(typeof(TCommand), out handler)
				&& handler is Func<IExecute<TCommand, TResult>>;

			object asyncHandler;
			var asyncHandlerFound =
				_asyncHandlers.TryGetValue(typeof(TCommand), out asyncHandler)
				&& asyncHandler is Func<IExecuteAsync<TCommand, TResult>>;

			if (handlerFound)
			{
				var createHandler = handler as Func<IExecute<TCommand, TResult>>;

				return Schedule(createHandler(), command);
			}
			else if (asyncHandlerFound)
			{
				var createHandler = asyncHandler as Func<IExecuteAsync<TCommand, TResult>>;

				return Schedule(createHandler(), command);
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
			return await await Observable.Start(async () => await handler.Execute(command), _scheduler);
		}

		public void Register<TCommand>(Func<IEnumerable<IExecute<TCommand>>> resolveHandlers)
			where TCommand : class, ICommand
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_handlers.AddOrUpdate(typeof(TCommand), resolveHandlers, (_, __) => resolveHandlers);
		}

		public void Register<TCommand>(Func<IEnumerable<IExecuteAsync<TCommand>>> resolveHandlers) where TCommand : class, ICommand
		{
			if (resolveHandlers == null)
				throw new ArgumentNullException("resolveHandlers", "resolveHandlers is null.");

			_asyncHandlers.AddOrUpdate(typeof(TCommand), resolveHandlers, (_, __) => resolveHandlers);
		}

		// TODO: Should throw exception if more than one handler is registered for same TCommand / TResult combination

		public void Register<TCommand, TResult>(Func<IExecute<TCommand, TResult>> resolveHandler)
			where TCommand : class, ICommand<TResult>
		{
			if (resolveHandler == null)
				throw new ArgumentNullException("resolveHandler", "resolveHandler is null.");

			_handlers.AddOrUpdate(typeof(TCommand), resolveHandler, (_, __) => resolveHandler);
		}

		public void Register<TCommand, TResult>(Func<IExecuteAsync<TCommand, TResult>> resolveHandler) where TCommand : class, ICommand<TResult>
		{
			if (resolveHandler == null)
				throw new ArgumentNullException("resolveHandler", "resolveHandler is null.");

			_asyncHandlers.AddOrUpdate(typeof(TCommand), resolveHandler, (_, __) => resolveHandler);
		}
	}
}