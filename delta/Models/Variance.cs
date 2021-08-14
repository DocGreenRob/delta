using Newtonsoft.Json.Linq;
using System;

namespace Delta.Models
{
	public class Variance
	{
		public object NewValue { get; set; }
		public JToken NewValueObject { get; set; }
		public object OldValue { get; set; }
		public string Property { get; set; }
		public Type Type { get; set; }
	}
}
