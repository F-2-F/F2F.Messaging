# F2F.Messaging

F2F.Messaging is a .NET Standard 1.3 compatible library which provides asynchronous messaging via Command Bus and Event Bus based on [Reactive Extensions](https://rx.codeplex.com/).

Builds are available via NuGet:
- [F2F.Messaging](http://www.nuget.org/packages/F2F.Messaging/)

## Basic interfaces ##

The **CommandBus** executes commands asynchronously at registered command handlers.

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

A command handler gets executed asynchronously and is scheduled by a given **IScheduler** at **CommandBus**.

```csharp
public interface IExecuteCommand<TCommand>
	where TCommand : ICommand
{
	void Execute(TCommand command);
}

public interface IExecuteCommand<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	TResult Execute(TCommand command);
}
```

The **EventBus** provides publish / subscribe mechanism for messages based on [Rx](https://rx.codeplex.com/).

```csharp
public interface IEventBus
{
	void Publish<TEvent>(TEvent message)
		where TEvent : IEvent;

	IObservable<TEvent> ListenTo<TEvent>()
		where TEvent : IEvent;
}
```
