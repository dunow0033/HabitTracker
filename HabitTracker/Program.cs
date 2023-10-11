using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
	class Program
	{
		static string connectionString = @"Data Source=habit-Tracker.db";

		static void Main(string[] args)
		{
			using (var connection = new SqliteConnection(connectionString))
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
		}

		static void GetUserInput()
		{
			bool closeApp = false;
			while (closeApp == false)
			{
				Console.Clear();
				Console.WriteLine("\nMAIN MENU");
				Console.WriteLine("\n\nWhat would you like to do?");
				Console.WriteLine("\n0. Close the app");
				Console.WriteLine("1. View all habits");
				Console.WriteLine("2. Create a new habit");
				Console.WriteLine("3. Delete a habit");
				Console.WriteLine("4. Update a habit");
				Console.WriteLine("-------------------------------\n");

				var option = Console.ReadLine();

				switch (option.Trim())
				{
					case "0":
						Console.WriteLine("Thank you!!  Have a nice day!!");
						closeApp = true;
						Environment.Exit(1);
						break;
					case "1":
						ViewAll();
						break;
					case "2":
						Insert();
						break;
					case "3":
						Delete();
						break;
					case "4":
						Update();
						break;
					default:
						Console.WriteLine("Break point");
						break;
				}
			}

			static void ViewAll()
			{
				using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				var tableCmd = con.CreateCommand();
				tableCmd.CommandText = $"SELECT * FROM drinking_water";

				SqliteDataReader reader = tableCmd.ExecuteReader();

				List<DrinkingWater> entries = new List<DrinkingWater>();

				if (!reader.HasRows)
				{
					Console.WriteLine("\n\nSorry, no entries to view!!  Add some data first!!");
					Console.WriteLine("\nPress any key to continue...");
					Console.ReadKey();
					GetUserInput();
				} else {

						Console.Clear();

						Console.WriteLine("Here is all of your data:  ");

						while (reader.Read())
					{
						entries.Add(
						new DrinkingWater
						{
							Id = reader.GetInt32(0),
							Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
							Quantity = reader.GetInt32(2)
						});
					}

					con.Close();

					Console.WriteLine("------------------------------------------\n");
					foreach (var entry in entries)
					{
						Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yyyy")} - Quantity: {entry.Quantity}");
					}
					Console.WriteLine("------------------------------------------\n");
				}

				Console.WriteLine("\n\nPress any key to return to the main menu...");
				Console.ReadKey();
				}
			}

			static void Insert()
			{
				Console.Clear();

				Console.Write("How much water did you drink today:  ");
				var quantity = Console.ReadLine();

				string date = DateTime.Now.ToString("dd-MM-yyyy");

				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var tableCmd = connection.CreateCommand();

					tableCmd.CommandText =
						$"INSERT INTO drinking_water (date, quantity) VALUES ('{date}', {quantity})";

					tableCmd.ExecuteNonQuery();

					connection.Close();
				}
			}

			static void Delete()
			{
				using (var con = new SqliteConnection(connectionString))
				{
					con.Open();

					var tableCmd = con.CreateCommand();
					tableCmd.CommandText = $"SELECT * FROM drinking_water";

					SqliteDataReader reader = tableCmd.ExecuteReader();

					List<DrinkingWater> entries = new List<DrinkingWater>();

					if (!reader.HasRows)
					{
						Console.WriteLine("\n\nSorry, no entries to delete!!  Add some data first!!");
						Console.WriteLine("\nPress any key to continue...");
						Console.ReadKey();
						GetUserInput();
					}
					else
					{
						Console.Clear();

						Console.Write("Very good, here are all of your entries:  ");

						while (reader.Read())
						{
							entries.Add(
							new DrinkingWater
							{
								Id = reader.GetInt32(0),
								Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
								Quantity = reader.GetInt32(2)
							});
						}

						con.Close();
					}

					Console.WriteLine("------------------------------------------\n");
					foreach (var entry in entries)
					{
						Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yyyy")} - Quantity: {entry.Quantity}");
					}
					Console.WriteLine("------------------------------------------\n");
				}

				Console.Write("\n\nWhich entry would you like to delete? ");
				var id = Console.ReadLine();

				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var tableCmd = connection.CreateCommand();

					tableCmd.CommandText =
						$"DELETE from drinking_water WHERE Id = '{id}'";

					int rowCount = tableCmd.ExecuteNonQuery();

					if (rowCount == 0)
					{
						Console.WriteLine($"\n\nRecord with Id {id} doesn't exist. \n\n");
						Delete();
					}

					connection.Close();
				}
			}

			static void Update()
			{
				using (var con = new SqliteConnection(connectionString))
				{
					con.Open();

					var tableCmd = con.CreateCommand();
					tableCmd.CommandText = $"SELECT * FROM drinking_water";

					SqliteDataReader reader = tableCmd.ExecuteReader();

					List<DrinkingWater> entries = new List<DrinkingWater>();

					if (!reader.HasRows)
					{
						Console.WriteLine("\n\nSorry, no entries to edit!!  Add some data first!!");
						Console.WriteLine("\nPress any key to continue...");
						Console.ReadKey();
						GetUserInput();
					}
					else
					{
						Console.Clear();

						Console.Write("Very good, here are all of your entries:  ");

						while (reader.Read())
						{
							entries.Add(
							new DrinkingWater
							{
								Id = reader.GetInt32(0),
								Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
								Quantity = reader.GetInt32(2)
							});
						}

						con.Close();
					}

					Console.WriteLine("------------------------------------------\n");
					foreach (var entry in entries)
					{
						Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yyyy")} - Quantity: {entry.Quantity}");
					}
					Console.WriteLine("------------------------------------------\n");
				}

				Console.Write("\n\nWhich entry would you like to edit? ");
				var id = Console.ReadLine();

				Console.Write("\n\nAnd what would you like to change the Quantity to? ");
				var quantity = Console.ReadLine();

				using(var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var tableCmd = connection.CreateCommand();

					tableCmd.CommandText =
						$"UPDATE drinking_water SET Quantity = '{quantity}' WHERE Id = '{id}'";

					int rowCount = tableCmd.ExecuteNonQuery();

					if (rowCount == 0)
					{
						Console.WriteLine($"\n\nRecord with Id {id} doesn't exist. \n\n");
						Delete();
					}

					connection.Close();
				}
			}
		}
	}
}