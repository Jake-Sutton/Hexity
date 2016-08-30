using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Core;
using Hexity.Engines;
using Hexity.Strings;

namespace HexCommands
{
	public static class Manager 
	{
		public static string WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		public static HashSet<string> HexCommands = new HashSet<string>();
		public static string CurrentPool;
		public static Dictionary<string, ObjectPool> State;
	}

	class Create : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			if (arguments.HasPools)
			{
				CreateWithProperties(arguments.Pools.First(), arguments.NamedMembers.ToArray());
			}

			return true;
		}

		void CreateWithProperties( string name, string[] properties )
		{
			// where we can implement a setter for properties
			var objectPool = new ObjectPool( properties );
			Manager.State.Add(name, objectPool);
			Manager.CurrentPool = name;
		}
	}

	class Delete : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			if (!arguments.HasPools)
			{
				Console.WriteLine("Failed to specify which pools should be deleted.");

				return false;
			}

			string toDelete = arguments.Pools.First();

			if (toDelete == Manager.CurrentPool)
			{
				Console.WriteLine("Cannot delete the currently open object pool.");

				return true;
			}

			if (Manager.State[toDelete].Count() > 0)
			{
				Console.WriteLine("{0} contains objects. Are you sure?", toDelete);

				string conf = Console.ReadLine();
				if (conf == "y")
				{
					Manager.State.Remove(toDelete);
				}
			}
			else
			{
				Manager.State.Remove(toDelete);
			}

			return true;
		}
	}

	class Add : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			if (arguments.HasPools)
			{
				CreateWithProperties(arguments.Pools.First(), arguments.NamedMembers.ToArray());
			}

			return true;
		}


		void CreateWithProperties(string name, string[] properties)
		{
			var eng = new ObjectEngine(name, properties);
			Manager.State[Manager.CurrentPool].AddObject(eng);
		}
	}

	class Remove : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			string toRemove = arguments.Pools.First();

			var engTemp = new List<ObjectEngine>();
			foreach (var item in Manager.State[Manager.CurrentPool].GetObjects())
			{
				if (item.Hex.Name == toRemove)
				{
					engTemp.Add(item);
				}
			}

			foreach (var del in engTemp)
			{
				Manager.State[Manager.CurrentPool].RemoveObject(del);
			}

			return true;
		}
	}

	class List : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			Console.WriteLine(Manager.CurrentPool);
			foreach (var item in Manager.State[Manager.CurrentPool].GetObjects())
			{
				Console.WriteLine("* " + item.Hex.Name);
			}

			return true;
		}
	}

	class Pools : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			Console.WriteLine("* " + Manager.CurrentPool);
			foreach (var item in Manager.State.Keys)
			{
				if (!item.Equals(Manager.CurrentPool))
					Console.WriteLine(item);
			}

			return true;
		}
	}

	class Open : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			Manager.CurrentPool = arguments.Pools.First();

			return true;
		}
	}

	class Link : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			if (arguments.Pools.Count == 2) // TODO refactor this so you can see shared items in n pools TODO
			{
				string first = arguments.PoolList[0];
				string second = arguments.PoolList[1];

				foreach (var item in Manager.State[first].GetObjects())
				{
					if (Manager.State[second].Contains(item.Hex.Name))
					{
						Console.WriteLine(item.Hex.Name);
					}
				}
			}

			return true;
		}
	}

	class Man : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			string action = arguments.Pools.First();

			// Creates a TextInfo based on the "en-US" culture.
			var textInfo = new CultureInfo("en-US", false).TextInfo;

			action = textInfo.ToTitleCase(action);

			var type = Type.GetType(AppData.CommandNamespace + "." + action, true);
			var newInstance = (IRunnable)Activator.CreateInstance(type);

			Console.WriteLine(newInstance.Help());

			return true;
		}
	}

	class Pwd : HexityCommand
	{
		public override bool Run(ArgumentSet arguments)
		{
			Console.WriteLine( Manager.WorkingDirectory );

			return true;
		}
	}

	class Read : HexityCommand
	{

		public override bool Run(ArgumentSet arguments)
		{
			if (arguments.Pools.Count  == 2)
			{
				string fileName = arguments.PoolList[0];

				string poolName = arguments.PoolList[1];

				IHexityCSVParser parser = new HexityCSVParser();

				string fullPath = Path.Combine(Manager.WorkingDirectory, fileName);

				ObjectPool objectPool = parser.ReadCSVForObjectPool(fullPath, arguments.NamedMembers.ToArray());

				Manager.State.Add(poolName, objectPool);
				Manager.CurrentPool = poolName;
			}

			return true;
		}
	}
}