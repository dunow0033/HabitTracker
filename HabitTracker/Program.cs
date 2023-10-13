using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
						Console.WriteLine("Invalid Option!!  Please try again!!");
						Thread.Sleep(2000);
						GetUserInput();
						break;
				}
			}
		}

			public static void ViewAll()
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

				Console.Write("How much water did you drink today (whole numbers only, 0 for main menu):  ");
				var quantity = Console.ReadLine();

				while(!Int32.TryParse(quantity, out _) || Convert.ToInt32(quantity) < 0)
				{
					Console.WriteLine("\n\nInvalid number.  Please try again: \n");
					quantity = Console.ReadLine();
				}

				if (Int32.Parse(quantity) == 0)
					GetUserInput();

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

					bool found = false;

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

					Console.Write("\n\nWhich entry # would you like to delete (0 to return to main menu)? ");
					var id = Console.ReadLine();

					while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) < 0)
					{
						Console.WriteLine("\n\nInvalid number.  Please try again: \n");
						id = Console.ReadLine();
					}

					if (Int32.Parse(id) == 0)
						GetUserInput();

				foreach (var entry in entries)
				{
					if (entry.Id == Int32.Parse(id))
					{
						found = true;
						break;
					}
				}

				while (!found)
				{
					Console.WriteLine($"\n\nSorry, entry #{id} doesn't exist. Please try again (0 for main menu): \n");
					id = Console.ReadLine();

					while (!Regex.IsMatch(id, @"^\d+$"))
					{
						Console.WriteLine("Sorry, please enter numbers only: \n");
						id = Console.ReadLine();
					}

					if (Int32.Parse(id) == 0)
						GetUserInput();

					foreach (var entry in entries)
					{
						if (entry.Id == Int32.Parse(id))
						{
							found = true;
							break;
						}
					}
				}

				con.Open();

					tableCmd.CommandText =
						$"DELETE from drinking_water WHERE Id = '{id}'";

					int rowCount = tableCmd.ExecuteNonQuery();

					if (rowCount == 0)
					{
						Console.WriteLine($"\n\nRecord with Id {id} doesn't exist. \n\n");
						Delete();
					}

					con.Close();

					Console.WriteLine($"\nEntry with Id {id} was successfully deleted!!");
					Console.WriteLine("\nPress any key to continue...");
					Console.ReadKey();
				}
			}

		static void Update()
		{
			using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				bool found = false;

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

				Console.Write("\n\nWhich entry would you like to edit? (0 for main menu)");

				var id = Console.ReadLine();

				while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) < 0)
				{
					Console.WriteLine("\n\nInvalid number.  Please try again: \n");
					id = Console.ReadLine();
				}

				if (Int32.Parse(id) == 0)
					GetUserInput();

				while (!Regex.IsMatch(id, @"^\d+$"))
				{
					Console.WriteLine("Sorry, please enter numbers only: \n");
					id = Console.ReadLine();
				}

				foreach (var entry in entries)
				{
					if (entry.Id == Int32.Parse(id))
					{
						found = true;
						break;
					}
				}

				while (!found)
				{
					Console.WriteLine($"\n\nSorry, entry #{id} doesn't exist. Please try again: \n");
					id = Console.ReadLine();

					while (!Regex.IsMatch(id, @"^\d+$"))
					{
						Console.WriteLine("Sorry, please enter numbers only: \n");
						id = Console.ReadLine();
					}

					foreach (var entry in entries)
					{
						if (entry.Id == Int32.Parse(id))
						{
							found = true;
							break;
						}
					}
				}

				Console.Write("\n\nAnd what would you like to change the Quantity to? (0 for main menu) ");
				var quantity = Console.ReadLine();

				while (!Int32.TryParse(quantity, out _) || Convert.ToInt32(id) < 0)
				{
					Console.WriteLine("\n\nInvalid number.  Please try again: \n");
					quantity = Console.ReadLine();
				}

				if (Int32.Parse(quantity) == 0)
					GetUserInput();

				string date = DateTime.Now.ToString("dd-MM-yyyy");

				con.Open();

				tableCmd.CommandText =
					$"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";

				int rowCount = tableCmd.ExecuteNonQuery();

				if (rowCount == 0)
				{
					Console.WriteLine($"\n\nRecord with Id {id} doesn't exist. \n\n");
					Update();
				}

				con.Close();

				Console.WriteLine($"\nEntry with Id {id} had it's quantity successfully changed to {quantity}!!");
				Console.WriteLine("\nPress any key to continue...");
				Console.ReadKey();
			}
		}
	}
}