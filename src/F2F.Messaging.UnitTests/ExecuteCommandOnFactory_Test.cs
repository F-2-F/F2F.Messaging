using System;
using System.Collections.Generic;
using System.Linq;
using F2F.Testing.Xunit.FakeItEasy;
using FakeItEasy;
using Microsoft.Reactive.Testing;
using Ploeh.AutoFixture;
using Xunit;

namespace F2F.Messaging.UnitTests
{
	public class ExecuteCommandOnFactory_Test : AutoMockFeature
	{
		public class Moep : ICommand
		{
		}

		[Fact]
		public void Execute_ShouldCallRegisteredHandler()
		{
			// Arrange
			var sut = Fixture.Create<CommandBus>();
			var cmd = new Moep();
			var scheduler = new TestScheduler();
			Fixture.Inject<IStartTask>(new OnScheduler(scheduler));
			var handlers = Fixture.CreateMany<ExecuteCommandOnFactory<Moep>>(1);

			sut.Register(() => handlers);

			// Act
			sut.Execute(cmd);

			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustNotHaveHappened());

			scheduler.AdvanceBy(1);

			// Assert
			handlers.ToList().ForEach(h => A.CallTo(() => h.Execute(cmd)).MustHaveHappened());
		}
	}
}