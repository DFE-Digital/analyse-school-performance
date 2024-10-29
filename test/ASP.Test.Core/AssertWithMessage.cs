using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Xunit
{
    public partial class Assert
    {
        public static void NotNull([NotNull] object? @object, string message)
        {
            try
            {
                NotNull(@object);
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
                Null(@object);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void NotEmpty([NotNull] string @string, string message)
        {
            try
            {
                NotEmpty(@string);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void Empty(string @string, string message)
        {
            try
            {
                Empty(@string);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static T IsAssignableFrom<T>(object @object, string message)
        {
            try
            {
                return IsAssignableFrom<T>(@object);
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
                Equal(expected, actual);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }

        public static void NotEqual<T>(T expected, T actual, string message)
        {
            try
            {
                NotEqual(expected, actual);
            }
            catch (XunitException)
            {
                throw new XunitException(message);
            }
        }
    }
}
