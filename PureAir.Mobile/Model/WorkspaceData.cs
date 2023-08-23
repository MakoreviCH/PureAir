using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Model
{
	public class WorkspaceData
	{
		public int Id { get; set; }
		public double? Temperature { get; set; }
		public double? Humidity { get; set; }
		public double? GasQuality { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
