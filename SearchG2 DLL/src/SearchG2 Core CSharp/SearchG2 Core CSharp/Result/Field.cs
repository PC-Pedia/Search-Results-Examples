using System;
using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a single facet result from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> on a CrownPeak SearchG2 collection.
	/// </summary>

	public class Field : IField
	{
		/// <summary>The name of this field.</summary>
		public string Name { get; protected set; }
		/// <summary>The value(s) for this field.</summary>
		public object[] Values { get; protected set; }
		// TODO: multi-value rather than the first one
		/// <summary>The value (may be concatenated for multi-valued fields) of this field.</summary>
		public object Value { get { return Values.FirstOrDefault(); } }
		/// <summary>The number of values that this field contains.</summary>
		public int Count { get { return Values.Length; } }
		/// <summary>The highlights for this field.</summary>
		public string[] Highlights { get; set; }

		private Field()
		{
			Name = string.Empty;
			Values = new object[] {};
			Highlights = new string[] {};
		}

		internal Field(string name, object value)
			: this()
		{
			Name = name;
			if (value is IEnumerable<object>)
				Values = (value as IEnumerable<object>).ToArray();
			else if (value is System.Collections.ArrayList)
				Values = ((System.Collections.ArrayList)value).ToArray();
			else
				Values = new [] { value };
		}

		/// <summary>
		/// Get the numbered value from this collection.
		/// </summary>
		/// <param name="index">The index to retrieve.</param>
		/// <returns>Object containing the value.</returns>
		public object this[int index]
		{
			get
			{
				if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");
				return Values[index];
			}
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{Object}"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{Object}"/></returns>
		public IEnumerator<object> GetEnumerator()
		{
			return (IEnumerator<object>)Values.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="System.Collections.IEnumerator"/> for this collection.
		/// </summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		/// <summary>
		/// Convert the value of this field into the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="type">The desired result <see cref="Type"/>.</param>
		/// <returns><see cref="Object"/> which can be cast to the desired type.</returns>
		public object GetAsType(Type type)
		{
			return Convert.ChangeType(Value, type);
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <param name="type">The desired result <see cref="Type"/>.</param>
		/// <returns><see cref="Object"/> which can be cast to the desired type.</returns>
		public object GetAsType(int index, Type type)
		{
			return Convert.ChangeType(this[index], type);
		}

		/// <summary>
		/// Return a string that represents the current <see cref="Field"/>.
		/// </summary>
		/// <returns><see cref="String"/></returns>
		public override string ToString()
		{
			return GetAsString();
		}

		#region Convenience GET methods
		/// <summary>
		/// Convert the value of this field into a <see cref="byte"/> array.
		/// </summary>
		/// <returns>Array of <see cref="byte"/>.</returns>
		public byte[] GetAsByteArray()
		{
			return (byte[])GetAsType(typeof(byte[]));
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into a <see cref="byte"/> array.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns>Array of <see cref="byte"/>.</returns>
		public byte[] GetAsByteArray(int index)
		{
			return (byte[])GetAsType(index, typeof(byte[]));
		}

		/// <summary>
		/// Convert the value of this field into a <see cref="double"/>.
		/// </summary>
		/// <returns><see cref="double"/></returns>
		public double GetAsDouble()
		{
			return (double)GetAsType(typeof(double));
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into a <see cref="double"/>.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns><see cref="double"/></returns>
		public double GetAsDouble(int index)
		{
			return (double)GetAsType(index, typeof(double));
		}

		/// <summary>
		/// Convert the value of this field into an <see cref="Int32"/>.
		/// </summary>
		/// <returns><see cref="Int32"/></returns>
		public int GetAsInt32()
		{
			return (int)GetAsType(typeof(int));
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into an <see cref="Int32"/>.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns><see cref="Int32"/></returns>
		public int GetAsInt32(int index)
		{
			return (int)GetAsType(index, typeof(int));
		}

		/// <summary>
		/// Convert the value of this field into an <see cref="Object"/>.
		/// </summary>
		/// <returns><see cref="Object"/></returns>
		public object GetAsObject()
		{
			return Value;
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into an <see cref="Object"/>.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns><see cref="Object"/></returns>
		public object GetAsObject(int index)
		{
			return this[index];
		}

		/// <summary>
		/// Convert the value of this field into a <see cref="string"/>.
		/// </summary>
		/// <returns><see cref="string"/></returns>
		public string GetAsString()
		{
			return (string)GetAsType(typeof(string));
		}

		/// <summary>
		/// Convert the nth value of this multi-valued field into a <see cref="string"/>.
		/// </summary>
		/// <param name="index">The index of the value to retrieve.</param>
		/// <returns><see cref="string"/></returns>
		public string GetAsString(int index)
		{
			return (string)GetAsType(index, typeof(string));
		}
		#endregion
	}
}
