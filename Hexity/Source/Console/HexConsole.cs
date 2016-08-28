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
    interface IRunnable
    {
    	bool Run(string[] parameters);
		string Help();
    }

	public static class Manager 
	{

		public static string WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		public static HashSet<string> HexCommands = new HashSet<string>();
		public static string CurrentPool;
		public static Dictionary<string, ObjectPool> State;
	}

	class Create : IRunnable
	{
		public string Help()
		{
			return
@"NAME:

    'create' - creates an object pool.

DESCRIPTION:

    'create' - creates an object pool. 
    As of now there are no options this
    method takes.

EXAMPLE: 
  
    ~~~> pools 
    * Currently_Active_Pool_Name
    Another_Pool

    ~~~> add One_More

    ~~~> pools 
    * Currently_Active_Pool_Name
    Another_Pool
    One_More

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'delete' - deletes an object pool.

DESCRIPTION:

    'delete' - deletes an object pool. As of now 
    there are no options this method takes.

EXAMPLE: 

    ~~~> pools 
    * Currently_Active_Pool_Name
    Another_Pool
    One_More

    ~~~> delete One_More

    ~~~> pools 
    * Currently_Active_Pool_Name
    Another_Pool

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'add' - adds a member object whose name active
    object pool.

DESCRIPTION:

    'add' - adds a member object whose name active
    object pool. As of now there are no options
    this method takes.

EXAMPLE: 

    ~~~> list 
    Currently_Active_Pool_Name
    * A_Pool_Member

    ~~~> add One_More

    ~~~> list 
    Currently_Active_Pool_Name
    * A_Pool_Member
    * One_More

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
		}

		public bool Run(string[] parameters)
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


		void CreateWithProperties(string name, string[] properties)
		{
			var eng = new ObjectEngine(name, properties);
			Manager.State[Manager.CurrentPool].AddObject(eng);
		}
	}

	class Remove : IRunnable
	{
		public string Help()
		{
			return
@"NAME:

    'remove' - removes a member of object whose name
    is passed as the first argument from the currently 
    active object pool.

DESCRIPTION:

    'remove' - removes a member of object whose name
    is passed as the first argument from the currently 
    active object pool. As of now there are no options
    this method takes.

EXAMPLE: 

    ~~~> list 
    Currently_Active_Pool_Name
    * A_Pool_Member
    * One_More

    ~~~> remove One_More

    ~~~> list 
    Currently_Active_Pool_Name
    * A_Pool_Member

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'list' - prints the name of the currently active
    object pool and list of the its member object 
    engines.

DESCRIPTION:

    'list' - prints the name of the currently active
    object pool and list of the its member object 
    engines. As of now there are no options this 
    method takes.

EXAMPLE: 

    ~~~> list 
    Currently_Active_Pool_Name
    * A_Pool_Member
    * One_More

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'pools' - prints a list of currently available
    object pools.

DESCRIPTION:

    'pools' - prints a list of currently available
    object pools. As of now there are no options 
    this method takes.

EXAMPLE: 

    ~~~> pools 
    * Currently_Active_Pool_Name
    Another_Pool
    One_More

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'open' - Makes the object pool named by its first 
    argument active, meaning objects may be added and 
    removed.

DESCRIPTION:

    'open' makes the object pool named by its first 
    argument active, meaning objects may be added and 
    removed. As of now there are no options this method
    takes.

EXAMPLE: 

    ~~~> list 
    Some_Pool
    * Some_Pool_Member
    
    ~~~> open Hexity
    
    ~~~> list
    Hexity

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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
			return
@"NAME:

    'link' - when passed two object pools, seperated by 
    '<->' returns the list of object engines common
    to both pools.

DESCRIPTION:

    'link' - when passed two object pools, seperated by 
    '<->' returns the list of object engines common
    to both pools. As of now there are no options this 
    method takes.

EXAMPLE: 

    ~~~> list 
    Some_Pool
    * Some_Pool_Member
    
    ~~~> open Hexity
    
    ~~~> list
    Hexity
    * Some_Pool_Member

    ~~~> link Hexity <-> Some_Pool
    Some_Pool_Member

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";
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

	class Man : IRunnable
	{
		public string Help()
		{
			return
@"NAME:

    'man' - Prints a description of the command named by its 
    first argument and how to properly use it.

DESCRIPTION:

    'man' prints a description of the command named by its 
    first argument and how to properly use it. As of now
    there are no options this method takes.

EXAMPLE: 

    ~~~> man create
    Manual Page: 'create'
    
    Description of the function create with examples.

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";

		}

		public bool Run(string[] parameters)
		{
			string action = parameters[1];

			// Creates a TextInfo based on the "en-US" culture.
			var textInfo = new CultureInfo("en-US", false).TextInfo;

			action = textInfo.ToTitleCase(action);

			var type = Type.GetType(AppData.CommandNamespace + "." + action, true);
			var newInstance = (IRunnable)Activator.CreateInstance(type);

			Console.WriteLine(newInstance.Help());

			return true;
		}
	}

	class Pwd : IRunnable
	{
		public string Help()
		{
			return
@"NAME:

    'pwd' - Prints the current working directory.

DESCRIPTION:

    'pwd' Prints the current working directory. From
    here, all files will be loaded and saved.
    As of now there are no options this method takes.

EXAMPLE: 

    ~~~> pwd
    /Users/user/Desktop

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";

		}

		public bool Run(string[] parameters)
		{
			Console.WriteLine( Manager.WorkingDirectory );

			return true;
		}
	}

	class Read : IRunnable
	{
		public string Help()
		{
			return
@"NAME:

    'read' - loads a file into an object pool.

DESCRIPTION:

    'read' loads a file into an object pool.
    As of now there are no options this method takes.

EXAMPLE: 

    ~~~> read cats.csv Cats

AUTHOR:
    Jake Sutton, 2016.

SEE ALSO:
";

		}

		public bool Run(string[] parameters)
		{
			string fileName = parameters[1];

			string poolName = parameters[2];

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

			IHexityCSVParser parser = new HexityCSVParser();

			string fullPath = Path.Combine(Manager.WorkingDirectory, fileName);

			ObjectPool objectPool = parser.ReadCSVForObjectPool(fullPath, data);

			Manager.State.Add(poolName, objectPool);
			Manager.CurrentPool = poolName;

			return true;
		}
	}
}