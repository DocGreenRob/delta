using Delta.Models;

namespace Delta.Tests
{
	public class TestDataFactory
	{
		public static Person GetTestEntityAfterUpdate()
		{
			return new Person
			{
				FirstName = "John",
				LastName = "Smith",
				Age = 41
			};
		}

		public static Person GetTestEntityBeforeUpdate()
		{
			return new Person
			{
				FirstName = "Jon",
				LastName = "Smtih",
				Age = 41
			};
		}
	}
}
