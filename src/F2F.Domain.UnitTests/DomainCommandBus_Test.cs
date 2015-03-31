using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Xunit;
using Xunit.Extensions;

namespace F2F.Domain.UnitTests
{
	public class DomainCommandBus_Test
	{
		public class Moep : IDomainCommand
		{
		}

		private readonly IFixture Fixture;

		public DomainCommandBus_Test()
		{
			Fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
		}

		[Fact(Skip = "Dummy test")]
		public void Dummy()
		{
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public async Task Execute_ShouldCallRegisteredHandlers(int handlerCount)
		{
			// Arrange
			var sut = Fixture.Create<DomainCommandBus>();
			var cmd = new Moep();
			var handlers = Fixture.CreateMany<IExecuteDomainCommand<Moep>>(handlerCount);

			sut.Register(() => handlers);

			// Act
			await sut.Execute(cmd);

			// Assert
			handlers.ToList().ForEach(h => A.CallTo(() => h.ExecuteAsync(cmd)).MustHaveHappened());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(50)]
		public async Task Execute_ReturnsTaskWhichWaitsForFinishingAllRegisteredHandlers(int handlerCount)
		{
			// Arrange
			var sut = Fixture.Create<DomainCommandBus>();
			var cmd = new Moep();
			var handlers = Fixture.CreateMany<IExecuteDomainCommand<Moep>>(handlerCount);
			int i = 0;

			handlers
				.ToList()
				.ForEach(h =>
					A.CallTo(() => h.ExecuteAsync(cmd))
						.ReturnsLazily(() =>
								Task.Delay(10)
									.ContinueWith(_ => Interlocked.Increment(ref i))));

			sut.Register(() => handlers);

			// Act
			await sut.Execute(cmd);

			// Assert
			i.Should().Be(handlerCount);
		}
	}
}