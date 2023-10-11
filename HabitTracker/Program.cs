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
						//Update(connectionString);
						Console.WriteLine("case 4");
						break;
					default:
						Console.WriteLine("Break point");
						break;
				}
			}

			static void ViewAll()
			{
				Console.Clear();

				using(SqliteConnection con = new SqliteConnection(connectionString))
				{
					con.Open();

					var tableCmd = con.CreateCommand();
					tableCmd.CommandText = $"SELECT * FROM drinking_water";

					Console.Write("Here are all of your entries:  ");

					SqliteDataReader reader = tableCmd.ExecuteReader();

					List<DrinkingWater> entries = new List<DrinkingWater>();

					if (reader.HasRows)
					{
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
					}
					else
					{
						Console.WriteLine("No rows found");
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
				Console.Clear();

				using (var con = new SqliteConnection(connectionString))
				{
					con.Open();

					var tableCmd = con.CreateCommand();
					tableCmd.CommandText = $"SELECT * FROM drinking_water";

					Console.Write("Very good, here are all of your entries:  ");

					SqliteDataReader reader = tableCmd.ExecuteReader();

					List<DrinkingWater> entries = new List<DrinkingWater>();

					//if (reader.HasRows)
					//{
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
					//}
					//else
					//{
					//	Console.WriteLine("No rows found");
					//}

					con.Close();

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
		}
	}
}