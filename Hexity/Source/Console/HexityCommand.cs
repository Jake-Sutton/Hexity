using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

namespace HexCommands
{
	interface IRunnable
	{
		bool Run(string[] parameters);
		string Help();
		string CacheManualPage();
	}

	abstract class HexityCommand : IRunnable
	{

		string ManPage;

		public abstract bool Run(string[] parameters);

		public string Help()
		{
			if (string.IsNullOrEmpty(ManPage))
			{
				ManPage = CacheManualPage();
			}

			return ManPage;
		}

		public string CacheManualPage()
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../Resources/Manual Pages/",
									   GetType().Name + ".man");

			// This text is added only once to the file.
			if (!File.Exists(path))
			{
				throw new FileNotFoundException();
			}

			// Open the file to read from.
			return File.ReadAllText(path);
		}
	}

	public class ArgumentSet 
	{
		public bool HasFlags
		{
			get;
			private set;
		}

		public HashSet<char> Flags
		{
			get;
			private set;
		}

		public bool HasNamedMembers
		{
			get;
			private set;
		}

		public HashSet<string> NamedMembers
		{
			get;
			private set;
		}

		public ArgumentSet(string trailingData)
		{
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

			Flags = new HashSet<char>();
			NamedMembers = new HashSet<string>();

			HasFlags = false;
			HasNamedMembers = false;

			int firstInstanceoOfCurly = trailingData.IndexOf('{');

			if (firstInstanceoOfCurly != -1) 
			{
				
				HasNamedMembers = true;

				string memberData = trailingData.Substring( firstInstanceoOfCurly );

				memberData = memberData.TrimStart('{').TrimEnd('}');

				if (memberData.Trim().Length == 0) 
				{
					HasNamedMembers = false;
				} 
				else 
				{
					string[] namedMembers = memberData.Split(',').Select(n => textInfo.ToTitleCase(n).Trim()).ToArray();

					foreach (var member in namedMembers)
					{
						NamedMembers.Add(member);
					}

					trailingData = trailingData.Substring(0, firstInstanceoOfCurly);
				}
			}

			int firstInstanceoOfHyphen = trailingData.IndexOf('-');

			if (firstInstanceoOfHyphen != -1)
			{

				HasFlags = true;

				string[] flagSets = trailingData.Split(' ').Select(n => textInfo.ToUpper(n.TrimStart('-'))).ToArray();

				foreach (string flagSet in flagSets)
				{
					foreach (char flag in flagSet) {
						Flags.Add( flag );
					}
				}
			}
		}

	}
}
