using System;
using Soso.Utils.Logging;

namespace Soso.Utils.Tests
{
	public class LoggingTests
	{
		[Flags]
		public enum TEST_CHANNELS
		{
			A = 1 << 0,
			B = 1 << 1,
			C = 1 << 2,
			All = A | B | C
		}
		[Test]
		public void NullTest()
		{
			SosoLogger<TEST_CHANNELS> log = new SosoLogger<TEST_CHANNELS>();
			log.Level = LOG_LEVEL.Debug;
			log.ActiveChannels = TEST_CHANNELS.All;

			Assert.DoesNotThrow(() =>
			{
				log.Info(TEST_CHANNELS.A, null);
			}, "Null test failed");
		}
		[Test]
		public void NoArgsTest()
		{
			SosoLogger<TEST_CHANNELS> log = new SosoLogger<TEST_CHANNELS>();
			log.Level = LOG_LEVEL.Debug;
			log.ActiveChannels = TEST_CHANNELS.All;

			Assert.DoesNotThrow(() =>
			{
				log.Info(TEST_CHANNELS.A, "Hello");
			}, "Test failed");
		}
		[Test]
		public void InterpolatedStringTest()
		{
			SosoLogger<TEST_CHANNELS> log = new SosoLogger<TEST_CHANNELS>();
			log.Level = LOG_LEVEL.Debug;
			log.ActiveChannels = TEST_CHANNELS.All;

			string test1 = "there{test}";
			string test2 = "there";

			Assert.DoesNotThrow(() =>
			{
				log.Info(TEST_CHANNELS.A, $"Hello {test1}\n{test2}");
			}, "Test failed");
		}
		[Test]
		public void NewlineTest()
		{
			SosoLogger<TEST_CHANNELS> log = new SosoLogger<TEST_CHANNELS>();
			log.Level = LOG_LEVEL.Debug;
			log.ActiveChannels = TEST_CHANNELS.All;

			Assert.DoesNotThrow(() =>
			{
				log.Info(TEST_CHANNELS.A, "Hello\nthere");
			}, "Test failed");
		}
	}
}
