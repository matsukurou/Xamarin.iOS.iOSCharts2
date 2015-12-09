using System;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using UIKit;

namespace iOSCharts
{
	public class BarChartDataSet : BarLineScatterCandleBubbleChartDataSet<BarChartDataEntry>
	{
		/// space indicator between the bars in percentage of the whole width of one value (0.15 == 15% of bar width)
		public nfloat barSpace = 0.15f;

		/// the maximum number of bars that are stacked upon each other, this value
			/// is calculated from the Entries that are added to the DataSet
		private int _stackSize = 1;

		/// the color used for drawing the bar-shadows. The bar shadows is a surface behind the bar that indicates the maximum value
		public UIColor barShadowColor = new UIColor(215.0f/255.0f, 215.0f/255.0f, 215.0f/255.0f, 1.0f);

		/// the alpha value (transparency) that is used for drawing the highlight indicator bar. min = 0.0 (fully transparent), max = 1.0 (fully opaque)
		public nfloat highlightAlpha = 120.0f / 255.0f;

		/// the overall entry count, including counting each stack-value individually
		private int _entryCountStacks;

		/// array of labels used to describe the different values of the stacked bars
		public List<string> stackLabels = new List<string>() { "Stack" };
		
		public BarChartDataSet()
		{
		}

		public BarChartDataSet(List<BarChartDataEntry> yVals, string label)
			: base (yVals, label)
		{
			this.highlightColor = UIColor.Black;

			this.calcStackSize(yVals);
			this.calcEntryCountIncludingStacks(yVals);
		}

		// MARK: NSCopying

		public new BarChartDataSet copyWithZone(NSZone zone)
		{
			var copy = base.copyWithZone(zone) as BarChartDataSet;
			copy.barSpace = barSpace;
			copy._stackSize = _stackSize;
			copy.barShadowColor = barShadowColor;
			copy.highlightAlpha = highlightAlpha;
			copy._entryCountStacks = _entryCountStacks;
			copy.stackLabels = stackLabels;
			return copy;
		}

		/// Calculates the total number of entries this DataSet represents, including
		/// stacks. All values belonging to a stack are calculated separately.
		private void calcEntryCountIncludingStacks(List<BarChartDataEntry> yVals)
		{
			_entryCountStacks = 0;

			for (var i = 0; i < yVals.Count; i++)
			{
				var vals = ((BarChartDataEntry)yVals[i]).values;

				if (vals == null)
				{
					_entryCountStacks++;
				}
				else
				{
					_entryCountStacks += vals.Count;
				}
			}
		}

		/// calculates the maximum stacksize that occurs in the Entries array of this DataSet
		private void calcStackSize(List<BarChartDataEntry> yVals)
		{
			for (var i = 0; i < yVals.Count; i++)
			{				
				var vals = yVals[i].values;
				if (vals.Count > _stackSize)
				{
					_stackSize = vals.Count;
				}
			}
		}
			
		internal new void calcMinMax(int start, int end)
		{
			var yValCount = _yVals.Count;

			if (yValCount == 0)
			{
				return;
			}

			int endValue = end;

			if (end == 0 || end >= yValCount)
			{
				endValue = yValCount - 1;
			}

			_lastStart = start;
			_lastEnd = endValue;

			_yMin = Double.MaxValue;
			_yMax = Double.MinValue;

			for (var i = start; i <= endValue; i++)
			{
				var e = _yVals[i] as BarChartDataEntry;
				if (e.values == null)
				{
					if (e.value < _yMin)
					{
						_yMin = e.value;
					}

					if (e.value > _yMax)
					{
						_yMax = e.value;
					}
				}
				else
				{
					if (-e.negativeSum < _yMin)
					{
						_yMin = -e.negativeSum;
					}

					if (e.positiveSum > _yMax)
					{
						_yMax = e.positiveSum;
					}
				}
			}

			if (_yMin == Double.MaxValue)
			{
				_yMin = 0.0;
				_yMax = 0.0;
			}
		}

		/// - returns: the maximum number of bars that can be stacked upon another in this DataSet.
		public int stackSize
		{
			get { return _stackSize; }
		}

		/// - returns: true if this DataSet is stacked (stacksize > 1) or not.
		public bool isStacked
		{
			get { return _stackSize > 1 ? true : false; }
		}

		/// - returns: the overall entry count, including counting each stack-value individually
		public int entryCountStacks
		{
			get { return _entryCountStacks; }
		}
	}
}

