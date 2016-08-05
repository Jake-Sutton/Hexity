using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Hexity.Engines;

namespace HexCommands
{

    interface IRunnable
    {
    	bool Run(string[] parameters);
		string Help();
    }

	public static class Manager 
	{
		public static bool Responding;
		public static string CurrentPool;
		public static Dictionary<string, ObjectPool> State;
	}

	class Create : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		bool IRunnable.Run(string[] parameters)
		{
			if (parameters.Length > 2)
			{

				int parameterStart = -1;
				int parameterEnd = -1;
				for (int i = 0; i < parameters.Length; ++i)
				{
					if (parameters[i].Contains("{"))
					{
						parameterStart = i;
						break;
					}
				}

				for (int i = parameterStart - 1; i < parameters.Length; ++i)
				{
					if (parameters[i].Contains("}"))
					{
						parameterEnd = i;
						break;
					}
				}

				string[] data = parameters.Skip(parameterStart).Take(parameterEnd - (parameterStart - 1)).ToArray();

				string final = string.Join("", data);

				final = final.TrimStart('{').TrimEnd('}');

				TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

				data = final.Split(',').Select(n => textInfo.ToTitleCase(n)).ToArray();

				CreateWithProperties(parameters[1], data);
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

	class Delete : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			string toDelete = parameters[1];

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

	class Add : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			for (int i = 1; i < parameters.Length; ++i)
			{
				var eng = new ObjectEngine(parameters[i]);
				Manager.State[Manager.CurrentPool].AddObject(eng);
			}

			return true;
		}
	}

	class Remove : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			string toRemove = parameters[1];

			List<ObjectEngine> engTemp = new List<ObjectEngine>() { };
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

	class List : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			Console.WriteLine(Manager.CurrentPool);
			foreach (var item in Manager.State[Manager.CurrentPool].GetObjects())
			{
				Console.WriteLine("* " + item.Hex.Name);
			}

			return true;
		}
	}

	class Pools : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
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

	class Open : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			Manager.CurrentPool = parameters[1];

			return true;
		}
	}

	class Link : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			string first = parameters[1];
			string second = parameters[3];
			string linkType = parameters[2];

			if (linkType.Equals("<->"))
			{
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

	class Q : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			Manager.Responding = false;

			return true;
		}
	}
}