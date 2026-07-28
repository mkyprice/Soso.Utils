using Soso.Utils.Logging.Internals;
using System;
using System.IO;

namespace Soso.Utils.Logging.Loggers
{
	public class FastConsoleLogger : ILogWriter
	{
		public void Write(LOG_LEVEL level, string template)
		{
			using var output = new StreamWriter(Console.OpenStandardOutput());
			output.WriteLine(template);
		}

		public void Write(LOG_LEVEL level, char[] template, ReadOnlySpan<MessageToken> tokens, ReadOnlySpan<object> props)
		{
			using var output = new StreamWriter(Console.OpenStandardOutput());
			foreach (var token in tokens)
			{
				if (token.PropertyIndex >= 0)
				{
					// Type type = token.Parameter.GetType();
					// Console.ForegroundColor = GetColor(type);
					// Console.Write(token.Parameter);
					
					output.Write(props[token.PropertyIndex]);
				}
				else
				{
					// Console.ForegroundColor = defaultColor;
					output.Write(template, token.Index, token.Length);
				}
			}
			output.Write(Environment.NewLine);
		}
	}
}
