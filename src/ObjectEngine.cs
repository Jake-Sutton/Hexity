using System;
using System.Linq;
using System.Collections.Generic;
using Hexity.Models;

namespace Hexity.Engines 
{
    public class ObjectEngine 
    {
        // unity IOC dependency injection occurrs...

        private Hex hex;

        public ObjectEngine() 
        {

        }

        public ObjectEngine(string objectName) 
        {
            Hex newObject = new Hex();
            newObject.Name = objectName;

            this.hex = newObject;
        }

        public Hex GetHex() 
        {
            return this.hex;
        }
    }

    public class ObjectPool 
    {
        private List<ObjectEngine> engines;

        public ObjectPool() 
        {
            this.engines = new List<ObjectEngine>();
        }

        public void AddObject( ObjectEngine eng ) 
        {
            engines.Add( eng );        
        }

        public List<ObjectEngine> GetObjects() 
        {
            return engines;
        }

        public bool Contains(string objectName)
        {
            return engines.Any(obj => obj.GetHex().Name==objectName);
        }

        // load pool from a file

        // remove object

        // find objects by name etc.

    }
}