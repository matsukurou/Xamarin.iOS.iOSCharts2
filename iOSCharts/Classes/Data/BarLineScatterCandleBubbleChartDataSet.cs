using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace iOSCharts
{
	public class BarLineScatterCandleBubbleChartDataSet<T> : ChartDataSet<T> where T : ChartDataEntry
	{
		public UIColor highlightColor = new UIColor(255.0f/255.0f, 187.0f/255.0f, 115.0f/255.0f, 1.0f);
		public nfloat highlightLineWidth = 0.5f;
		public nfloat highlightLineDashPhase;
		public List<nfloat> highlightLineDashLengths = new List<nfloat>();

		public BarLineScatterCandleBubbleChartDataSet()
		{
		}

		public BarLineScatterCandleBubbleChartDataSet(List<T> yVals, string label)
			: base (yVals, label)
		{
		}

			// MARK: NSCopying

		public new BarLineScatterCandleBubbleChartDataSet<T> copyWithZone(NSZone zone)
		{
			var copy = base.copyWithZone(zone) as BarLineScatterCandleBubbleChartDataSet<T>;
			copy.highlightColor = highlightColor;
			copy.highlightLineWidth = highlightLineWidth;
			copy.highlightLineDashPhase = highlightLineDashPhase;
			copy.highlightLineDashLengths = highlightLineDashLengths;
			return copy;
		}
			
	}
}

