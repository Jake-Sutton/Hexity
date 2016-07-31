using System;
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
        
        private bool responding;
        private ObjectPool objectPool;

        public bool Initialize() 
        {
            objectPool = new ObjectPool();
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
                        
                        string engine = input[1];
                        
                        for (int i = 2; i < input.Length; ++i) {
                            ObjectEngine eng = new ObjectEngine( input[i] );    
                            objectPool.AddObject( eng );
                        } 
                        
                        // write the object pool to the disk
                        break;

                    case "list":

                        foreach (var item in objectPool.GetObjects())
                        {
                            Console.WriteLine(item.GetHex().Name);
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