namespace ASP.Application
{
    public record FileStreamResponse(string FileName, Stream Content, string ContentType);
}
