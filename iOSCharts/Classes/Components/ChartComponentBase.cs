using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace iOSCharts
{
    public class ChartComponentBase : NSObject
    {
        /// flag that indicates if this component is enabled or not
        public bool enabled = true;

            /// Sets the used x-axis offset for the labels on this axis.
            /// **default**: 5.0
        public nfloat xOffset = 5.0f;

            /// Sets the used y-axis offset for the labels on this axis.
            /// **default**: 5.0 (or 0.0 on ChartYAxis)
        public nfloat yOffset = 5.0f;

        public ChartComponentBase()
        {            
        }

        public bool isEnabled { get { return enabled; } }
    }
}

