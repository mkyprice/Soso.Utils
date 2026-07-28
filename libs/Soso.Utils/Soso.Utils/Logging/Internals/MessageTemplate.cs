namespace Soso.Utils.Logging.Internals;

internal readonly struct MessageTemplate
{
    public readonly char[] Message;
    public readonly MessageToken[] Tokens;

    public MessageTemplate(char[] message, MessageToken[] tokens)
    {
        Message = message;
        Tokens = tokens;
    }
}