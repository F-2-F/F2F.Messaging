# F2F.Messaging

**Please note that this library is still under development!**

Builds are available via NuGet as prereleases:
- [F2F.Messaging](http://www.nuget.org/packages/F2F.Messaging/)

Asynchronous messaging via Command Bus and Event Bus based on [Reactive Extensions](https://rx.codeplex.com/).

## Basic interfaces ##

```csharp
public interface ICommandBus
{
	Task Execute<TCommand>(TCommand command)
		where TCommand : class, ICommand;

	Task<TResult> Execute<TCommand, TResult>(TCommand command)
		where TCommand : class, ICommand<TResult>;

	void Register<TCommand>(Func<IEnumerable<IExecuteCommand<TCommand>>> resolveHandlers)
		where TCommand : class, ICommand;

	void Register<TCommand, TResult>(Func<IExecuteCommand<TCommand, TResult>> resolveHandler)
		where TCommand : class, ICommand<TResult>;
}
```

```csharp
public interface IEventBus
{
	void Publish<TEvent>(TEvent message)
		where TEvent : IEvent;

	IObservable<TEvent> ListenTo<TEvent>()
		where TEvent : IEvent;
}
```
