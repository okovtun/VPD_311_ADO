﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesConnector
{
	class Program
	{
		static void Main(string[] args)
		{
			const string CONNECTION_STRING = "Data Source=(localdb)\\ProjectModels;Initial Catalog=Movies_VPD_311;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			Connector connector = new Connector(CONNECTION_STRING);

			connector.Select("SELECT * FROM Directors");
			connector.Select("SELECT * FROM Movies");
		}
	}
}
