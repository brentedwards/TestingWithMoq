using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestingWithMoq.Tests
{
	[TestClass]
	public sealed class ExceptionAssertTests
	{
		[TestMethod]
		public void DoNothingWhenExpectedException()
		{
			ExceptionAssert.Throws<InternalTestFailureException>(() => { throw new InternalTestFailureException(); });
		}

		[TestMethod]
		public void ExceptionOfUnexpectedTypeFails()
		{
			try
			{
				ExceptionAssert.Throws<InternalTestFailureException>(() => { throw new NotSupportedException(); });
				Assert.Fail();
			}
			catch (AssertFailedException) { }
		}

		[TestMethod]
		public void ExceptionNoException()
		{
			try
			{
				ExceptionAssert.Throws<InternalTestFailureException>(() => { return; });
				Assert.Fail();
			}
			catch (AssertFailedException) { }
		}

		[TestMethod]
		public void ExecutesActionWhenCorrectExceptionThrown()
		{
			var succeeded = false;

			ExceptionAssert.Throws<InternalTestFailureException>(
				() => { throw new InternalTestFailureException(); },
				(ex) => { succeeded = true; });

			Assert.IsTrue(succeeded);
		}

		[TestMethod]
		public void ThrowsWhenActionIsNull()
		{
			ExceptionAssert.Throws<ArgumentNullException>(
				() => ExceptionAssert.Throws<NotSupportedException>(null));
		}
	}
}
