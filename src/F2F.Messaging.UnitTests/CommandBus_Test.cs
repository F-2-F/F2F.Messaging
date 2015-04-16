using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using F2F.Testing.Xunit.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace F2F.Messaging.UnitTests
{
	public class CommandBus_Test : AutoMockFeature
	{
		public class Moep : ICommand
		{
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
			var sut = Fixture.Create<CommandBus>();
			var cmd = new Moep();
			var handlers = Fixture.CreateMany<IExecuteCommand<Moep>>(handlerCount);

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
			var sut = Fixture.Create<CommandBus>();
			var cmd = new Moep();
			var handlers = Fixture.CreateMany<IExecuteCommand<Moep>>(handlerCount);
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