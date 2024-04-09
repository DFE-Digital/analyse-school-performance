namespace ASP.Core.Results
{
    public class UnexpectedError : Error
    {
        public UnexpectedError(string message)
            : base(message)
        {
        }
    }
}
