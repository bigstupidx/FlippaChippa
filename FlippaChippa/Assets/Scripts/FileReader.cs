using System;
using System.IO;

public class FileReader
{
	public static string ReadFile(string fileLocation) {
		return System.IO.File.ReadAllText (fileLocation);
	}
}

