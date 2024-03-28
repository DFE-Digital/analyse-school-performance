using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace ASP.Test.Core
{
    public partial class AssertWithMessage
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

        public static void NotNull([NotNull] object? @object, string message)
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

        public static void Null(object? @object, string message)
        {
            try
            {
                Assert.Null(@object);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void Fail(string message)
        {
            throw new XunitException(message);
        }

        public static T IsAssignableFrom<T>(object @object, string message)
        {
            try
            {
                return Assert.IsAssignableFrom<T>(@object);
            }
            catch(XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void Equal<T>(T expected, T actual, string message)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }
    }
}
