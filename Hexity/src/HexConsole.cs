using System;

namespace HexCommands
{
    interface IRunnable
    {
    	bool Run(string[] parameters);
		string Help();

    }

	class Create : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{


			throw new NotImplementedException();
		}
	}

	class Delete : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			throw new NotImplementedException();
		}
	}

	class Add : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			throw new NotImplementedException();
		}
	}

	class Remove : IRunnable
	{
		public string Help()
		{
			throw new NotImplementedException();
		}

		public bool Run(string[] parameters)
		{
			throw new NotImplementedException();
		}
	}
}