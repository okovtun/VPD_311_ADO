using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MoviesConnector
{
	class Connector
	{
		static readonly int PADDING = 33;
		readonly string CONNECTION_STRING;
		readonly SqlConnection connection;
		public Connector(string connection_string)
		{
			this.CONNECTION_STRING = connection_string;
			this.connection = new SqlConnection(CONNECTION_STRING);
			Console.WriteLine(CONNECTION_STRING);
		}
		public void Select(string cmd)
		{
			//1) Создаем подключение к Базе:
			//SqlConnection connection = new SqlConnection(CONNECTION_STRING);

			//2) Создаем команду, которую хотим выполнить на Сервере:
			//string cmd = "SELECT * FROM Movies";
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
