using NUnit.Framework;
using Core;
using Hexity.Engines;

namespace HexityTester
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
			IHexityCSVParser parse = new HexityCSVParser();

			ObjectPool eng = parse.ReadCSVForObjectPool("/Users/jakesutton/Desktop/Temp.csv",
			                                             new string[]{"Name", "Type"});

			Assert.True(eng.HasProperty("Name"));
			Assert.True(eng.HasProperty("Type"));

			Assert.False(eng.HasProperty("Company"));

			Assert.True(eng.GetObjects().Count == 9);
			Assert.True(eng.Contains("Cardinal Slant-D® Ring Binder, Heavy Gauge Vinyl"));
		}
	}
}

