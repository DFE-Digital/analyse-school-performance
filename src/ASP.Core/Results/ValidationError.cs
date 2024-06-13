namespace ASP.Core.Results
{
    public class ValidationError : Error
    {
        public override string ErrorType => "Invalid";

        public ValidationError(string message)
            : base(message)
        {
        }
    }
}
