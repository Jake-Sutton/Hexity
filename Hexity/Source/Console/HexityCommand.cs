using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

namespace HexCommands
{
	interface IRunnable
	{
		bool Run(ArgumentSet arguments);
		string Help();
		string CacheManualPage();
	}

	abstract class HexityCommand : IRunnable
	{

		string ManPage;

		public abstract bool Run(ArgumentSet arguments);

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

			if (!File.Exists(path))
			{
				throw new FileNotFoundException();
			}

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

		public bool HasPools
		{
			get;
			private set;
		}

		public List<string> PoolList
		{
			get;
			private set;
		}

		public HashSet<string> Pools
		{
			get;
			private set;
		}

		public ArgumentSet(string trailingData)
		{
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

			Flags = new HashSet<char>();
			NamedMembers = new HashSet<string>();
			PoolList = new List<string>();
			Pools = new HashSet<string>();

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
					 
					trailingData = trailingData.Substring(0, firstInstanceoOfCurly); // need to refactor so this only
					// occurs once
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

				string flagData = trailingData.Substring(firstInstanceoOfHyphen);

				string[] flagSets = flagData.Split(' ').Select(n => textInfo.ToUpper(n.TrimStart('-'))).ToArray();

				foreach (string flagSet in flagSets)
				{
					foreach (char flag in flagSet) {
						Flags.Add( flag );
					}
				}

				trailingData = trailingData.Substring(0, firstInstanceoOfHyphen);
			}

			if (trailingData.Length > 0)
			{
				HasPools = true;

				string[] pools = trailingData.Trim().Split(' ').Select(n => textInfo.ToTitleCase(n)).ToArray();

				foreach (string pool in pools)
				{
					Pools.Add( pool );
					PoolList.Add( pool );
				}
			}
		}
	}
}
