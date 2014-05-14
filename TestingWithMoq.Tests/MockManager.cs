using Moq;
using System;
using System.Collections.Generic;

namespace TestingWithMoq.Tests
{
	public sealed class MockManager
	{
		private readonly List<Mock> Mocks;

		private MockManager()
		{
			this.Mocks = new List<Mock>();
		}

		public Mock<T> GetMock<T>(MockBehavior mockBehavior = MockBehavior.Strict)
			where T : class
		{
			var mock = new Mock<T>(mockBehavior);
			this.Mocks.Add(mock);

			return mock;
		}

		public static void Test(Action<MockManager> testAction)
		{
			if (testAction == null)
			{
				throw new ArgumentNullException("testAction");
			}

			var mockManager = new MockManager();
			testAction(mockManager);
			mockManager.VerifyAllMocks();
		}

		private void VerifyAllMocks()
		{
			foreach (var mock in this.Mocks)
			{
				mock.VerifyAll();
			}
		}
	}
}
