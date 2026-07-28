using Soso.Utils.Logging.Internals;
using System;

namespace Soso.Utils.Logging
{
	public interface ILogWriter
	{
		public void Write(LOG_LEVEL level, string template);
		public void Write(LOG_LEVEL level, char[] template, ReadOnlySpan<MessageToken> tokens, ReadOnlySpan<object> props);
	}
}
