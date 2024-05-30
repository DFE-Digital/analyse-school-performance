namespace ASP.Core.Results
{
    public static class Maybe
    {
        private static readonly None _none = new None();
        public static None None => _none;

        public static Maybe<TValue> Some<TValue>(TValue value) where TValue : notnull
        {
            return new Some<TValue>(value);
        }
    }

    public abstract class Maybe<TValue> where TValue : notnull
    {
        public static implicit operator Maybe<TValue>(TValue value)
        {
            return new Some<TValue>(value);
        }

        public static implicit operator Maybe<TValue>(None none)
        {
            return new None<TValue>();
        }

        public static Maybe<TValue> Some(TValue value)
        {
            return new Some<TValue>(value);
        }

        public static Maybe<TValue> None => new None<TValue>();

        public TValue? ToNullable() => this switch {
            Some<TValue> some => some.Value,
            _ => default
        };

        public abstract Maybe<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction) where TNextValue : notnull;
    }
}