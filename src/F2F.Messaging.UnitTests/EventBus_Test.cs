using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using F2F.Testing.Xunit.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace F2F.Messaging.UnitTests
{
	public class EventBus_Test : AutoMockFeature
	{
		public class DummyEvent : IEvent
		{
		}

		[Fact]
		public void Publish_ShouldCallRegisteredHandlers()
		{
			// Arrange
			var sut = Fixture.Create<EventBus>();
			var msg = new DummyEvent();
			var o = sut.ListenTo<DummyEvent>();

			DummyEvent receivedMsg = null;

			// Act
			using (o.Subscribe(m => receivedMsg = m))
			{
				sut.Publish(msg);
			}

			// Assert
			receivedMsg.Should().Be(msg);
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

			var sut = Fixture.Create<EventBus>();

			var handlers = Fixture.CreateMany<IHandle<DummyEvent>>(handlerCount);
			sut.Register(() => handlers);

			var evt = Fixture.Create<DummyEvent>();

			// Act
			sut.Publish(evt);

			// Assert
			handlers.ToList().ForEach(h => A.CallTo(() => h.Handle(evt)).MustNotHaveHappened());

			scheduler.AdvanceBy(1);

			handlers.ToList().ForEach(h => A.CallTo(() => h.Handle(evt)).MustHaveHappened());
		}
	}
}