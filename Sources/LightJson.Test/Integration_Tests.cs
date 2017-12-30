using NUnit.Framework;

namespace LightJson.Test
{
    [TestFixture]
    public class Integration_Tests
    {
        [Test]
        public void Primitives()
        {
            var before = PrimitiveObject.Instance();

            var json = new JsonObject(before).ToString(true);

            var after = (PrimitiveObject) JsonValue.Parse(json).As(typeof(PrimitiveObject));

            Assert.IsTrue(PrimitiveObject.AreEqual(before, after));
        }
    }
}