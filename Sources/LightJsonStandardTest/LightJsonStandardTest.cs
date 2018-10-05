using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightJson;


namespace LightJsonStandardTest
{
    [TestClass]
    public class LightJsonStandardTest
    {
        [TestMethod]
        public void should_return_parsed_double()
        {
            double result = new JsonValue("0.5").AsNumber;
            Assert.AreEqual(result, 0.5);
        }

        // incompleted
    }
}
