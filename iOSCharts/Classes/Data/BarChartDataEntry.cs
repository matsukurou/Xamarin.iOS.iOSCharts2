using System;
using System.Collections.Generic;
using Foundation;

namespace iOSCharts
{
	public class BarChartDataEntry : ChartDataEntry
	{
		/// the values the stacked barchart holds
		private List<double> _values = new List<double>();

		/// the sum of all negative values this entry (if stacked) contains
		private double _negativeSum;

		/// the sum of all positive values this entry (if stacked) contains
		private double _positiveSum;

		public BarChartDataEntry() : base()
		{
		}

		/// Constructor for stacked bar entries.
		public BarChartDataEntry(List<double> values, int xIndex) 
			: base (BarChartDataEntry.calcSum(values), xIndex)
		{
			this.values = values;
			calcPosNegSum();
		}

		/// Constructor for normal bars (not stacked).
		public BarChartDataEntry(double value, int xIndex) 
			: base(value, xIndex)
		{
		}

		/// Constructor for stacked bar entries.
		public BarChartDataEntry(List<double> values, int xIndex, string label) 
			: base (BarChartDataEntry.calcSum(values), xIndex, label)
		{
			this.values = values;
		}

		/// Constructor for normal bars (not stacked).
		public BarChartDataEntry(double value, int xIndex, BarChartDataEntry data) 
			: base(value, xIndex, data)
		{
		}

		public double getBelowSum(int stackIndex)
		{
			if (values == null)
			{
				return 0;
				}

			var remainder = 0.0d;
			var index = values.Count - 1;

				while (index > stackIndex && index >= 0)
					{
				remainder += values[index];
				index--;
					}

			return remainder;
		}

		/// - returns: the sum of all negative values this entry (if stacked) contains. (this is a positive number)
		public double negativeSum
		{
			get { return _negativeSum; }
		}

		/// - returns: the sum of all positive values this entry (if stacked) contains.
		public double positiveSum
		{
			get { return _positiveSum; }
		}

		public void calcPosNegSum()
		{
			if (_values == null)
			{
				this._positiveSum = 0.0d;
				this._negativeSum = 0.0d;
				return;
			}

			var sumNeg = 0.0d;
			var sumPos = 0.0d;

			foreach (var f in _values)
			{
				if (f < 0.0)
				{
					sumNeg += -f;
				}
				else
				{
					sumPos += f;
				}
			}

			_negativeSum = sumNeg;
			_positiveSum = sumPos;
		}

		// MARK: Accessors

		/// the values the stacked barchart holds
		public bool isStacked { get { return _values != null; } }

		/// the values the stacked barchart holds
		public List<double> values
		{
			get { return this._values; }
			set
			{
				this.value = BarChartDataEntry.calcSum(value);
				this._values = value;
				calcPosNegSum();
			}
		}

		// MARK: NSCopying

		public new BarChartDataEntry copyWithZone(NSZone zone)
		{
			var copy = base.copyWithZone(zone) as BarChartDataEntry;
			copy._values = _values;
			copy.value = value;
			copy._negativeSum = _negativeSum;
			return copy;
		}

		/// Calculates the sum across all values of the given stack.
		///
		/// - parameter vals:
		/// - returns:
		private static double calcSum(List<double> vals)
		{
			if (vals == null)
			{
				return 0.0d;
			}

			var sum = 0.0d;

			foreach (var f in vals)
			{
				sum += f;
			}

			return sum;
		}
	}
}

