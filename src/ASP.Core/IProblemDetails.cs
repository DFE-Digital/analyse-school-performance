using ASP.Core.Exceptions;

namespace ASP.Core
{
    public interface IProblemDetails<T>
    {
        T Create(ProblemDetails problemDetails);
    }
}
