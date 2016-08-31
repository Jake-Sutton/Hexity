using System;
using System.Linq;
using System.Collections.Generic;
using Hexity.Models;

namespace Hexity.Engines 
{
    public class ObjectEngine 
    {
        public Hex Hex { get; set; }

		public HashSet<string> Properties
		{
			get;
			private set;
		}

		public Dictionary<string, object> Values
		{
			get;
			private set;
		}


		public ObjectEngine() 
        {
            this.Hex = new Hex();
			this.Properties = new HashSet<string>();
        }

        public ObjectEngine(string objectName) 
        {
            this.Hex = new Hex();
            this.Hex.Name = objectName;
			this.Properties = new HashSet<string>();
        }

		public ObjectEngine(string objectName, Dictionary<string, object> values)
		{
			this.Hex = new Hex();
			this.Hex.Name = objectName;
			this.Properties = new HashSet<string>(values.Keys);

			this.Values = values;
		}


		public ObjectEngine(string objectName, string[] properties)
		{
			this.Hex = new Hex();
			this.Hex.Name = objectName;
			this.Properties = new HashSet<string>();
			this.Values = new Dictionary<string, object>();

			foreach (var prop in properties)
			{
				this.Properties.Add(prop);
				this.Values.Add(prop, null);
			}
		}
    }

    public class ObjectPool 
    {
		List<ObjectEngine> Objects;
		HashSet<string> MemberProperties;

        public ObjectPool() 
        {
			this.MemberProperties = new HashSet<string>();
            this.Objects = new List<ObjectEngine>();
        }

		public ObjectPool(string[] properties)
		{
			this.MemberProperties = new HashSet<string>();
			this.Objects = new List<ObjectEngine>();

			foreach (var prop in properties) {
				this.MemberProperties.Add( prop );
			}
		}

		public ObjectPool(string[] properties, List<ObjectEngine> objects)
		{
			this.MemberProperties = new HashSet<string>();
			this.Objects = new List<ObjectEngine>();

			foreach (var prop in properties)
			{
				this.MemberProperties.Add(prop);
			}

			this.Objects = objects;
		}

		public bool HasProperty(string prop) {
			return MemberProperties.Contains( prop );
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