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

                var input = Console.ReadLine().Trim().Split(' ');

                string action = input[0];

                switch (action)
                {
                    case "create":
                        
                        for (int i = 1; i < input.Length; ++i) {
                            ObjectPool objectPool = new ObjectPool();
                            state.Add( input[i], objectPool );

                            currentPool = input[i];
                        }
                        
                        // write the object pool to the disk
                        break;

                    case "delete":
                        
                        if (state[input[1]].Count() > 0) 
                            {
                                Console.WriteLine(input[1] + " contains objects. Are you sure?");

                                string conf = Console.ReadLine();
                                if ( conf == "y" ) {
                                    state.Remove( input[1] );
                                }
                            }
                            else 
                            {
                                state.Remove( input[1] );
                            }
                        
                        // write the object pool to the disk
                        break;

                    case "add":
                        
                        for (int i = 1; i < input.Length; ++i) {
                            ObjectEngine eng = new ObjectEngine( input[i] );    
                            state[currentPool].AddObject( eng );
                        } 
                        
                        // write the object pool to the disk
                        break;

                    case "remove":

                        List<ObjectEngine> engTemp = new List<ObjectEngine>() {};
                        foreach (var item in state[currentPool].GetObjects())
                        {
                            if (item.Hex.Name == input[1]) 
                            {
                                engTemp.Add( item );
                            }
                        } 

                        foreach(var del in engTemp) 
                        {          
                            state[currentPool].RemoveObject( del );
                        }
                    
                        // write the object pool to the disk
                        break;

                    case "ls":
                    case "list":

                        Console.WriteLine(currentPool);
                        foreach (var item in state[currentPool].GetObjects())
                        {
                            Console.WriteLine("* " + item.Hex.Name);
                        }                 

                        break;

                    case "pools":

                        Console.WriteLine("* " + currentPool);
                        foreach (var item in state.Keys) 
                        {
                            if (!item.Equals(currentPool))
                                Console.WriteLine(item);
                        }
                        
                        break;
                    
                    case "open":

                        currentPool = input[1];

                        break;

                    case "link":

                        string first = input [1];
                        string second = input [3];
                        string linkType = input[2];

                        if ( linkType.Equals("<->") ) 
                        {
                            foreach( var item in state[first].GetObjects() ) 
                            {
                                if ( state[second].Contains(item.Hex.Name)) 
                                {
                                    Console.WriteLine( item.Hex.Name );
                                }
                            }
                        }

                        break;

                    case "q":                    
                        Console.WriteLine("Thank you for using Hexity. Goodbye.");
                        responding = false;
                        break;

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