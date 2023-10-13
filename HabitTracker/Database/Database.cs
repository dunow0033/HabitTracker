using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
	internal class Database
	{
		private static string connectionString = @"Data Source=habit-Tracker.db";
		private static string date = DateTime.Now.ToString("dd-MM-yyyy");

		public static List<DrinkingWater> GetEntries()
		{
			List<DrinkingWater> entries = new();

			using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				var tableCmd = con.CreateCommand();
				tableCmd.CommandText = $"SELECT * FROM drinking_water";

				using (SqliteDataReader reader = tableCmd.ExecuteReader())
				{
					if (!reader.HasRows)
					{
						return null;
					}
					else
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

					con.Close();
				}
			}

			return entries;
		}

		public static void AddEntry(int quantity)
		{
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
	}
}
