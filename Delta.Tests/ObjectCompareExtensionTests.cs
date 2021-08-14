using Delta.Extensions;
using Delta.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Delta.Tests
{
	public class ObjectCompareExtensionTests
	{
		[Fact]
		public void Should_Compare_Two_Objects_and_Return_Deltas()
		{
			// Arrange
			var entityBeforeUpdate = TestDataFactory.GetTestEntityBeforeUpdate();
			var entityAfterUpdate = TestDataFactory.GetTestEntityAfterUpdate();

			var expected = new List<Variance>()
			{
				new Variance()
				{
					Property = "FirstName",
					OldValue = entityBeforeUpdate.FirstName,
					NewValue = entityAfterUpdate.FirstName,
					Type = typeof(string),
					NewValueObject = JToken.FromObject(new object())
				},
				new Variance()
				{
					Property = "LastName",
					OldValue = entityBeforeUpdate.LastName,
					NewValue = entityAfterUpdate.LastName,
					Type = typeof(string),
					NewValueObject = JToken.FromObject(new object())
				}
			};

			// Act
			var actual = entityBeforeUpdate.DetailedCompare(entityAfterUpdate);

			// Assert
			Assert.Contains(actual, x => expected.Select(y => y.Property).Contains(x.Property)
										&& expected.Select(y => y.OldValue).Contains(x.OldValue)
										&& expected.Select(y => y.NewValue).Contains(x.NewValue)
										&& expected.Select(y => y.Type).Contains(x.Type));
		}
	}
}
