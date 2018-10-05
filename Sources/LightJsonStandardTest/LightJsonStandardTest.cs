using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightJson;


namespace LightJsonStandardTest
{
    [TestClass]
    public class LightJsonStandardTest
    {
        [TestMethod]
        public void AsNumber_should_return_parsed_double()
        {
            double result = new JsonValue("0.5").AsNumber;
            Assert.AreEqual(result, 0.5);
        }


        [TestMethod]
        public void AsString_should_return_equal_string()
        {
            string result = new JsonValue("0.5").AsString;
            Assert.AreEqual(result, "0.5");
        }

        [TestMethod]
        public void Desrialzie_JsonObject()
        {
            string deserilizationReuslt = new JsonObject()
                .Add("menu", new JsonArray()
                .Add("home")
                .Add("projects")
                .Add("about")).ToString(true).Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Replace(" ", "");


            string expectedResult = "{\"menu\": [  \"home\", \"projects\", \"about\" ] }".Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Replace(" ", "");


            Assert.AreEqual(deserilizationReuslt, expectedResult);
        }

        // incompleted
    }
}
