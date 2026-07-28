using Soso.Utils.Logging.Internals;
using System;
using System.Text;

namespace Soso.Utils.Logging.Loggers
{
	public class PrettyConsoleLogger : ILogWriter
	{
		public void Write(LOG_LEVEL level, string template)
		{
			var defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = defaultColor;
			Console.Write(template);
		}

		public void Write(LOG_LEVEL level, char[] template, ReadOnlySpan<MessageToken> tokens, ReadOnlySpan<object> props)
		{
			var defaultColor = Console.ForegroundColor;
			foreach (var token in tokens)
			{
				if (token.PropertyIndex >= 0)
				{
					var prop = props[token.PropertyIndex];
					if (prop != null)
					{
						Type type = prop.GetType();
						Console.ForegroundColor = GetColor(type);
						Console.Write(prop);
					}
				}
				else
				{
					Console.ForegroundColor = defaultColor;
					Console.Write(template, token.Index, token.Length);
				}
			}
			Console.Write(Environment.NewLine);
			// Console.ResetColor();
		}
		
		
		private ConsoleColor GetColor(Type type)
		{
			if (type.IsPrimitive)
			{
				return PRIMITIVE_COLOR;
			}
			if (type.BaseType == typeof(Exception))
			{
				return EXCEPTION_COLOR;
			}
			return DEFAULT_COLOR;
		}
		private const ConsoleColor DEFAULT_COLOR = ConsoleColor.Green;
		private const ConsoleColor EXCEPTION_COLOR = ConsoleColor.DarkMagenta;
		private const ConsoleColor PRIMITIVE_COLOR = ConsoleColor.Cyan;
	}
}
