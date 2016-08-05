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
		List<ObjectEngine> Objects;
		HashSet<string> ObjectProperties;
		//Dictionary<string, object> Columns;

        public ObjectPool() 
        {
			this.ObjectProperties = new HashSet<string>();
            this.Objects = new List<ObjectEngine>();
        }

		public ObjectPool(string[] properties)
		{
			this.ObjectProperties = new HashSet<string>();
			this.Objects = new List<ObjectEngine>();

			foreach (var prop in properties) {
				this.ObjectProperties.Add( prop );
			}
		}

        public void AddObject( ObjectEngine eng ) 
        {
            this.Objects.Add( eng );        
        }

        public void RemoveObject( ObjectEngine eng ) 
        {
            this.Objects.Remove( eng );        
        }

        public List<ObjectEngine> GetObjects() 
        {
            return Objects;
        }
        
        public bool Contains(string objectName)
        {
            return Objects.Any( obj => obj.Hex.Name==objectName );
        }

        public int Count()
        {
            return Objects.Count;
        }
    }
}