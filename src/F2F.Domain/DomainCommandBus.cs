using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2F.Domain
{
	public class DomainCommandBus : IDomainCommandBus
	{
		private readonly ConcurrentDictionary<Type, object> _handlers = new ConcurrentDictionary<Type, object>();

		public Task Execute<TDomainCommand>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand
		{
			object value;
			if (_handlers.TryGetValue(typeof(TDomainCommand), out value)
				&& value is Func<IEnumerable<IExecuteDomainCommand<TDomainCommand>>>)
			{
				var resolver = value as Func<IEnumerable<IExecuteDomainCommand<TDomainCommand>>>;

				return Task.WhenAll(resolver().Select(h => h.ExecuteAsync(command)));
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TDomainCommand)));
			}
		}

		public Task<TResult> Execute<TDomainCommand, TResult>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand<TResult>
		{
			object value;
			if (_handlers.TryGetValue(typeof(TDomainCommand), out value)
				&& value is Func<IExecuteDomainCommand<TDomainCommand, TResult>>)
			{
				var resolver = value as Func<IExecuteDomainCommand<TDomainCommand, TResult>>;

				return resolver().ExecuteAsync(command);
			}
			else
			{
				throw new InvalidOperationException(
					String.Format("There is no handler registered for {0}", typeof(TDomainCommand)));
			}
		}

		public void Register<TDomainCommand>(Func<IEnumerable<IExecuteDomainCommand<TDomainCommand>>> resolveHandlers)
			where TDomainCommand : class, IDomainCommand
		{
			_handlers.AddOrUpdate(typeof(TDomainCommand), resolveHandlers, (t, h) => resolveHandlers);
		}

		public void Register<TDomainCommand, TResult>(Func<IExecuteDomainCommand<TDomainCommand, TResult>> resolveHandler)
			where TDomainCommand : class, IDomainCommand<TResult>
		{
			_handlers.AddOrUpdate(typeof(TDomainCommand), resolveHandler, (t, h) => resolveHandler);
		}
	}
}