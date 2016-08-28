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

			List<ObjectEngine> entries = new List<ObjectEngine>();

			var csv = new CsvReader( textReader );
			while ( csv.Read() )
			{
				Dictionary<string, object> tempData = new Dictionary<string, object>();

				Array.ForEach( columnsToRead, m =>  tempData.Add(m, csv.GetField<string>(m) ) );

				var nextEntry = new ObjectEngine( (string) tempData[ columnsToRead[0] ], tempData);

				entries.Add( nextEntry );
			}

			ObjectPool result = new ObjectPool(columnsToRead, entries);

			return result;
		}
	}
}

