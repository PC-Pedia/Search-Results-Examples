using System;
using System.Collections.Generic;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Interface to describe a single result Field from a 
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/>.
	/// </summary>
	public interface IField : IEnumerable<object>
	{
		/// <summary>The name of this <see cref="Field"/>.</summary>
		string Name { get; }
		/// <summary>Array of values for this <see cref="Field"/>.</summary>
		object[] Values { get; }
		/// <summary>Value for this <see cref="Field"/>.</summary>
		object Value { get; }
		/// <summary>Count of the number of values currently held in this <see cref="Field"/>.</summary>
		int Count { get; }
		/// <summary>Collection of highlights currently held in this <see cref="Field"/>.</summary>
		string[] Highlights { get; }

		/// <summary>Convert the value into a specified <see cref="Type"/>.</summary>
		/// <param name="type">The <see cref="Type"/> to be returned.</param>
		/// <returns><see cref="Type"/> requested.</returns>
		object GetAsType(Type type);

		/// <summary>Convert the nth value of this multi-valued <see cref="Field"/> into a specified <see cref="Type"/>.</summary>
		/// <param name="index">The index of the value to return.</param>
		/// <param name="type">The <see cref="Type"/> to be returned.</param>
		/// <returns><see cref="Type"/> requested.</returns>
		object GetAsType(int index, Type type);
	}
}
