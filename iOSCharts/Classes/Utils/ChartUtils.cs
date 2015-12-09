using System;
using System.Collections.Generic;

using Foundation;

namespace iOSCharts
{
    public class ChartUtils
    {
        private static NSNumberFormatter _defaultValueFormatter = ChartUtils.generateDefaultValueFormatter();

        public ChartUtils()
        {
        }

        private static NSNumberFormatter generateDefaultValueFormatter()
        {
            var formatter = new NSNumberFormatter();
            formatter.MinimumIntegerDigits = 1;
            formatter.MaximumFractionDigits = 1;
            formatter.MinimumFractionDigits = 1;
            formatter.UsesGroupingSeparator = true;
            return formatter;
        }

        /// - returns: the default value formatter used for all chart components that needs a default
        public static NSNumberFormatter defaultValueFormatter()
        {
            return _defaultValueFormatter;
        }
			        
		// SwiftではListに複数の型を格納できる。C#でこれはできないため、一旦コメントアウト
		internal static List<object> bridgedObjCGetStringArray (List<string> array)
		{
			var newArray = new List<object>();
			foreach (var val in array)
					{
						if (val == null)
						{
					newArray.Add(new NSNull());
						}
						else
						{
					newArray.Add(val);
						}
			}
			return newArray;
		}

		internal static List<string> bridgedObjCGetStringArray (List<object> array)
		{
			var newArray = new List<String>();
			foreach (var obj in array)
			{
				newArray.Add(obj as string);
			}
			return newArray;
		}
    }
}

