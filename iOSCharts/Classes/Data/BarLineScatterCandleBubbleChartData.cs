using System;
using System.Collections.Generic;
using Foundation;

namespace iOSCharts
{
	//BarLineScatterCandleBubbleData<T extends BarLineScatterCandleBubbleDataSet<? extends Entry>>
	public class BarLineScatterCandleBubbleChartData<T, W> : ChartData<T, W> where T : BarLineScatterCandleBubbleChartDataSet<W> where W : ChartDataEntry
	{
		public BarLineScatterCandleBubbleChartData()
		{
		}

		public BarLineScatterCandleBubbleChartData(List<string> xVals, List<T> dataSets)
			: base(xVals, dataSets)
		{
		}

		public BarLineScatterCandleBubbleChartData(List<Object> xVals, List<T> dataSets)
			: base(xVals, dataSets)
		{
		}
	}
}

