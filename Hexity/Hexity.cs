using System;
using System.Collections.Generic;
using Hexity.Strings;
using Hexity.Engines;
using System.Globalization;
using HexCommands;

namespace HexityStartUp 
{
    class App_Start
    {

        static void Main(string[] args) 
        {
            Console.WriteLine(AppData.StartUpString, AppData.Year, AppData.Name, AppData.Version, AppData.Author);

            var mainRuntime = new Core();

            bool success = mainRuntime.Initialize();
            
            if (success)
            {
                mainRuntime.Start();
            }
        }
    }

    class Core 
    {
        public bool Initialize() 
        {
			Manager.Responding = true;
            Manager.State = new Dictionary<string, ObjectPool>();
			Manager.CurrentPool = AppData.DefaultPoolName;

            var objectPool = new ObjectPool();
			Manager.State.Add(AppData.DefaultPoolName, objectPool);

            return Manager.Responding;
        }

        public int Start() 
        {

            while (Manager.Responding) 
            {
                Console.Write( AppData.Prompt );

                var input = Console.ReadLine().Trim().Split(' ');

                string action = input[0];

				RunAction(action, input);
            }

            // success error code
            return 0;
        }

		private static void RunAction(string action, string[] arguments) {

			// Creates a TextInfo based on the "en-US" culture.
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

			action = textInfo.ToTitleCase(action);


			try
			{
				Type type = Type.GetType("HexCommands." + action, true);
				var newInstance = (HexCommands.IRunnable)Activator.CreateInstance(type);

				newInstance.Run(arguments);
			}
			catch
			{
				Console.WriteLine("Unknown Command");
			}
		}
    }
}