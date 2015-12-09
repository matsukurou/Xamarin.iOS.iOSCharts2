using System;
using Foundation;

namespace iOSCharts
{
	public class ChartRange : NSObject
	{
		public double from;
		public double to;

		public ChartRange()
		{
		}

		public ChartRange(double from, double to)
		{
			this.from = from;
			this.to = to;
		}

		/// Returns true if this range contains (if the value is in between) the given value, false if not.
		/// - parameter value:
		public bool contains(double value)
		{
			if (value > from && value <= to)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool isLarger(double value)
		{
			return value > to;
			}

		public bool isSmaller(double value)
		{
			return value < from;
		}
	}
}

