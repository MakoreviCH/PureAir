using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Model
{
	public class Workspace
	{
		public int Id { get; set; }
		public string WorkspaceName { get; set; }
		public double? TemperatureThreshold { get; set; }
		public double? HumidityThreshold { get; set; }
		public double? GasThreshold { get; set; }
	}
}
