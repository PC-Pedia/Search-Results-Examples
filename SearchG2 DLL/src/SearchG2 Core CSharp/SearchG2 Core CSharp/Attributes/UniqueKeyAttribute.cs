using System;

namespace CrownPeak.SearchG2.Attributes
{
	/// <summary>
	/// Marks a property as unique key. By default the field name is the property name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class UniqueKeyAttribute : FieldAttribute
	{
		/// <summary>
		/// Marks a property as unique key. By default the field name is the property name.
		/// </summary>
		public UniqueKeyAttribute()
		{ }

		/// <summary>
		/// Marks a property as unique key.
		/// </summary>
		/// <param name="fieldName">The field name to use for this property.</param>
		public UniqueKeyAttribute(string fieldName)
			: base(fieldName)
		{ }
	}
}
