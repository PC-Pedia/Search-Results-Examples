using System;

namespace CrownPeak.SearchG2.Attributes
{
	/// <summary>
	/// Marks a property as present on the index. By default the field name is the property name
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class FieldAttribute : Attribute
	{
		/// <summary>
		/// Marks a property as present on the index. By default the field name is the property name
		/// </summary>
		public FieldAttribute()
		{ }

		/// <summary>
		/// Marks a property as present on the index with the defined field name
		/// </summary>
		/// <param name="fieldName">The field name to use for this property.</param>
		public FieldAttribute(string fieldName)
		{
			FieldName = fieldName;
		}

		/// <summary>
		/// Overrides index field name
		/// </summary>
		public string FieldName { get; set; }

		/// <summary>
		/// Adds an index time boost to a field.
		/// </summary>
		public float Boost { get; set; }
	}
}
