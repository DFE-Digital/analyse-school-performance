namespace ASP.Core.Logging
{
    public interface IProblemDetails<T>
    {
        T Create(ProblemDetails problemDetails);
    }
}
