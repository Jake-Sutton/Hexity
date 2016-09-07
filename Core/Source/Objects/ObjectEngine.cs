using System.Linq;
using System.Collections.Generic;

namespace Hexity.Engines
{
	public class ObjectPool
	{
		public Hex Hex
		{
			get;
			private set;
		}

		public HashSet<string> MemberProperties;
		public Dictionary<string, ObjectPool> Values;

		public ObjectPool()
		{
			MemberProperties = new HashSet<string>();
			Values = new Dictionary<string, ObjectPool>();
		}

		public ObjectPool(string defaultDisplay) : this()
		{
			Hex = new Hex();
			Hex.DefaultDisplayName = defaultDisplay;
		}

		public ObjectPool(string defaultDisplay, Dictionary<string, ObjectPool> properties) : this(defaultDisplay)
		{
			Hex = new Hex();
			Hex.DefaultDisplayName = defaultDisplay;

			foreach (var prop in properties.Keys)
			{
				MemberProperties.Add(prop);
			}

			Values = properties;
		}

		public ObjectPool(string defaultDisplay, string[] properties) : this(defaultDisplay)
		{
			Hex = new Hex();
			Hex.DefaultDisplayName = defaultDisplay;

			foreach (var prop in properties)
			{
				MemberProperties.Add(prop);
			}
		}

		public bool HasProperty(string prop)
		{
			return MemberProperties.Contains(prop);
		}

		public void AddObject(string defaultDisplayName, ObjectPool eng)
		{
			Values.Add(defaultDisplayName, eng);
		}

		public void RemoveObject(string defaultDisplayName)
		{
			Values.Remove(defaultDisplayName);
		}

		public List<ObjectPool> GetObjects()
		{
			return Values.Values.ToList();
		}

		public bool Contains(string objectName)
		{
			return MemberProperties.Any(obj => obj.ToString() == objectName);
		}

		public int Count()
		{
			return Values.Count;
		}
	}
}