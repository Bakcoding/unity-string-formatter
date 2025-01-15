using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace DinoLabs
{
    public static class StringFormmatter
    {
        #region Comma String
        public static string ToCommaString(string val, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty(val))
                throw new ArgumentException("Input string cannot be null or empty", nameof(val));

            culture = culture ?? CultureInfo.InvariantCulture;

            if (int.TryParse(val, out int iVal))
                return iVal.ToString("N0", culture);

            if (long.TryParse(val, out long lVal))
                return lVal.ToString("N0", culture);

            if (float.TryParse(val, out float fVal))
                return fVal.ToString("N0", culture);

            if (double.TryParse(val, out double dVal))
                return dVal.ToString("N0", culture);

            if (decimal.TryParse(val, out decimal decVal))
                return decVal.ToString("N0", culture);

            throw new FormatException($"Unable to parse the input string: {val}");
        }

        public static string ToCommaString(int val, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            return val.ToString("N0", culture);
        }
        public static string ToCommaString(long val, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            return val.ToString("N0", culture);
        }
        public static string ToCommaString(float val, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            return val.ToString("N0", culture);
        }
        public static string ToCommaString(double val, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            return val.ToString("N0", culture);
        }
        public static string ToCommaString(decimal val, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.InvariantCulture;

            return val.ToString("N0", culture);
        }
        #endregion

        #region Compact String
        private static readonly (decimal Value, string Unit)[] DefaultUnits = new[]
        {
            (1_000_000_000_000_000M, "Q"),
            (1_000_000_000_000M, "T"),
            (1_000_000_000M, "B"),
            (1_000_000M, "M"),
            (1_000M, "K")
        };

        //# Custom Units 
        //var customUnits = new[]
        //{
        //    (1000000000M, "B"),
        //    (1000000M, "Million"),
        //    (1000M, "Thousand")
        //};

        /// <summary>
        /// Converts a number into a simplified string with units (K, M, B, T, Q).
        /// </summary>
        /// <typeparam name="T">The type of the number to convert (int, long, float, double, decimal).</typeparam>
        /// <param name="val">The numeric value to convert.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <param name="upperCase">Specifies whether to display the unit in uppercase (default: true).</param>
        /// <param name="rounding">The rounding mode to apply (default: MidpointRounding.AwayFromZero).</param>
        /// <param name="units">Custom unit array to use (default: null - uses standard units).</param>
        /// <returns>A string with the unit included (e.g., "1.5K", "2.3M", "1.2B").</returns>
        /// <exception cref="ArgumentException">Thrown if the provided numeric type is not supported.</exception>
        public static string ToCompactString<T>(
               T val,
               int decimals = 1,                        
               bool upperCase = true,                   
               MidpointRounding rounding = MidpointRounding.AwayFromZero, 
               (decimal Value, string Unit)[] units = null) 
        {
            decimal decimalValue;

            // Convert to decimal
            if (val is string strVal)
            {
                if (string.IsNullOrEmpty(strVal))
                    throw new ArgumentException("Input string cannot be null or empty", nameof(val));

                if (!decimal.TryParse(strVal, out decimalValue))
                    throw new FormatException($"Unable to parse the input string: {strVal}");
            }
            else if (val is int intVal)
                decimalValue = Convert.ToDecimal(intVal);
            else if (val is long longVal)
                decimalValue = Convert.ToDecimal(longVal);
            else if (val is float floatVal)
                decimalValue = Convert.ToDecimal(floatVal);
            else if (val is double doubleVal)
                decimalValue = Convert.ToDecimal(doubleVal);
            else if (val is decimal decVal)
                decimalValue = decVal;
            else
                throw new ArgumentException("Unsupported numeric type", nameof(val));

            // Use default units if not specified
            units ??= DefaultUnits;

            decimal absValue = Math.Abs(decimalValue);
            string sign = decimalValue < 0 ? "-" : "";

            // Format number with specified units
            foreach (var (unitValue, unitSymbol) in units)
            {
                if (absValue >= unitValue)
                {
                    var divided = decimalValue / unitValue;
                    int factor = (int)Math.Pow(10, decimals);
                    var truncated = Math.Truncate(divided * factor) / factor;
                    var unit = upperCase ? unitSymbol.ToUpper() : unitSymbol.ToLower();

                    return string.Format("{0}{1:F" + decimals + "}{2}", sign, truncated, unit);
                }
            }

            // If no unit matches, return the plain number
            return Math.Round(decimalValue, decimals, rounding).ToString($"F{decimals}");
        }
        #endregion

        #region Data Size String

        /// <summary>
        /// Converts a byte size into bytes.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A string representing the file size in B (e.g., "1.5B").</returns>
        public static string ToFileSizeByte<T>(this T bytes, int decimals = 1, bool upperCase = true) where T : struct
        {
            decimal size;
            try
            {
                size = Convert.ToDecimal(bytes);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unsupported numeric type", nameof(bytes));
            }

            var unit = upperCase ? "MB" : "mb";
            return string.Format("{0:F" + decimals + "}{1}", size, unit);
        }

        /// <summary>
        /// Converts a byte size into kilobytes.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A string representing the file size in KB (e.g., "1.5KB").</returns>
        public static string ToFileSizeKB<T>(this T bytes, int decimals = 1, bool upperCase = true) where T : struct
        {
            decimal size;
            try
            {
                size = Convert.ToDecimal(bytes) / 1024;
            }
            catch (Exception)
            {
                throw new ArgumentException("Unsupported numeric type", nameof(bytes));
            }

            var unit = upperCase ? "MB" : "mb";
            return string.Format("{0:F" + decimals + "}{1}", size, unit);
        }

        /// <summary>
        /// Converts a byte size into megabytes.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A string representing the file size in MB (e.g., "1.5MB").</returns>
        public static string ToFileSizeMB<T>(this T bytes, int decimals = 1, bool upperCase = true) where T : struct
        {
            decimal size;
            try
            {
                size = Convert.ToDecimal(bytes) / (1024 * 1024);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unsupported numeric type", nameof(bytes));
            }

            var unit = upperCase ? "MB" : "mb";
            return string.Format("{0:F" + decimals + "}{1}", size, unit);
        }

        /// <summary>
        /// Converts a byte size into gigabytes.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A string representing the file size in GB (e.g., "1.5GB").</returns>
        public static string ToFileSizeGB<T>(this T bytes, int decimals = 1, bool upperCase = true) where T : struct
        {
            decimal size;
            try
            {
                size = Convert.ToDecimal(bytes) / (1024 * 1024 * 1024);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unsupported numeric type", nameof(bytes));
            }

            var unit = upperCase ? "GB" : "gb";
            return string.Format("{0:F" + decimals + "}{1}", size, unit);
        }

        private static readonly string[] SizeUnits =
        {
            "B","KB","MB","GB"
        };

        /// <summary>
        /// Converts a byte size into a human-readable string with appropriate units.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A string representing the file size with units (e.g., "1.5MB").</returns>
        public static string ToFileSize<T>(this T bytes, int decimals = 1, bool upperCase = true, string[] sizeUnits = null) where T : struct
        {
            decimal size;
            try
            {
                size = Convert.ToDecimal(bytes);
            }
            catch (Exception)
            {
                throw new ArgumentException("Unsupported numeric type", nameof(bytes));
            }

            if (size <= 0)
                return "0B";

            int unitIndex = 0;

            sizeUnits ??= SizeUnits;

            while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            var unit = upperCase ? sizeUnits[unitIndex].ToUpper() : sizeUnits[unitIndex].ToLower();
            return string.Format("{0:F" + decimals + "}{1}", size, unit);
        }

        /// <summary>
        /// Converts a byte size into a human-readable string with appropriate units and includes the original byte size.
        /// </summary>
        /// <typeparam name="T">The numeric type of the value to convert (e.g., int, uint, long, ulong, etc.).</typeparam>
        /// <param name="bytes">The size in bytes.</param>
        /// <param name="decimals">The number of decimal places to include (default: 1).</param>
        /// <returns>A detailed string representing the file size with units and the original byte size (e.g., "1.5MB (1,572,864 bytes)").</returns>
        public static string ToDetailedFileSize<T>(this T bytes, int decimals = 1) where T : struct
        {
            if (Convert.ToDecimal(bytes) <= 0)
                return "0B (0 bytes)";

            string sizeStr = ToFileSize(bytes, decimals);
            return string.Format("{0} ({1:N0} bytes)", sizeStr, bytes);
        }
        #endregion

        #region Time String
        private const string DEFAULT_TIME_SEPERATOR = ":";
        /// <summary>
        /// Converts seconds to time string with customizable format.
        /// </summary>
        /// <param name="seconds">The total seconds to convert.</param>
        /// <param name="showZeroUnits">If true, shows units even when they are zero.</param>
        /// <param name="separator">The separator between time units.</param>
        /// <param name="separateDays">If true, shows days separately. If false, adds days to hours.</param>
        /// <returns>Formatted time string.</returns>
        public static string ToTimeString(this float seconds,
            bool showZeroUnits = true,
            string separator = DEFAULT_TIME_SEPERATOR,
            bool separateDays = true)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            if (separateDays)
            {
                if (showZeroUnits)
                {
                    if (time.Days > 0)
                        return string.Format("{0}{4}{1:00}{4}{2:00}{4}{3:00}",
                            time.Days,
                            time.Hours,
                            time.Minutes,
                            time.Seconds,
                            separator);

                    return string.Format("{0:00}{3}{1:00}{3}{2:00}",
                        time.Hours,
                        time.Minutes,
                        time.Seconds,
                        separator);
                }
                else
                {
                    if (time.Days > 0)
                        return string.Format("{0}{4}{1:00}{4}{2:00}{4}{3:00}",
                            time.Days,
                            time.Hours,
                            time.Minutes,
                            time.Seconds,
                            separator);
                    if (time.Hours > 0)
                        return string.Format("{0}{3}{1:00}{3}{2:00}",
                            time.Hours,
                            time.Minutes,
                            time.Seconds,
                            separator);
                    if (time.Minutes > 0)
                        return string.Format("{0}{2}{1:00}",
                            time.Minutes,
                            time.Seconds,
                            separator);

                    return time.Seconds.ToString();
                }
            }
            else
            {
                int totalHours = (time.Days * 24) + time.Hours;

                if (showZeroUnits)
                {
                    return string.Format("{0:00}{3}{1:00}{3}{2:00}",
                        totalHours,
                        time.Minutes,
                        time.Seconds,
                        separator);
                }
                else
                {
                    if (totalHours > 0)
                        return string.Format("{0}{3}{1:00}{3}{2:00}",
                            totalHours,
                            time.Minutes,
                            time.Seconds,
                            separator);
                    if (time.Minutes > 0)
                        return string.Format("{0}{2}{1:00}",
                            time.Minutes,
                            time.Seconds,
                            separator);

                    return time.Seconds.ToString();
                }
            }
        }

        private static readonly (char Key, string Unit)[] TimeUnits = new[]
        {
            ('d', "Day"),
            ('h', "Hour"),
            ('m', "Minute"),
            ('s', "Second")
        };

        /// <summary>
        /// Converts seconds to time string with unit labels.
        /// </summary>
        /// <param name="seconds">The total seconds to convert.</param>
        /// <param name="showZeroUnits">If true, shows units even when they are zero.</param>
        /// <param name="separateDays">If true, shows days separately. If false, adds days to hours.</param>
        /// <param name="useShortUnit">If true, uses short unit format (d,h,m,s). If false, uses full unit names.</param>
        /// <param name="customTimeUnits">Optional custom time units. If null, uses default TimeUnits.</param>
        /// <returns>Formatted time string with unit labels.</returns>
        public static string ToTimeStringWithLabels(this float seconds,
            bool showZeroUnits = false,
            bool separateDays = true,
            bool useShortUnit = true,
            (char Key, string Unit)[] customTimeUnits = null)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            List<string> parts = new List<string>();
            
            customTimeUnits ??= TimeUnits;

            if (separateDays)
            {
                if (showZeroUnits || time.Days > 0)
                    parts.Add($"{time.Days:00}{GetUnitLabel(customTimeUnits[0].Key, useShortUnit, customTimeUnits)}");
                if (showZeroUnits || time.Days > 0 || time.Hours > 0)
                    parts.Add($"{time.Hours:00}{GetUnitLabel(customTimeUnits[1].Key, useShortUnit, customTimeUnits)}");
            }
            else
            {
                int totalHours = (time.Days * 24) + time.Hours;
                if (showZeroUnits || totalHours > 0)
                    parts.Add($"{totalHours:00}{GetUnitLabel(customTimeUnits[1].Key, useShortUnit, customTimeUnits)}");
            }

            if (showZeroUnits || time.Days > 0 || time.Hours > 0 || time.Minutes > 0)
                parts.Add($"{time.Minutes:00}{GetUnitLabel(customTimeUnits[2].Key, useShortUnit, customTimeUnits)}");

            parts.Add($"{time.Seconds:00}{GetUnitLabel(customTimeUnits[3].Key, useShortUnit, customTimeUnits)}");
            return string.Join("", parts);
        }

        private static string GetUnitLabel(char key, bool useShortUnit, (char Key, string Unit)[] units)
        {
            return useShortUnit ? key.ToString() : units.First(x => x.Key == key).Unit;
        }
        #endregion

        #region Percent String
        private const string DEFAULT_PERCENT_UNIT = "%";
        public static string ToPercentString(decimal val, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate(val * factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }
        public static string ToPercentString(double val, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate(val * factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }
        public static string ToPercentString(float val, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate(val * factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }
        public static string ToPercentString(int val, string unit = null)
        {
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{val}{unit}";
        }

        public static string ToPercentStringWithTotal(decimal currVal, decimal totalVal, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate((currVal / totalVal) *factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }

        public static string ToPercentStringWithTotal(double currVal, double totalVal, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate((currVal / totalVal) * factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }

        public static string ToPercentStringWithTotal(float currVal, float totalVal, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var truncated = Math.Truncate((currVal / totalVal) * factor) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }

        public static string ToPercentStringWithTotal(int currVal, int totalVal, int decimals = 1, string unit = null)
        {
            int factor = (int)Math.Pow(10, decimals);
            var val = ((float)currVal / totalVal) * factor;
            var truncated = Math.Truncate(val) / factor;
            unit ??= DEFAULT_PERCENT_UNIT;
            return $"{truncated}{unit}";
        }
        #endregion
    }
}
