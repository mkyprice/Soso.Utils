using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Soso.Utils.Logging.Internals;
using Soso.Utils.Logging.Loggers;

namespace Soso.Utils.Logging;

public class SosoLogger
{
    public LOG_LEVEL Level = LOG_LEVEL.Debug;

    public ILogWriter Writer;

    public SosoLogger() : this(new FastConsoleLogger())
    {
    }
    
    public SosoLogger(ILogWriter writer)
    {
        Writer = writer;
    }

    public void Debug(string message, params object[]? properties)
    {
        Log(LOG_LEVEL.Debug, message, properties);
    }
    
    public void Info(string message, params object[]? properties)
    {
        Log(LOG_LEVEL.Info, message, properties);
    }
    
    public void Warn(string message, params object[]? properties)
    {
        Log(LOG_LEVEL.Warn, message, properties);
    }
    
    public void Error(string message, params object[]? properties)
    {
        Log(LOG_LEVEL.Error, message, properties);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEnabled(LOG_LEVEL level) => level >= Level;
    
    private readonly MessageProcessor _processor = new MessageProcessor(100);
    private readonly ConcurrentDictionary<string, MessageTemplate> _messageCache = new ConcurrentDictionary<string, MessageTemplate>();
    protected void Log(LOG_LEVEL level, string messageTemplate, params object[]? properties)
    {
        if (IsEnabled(level) == false)
        {
            return;
        }

        // Special case: only string/no properties
        if (string.IsNullOrEmpty(messageTemplate) || properties?.Length == 0)
        {
            Writer.Write(level, messageTemplate);
            return;
        }
        
        ReadOnlySpan<MessageToken> tokens;
        char[] message;
        if (_messageCache.TryGetValue(messageTemplate, out var log) == false)
        {
            message = messageTemplate.ToCharArray();
            _processor.Process(messageTemplate, out tokens);
            log = new MessageTemplate(message, tokens.ToArray());
            _messageCache.TryAdd(messageTemplate, log);
        }
        else
        {
            tokens = log.Tokens;
            message = log.Message;
        }
        
        Span<object> props = properties == null ? Span<object>.Empty : properties.AsSpan();
        Writer.Write(level, message, tokens, props);
    }

}