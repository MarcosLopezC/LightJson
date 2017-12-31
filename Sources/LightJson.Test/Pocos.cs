using System;

namespace LightJson.Test
{
    public class PrimitiveObject
    {
        public byte Byte;
        public short Short;
        public int Int;
        public float Float;
        public bool Bool;
        public string String;

        public static PrimitiveObject Instance()
        {
            return new PrimitiveObject
            {
                Byte = Randomizer.RandomByte(),
                Short = Randomizer.RandomShort(),
                Int = Randomizer.RandomInt(),
                Float = Randomizer.RandomFloat(),
                Bool = Randomizer.RandomBool(),
                String = Randomizer.RandomString()
            };
        }

        public static bool AreEqual(PrimitiveObject lhs, PrimitiveObject rhs)
        {
            return lhs.Byte == rhs.Byte
                   && lhs.Short == rhs.Short
                   && lhs.Int == rhs.Int
                   && Math.Abs(lhs.Float - rhs.Float) < float.Epsilon
                   && lhs.Bool == rhs.Bool
                   && lhs.String == rhs.String;
        }
    }

    public class PrimitiveArrayObject
    {
        public byte[] Byte;
        public short[] Short;
        public int[] Int;
        public float[] Float;
        public bool[] Bool;
        public string[] String;

        public static PrimitiveArrayObject Instance()
        {
            return new PrimitiveArrayObject
            {
                Byte = Randomizer.RandomArray(Randomizer.RandomByte),
                Short = Randomizer.RandomArray(Randomizer.RandomShort),
                Int = Randomizer.RandomArray(Randomizer.RandomInt),
                Float = Randomizer.RandomArray(Randomizer.RandomFloat),
                Bool = Randomizer.RandomArray(Randomizer.RandomBool),
                String = Randomizer.RandomArray(Randomizer.RandomString)
            };
        }

        public static bool AreEqual(PrimitiveArrayObject lhs, PrimitiveArrayObject rhs)
        {
            return AreEqual(lhs.Byte, rhs.Byte, (a, b) => a == b)
                && AreEqual(lhs.Short, rhs.Short, (a, b) => a == b)
                && AreEqual(lhs.Int, rhs.Int, (a, b) => a == b)
                && AreEqual(lhs.Float, rhs.Float, (a, b) => Math.Abs(a - b) < float.Epsilon)
                && AreEqual(lhs.Bool, rhs.Bool, (a, b) => a == b)
                && AreEqual(lhs.String, rhs.String, (a, b) => a == b);
        }

        private static bool AreEqual<T>(T[] a, T[] b, Func<T, T, bool> comparator)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0, len = a.Length; i < len; i++)
            {
                if (!comparator(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class CompositeObject
    {
        public PrimitiveObject Primitive;
        public PrimitiveArrayObject Array;

        public static CompositeObject Instance()
        {
            return new CompositeObject
            {
                Primitive = PrimitiveObject.Instance(),
                Array = PrimitiveArrayObject.Instance()
            };
        }

        public static bool AreEqual(CompositeObject lhs, CompositeObject rhs)
        {
            return PrimitiveObject.AreEqual(lhs.Primitive, rhs.Primitive)
                   && PrimitiveArrayObject.AreEqual(lhs.Array, rhs.Array);
        }
    }

    public class CompositeArrayObject
    {
        public CompositeObject[] Composites;

        public static CompositeArrayObject Instance()
        {
            return new CompositeArrayObject
            {
                Composites = Randomizer.RandomArray(CompositeObject.Instance)
            };
        }

        public static bool AreEqual(CompositeArrayObject lhs, CompositeArrayObject rhs)
        {
            var a = lhs.Composites;
            var b = rhs.Composites;

            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0, len = a.Length; i < len; i++)
            {
                if (!CompositeObject.AreEqual(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class PrimitiveNamed
    {
        [JsonName("bar")]
        public int Foo;
    }

    public class PrimitiveIgnored
    {
        public int Foo;

        [JsonIgnore]
        public float Bar;
    }
}