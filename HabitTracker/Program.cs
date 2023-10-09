using System;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
	class Program
	{
		static void Main(string[] args)
		{
			string connectionString = @"Data Source=habit-Tracker.db";

			using(var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var tableCmd = connection.CreateCommand();

				tableCmd.CommandText = 
					@"CREATE TABLE IF NOT EXISTS drinking_water (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						Date TEXT,
						Quantity INTEGER
						)";

				tableCmd.ExecuteNonQuery();

				connection.Close();
			}

			GetUserInput();

			Insert(connectionString);
		}

		static void GetUserInput()
		{
			Console.Clear();
			bool closeApp = false;
			while(closeApp == false)
			{
				Console.WriteLine("\n\nMAIN MENU");
				Console.WriteLine("\nWhat would you like to do?");
				Console.WriteLine("0. Close the app");
				Console.WriteLine("1. View all habits");
				Console.WriteLine("2. Create a new habit");
				Console.WriteLine("3. Delete a habit");
				Console.WriteLine("4. Update a habit");
			}
		}

		static void Insert(string connectionString)
		{
			Console.Clear();

			Console.Write("How much water did you drink today:  ");
			var habit = Console.ReadLine();

			DateTime date = DateTime.Now;

			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var tableCmd = connection.CreateCommand();

				tableCmd.CommandText =
					@"INSERT INTO drinking_water (
						Date, Integer) 
						VALUES (
						date, habit
					))";

				tableCmd.ExecuteNonQuery();

				connection.Close();
			}
	}
}