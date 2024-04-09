namespace ASP.Core.Results
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message)
            : base(message)
        {
        }
    }
}
