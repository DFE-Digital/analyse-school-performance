namespace ASP.Core.Network
{
    public record FileStreamResponse(string FileName, Stream Content, string ContentType);
}
