using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using F2F.Testing.Xunit.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace F2F.Messaging.UnitTests
{
	public class CommandBus_Test : AutoMockFeature
	{
		public class DummyCommand : ICommand
		{
		}

		public class DummyCommandWithResult : ICommand<DummyEvent>
		{
		}

		public class DummyEvent : IEvent
		{
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public void Execute_ShouldCallRegisteredHandlers(int handlerCount)
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handlers = Fixture.CreateMany<IExecute<DummyCommand>>(handlerCount);
			sut.RegisterHandlers(_ => handlers);

			var cmd = Fixture.Create<DummyCommand>();

			// Act
			sut.Execute(cmd);

			// Assert
			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustNotHaveHappened());

			scheduler.AdvanceBy(1);

			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustHaveHappened());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public void Execute_ShouldCallRegisteredAsyncHandlers(int handlerCount)
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handlers = Fixture.CreateMany<IExecuteAsync<DummyCommand>>(handlerCount);
			sut.RegisterAsyncHandlers(_ => handlers);

			var cmd = Fixture.Create<DummyCommand>();

			// Act
			sut.Execute(cmd);

			// Assert
			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustNotHaveHappened());

			scheduler.AdvanceBy(1);

			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustHaveHappened());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public void Execute_ShouldReturnTaskWhichWaitsForFinishingAllRegisteredSynchronousHandlers(int handlerCount)
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handlers = Fixture.CreateMany<IExecute<DummyCommand>>(handlerCount);
			sut.RegisterHandlers(_ => handlers);

			var cmd = Fixture.Create<DummyCommand>();

			handlers
				.ToList()
				.ForEach(h => A.CallTo(() => h.Execute(cmd)).Invokes(() => Task.Delay(1)));

			// Act
			var t = sut.Execute(cmd);

			// Assert
			t.IsCompleted.Should().BeFalse();

			scheduler.AdvanceBy(1);

			t.IsCompleted.Should().BeTrue();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public void Execute_ShouldReturnTaskWhichWaitsForFinishingAllRegisteredAsyncHandlers(int handlerCount)
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handlers = Fixture.CreateMany<IExecuteAsync<DummyCommand>>(handlerCount);
			sut.RegisterAsyncHandlers(_ => handlers);

			var cmd = Fixture.Create<DummyCommand>();

			handlers
				.ToList()
				.ForEach(h => A.CallTo(() => h.Execute(cmd)).Invokes(() => Task.Delay(1)));

			// Act
			var t = sut.Execute(cmd);

			// Assert
			t.IsCompleted.Should().BeFalse();

			scheduler.AdvanceBy(1);

			t.IsCompleted.Should().BeTrue();

			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustHaveHappened());
		}

		private class AsyncDummyHandler : IExecuteAsync<DummyCommand>
		{
			public async Task Execute(DummyCommand command)
			{
				await Task.Delay(1).ConfigureAwait(false);
				await Task.Delay(1).ConfigureAwait(false);
				await Task.Delay(1).ConfigureAwait(false);

				Finished = true;
			}

			public bool Finished { get; set; }
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public async Task Execute_ShouldReturnTaskWhichWaitsForFinishingAllRegisteredAsyncHandlers2(int handlerCount)
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handlers = Fixture.CreateMany<AsyncDummyHandler>(handlerCount);
			sut.RegisterAsyncHandlers(_ => handlers);

			var cmd = Fixture.Create<DummyCommand>();

			// Act
			var t = sut.Execute(cmd);

			t.IsCompleted.Should().BeFalse();

			scheduler.AdvanceBy(5);

			await t;
			
			t.IsCompleted.Should().BeTrue();

			handlers.All(h => h.Finished);
		}


		[Fact]
		public void Execute_CommandWithResult_ShouldCallRegisteredHandler()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecute<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();

			// Act
			sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			// Assert
			A.CallTo(() => handler.Execute(cmd)).MustNotHaveHappened();

			scheduler.AdvanceBy(1);

			A.CallTo(() => handler.Execute(cmd)).MustHaveHappened();
		}

		[Fact]
		public void Execute_CommandWithResult_ShouldCallRegisteredAsyncHandler()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecuteAsync<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterAsyncHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();

			// Act
			sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			// Assert
			A.CallTo(() => handler.Execute(cmd)).MustNotHaveHappened();

			scheduler.AdvanceBy(1);

			A.CallTo(() => handler.Execute(cmd)).MustHaveHappened();
		}

		[Fact]
		public void Execute_CommandWithResult_ShouldReturnTaskWhichWaitsForEndOfRegisteredHandler()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecute<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();

			A.CallTo(() => handler.Execute(cmd)).Invokes(() => Task.Delay(1));

			// Act
			var t = sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			// Assert
			t.IsCompleted.Should().BeFalse();

			scheduler.AdvanceBy(1);

			t.IsCompleted.Should().BeTrue();
		}


		[Fact]
		public void Execute_CommandWithResult_ShouldReturnTaskWhichWaitsForEndOfRegisteredAsyncHandler()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecuteAsync<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterAsyncHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();

			A.CallTo(() => handler.Execute(cmd)).Invokes(() => Task.Delay(1));

			// Act
			var t = sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			// Assert
			t.IsCompleted.Should().BeFalse();

			scheduler.AdvanceBy(1);

			t.IsCompleted.Should().BeTrue();
		}

		[Fact]
		public void Execute_CommandWithResult_ShouldReturnExpectedResult()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecute<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();
			var ev = Fixture.Create<DummyEvent>();

			A.CallTo(() => handler.Execute(cmd)).Returns(ev);

			// Act
			var t = sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			scheduler.AdvanceBy(1);

			// Assert
			t.Result.Should().Be(ev);
		}


		[Fact]
		public void Execute_AsyncCommandWithResult_ShouldReturnExpectedResult()
		{
			// Arrange
			var scheduler = new TestScheduler();
			Fixture.Inject<IScheduler>(scheduler);

			var sut = Fixture.Create<CommandBus>();

			var handler = Fixture.Create<IExecuteAsync<DummyCommandWithResult, DummyEvent>>();
			sut.RegisterAsyncHandler((_, __) => handler);

			var cmd = Fixture.Create<DummyCommandWithResult>();
			var ev = Fixture.Create<DummyEvent>();

			A.CallTo(() => handler.Execute(cmd)).Returns(ev);

			// Act
			var t = sut.Execute<DummyCommandWithResult, DummyEvent>(cmd);

			scheduler.AdvanceBy(1);

			// Assert
			t.Result.Should().Be(ev);
		}
	}
}