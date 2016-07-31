using System;
using System.Collections.Generic;
using Hexity.Strings;
using Hexity.Engines;

namespace HexityStartUp 
{
    class App_Start
    {

        static void Main(string[] args) 
        {
            Console.WriteLine(AppData.StartUpString, AppData.Year, AppData.Name, AppData.Version, AppData.Author);

            Core mainRuntime = new Core();

            bool success = mainRuntime.Initialize();
            
            if (success)
            {
                mainRuntime.Start();
            }
        }
    }

    class Core 
    {
        private string currentPool = "default";
        private Dictionary<string, ObjectPool> state;

        private bool responding;

        public bool Initialize() 
        {
            state = new Dictionary<string, ObjectPool>();

            ObjectPool objectPool = new ObjectPool();
            state.Add("default", objectPool);

            responding = true;

            return responding;
        }

        public int Start() 
        {

            while (responding) 
            {
                Console.Write( AppData.Prompt );

                var input = Console.ReadLine().Split(' ');

                string action = input[0];

                switch (action)
                {
                    case "create":
                        
                        // check to see if the usr wants another pool

                        string engine = input[1];
                        
                        for (int i = 2; i < input.Length; ++i) {
                            ObjectEngine eng = new ObjectEngine( input[i] );    
                            state[currentPool].AddObject( eng );
                        } 
                        
                        // write the object pool to the disk
                        break;

                    case "list":

                        Console.WriteLine(currentPool);
                        foreach (var item in state[currentPool].GetObjects())
                        {
                            Console.WriteLine("* " + item.GetHex().Name);
                        }

                        break;

                    case "q":
                    case "quit": 
                    case "done":
                    case "exit":                     
                        Console.WriteLine("Thank you for using Hexity. Goodbye.");
                        responding = false;
                        return 0;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }

            // success error code
            return 0;
        }
    }
}