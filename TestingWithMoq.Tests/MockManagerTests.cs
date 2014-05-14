using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestingWithMoq.Tests
{
	[TestClass]
	public sealed class MockManagerTests
	{
		[TestMethod]
		public void PassesWhenVerifyAllPasses()
		{
			MockManager.Test(mockManager =>
			{
				var mock1 = mockManager.GetMock<ITestInterface>();
				mock1.Setup(_ => _.SomeMethod()).Returns(true);

				var mock2 = mockManager.GetMock<ITestInterface>();
				mock2.Setup(_ => _.SomeMethod()).Returns(true);

				mock1.Object.SomeMethod();
				mock2.Object.SomeMethod();
			});
		}

		[TestMethod]
		public void FailsWhenVerifyAllFails()
		{
			ExceptionAssert.Throws<MockException>(() =>
			{
				MockManager.Test(mockManager =>
				{
					var mock1 = mockManager.GetMock<ITestInterface>();
					mock1.Setup(_ => _.SomeMethod()).Returns(true);

					var mock2 = mockManager.GetMock<ITestInterface>();
					mock2.Setup(_ => _.SomeMethod()).Returns(true);

					mock2.Object.SomeMethod();
				});
			});
		}

		[TestMethod]
		public void FailingShowsTheCorrectError()
		{
			ExceptionAssert.Throws<MockException>(() =>
			{
				MockManager.Test(mockManager =>
				{
					var mock1 = mockManager.GetMock<ITestInterface>();
					mock1.Setup(_ => _.SomeMethod()).Returns(true);

					var mock2 = mockManager.GetMock<ITestInterface>();
					mock2.Setup(_ => _.SomeMethod()).Returns(true);

					mock1.Object.SomeOtherMethod();
					mock2.Object.SomeMethod();
				});
			},
			(exception) =>
			{
				Assert.IsTrue(exception.Message.Contains("All invocations on the mock must have a corresponding setup."));
			});
		}

		public interface ITestInterface
		{
			bool SomeMethod();
			bool SomeOtherMethod();
		}
	}
}
