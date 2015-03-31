using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Domain
{
	[dbc.ContractClass(typeof(IDomainCommandBusContract))]
	public interface IDomainCommandBus
	{
		Task Execute<TDomainCommand>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand;

		Task<TResult> Execute<TDomainCommand, TResult>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand<TResult>;

		void Register<TDomainCommand>(Func<IEnumerable<IExecuteDomainCommand<TDomainCommand>>> resolveHandlers)
			where TDomainCommand : class, IDomainCommand;

		void Register<TDomainCommand, TResult>(Func<IExecuteDomainCommand<TDomainCommand, TResult>> resolveHandler)
			where TDomainCommand : class, IDomainCommand<TResult>;
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="IDomainCommandBus"/></summary>
	[dbc.ContractClassFor(typeof(IDomainCommandBus))]
	internal abstract class IDomainCommandBusContract : IDomainCommandBus
	{
		public Task Execute<TDomainCommand>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task);
		}

		public Task<TResult> Execute<TDomainCommand, TResult>(TDomainCommand command)
			where TDomainCommand : class, IDomainCommand<TResult>
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task<TResult>);
		}

		public void Register<TDomainCommand>(Func<IEnumerable<IExecuteDomainCommand<TDomainCommand>>> resolveHandlers)
			where TDomainCommand : class, IDomainCommand
		{
			dbc.Contract.Requires<ArgumentNullException>(resolveHandlers != null, "resolveHandlers is null");
		}

		public void Register<TDomainCommand, TResult>(Func<IExecuteDomainCommand<TDomainCommand, TResult>> resolveHandler)
			where TDomainCommand : class, IDomainCommand<TResult>
		{
			dbc.Contract.Requires<ArgumentNullException>(resolveHandler != null, "resolveHandler is null");
		}
	}

#pragma warning restore
}