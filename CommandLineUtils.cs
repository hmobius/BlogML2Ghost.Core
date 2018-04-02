
using System;
using System.IO;

namespace BlogML2Ghost.Core
{
	/// <summary>
	/// Description of CommandLineUtils.
	/// </summary>
	public static class CommandLineUtils
	{
		public static bool TryGetFileLocation(string fileDescription, out string filelocation)
		{
			Console.Write("{0} (full path)? ", fileDescription);
			filelocation = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(filelocation))
			{
				Console.WriteLine("{0} is not a valid file location", filelocation);	
				return false;
			}

			if (!File.Exists(filelocation))
			{
				Console.WriteLine("{0} does not exist", filelocation);
				return false;
			}

			return true;
		}

		public static bool GetBoolean(string prompt)
		{
			Console.Write("{0} (type y for yes, no by default) ", prompt);
			return Console.ReadLine().ToLower() == "y";			
		}

		public static int GetInteger(string prompt)
		{
			int returnValue;
			bool valid = false;

			do
			{
				Console.Write("{0} ", prompt);
				valid = int.TryParse(Console.ReadLine(), out returnValue);
				if (!valid)
				{
					Console.WriteLine("Not a valid integer");
				}
			} while (!valid);

			return returnValue;
		}
	}
}
