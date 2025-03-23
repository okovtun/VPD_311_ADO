using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace MoviesConnector
{
	class Connector
	{
		static readonly int PADDING = 33;
		readonly string CONNECTION_STRING;
		readonly SqlConnection connection;
		public Connector():this(ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString)
		{
			//CONNECTION_STRING =
			//	ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString;
			//this.connection = new SqlConnection(CONNECTION_STRING);
		}
		public Connector(string connection_string)
		{
			this.CONNECTION_STRING = connection_string;
			this.connection = new SqlConnection(CONNECTION_STRING);
			Console.WriteLine(CONNECTION_STRING);
		}
		public void InsertDirector(string first_name, string last_name)
		{
			string condition = $"last_name=N'{last_name}' AND first_name=N'{first_name}'";
			string query =
				$"INSERT Directors(first_name,last_name) VALUES (N'{first_name}',N'{last_name}')";
			string cmd =
				$"IF NOT EXISTS (SELECT director_id FROM Directors WHERE {condition}) BEGIN {query} END";
			Console.WriteLine(cmd);
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
		public void Select(string fields, string tables, string condition = "")
		{
			//1) Создаем подключение к Базе:
			//SqlConnection connection = new SqlConnection(CONNECTION_STRING);

			//2) Создаем команду, которую хотим выполнить на Сервере:
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";

			SqlCommand command = new SqlCommand(cmd, connection);

			//3) Получаем результаты запроса с Сервера:
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			//4) Обрабатываем результаты запроса:
			if (reader.HasRows)
			{
				Border(reader.FieldCount);
				for (int i = 0; i < reader.FieldCount; i++)
					Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
				Console.WriteLine();
				Border(reader.FieldCount);
				while (reader.Read())
				{
					//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t");
					for (int i = 0; i < reader.FieldCount; i++)
					{
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}
			}
			Border(reader.FieldCount, "=");

			//5) Закрываем поток и соединение с Сервером:
			reader.Close();
			connection.Close();
		}
		void Border(int fields_count, string symbol="-")
		{
			for (int i = 0; i < fields_count; i++)
			{
				for (int j = 0; j < PADDING; j++)
				{
					Console.Write(symbol);
				}
			}
			Console.WriteLine();
		}
	}
}
