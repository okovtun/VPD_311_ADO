using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

namespace Academy
{
	public partial class MainForm : Form
	{
		Connector connector;

		Query[] queries = new Query[]
			{
				new Query("*", "Students JOIN Groups ON([group]=group_id) JOIN Directions ON (direction=direction_id)"),
				new Query
				(
					"group_id,group_name,COUNT(stud_id) AS students_count,direction_name",
					"Students,Groups,Directions",
					"direction=direction_id AND [group]=group_id",
					"group_id,group_name,direction_name"
				),
				new Query
				(
					@"direction_name,
					COUNT(DISTINCT group_id)    AS  N'Количество групп',
					COUNT(DISTINCT stud_id)     AS  N'Количество студентов'",

					@"Students
					JOIN			Groups		ON	([group]	=	group_id)
					RIGHT	JOIN	Directions	ON	(direction	=	direction_id)",	//tables
					"",	//WHERE
					"direction_name"
				),
				new Query("*", "Disciplines"),
				new Query("*", "Teachers"),
			};
		DataGridView[] tables;
		string[] status_messages = new string[]
			{
				"Количество студентов: ",
				"Количество групп: ",
				"Количество направлений: ",
				"Количество дисциплин: ",
				"Количество преподавателей: ",
			};
		/// //////////////////////////////////////////////////////
		public Dictionary<string, int> d_directions;
		public Dictionary<string, int> d_groups;
		public MainForm()
		{
			InitializeComponent();

			tables = new DataGridView[]
				{
					dgvStudents,
					dgvGroups,
					dgvDirections,
					dgvDisciplines,
					dgvTeachers
				};

			connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
			dgvStudents.DataSource = connector.Select("*", "Students");
			statusStripCountLabel.Text = $"Количество студентов: {dgvStudents.RowCount - 1}";

			d_directions = connector.GetDictionary("Directions");
			d_groups = connector.GetDictionary("Groups");
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key.ToString()).ToArray());
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key.ToString()).ToArray());	//LINQ
			cbGroupsDirection.Items.AddRange(d_directions.Select(d => d.Key.ToString()).ToArray());	//LINQ
		}

		void LoadTab(Query query = null)
		{
			int i = tabControl.SelectedIndex;
			if (query == null) query = queries[i];
			tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.GroupBy);
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount - 1}";
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			//int i = tabControl.SelectedIndex;
			LoadTab();
		}

		private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			Query query = new Query(queries[i]);
			Console.WriteLine(query.Condition);
			string tab_name = (sender as ComboBox).Name;
			string field_name = tab_name.Substring(Array.FindLastIndex<char>(tab_name.ToCharArray(), Char.IsUpper));
			Console.WriteLine(field_name);
			string member_name = $"d_{field_name.ToLower()}s";
			Console.WriteLine(member_name == nameof(d_directions));
			Dictionary<string, int> source = this.GetType().GetField(member_name).GetValue(this) as Dictionary<string,int>;
			//Console.WriteLine(this.GetType().GetField(member_name).GetValue(this));
			//Console.WriteLine(this.GetType());
			if(query.Condition != "")query.Condition += " AND";
			query.Condition += $" [{field_name.ToLower()}] = {source[(sender as ComboBox).SelectedItem.ToString()]}";
			LoadTab(query);
			Console.WriteLine((sender as ComboBox).Name);
			Console.WriteLine(e);
		}
	}
}
