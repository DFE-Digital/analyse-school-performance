namespace ASP.Core.Results
{
    public class NotAllowedError : Error
    {
        public override string ErrorType => "Not allowed";

        public NotAllowedError(string message)
            : base(message)
        {
        }
    }
}
