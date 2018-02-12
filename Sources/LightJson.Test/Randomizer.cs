using System;
using System.Collections.Generic;

namespace LightJson.Test
{
    public static class Randomizer
    {
        private static readonly Random _random = new Random(12345);

        public static T[] RandomArray<T>(Func<T> factory)
        {
            var len = _random.Next(1, 6);
            var arr = new T[len];
            for (var i = 0; i < len; i++)
            {
                arr[i] = factory();
            }

            return arr;
        }

        public static Dictionary<K, V> RandomDict<K, V>(
            Func<K> kfactory,
            Func<V> vfactory)
        {
            var len = _random.Next(1, 6);
            var dict = new Dictionary<K, V>();
            while (len-- > 0)
            {
                dict[kfactory()] = vfactory();
            }

            return dict;
        }

        public static long RandomLong()
        {
            var buf = new byte[8];
            _random.NextBytes(buf);

            return BitConverter.ToInt64(buf, 0);
        }

        public static float RandomFloat()
        {
            var buf = new byte[4];
            _random.NextBytes(buf);

            return BitConverter.ToSingle(buf, 0);
        }

        public static double RandomDouble()
        {
            var buf = new byte[8];
            _random.NextBytes(buf);

            return BitConverter.ToDouble(buf, 0);
        }

        public static string RandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool RandomBool()
        {
            return _random.Next(0, 2) == 0;
        }

        public static int RandomInt()
        {
            return _random.Next(int.MinValue, int.MaxValue);
        }

        public static short RandomShort()
        {
            return (short) _random.Next(short.MinValue, short.MaxValue);
        }

        public static byte RandomByte()
        {
            return (byte) _random.Next(byte.MinValue, byte.MaxValue);
        }
    }
}