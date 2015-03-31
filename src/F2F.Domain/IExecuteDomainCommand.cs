using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbc = System.Diagnostics.Contracts;

namespace F2F.Domain
{
	[dbc.ContractClass(typeof(IExecuteDomainCommandContract<>))]
	public interface IExecuteDomainCommand<TDomainCommand>
		where TDomainCommand : IDomainCommand
	{
		Task ExecuteAsync(TDomainCommand command);
	}

	[dbc.ContractClass(typeof(IExecuteDomainCommandContract<,>))]
	public interface IExecuteDomainCommand<TDomainCommand, TResult>
		where TDomainCommand : IDomainCommand<TResult>
	{
		Task<TResult> ExecuteAsync(TDomainCommand command);
	}

#pragma warning disable 0067  // suppress warning CS0067 "unused event" in contract classes

	/// <summary>Contract for <see cref="IExecuteDomainCommand<>"/></summary>
	[dbc.ContractClassFor(typeof(IExecuteDomainCommand<>))]
	internal abstract class IExecuteDomainCommandContract<TDomainCommand> : IExecuteDomainCommand<TDomainCommand>
		where TDomainCommand : IDomainCommand
	{
		public Task ExecuteAsync(TDomainCommand command)
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task);
		}
	}

	/// <summary>Contract for <see cref="IExecuteDomainCommand<,>"/></summary>
	[dbc.ContractClassFor(typeof(IExecuteDomainCommand<,>))]
	internal abstract class IExecuteDomainCommandContract<TDomainCommand, TResult> : IExecuteDomainCommand<TDomainCommand, TResult>
		where TDomainCommand : IDomainCommand<TResult>
	{
		public Task<TResult> ExecuteAsync(TDomainCommand command)
		{
			dbc.Contract.Requires<ArgumentNullException>(command != null, "command is null");

			return default(Task<TResult>);
		}
	}

#pragma warning restore
}