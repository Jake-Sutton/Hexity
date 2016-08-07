using System;
using Hexity.Engines;

namespace Core
{
	public class HexityCSVParser : IHexityCSVParser
	{
		ObjectPool IHexityCSVParser.ReadCSVForObjectPool(string fileName, string[] columnsToRead)
		{
			// TODO

			Console.WriteLine(fileName);
			Array.ForEach(columnsToRead, m => Console.WriteLine(m));

			return new ObjectPool();
		}
	}
}

