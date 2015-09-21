using System.Configuration;

namespace CrownPeak.SearchG2.Common
{
	internal static class AppConfiguration
	{
		private const string KEY_PREFIX = "searchg2.";

		public static string ReadString(string key, string defaultValue = null)
		{
			string value = ConfigurationManager.AppSettings[TidyKeyName(key)];
			if (!string.IsNullOrWhiteSpace(value)) return value;
			return defaultValue;
		}

		public static bool? ReadBoolean(string key, bool? defaultValue = null)
		{
			bool value;
			if (bool.TryParse(ReadString(key), out value)) return value;
			return defaultValue;
		}

		public static decimal? ReadDecimal(string key, decimal? defaultValue = null)
		{
			decimal value;
			if (decimal.TryParse(ReadString(key), out value)) return value;
			return defaultValue;
		}

		public static double? ReadDouble(string key, double? defaultValue = null)
		{
			double value;
			if (double.TryParse(ReadString(key), out value)) return value;
			return defaultValue;
		}

		public static int? ReadInt(string key, int? defaultValue = null)
		{
			int value;
			if (int.TryParse(ReadString(key), out value)) return value;
			return defaultValue;
		}

		private static string TidyKeyName(string key)
		{
			if (!key.ToLower().StartsWith(KEY_PREFIX)) return string.Concat(KEY_PREFIX, key);
			return key;
		}
	}
}
