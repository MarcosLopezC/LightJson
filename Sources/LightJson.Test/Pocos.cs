namespace LightJson.Test
{
    public class PrimitiveObject
    {
        public byte Byte;
        public short Short;
        public int Int;
        public long Long;
        public float Float;
        public double Double;
        public bool Bool;
        public string String;

        public static PrimitiveObject Instance()
        {
            return new PrimitiveObject
            {
                Byte = Randomizer.RandomByte(),
                Short = Randomizer.RandomShort(),
                Int = Randomizer.RandomInt(),
                Long = Randomizer.RandomLong(),
                Float = Randomizer.RandomFloat(),
                Double = Randomizer.RandomDouble(),
                Bool = Randomizer.RandomBool(),
                String = Randomizer.RandomString()
            };
        }
    }

    public class PrimitiveArrayObject
    {
        public byte[] Byte;
        public short[] Short;
        public int[] Int;
        public long[] Long;
        public float[] Float;
        public double[] Double;
        public bool[] Bool;
        public string[] String;

        public static PrimitiveArrayObject Instance()
        {
            return new PrimitiveArrayObject
            {
                Byte = Randomizer.RandomArray(Randomizer.RandomByte),
                Short = Randomizer.RandomArray(Randomizer.RandomShort),
                Int = Randomizer.RandomArray(Randomizer.RandomInt),
                Long = Randomizer.RandomArray(Randomizer.RandomLong),
                Double = Randomizer.RandomArray(Randomizer.RandomDouble),
                Bool = Randomizer.RandomArray(Randomizer.RandomBool),
                String = Randomizer.RandomArray(Randomizer.RandomString)
            };
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
    }
}