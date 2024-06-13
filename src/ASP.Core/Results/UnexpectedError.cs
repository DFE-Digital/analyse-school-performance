namespace ASP.Core.Results
{
    public class UnexpectedError : Error
    {
        public override string ErrorType => "Unexpected";

        public UnexpectedError(string message)
            : base(message)
        {
        }
    }
}
