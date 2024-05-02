using ASP.Core.Logging;

namespace ASP.Core
{
    public interface IProblemDetails<T>
    {
        T Create(ProblemDetails problemDetails);
    }
}
