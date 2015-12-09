using System;
using System.Collections.Generic;

using UIKit;
using iOSCharts;

namespace Sample
{
	public partial class ViewController : UIViewController
	{
		public ViewController(IntPtr handle)
			: base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var months = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			var unitsSold = new List<double>() {20.0, 4.0, 6.0, 3.0, 12.0, 16.0, 4.0, 18.0, 2.0, 4.0, 5.0, 4.0};

			setChart(months, unitsSold);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void setChart(List<string> dataPoints, List<double> values)
		{
			var datas = new List<BarChartDataEntry>();

			for (int i = 0; i < dataPoints.Count; i++)
			{
				var entryData = new BarChartDataEntry(values[i], i);
				datas.Add(entryData);
			}

			var chartDataSet = new BarChartDataSet(datas, "Units Sold");
			var chartData = new BarChartData(dataPoints, new List<BarChartDataSet>() { chartDataSet });

			//barChartView.data = chartData;
		}

	}
}

