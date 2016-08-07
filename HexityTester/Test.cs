using NUnit.Framework;
using System;
using Core;

namespace HexityTester
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
			IHexityCSVParser parse = new HexityCSVParser();

			parse.ReadCSVForObjectPool("test", new string[]{"test"});

			Assert.Fail();
		}
	}
}

