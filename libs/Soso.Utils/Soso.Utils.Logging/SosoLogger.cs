using Soso.Utils.Logging;
using Soso.Utils.Logging.Internals;
using Soso.Utils.Logging.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soso.Utils.Logging.Extensions;

public class SosoLogger<T>
    where T : unmanaged, Enum
{
    public T ActiveChannels;

    public LOG_LEVEL Level = LOG_LEVEL.Debug;

    public ILogWriter Writer;

    public SosoLogger() : this(new FastConsoleLogger())
    {
    }
    
    public SosoLogger(ILogWriter writer)
    {
        Writer = writer;
    }

    public void Debug(T channel, string message, params object[]? properties)
    {
        _LogInternal(LOG_LEVEL.Debug, channel, message, properties);
    }
    
    public void Info(T channel, string message, params object[]? properties)
    {
        _LogInternal(LOG_LEVEL.Info, channel, message, properties);
    }
    
    public void Warn(T channel, string message, params object[]? properties)
    {
        _LogInternal(LOG_LEVEL.Warn, channel, message, properties);
    }
    
    public void Error(T channel, string message, params object[]? properties)
    {
        _LogInternal(LOG_LEVEL.Error, channel, message, properties);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEnabled(LOG_LEVEL level, T channel) => (level >= Level && (ActiveChannels.HasFlagNoAlloc(channel)));


    private readonly MessageProcessor _processor = new MessageProcessor(100);
    private readonly ConcurrentDictionary<string, MessageTemplate> _messageCache = new ConcurrentDictionary<string, MessageTemplate>();
    
    private void _LogInternal(LOG_LEVEL level, T channel, string messageTemplate, params object[]? properties)
    {
        if (IsEnabled(level, channel) == false)
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