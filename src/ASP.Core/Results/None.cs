namespace ASP.Core.Results
{
    public class None
    {
        internal None()
        {
        }
    }

    public class None<TValue> : Maybe<TValue> where TValue : notnull
    {
        public override Maybe<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new None<TNextValue>();
        }
    }
}