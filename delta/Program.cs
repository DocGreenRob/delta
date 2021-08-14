using Delta.Extensions;
using Delta.Models;
using System;

namespace delta
{
	class Program
	{
		static void Main(string[] args)
		{
			var person_new = new Person { FirstName = "Robetr", LastName = "Green", Age = 41 };
			// fix the misspelled FirstName
			var person_update = new Person { FirstName = "Robert", LastName = "Green-updated", Age = 41 };

			var deltas = person_new.DetailedCompare(person_update);

			foreach (var delta in deltas)
			{
				Console.WriteLine($"Property: {delta.Property}, Old Value: {delta.OldValue}, New Value: {delta.NewValue}");
			}

			Console.WriteLine("");
			Console.WriteLine("Press any key to close the application.");
			Console.ReadKey();
		}
	}
}
