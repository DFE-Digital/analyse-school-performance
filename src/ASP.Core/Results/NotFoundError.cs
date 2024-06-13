namespace ASP.Core.Results
{
    public class NotFoundError : Error
    {
        public override string ErrorType => "Not found";

        public NotFoundError(string message)
            : base(message)
        {
        }
    }
}
