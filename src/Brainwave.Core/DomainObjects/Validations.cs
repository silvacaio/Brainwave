using System.Text.RegularExpressions;

namespace Brainwave.Core.DomainObjects
{
    public class Validations
    {
        public static void ValidateIfEqual(object object1, object object2, string message)
        {
            if (object1.Equals(object2))
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfDifferent(object object1, object object2, string message)
        {
            if (!object1.Equals(object2))
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfNotMatch(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(value))
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateMaxLength(string value, int maxLength, string message)
        {
            var length = value.Trim().Length;
            if (length > maxLength)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateLength(string value, int minLength, int maxLength, string message)
        {
            var length = value.Trim().Length;
            if (length < minLength || length > maxLength)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfEmpty(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateRange(double value, double min, double max, string message)
        {
            if (value < min || value > max)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateRange(float value, float min, float max, string message)
        {
            if (value < min || value > max)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateRange(int value, int min, int max, string message)
        {
            if (value < min || value > max)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateRange(long value, long min, long max, string message)
        {
            if (value < min || value > max)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateRange(decimal value, decimal min, decimal max, string message)
        {
            if (value < min || value > max)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfLessThan(long value, long min, string message)
        {
            if (value < min)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfLessThan(double value, double min, string message)
        {
            if (value < min)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfLessThan(decimal value, decimal min, string message)
        {
            if (value < min)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfLessThan(int value, int min, string message)
        {
            if (value < min)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfFalse(bool boolValue, string message)
        {
            if (!boolValue)
            {
                throw new DomainException(message);
            }
        }

        public static void ValidateIfTrue(bool boolValue, string message)
        {
            if (boolValue)
            {
                throw new DomainException(message);
            }
        }
    }

}
