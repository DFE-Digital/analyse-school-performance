namespace ASP.Core.Results
{
    public class Some<TValue> : Maybe<TValue> where TValue : notnull
    {
        public TValue Value { get; }

        public Some(TValue value)
        {
            Value = value;
        }

        public override Maybe<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new Some<TNextValue>(mapFunction(Value));
        }
    }
}