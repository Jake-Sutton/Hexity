using System;
using System.Collections.Generic;
using Hexity.Strings;
using Hexity.Engines;
using System.Globalization;
using HexCommands;
using System.Reflection;
using System.Linq;

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
            Manager.State = new Dictionary<string, ObjectPool>();
			Manager.CurrentPool = AppData.DefaultPoolName;

            var objectPool = new ObjectPool();
			Manager.State.Add(AppData.DefaultPoolName, objectPool);

			string @namespace = AppData.CommandNamespace; 

			var q =
				from 	t in Assembly.GetExecutingAssembly().GetTypes()
				where 	t.IsClass && t.Namespace == @namespace && !t.Name.StartsWith("<", StringComparison.Ordinal)
				select	t;
			
			q.ToList().ForEach(m => Manager.HexCommands.Add( m.Name ));

			return true;
        }

        public int Start() 
        {
			// Full credit to Miguel de Icaza (miguel@novell.com), authored 2008
			// This file is dual-licensed under the terms of the MIT X11 license or the
			// Apache License 2.0
			// You may obtain a copy of the License at
			// http://www.apache.org/licenses/LICENSE-2.0
			var lineEditor = new Mono.Terminal.LineEditor(AppData.Name);
			string s;

			while ( (s = lineEditor.Edit(AppData.Prompt, "")) != "q" ) 
            {
				var splitInput = s.Trim().Split(' ');

				var action = splitInput[0];

				RunAction(action, splitInput);
            }

            return 0;
        }

		static void RunAction(string action, string[] arguments)
		{
			// Creates a TextInfo based on the "en-US" culture.
			var textInfo = new CultureInfo("en-US", false).TextInfo;

			action = textInfo.ToTitleCase(action);

			if (Manager.HexCommands.Contains(action))
			{
				var type = Type.GetType( AppData.CommandNamespace + "." + action, true );
				var newInstance = (IRunnable) Activator.CreateInstance(type);

				newInstance.Run(arguments);
			}
			else 
			{
				Console.WriteLine( AppData.ErrInvalidCommand, action, "TODO" );
			}
		}
    }
}