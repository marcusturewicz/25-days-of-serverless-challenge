using System;
using System.Security.Cryptography;

namespace Day01ServerlessDreidel
{
    public class Dreidel
    {
        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        private static int NextInt(int min, int max)
        {
            var buffer = new byte[4];

            _rng.GetBytes(buffer);
            var result = BitConverter.ToInt32(buffer, 0);

            return new Random(result).Next(min, max);
        }

        public static string Spin()
        {
            var number = NextInt(1, 5);

            string value;
            switch(number)
            {
                case 1: 
                    value = "נ"; 
                    break;
                case 2: 
                    value = "ג"; 
                    break;
                case 3: 
                    value = "ה"; 
                    break;
                case 4: 
                    value = "ש";
                    break;
                default:
                    throw new ArgumentException(nameof(number));
            }

            return value;
        }
    }
}