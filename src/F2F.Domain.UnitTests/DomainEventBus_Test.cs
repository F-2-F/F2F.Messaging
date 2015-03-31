using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Xunit;

namespace F2F.Domain.UnitTests
{
	public class DomainEventBus_Test
	{
		public class Moep : IDomainEvent
		{
		}

		private readonly IFixture Fixture;

		public DomainEventBus_Test()
		{
			Fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
		}

		[Fact]
		public void Execute_ShouldCallRegisteredHandlers()
		{
			// Arrange
			var sut = Fixture.Create<DomainEventBus>();
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