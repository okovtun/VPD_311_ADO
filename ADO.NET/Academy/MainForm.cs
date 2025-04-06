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
				new Query("*", "Students"),
				new Query
				(
					"group_id,group_name,COUNT(stud_id) AS students_count,direction_name",
					"Students,Groups,Directions",
					"direction=direction_id AND [group]=group_id",
					"group_id,group_name,direction_name"
				),
				new Query("*", "Directions"),
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
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			tables[i].DataSource = connector.Select(queries[i].Columns, queries[i].Tables, queries[i].Condition, queries[i].GroupBy);
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount - 1}";
		}
	}
}
