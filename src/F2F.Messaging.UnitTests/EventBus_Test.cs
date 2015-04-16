using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Xunit.FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace F2F.Messaging.UnitTests
{
	public class EventBus_Test : AutoMockFeature
	{
		public class Moep : IEvent
		{
		}

		[Fact]
		public void Execute_ShouldCallRegisteredHandlers()
		{
			// Arrange
			var sut = Fixture.Create<EventBus>();
			var msg = new Moep();
			var o = sut.ListenTo<Moep>();

			Moep receivedMsg = null;

			// Act
			using (o.Subscribe(m => receivedMsg = m))
			{
				sut.Publish(msg);
			}

			// Assert
			receivedMsg.Should().Be(msg);
		}
	}
}