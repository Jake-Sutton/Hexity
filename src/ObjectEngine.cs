using System;
using System.Linq;
using System.Collections.Generic;
using Hexity.Models;

namespace Hexity.Engines 
{
    public class ObjectEngine 
    {
        public Hex Hex { get; set; }

        public ObjectEngine() 
        {
            this.Hex = new Hex();
        }

        public ObjectEngine(string objectName) 
        {
            this.Hex = new Hex();
            this.Hex.Name = objectName;
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
            this.engines.Add( eng );        
        }

        public void RemoveObject( ObjectEngine eng ) 
        {
            this.engines.Remove( eng );        
        }

        public List<ObjectEngine> GetObjects() 
        {
            return engines;
        }
        
        public bool Contains(string objectName)
        {
            return engines.Any( obj => obj.Hex.Name==objectName );
        }

        public int Count()
        {
            return engines.Count;
        }
    }
}