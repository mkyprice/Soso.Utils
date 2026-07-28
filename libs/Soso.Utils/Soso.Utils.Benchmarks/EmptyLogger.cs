using Soso.Utils.Logging;
using Soso.Utils.Logging.Internals;

namespace Soso.Utils.Benchmarks;

public class EmptyLogger : ILogWriter
{
    public void Write(LOG_LEVEL level, string template)
    {
        
    }

    public void Write(LOG_LEVEL level, char[] template, ReadOnlySpan<MessageToken> tokens, ReadOnlySpan<object> props)
    {
        foreach (var token in tokens)
        {
            if (token.PropertyIndex >= 0)
            {
                var prop = props[token.PropertyIndex];
            }
        }
    }
}