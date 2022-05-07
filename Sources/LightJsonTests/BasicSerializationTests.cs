using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightJson;
using LightJson.Serialization;
using System.IO;

namespace LightJsonTests
{
	[TestClass]
	public class BasicSerializationTests
	{
		[TestMethod]
		public void ParseExampleMessage()
		{
			var message = @"
				{
					""menu"": [
						""home"",
						""projects"",
						""about""
					]
				}
			";

			var json = JsonValue.Parse(message);

			Assert.IsTrue(json.IsJsonObject);

			Assert.AreEqual(1, json.AsJsonObject.Count);
			Assert.IsTrue(json.AsJsonObject.ContainsKey("menu"));

			var items = json.AsJsonObject["menu"].AsJsonArray;

			Assert.IsNotNull(items);
			Assert.AreEqual(3, items.Count);
			Assert.IsTrue(items.Contains("home"));
			Assert.IsTrue(items.Contains("projects"));
			Assert.IsTrue(items.Contains("about"));
		}

		[TestMethod]
		public void SerializeExampleMessage()
		{
			var json = new JsonObject
			{
				["menu"] = new JsonArray
				{
					"home",
					"projects",
					"about",
				}
			};

			var message = json.ToString();

			var expectedMessage = @"{""menu"":[""home"",""projects"",""about""]}";

			Assert.AreEqual(expectedMessage, message);
		}

		[TestMethod]
		public void ParseReadOnlyFile()
		{
			// Serialize an object to a file and make it read-only
			var json = new JsonObject();
			var message = json.ToString();
			var filename = Path.GetTempFileName();
			File.WriteAllText(filename, message);
			File.SetAttributes(filename, FileAttributes.ReadOnly);

			// Make sure that we don't get an exception when parsing it
			JsonReader.ParseFile(filename);

			// Clean up
			File.SetAttributes(filename, FileAttributes.Normal);
			File.Delete(filename);
		}
	}
}
