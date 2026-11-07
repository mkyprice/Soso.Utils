using System;
using System.Runtime.CompilerServices;
using Soso.Utils.Logging.Extensions;
using Soso.Utils.Logging.Loggers;

namespace Soso.Utils.Logging;

public class SosoLogger<T> : SosoLogger
    where T : unmanaged, Enum
{
    public T ActiveChannels;
    public SosoLogger() : this(new FastConsoleLogger())
    {
    }
    public SosoLogger(ILogWriter writer) : base(writer)
    {
    }

    public void Debug(T channel, string message, params object[]? properties)
    {
        if (IsEnabled(LOG_LEVEL.Debug, channel) == false) return;
        Log(LOG_LEVEL.Debug, message, properties);
    }
    
    public void Info(T channel, string message, params object[]? properties)
    {
        if (IsEnabled(LOG_LEVEL.Info, channel) == false) return;
        Log(LOG_LEVEL.Info, message, properties);
    }
    
    public void Warn(T channel, string message, params object[]? properties)
    {
        if (IsEnabled(LOG_LEVEL.Warn, channel) == false) return;
        Log(LOG_LEVEL.Warn, message, properties);
    }
    
    public void Error(T channel, string message, params object[]? properties)
    {
        if (IsEnabled(LOG_LEVEL.Error, channel) == false) return;
        Log(LOG_LEVEL.Error, message, properties);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEnabled(LOG_LEVEL level, T channel) => (level >= Level && (ActiveChannels.HasFlagNoAlloc(channel)));
}