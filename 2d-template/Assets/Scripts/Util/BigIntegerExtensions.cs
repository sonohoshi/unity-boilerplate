using System.Numerics;

namespace Util
{
    public static class BigIntegerExtensions
    {
        public static string ToCurrencyNotation(this BigInteger num)
        {
            // maxUnit is 1e9 (billions)
            // maxRemainder is 1e7 (millions)
            var maxUnit = (BigInteger)1e9;
            var maxRemainder = (BigInteger)1e7;

            var divided = BigInteger.Divide(num, maxUnit);
            if (divided > 1)
            {
                var remainder = BigInteger.Remainder(num, maxUnit);
                var scaledRemainder = BigInteger.Divide(remainder, maxRemainder);

                var remainderString = string.Empty;
                if (scaledRemainder > 0)
                {
                    remainderString = $"{scaledRemainder:D2}".TrimEnd('0');
                    remainderString = $".{remainderString}B";
                }

                return $"{divided}{remainderString}";
            }

            return ToCurrencyNotation((decimal)num);
        }

        public static string ToCurrencyNotation(this long num)
        {
            return ToCurrencyNotation((decimal)num);
        }

        public static string ToCurrencyNotation(this int num)
        {
            return ToCurrencyNotation((decimal)num);
        }
        
        public static string ToCurrencyNotation(this decimal value)
        {
            string[] suffixes = { "", "K", "M", "B" };

            var suffixIndex = 0;
            while (value >= 1000 && suffixIndex < suffixes.Length)
            {
                value /= 1000;
                suffixIndex++;
            }

            return $"{$"{value:N2}".TrimEnd('0').TrimEnd('.')}{suffixes[suffixIndex]}";
        }
    }
}