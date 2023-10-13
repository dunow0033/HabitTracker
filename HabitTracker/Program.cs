using System;
using System.Collections.Generic;
using System.Globalization;
using HabitTracker;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
	{
		public static void Main(string[] args)
		{
			Database.SetupDB();

			Menu.GetUserInput();
		}
	}
}