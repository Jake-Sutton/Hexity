using System;
using NUnit.Framework;
using Core;
using Hexity.Engines;
using HexCommands;

namespace HexityTester
{
	[TestFixture]
	public class Test
	{
		[Test]
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

		[Test]
		public void ArgParserTestOne () 
		{
			var set1 = new ArgumentSet("-tv -f {name, address}");

			Assert.True(set1.HasFlags);
			Assert.True(set1.HasNamedMembers);
			Assert.True(set1.NamedMembers.Contains("Name") && set1.NamedMembers.Contains("Address"));
			Assert.True(set1.Flags.Contains('T') && set1.Flags.Contains('V') && set1.Flags.Contains('F'));

			var set2 = new ArgumentSet("-a -f");

			Assert.True(set2.HasFlags);
			Assert.False(set2.HasNamedMembers);
			Assert.True(set2.Flags.Contains('A') && set1.Flags.Contains('F'));
			Assert.True(set2.NamedMembers.Count == 0);
		}

		[Test]
		public void ArgParserTestTwo()
		{
			var set3 = new ArgumentSet("{nAme}");
			Assert.False(set3.HasFlags);
			Assert.True(set3.HasNamedMembers);
			Assert.True(set3.Flags.Count == 0);
			Assert.True(set3.NamedMembers.Contains("Name"));


			var set4 = new ArgumentSet("{}");
			Assert.False(set4.HasFlags);
			Assert.False(set4.HasNamedMembers);
			Assert.True(set4.Flags.Count == 0);
			Assert.True(set4.NamedMembers.Count == 0);
		}

		[Test]
		public void ArgParserTestThree()
		{
			var set1 = new ArgumentSet("jake another");
			Assert.False(set1.HasFlags);
			Assert.False(set1.HasNamedMembers);
			Assert.True(set1.HasPools);
			Assert.True(set1.Pools.Contains("Jake"));
			Assert.True(set1.Pools.Contains("Another"));


			var set2 = new ArgumentSet("jake -tv -f {name, address}");
			Assert.True(set2.HasFlags);
			Assert.True(set2.HasNamedMembers);
			Assert.True(set2.HasPools);
			Assert.True(set2.Pools.Contains("Jake"));
		}
	}
}

