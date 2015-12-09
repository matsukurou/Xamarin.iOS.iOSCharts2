using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace iOSCharts
{
	public class ChartDataSet<T> : NSObject where T : ChartDataEntry
	{
        public List<UIColor> colors = new List<UIColor>();
        internal List<T> _yVals = new List<T>();
            internal double _yMax;
        internal double _yMin;
        internal double _yValueSum;

        /// the last start value used for calcMinMax
        internal int _lastStart;

        /// the last end value used for calcMinMax
        internal int _lastEnd;

        public string label = "DataSet";
        public bool visible = true;
        public bool drawValuesEnabled = true;

        /// the color used for the value-text
        public UIColor valueTextColor = UIColor.Black;

        /// the font for the value-text labels
        public UIFont valueFont = UIFont.SystemFontOfSize(7.0f);

        /// the formatter used to customly format the values
        internal NSNumberFormatter _valueFormatter = ChartUtils.defaultValueFormatter();

        /// the axis this DataSet should be plotted against.
        public ChartYAxis.AxisDependency axisDependency = ChartYAxis.AxisDependency.Left;

        public List<T> yVals { get { return _yVals; } }
        public double yValueSum { get { return _yValueSum; } }
        public double yMin { get { return _yMin; } }
        public double yMax { get { return _yMax; } }

        /// if true, value highlighting is enabled
        public bool highlightEnabled = true;

        /// - returns: true if value highlighting is enabled for this dataset
        public bool isHighlightEnabled { get { return highlightEnabled; } }

		public ChartDataSet()
		{
		}

        public ChartDataSet(List<T> yVals, string label)
        {
            this.label = label;
            this._yVals = yVals;

            colors.Add(new UIColor(140.0f / 255.0f, 234.0f / 255.0f, 255.0f / 255.0f, 1.0f));

            this.calcMinMax(_lastStart, _lastEnd);
            this.calcYValueSum();
        }

        public ChartDataSet(List<T> yVals)
        {
            this.label = "";
            this._yVals = yVals;

            colors.Add(new UIColor(140.0f / 255.0f, 234.0f / 255.0f, 255.0f / 255.0f, 1.0f));

            this.calcMinMax(_lastStart, _lastEnd);
            this.calcYValueSum();
        }

        /// Use this method to tell the data set that the underlying data has changed
        public void notifyDataSetChanged()
        {
            calcMinMax(_lastStart, _lastEnd);
            calcYValueSum();
        }

        internal void calcMinMax(int start ,int end)
        {
            var yValCount = _yVals.Count;

            if (yValCount == 0)
                return;

            var endValue = end;

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
				var e = _yVals[i];
				if (!Double.IsNaN(e.value))
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
			}

			if (_yMin == Double.MaxValue)
			{
				_yMin = 0.0;
				_yMax = 0.0;
			}
        }

        private void calcYValueSum()
        {
            _yValueSum = 0;

            for (var i = 0; i < _yVals.Count; i++)
            {
                _yValueSum += Math.Abs(_yVals[i].value);
            }
        }

        /// - returns: the average value across all entries in this DataSet.
        public double average
        {
            get
            {
                return yValueSum / (double)valueCount;
            }
        }

        public int entryCount { get { return _yVals.Count; } }

        public double yValForXIndex(int x)
        {
            var e = this.entryForXIndex(x);

            if (e != null && e.xIndex == x)
            { 
                return e.value;
            }
            else
            { 
                return Double.NaN;
            }
        }

        /// - returns: the first Entry object found at the given xIndex with binary search.
        /// If the no Entry at the specifed x-index is found, this method returns the Entry at the closest x-index. 
        /// nil if no Entry object at that index.
        public ChartDataEntry entryForXIndex(int x)
        {
            var index = this.entryIndex(x);
            if (index > -1)
            {
                return _yVals[index];
            }
            return null;
        }

        public List<ChartDataEntry> entriesForXIndex(int x)
        {
            var entries = new List<ChartDataEntry>();

            var low = 0;
            var high = _yVals.Count - 1;

            while (low <= high)
            {
                var m = (int)((high + low) / 2);
                var entry = _yVals[m];

                if (x == entry.xIndex)
                {
                    while (m > 0 && _yVals[m - 1].xIndex == x)
                    {
                        m--;
                    }

                    high = _yVals.Count;
                    for (int i = m; i < high; i++)
                    {
                        entry = _yVals[i];
                        if (entry.xIndex == x)
                        {
                            entries.Add(entry);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (x > _yVals[m].xIndex)
                {
                    low = m + 1;
                }
                else
                {
                    high = m - 1;
                }
            }

            return entries;
        }

        public int entryIndex(int x)
        {
            var low = 0;
            var high = _yVals.Count - 1;
            var closest = -1;

            while (low <= high)
            {
                var m = (high + low) / 2;
                var entry = _yVals[m];

                if (x == entry.xIndex)
                {
                    while (m > 0 && _yVals[m - 1].xIndex == x)
                    {
                        m--;
                    }

                    return m;
                }

                if (x > entry.xIndex)
                {
                    low = m + 1;
                }
                else
                {
                    high = m - 1;
                }

                closest = m;
            }

            return closest;
        }

        public int entryIndex(ChartDataEntry e, bool isEqual)
        {
            if (isEqual)
            {
                for (var i = 0; i < _yVals.Count; i++)
                {
                    if (_yVals[i].Equals(e))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (var i = 0; i < _yVals.Count; i++)
                {
                    if (_yVals[i] == e)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// the formatter used to customly format the values
        public NSNumberFormatter valueFormatter
        {
            get
            {
                return _valueFormatter;
            }
            set
            {
                _valueFormatter = ChartUtils.defaultValueFormatter();
                if (value != null)
                {
                    _valueFormatter = value;
                }
            }
        }

        /// - returns: the number of entries this DataSet holds.
        public int valueCount { get { return _yVals.Count; }}

        /// Adds an Entry to the DataSet dynamically.
        /// Entries are added to the end of the list.
        /// This will also recalculate the current minimum and maximum values of the DataSet and the value-sum.
        /// - parameter e: the entry to add
        public void addEntry(T e)
        {
            var val = e.value;

            if (_yVals == null)
            {
                _yVals = new List<T>();
            }

            if (_yVals.Count == 0)
            {
                _yMax = val;
                _yMin = val;
            }
            else
            {
                if (_yMax < val)
                {
                    _yMax = val;
                }
                if (_yMin > val)
                {
                    _yMin = val;
                }
            }

            _yValueSum += val;

            _yVals.Add(e);
        }

        /// Adds an Entry to the DataSet dynamically.
        /// Entries are added to their appropriate index respective to it's x-index.
        /// This will also recalculate the current minimum and maximum values of the DataSet and the value-sum.
        /// - parameter e: the entry to add
        public void addEntryOrdered(T e)
        {
            var val = e.value;

            if (_yVals == null)
            {
                _yVals = new List<T>();
            }

            if (_yVals.Count == 0)
            {
                _yMax = val;
                _yMin = val;
            }
            else
            {
                if (_yMax < val)
                {
                    _yMax = val;
                }
                if (_yMin > val)
                {
                    _yMin = val;
                }
            }

            _yValueSum += val;

            if (_yVals[_yVals.Count - 1].xIndex > e.xIndex)
            {
                var closestIndex = entryIndex(e.xIndex);
                if (_yVals[closestIndex].xIndex < e.xIndex)
                {
                    closestIndex++;
                }
                _yVals.Insert(e.xIndex, e);
                return;
            }

            _yVals.Add(e);
        }

        public bool removeEntry(ChartDataEntry entry)
        {
            var removed = false;

            for (var i = 0; i < _yVals.Count; i++)
            {
                if (_yVals[i] == entry)
                {
                    _yVals.RemoveAt(i);
                    removed = true;
                    break;
                }
            }

            if (removed)
            {
                _yValueSum -= entry.value;
                calcMinMax(_lastStart, _lastEnd);
            }

            return removed;
        }

        public bool removeEntry(int xIndex)
        {
            var index = this.entryIndex(xIndex);
            if (index > -1)
            {
                var e = _yVals[index];
                _yValueSum -= e.value;

                _yVals.RemoveAt(index);

                calcMinMax(_lastStart, _lastEnd);

                return true;
            }

            return false;
        }

        /// Removes the first Entry (at index 0) of this DataSet from the entries array.
        ///
        /// - returns: true if successful, false if not.
        public bool removeFirst()
        {
            if (_yVals.Count != 0)
            {
                var entry = _yVals[0];
                var val = entry.value;
                _yValueSum -= val;

                _yVals.RemoveAt(0);

                calcMinMax(_lastStart, _lastEnd);
            }
            return true;
        }

        /// Removes the last Entry (at index size-1) of this DataSet from the entries array.
        ///
        /// - returns: true if successful, false if not.
        public bool removeLast()
        {
            if (_yVals.Count != 0)
            {
                var entry = _yVals[_yVals.Count - 1];
                var val = entry.value;
                _yValueSum -= val;

                _yVals.RemoveAt(_yVals.Count - 1);

                calcMinMax(_lastStart, _lastEnd);
            }
            return true;
        }

        public void resetColors()
        {
            colors.Clear();
        }

        public void addColor(UIColor color)
        {
            colors.Add(color);
        }

        public void setColor(UIColor color)
        {
            colors.Clear();
            colors.Add(color);
        }

        public UIColor colorAt(int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            return colors[index % colors.Count];
        }

        public bool isVisible
        {
            get { return visible; }
        }

        public bool isDrawValuesEnabled
        {
            get { return drawValuesEnabled; }
        }

        /// Checks if this DataSet contains the specified Entry.
        /// - returns: true if contains the entry, false if not.
        public bool contains(T e)
        {
            if (_yVals.Contains(e))
                return true;

            return false;
            
        }

        /// Removes all values from this DataSet and recalculates min and max value.
        public void clear()
        {
            _yVals.Clear();
            _lastStart = 0;
            _lastEnd = 0;
            notifyDataSetChanged();
        }

        // MARK: NSObject
		#if false
        public override string Description
        {
            get
            {
                return String.Format("ChartDataSet, label: %@, %i entries", this.label ?? "", _yVals.Count);
            }
        }

        public override string DebugDescription
        {
            get
            {
                var desc = Description + ":";

                for (var i = 0; i < _yVals.Count; i++)
                {
                    desc += "\n" + _yVals[i].Description;
                }

                return desc;
            }
        }
		#endif
            
        // MARK: NSCopying

		public ChartDataSet<T> copyWithZone (NSZone zone)
        {
			var copy = new ChartDataSet<T>();

            copy.colors = colors;
            copy._yVals = _yVals;
            copy._yMax = _yMax;
            copy._yMin = _yMin;
            copy._yValueSum = _yValueSum;
            copy._lastStart = _lastStart;
            copy._lastEnd = _lastEnd;
            copy.label = label;

            return copy;
        }
        
	}
}

