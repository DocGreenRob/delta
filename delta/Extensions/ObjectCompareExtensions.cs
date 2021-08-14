using Delta.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Delta.Extensions
{
	public static class ObjectCompareExtension
	{
		private static int _maxThreshold = 15;

		public static List<Variance> DetailedCompare<T>(this T originalObject, T modifiedObject)
		{
			// TODO: null check on Public methods()
			var original = JObject.FromObject(originalObject);
			var modified = JObject.FromObject(modifiedObject);

			var variances = new List<Variance>();
			FillDetailsForObject<T>(original, modified, variances);

			return variances;
		}

		static void FillDetailsForObject<T>(JObject orig, JObject mod, List<Variance> variances)
		{
			var origNames = orig.Properties().Select(x => x.Name).ToArray();
			var modNames = mod.Properties().Select(x => x.Name).ToArray();

			// Names removed in modified
			foreach (var k in origNames.Except(modNames))
			{
				var originalProprerty = orig.Property(k);

				var variance = new Variance()
				{
					Property = originalProprerty.Name,
					OldValue = originalProprerty.Value?.ToString(),
					NewValue = null,
					Type = typeof(T).GetProperty(k)?.PropertyType.UnderlyingSystemType,
					NewValueObject = null
				};
				variances.Add(variance);
			}

			// Names added in modified
			foreach (var k in modNames.Except(origNames))
			{
				var modifiedProperty = mod.Property(k);

				var variance = new Variance()
				{
					Property = modifiedProperty.Name,
					OldValue = null,
					NewValue = modifiedProperty.Value?.ToString(),
					Type = typeof(T).GetProperty(k)?.PropertyType.UnderlyingSystemType,
					NewValueObject = modifiedProperty?.Value
				};
				variances.Add(variance);
			}

			// Present in both
			foreach (var k in origNames.Intersect(modNames))
			{
				var originalProperty = orig.Property(k);
				var modifiedProperty = mod.Property(k);

				if (originalProperty.Value.Type != modifiedProperty.Value.Type ||
					originalProperty.Value.ToString() != modifiedProperty.Value.ToString())
				{
					var variance = new Variance()
					{
						Property = modifiedProperty.Name,
						OldValue = originalProperty.Value.ToString(),
						NewValue = modifiedProperty.Value?.ToString(),
						Type = typeof(T).GetProperty(k)?.PropertyType.UnderlyingSystemType,
						NewValueObject = modifiedProperty?.Value
					};
					variances.Add(variance);
				}
				else if (
					((originalProperty.Value as JValue)?.Value == null && (modifiedProperty.Value as JValue)?.Value != null) ||
					(originalProperty.Value as JValue)?.Value != null && !(originalProperty.Value as JValue).Value.Equals((modifiedProperty.Value as JValue)?.Value)
					)
				{
					if (originalProperty.Value.Type == JTokenType.Object)
					{
						if (_maxThreshold > 0)
						{
							_maxThreshold -= 1;
						}
						else
						{
							return;
						}
						// Recurse into objects
						FillDetailsForObject<T>(originalProperty.Value as JObject, modifiedProperty.Value as JObject, variances);
					}
					else
					{
						// Replace values directly
						var variance = new Variance()
						{
							Property = modifiedProperty.Name,
							OldValue = originalProperty.Value.ToString(),
							NewValue = modifiedProperty.Value?.ToString(),
							Type = typeof(T).GetProperty(k)?.PropertyType.UnderlyingSystemType,
							NewValueObject = modifiedProperty?.Value
						};
						variances.Add(variance);
					}
				}
			}
		}
	}
}
