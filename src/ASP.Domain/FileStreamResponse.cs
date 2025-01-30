namespace ASP.Domain
{
    public record FileStreamResponse(string FileName, Stream Content, string ContentType);
}
