using Xunit;
using Xunit.Sdk;

namespace ASP.Test.Core
{
    public static class AssertWithMessage
    {
        public static void True(bool actual, string message)
        {
            try
            {
                Assert.True(actual);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void NotNull(object? @object, string message)
        {
            try
            {
                Assert.NotNull(@object);
            }
            catch(XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void Failed(string message)
        {
            throw new XunitException(message);
        }
    }
}
