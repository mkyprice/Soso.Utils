using System;

namespace Soso.Utils.Logging.Internals;

internal readonly struct MessageProcessor(int capacity)
{
    private readonly MessageToken[] _tokenCache = new MessageToken[capacity];

    public void Process(string messageTemplate, out ReadOnlySpan<MessageToken> tokens)
    {
        if (string.IsNullOrEmpty(messageTemplate))
        {
            _tokenCache[0] = new MessageToken(0, 0, -1);
            tokens = new ReadOnlySpan<MessageToken>(_tokenCache, 0, 1);
            return;
        }
        int nextTokenIndex = 0;
        int previousTokenIndex = 0;
        int cacheIndex = 0;
        for (int i = 0; i < messageTemplate.Length; i++)
        {
            if (messageTemplate[i] == '{')
            {
                if (i > 0 && messageTemplate[i - 1] != '}')
                {
                    _tokenCache[cacheIndex] = new MessageToken(previousTokenIndex, i - previousTokenIndex, -1);
                    cacheIndex++;
                }
                
                int j = i + 1;
                bool found = false;
                for (; j < messageTemplate.Length; j++)
                {
                    if (messageTemplate[j] == '}')
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    throw new FormatException($"Invalid message template '{messageTemplate[i]}'");
                }
                
                int length = j - i + 1;
                i = j + 1;
                previousTokenIndex = i;
                _tokenCache[cacheIndex] = new MessageToken(i, length, nextTokenIndex);
                cacheIndex++;
                nextTokenIndex++;
            }
        }

        if (messageTemplate.Length > 0)
        {
            _tokenCache[cacheIndex] = new MessageToken(previousTokenIndex, messageTemplate.Length - previousTokenIndex, -1);
            cacheIndex++;
        }
        tokens = new Span<MessageToken>(_tokenCache, 0, cacheIndex);
    }
}