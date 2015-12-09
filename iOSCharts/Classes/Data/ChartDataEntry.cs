using System;

using Foundation;

namespace iOSCharts
{
    public class ChartDataEntry : NSObject
    {
        /// the actual value (y axis)
        public double value;

        /// the index on the x-axis
        public int xIndex;

        /// optional spot for additional data this Entry represents
		public object data;

        public ChartDataEntry()
        {
        }

        public ChartDataEntry(double value, int xIndex)
        {
            this.value = value;
            this.xIndex = xIndex;
        }

		public ChartDataEntry(double value, int xIndex, object data)
        {
            this.value = value;
            this.xIndex = xIndex;
            this.data = data;
        }

        // MARK: NSObject

        public override bool IsEqual(NSObject anObject)
        {
			if (anObject == null)
				return false;

			if (!(anObject is ChartDataEntry))
				return false;
				
            var dataEntry = (ChartDataEntry)anObject;

            if (dataEntry.data != this.data)
                return false;

            if (dataEntry.xIndex != this.xIndex)
                return false;

            if (Math.Abs(dataEntry.value - this.value) > 0.00001f)
                return false;

            return true;
        }

        // MARK: NSObject

        public override string Description
        {
            get
            {
                return string.Format("ChartDataEntry, xIndex: {0}, value: {1}", xIndex, value);
            }
        }

        // MARK: NSCopying

        public NSObject copyWithZone(NSZone zone)
        {
            var copy = new ChartDataEntry();

            copy.value = value;
            copy.xIndex = xIndex;
            copy.data = data;

            return copy;
        }
        
    }
}

#if false
public func ==(lhs: ChartDataEntry, rhs: ChartDataEntry) -> Bool
{
if (lhs === rhs)
{
return true
}

if (!lhs.isKindOfClass(rhs.dynamicType))
{
return false
}

if (lhs.data !== rhs.data && !lhs.data!.isEqual(rhs.data))
{
return false
}

if (lhs.xIndex != rhs.xIndex)
{
return false
}

if (fabs(lhs.value - rhs.value) > 0.00001)
{
return false
}

return true
}
#endif