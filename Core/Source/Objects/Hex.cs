using System.Collections.Generic;

namespace Hexity.Engines
{
	public class Hex : ObjectPool
	{
		public Hex()
		{
			MemberProperties = new HashSet<string>();
		}

		// implementation
		public string DefaultDisplayName
		{
			get;
			set;
		}

		public override string ToString()
		{
			return string.Format("{0}", DefaultDisplayName);
		}
	}
}