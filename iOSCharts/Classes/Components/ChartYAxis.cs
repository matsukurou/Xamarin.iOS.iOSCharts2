using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;

namespace iOSCharts
{
    public class ChartYAxis : ChartAxisBase
    {
        public enum YAxisLabelPosition : int
        {
            OutsideChart,
            InsideChart,
        };

        ///  Enum that specifies the axis a DataSet should be plotted against, either Left or Right.
        public enum AxisDependency : int
        {
            Left,
            Right,
        };

        public List<double> entries = new List<double>();
        public int entryCount { get { return entries.Count; } }

            /// the number of y-label entries the y-labels should have, default 6
        private int _labelCount = 6;

            /// indicates if the top y-label entry is drawn or not
        public bool drawTopYLabelEntryEnabled = true;

            /// if true, the y-labels show only the minimum and maximum value
        public bool showOnlyMinMaxEnabled = false;

            /// flag that indicates if the axis is inverted or not
        public bool inverted = false;

            /// if true, the y-label entries will always start at zero
        public bool startAtZeroEnabled = true;

            /// if true, the set number of y-labels will be forced
        public bool forceLabelsEnabled = false;

            /// the formatter used to customly format the y-labels
        public NSNumberFormatter valueFormatter;

            /// the formatter used to customly format the y-labels
        internal NSNumberFormatter _defaultValueFormatter = new NSNumberFormatter();

            /// A custom minimum value for this axis. 
            /// If set, this value will not be calculated automatically depending on the provided data. 
            /// Use `resetCustomAxisMin()` to undo this.
            /// Do not forget to set startAtZeroEnabled = false if you use this method.
            /// Otherwise, the axis-minimum value will still be forced to 0.
        public double customAxisMin = Double.NaN;

            /// Set a custom maximum value for this axis. 
            /// If set, this value will not be calculated automatically depending on the provided data. 
            /// Use `resetCustomAxisMax()` to undo this.
        public double customAxisMax = Double.NaN;

            /// axis space from the largest value to the top in percent of the total axis range
        public nfloat spaceTop = 0.1f;

            /// axis space from the smallest value to the bottom in percent of the total axis range
        public nfloat spaceBottom = 0.1f;

        public double axisMaximum = 0;
        public double axisMinimum = 0;

            /// the total range of values this axis covers
        public double axisRange = 0;

            /// the position of the y-labels relative to the chart
        public YAxisLabelPosition labelPosition = YAxisLabelPosition.OutsideChart;

            /// the side this axis object represents
        private AxisDependency _axisDependency = AxisDependency.Left;

            /// the minimum width that the axis should take
            /// 
            /// **default**: 0.0
        public nfloat minWidth = 0;

            /// the maximum width that the axis can take.
            /// use zero for disabling the maximum
            /// 
            /// **default**: 0.0 (no maximum specified)
        public nfloat maxWidth = 0;

        public ChartYAxis()
        {
            _defaultValueFormatter.MinimumIntegerDigits = 1;
            _defaultValueFormatter.MaximumFractionDigits = 1;
            _defaultValueFormatter.MinimumFractionDigits = 1;
            _defaultValueFormatter.UsesGroupingSeparator = true;
            this.yOffset = 0.0f;
        }

        public ChartYAxis(AxisDependency position)
        {
            _axisDependency = position;

            _defaultValueFormatter.MinimumIntegerDigits = 1;
            _defaultValueFormatter.MaximumFractionDigits = 1;
            _defaultValueFormatter.MinimumFractionDigits = 1;
            _defaultValueFormatter.UsesGroupingSeparator = true;
            this.yOffset = 0.0f;
        }

        public AxisDependency axisDependency
        {
            get { return _axisDependency; }
        }

            public void setLabelCount(int count, bool force)
        {
            _labelCount = count;

            if (_labelCount > 25)
            {
                _labelCount = 25;
            }
            if (_labelCount < 2)
            {
                _labelCount = 2;
            }

            forceLabelsEnabled = force;
        }

        /// the number of label entries the y-axis should have
        /// max = 25,
        /// min = 2,
        /// default = 6,
        /// be aware that this number is not fixed and can only be approximated
        public int labelCount
        {
            get
            {
                return _labelCount;
            }
            set
            {
                setLabelCount(value, false);
            }
        }

        /// By calling this method, any custom minimum value that has been previously set is reseted, and the calculation is done automatically.
        public void resetCustomAxisMin()
        {
            customAxisMin = Double.NaN;
        }

        /// By calling this method, any custom maximum value that has been previously set is reseted, and the calculation is done automatically.
        public void resetCustomAxisMax()
        {
            customAxisMax = Double.NaN;
        }

        public CGSize requiredSize()
        {
            var label = getLongestLabel();
            var size = label.StringSize(labelFont);
            size.Width += (nfloat)(xOffset * 2.0);
            size.Height += (nfloat)(yOffset * 2.0);
            size.Width = (nfloat)Math.Max(minWidth, Math.Min(size.Width, maxWidth > 0.0 ? maxWidth : size.Width));
            return size;
        }

        public nfloat getRequiredHeightSpace()
        {
            return (nfloat)(requiredSize().Height + 2.5 * 2.0 + yOffset);
        }

        public new string getLongestLabel()
        {
            var longest = string.Empty;

            for (var i = 0; i < entries.Count; i++)
            {
                var text = getFormattedLabel(i);

                if (longest.Length < text.Length)
                {
                    longest = text;
                }
            }

            return longest;
        }

        /// - returns: the formatted y-label at the specified index. This will either use the auto-formatter or the custom formatter (if one is set).
        public string getFormattedLabel(int index)
        {
            if (index < 0 || index >= entries.Count)
            {
                return string.Empty;
            }

            return (valueFormatter ?? _defaultValueFormatter).StringFromNumber(entries[index]);
        }

        /// - returns: true if this axis needs horizontal offset, false if no offset is needed.
        public bool needsOffset
        {
            get
            {
                if (isEnabled && isDrawLabelsEnabled && labelPosition == YAxisLabelPosition.OutsideChart)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isInverted { get { return inverted; } }

        public bool isStartAtZeroEnabled { get { return startAtZeroEnabled; } }

            /// - returns: true if focing the y-label count is enabled. Default: false
        public bool isForceLabelsEnabled { get { return forceLabelsEnabled; } }

        public bool isShowOnlyMinMaxEnabled { get { return showOnlyMinMaxEnabled; } }

        public bool isDrawTopYLabelEntryEnabled { get { return drawTopYLabelEntryEnabled; } }
    }
}

