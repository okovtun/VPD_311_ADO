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
		public MainForm()
		{
			InitializeComponent();
			connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
			dgvStudents.DataSource = connector.Select("*", "Students");
			statusStripCountLabel.Text = $"Количество студентов: {dgvStudents.RowCount - 1}";
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
