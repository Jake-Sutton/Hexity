using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Hexity.Engines;

namespace Core
{
	public class HexityCSVParser : IHexityCSVParser
	{
		ObjectPool IHexityCSVParser.ReadCSVForObjectPool(string fileName, string[] columnsToRead)
		{
			TextReader textReader = File.OpenText( fileName );

			var entries = new Dictionary<string, ObjectPool>();

			var csv = new CsvReader( textReader );
			while ( csv.Read() )
			{
				var tempData = new Dictionary<string, object>();

				Array.ForEach( columnsToRead, m =>  tempData.Add(m, csv.GetField<string>(m) ) );

				var nextEntry = new ObjectPool( (string) tempData[ columnsToRead[0] ], columnsToRead);

				entries.Add( (string)tempData[columnsToRead[0]], nextEntry );
			}

			var result = new ObjectPool(fileName, entries);

			return result;
		}
	}
}

