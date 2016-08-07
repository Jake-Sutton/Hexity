using System;
using Hexity.Engines;

namespace Core
{
	public interface IHexityCSVParser
	{
		ObjectPool ReadCSVForObjectPool(string fileName, string[] columnsToRead);
	}
}

