﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Data.SqlClient;
using System.Configuration;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		readonly string CONNECTION_STRING = "";
		SqlConnection connection = null;
		DataSet set = null;
		public MainForm()
		{
			InitializeComponent();
			CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
			Console.WriteLine(CONNECTION_STRING);
			//1) Создаем DataSet:
			set = new DataSet();
			AddTable("Directions","direction_id,direction_name");
			AddTable("Groups", "group_id,group_name,direction");
			AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
			PrintGroups();
			//LoadGroupsRelatedData();
		}
		public void AddTable(string table, string columns)
		{
			
			//	2.1) Добавляем таблицу в DataSet:
			set.Tables.Add(table);

			//	2.2) Добавляем поля (столбики) в таблицу:
			string[] a_columns = columns.Split(',');
			for (int i = 0; i < a_columns.Length; i++)
			{
				set.Tables[table].Columns.Add(a_columns[i]);
			}
			//	2.3) Определяем, какое поле будет первичным ключем:
			set.Tables[table].PrimaryKey =
				new DataColumn[] { set.Tables[table].Columns[0] };

			string cmd = $"SELECT {columns} FROM {table}";
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
			adapter.Fill(set.Tables[table]);
			Print(table);
		}
		public void AddRelation(string relation_name, string child, string parent)
		{
			set.Relations.Add
				(
					relation_name,
					set.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
					set.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
				);
		}
		void LoadGroupsRelatedData()
		{
			//1) Создаем DataSet:
			set = new DataSet();

			//2) Добавляем табоицы в DataSet:
			const string dsTable_Directions = "Directions";
			const string dst_col_direction_id = "direction_id";
			const string dst_col_direction_name = "direction_name";
			//	2.1) Добавляем таблицу в DataSet:
			set.Tables.Add(dsTable_Directions);
			//	2.2) Добавляем поля (столбики) в таблицу:
			set.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
			set.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
			//	2.3) Определяем, какое поле будет первичным ключем:
			set.Tables[dsTable_Directions].PrimaryKey = 
				new DataColumn[] { set.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

			const string dsTable_Groups = "Groups";
			const string dst_Groups_col_group_id = "group_id";
			const string dst_Groups_col_group_name = "group_name";
			const string dst_Groups_col_group_direction = "direction";
			set.Tables.Add(dsTable_Groups);
			set.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_id, typeof(int));
			set.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_name, typeof(string));
			set.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_direction, typeof(byte));
			set.Tables[dsTable_Groups].PrimaryKey =
				new DataColumn[] { set.Tables[dsTable_Groups].Columns[0] };

			//3) Строим связи между таблицами:
			set.Relations.Add
				(
					"GroupsDirections",
					set.Tables[dsTable_Directions].Columns[dst_col_direction_id],		//Parent field - первичный ключ в другой таблице
					set.Tables[dsTable_Groups].Columns[dst_Groups_col_group_direction]//Child field - внешний ключ
				);

			//4) Загрузка данных в DataSet:
			string directionsCmd = "SELECT * FROM Directions";
			string groupsCmd = "SELECT * FROM Groups";
			SqlDataAdapter directionsAdapter = new SqlDataAdapter(directionsCmd, connection);
			SqlDataAdapter groupsAdapter = new SqlDataAdapter(groupsCmd, connection);

			directionsAdapter.Fill(set.Tables[dsTable_Directions]);
			groupsAdapter.Fill(set.Tables[dsTable_Groups]);

			Print("Directions");
			Print("Groups");
		}
		public void Print(string table)
		{
			Console.WriteLine(table);
			Console.WriteLine("\n======================================================\n");
			for (int i = 0; i < set.Tables[table].Columns.Count; i++)
				Console.Write(set.Tables[table].Columns[i].Caption + "\t");
			Console.WriteLine("\n------------------------------------------------------\n");

			for (int i = 0; i < set.Tables[table].Rows.Count; i++)
			{
				//Console.Write(GroupsRelatedData.Tables[table].Rows[i]+":\t");
				for (int j = 0; j < set.Tables[table].Columns.Count; j++)
				{
					Console.Write(set.Tables[table].Rows[i][j] + "\t\t");
				}
				Console.WriteLine();
			}
			Console.WriteLine("\n======================================================\n");
		}
		void PrintGroups()
		{
			Console.WriteLine("\n======================================================\n");
			string table = "Groups";
			for (int i = 0; i < set.Tables[table].Rows.Count; i++)
			{
				for (int j = 0; j < set.Tables[table].Columns.Count; j++)
				{
					Console.Write(set.Tables[table].Rows[i][j] + "\t");
				}
				Console.WriteLine(set.Tables[table].Rows[i].GetParentRow("GroupsDirections")["direction_name"]);
				Console.WriteLine();
			}
			Console.WriteLine("\n======================================================\n");
		}
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();
	}
}
