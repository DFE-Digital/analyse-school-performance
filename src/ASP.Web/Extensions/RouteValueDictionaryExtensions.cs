namespace ASP.Web.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary Merge(this RouteValueDictionary values, RouteValueDictionary other)
        {
            var newValues = new RouteValueDictionary(values);

            foreach ((var key, var value) in other)
            {
                newValues[key] = value;
            }

            return newValues;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary values, object other)
        {
            return values.Merge(new RouteValueDictionary(other));
        }
    }
}