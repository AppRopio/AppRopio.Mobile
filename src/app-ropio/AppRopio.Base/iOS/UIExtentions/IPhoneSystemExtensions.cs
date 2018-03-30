using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class IPhoneSystemExtensions
    {
        public static T SwitchOfVersion<T>(this T self, T? IPhone5 = null, T? IPhone6 = null, T? IPhone6Plus = null) where T : struct
        {
            IPhone5 = IPhone5 ?? self;
            IPhone6 = IPhone6 ?? IPhone5;
            IPhone6Plus = IPhone6Plus ?? IPhone6;

            if (DeviceInfo.Is_iPhone6Plus)
                return IPhone6Plus.Value;

            if (DeviceInfo.Is_iPhone6)
                return IPhone6.Value;

            if (DeviceInfo.Is_iPhone5)
                return IPhone5.Value;

            return self;
        }

        public static T If_iPhone4<T>(this T value, T iphone4Value)
        {
            if (DeviceInfo.Is_iPhone4)
                return iphone4Value;
            return value;
        }

        public static T If_iPhone5<T>(this T value, T iphone5Value)
        {
            if (DeviceInfo.Is_iPhone5)
                return iphone5Value;
            return value;
        }

        public static T If_iPhone6<T>(this T value, T iphone6Value)
        {
            if (DeviceInfo.Is_iPhone6)
                return iphone6Value;
            return value;
        }

        public static T If_iPhone6Plus<T>(this T value, T iphone6PlusValue)
        {
            if (DeviceInfo.Is_iPhone6Plus)
                return iphone6PlusValue;
            return value;
        }

        public static T If_iPhoneX<T>(this T value, T iphoneXValue)
        {
            if (DeviceInfo.Is_iPhoneX)
                return iphoneXValue;
            return value;
        }

        public static DateTime NSDateToDateTime(this NSDate date)
        {
            double secs = date.SecondsSinceReferenceDate;
            if (secs < -63113904000)
                return DateTime.MinValue;
            if (secs > 252423993599)
                return DateTime.MaxValue;

            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate DateTimeToNSDate(this DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));

            if (date == default(DateTime))
                return NSDate.FromTimeIntervalSinceReferenceDate(
                    (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) - reference).TotalSeconds);
            if (date.Year == 1 && date.Month == 1 && date.Day == 1)
                return NSDate.FromTimeIntervalSinceReferenceDate(
                    (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, date.Hour, date.Minute, date.Second) - reference).TotalSeconds);

            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }

        public static DateTimeOffset NSDateToDateTimeOffset(this NSDate date)
        {
            double secs = date.SecondsSinceReferenceDate;
            if (secs < -63113904000)
                return DateTimeOffset.MinValue;
            if (secs > 252423993599)
                return DateTimeOffset.MaxValue;

            var reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));

            return new DateTimeOffset(reference.AddSeconds(date.SecondsSinceReferenceDate));
        }

        public static NSDate DateTimeOffsetToNSDate(this DateTimeOffset date)
        {
            var reference = new DateTimeOffset(TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0)));

            if (date == default(DateTimeOffset))
                return NSDate.FromTimeIntervalSinceReferenceDate(
                    (new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, TimeSpan.Zero) - reference).TotalSeconds
                );

            if (date.Year == 1 && date.Month == 1 && date.Day == 1)
                return NSDate.FromTimeIntervalSinceReferenceDate(
                    (new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, date.Hour, date.Minute, date.Second, TimeSpan.Zero) - reference).TotalSeconds
                );

            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }

        public static DateTime ToDateTimeUTC(this NSDate nsdate)
        {
            NSCalendar calendar = NSCalendar.CurrentCalendar;
            var date = calendar.Components(NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day | NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second, nsdate);
            date.TimeZone = NSTimeZone.FromName(@"UTC");
            return new DateTime((int)date.Year, (int)date.Month, (int)date.Day, (int)date.Hour, (int)date.Minute, (int)date.Second, DateTimeKind.Utc);
        }

        public static NSDate ToNSDateUTC(this DateTime date)
        {
            NSCalendar calendar = NSCalendar.CurrentCalendar;
            NSDateComponents comps = new NSDateComponents();
            comps.TimeZone = NSTimeZone.FromName(@"UTC");
            comps.Day = date.Day;
            comps.Month = date.Month;
            comps.Year = date.Year;
            comps.Hour = date.Hour;
            comps.Minute = date.Minute;
            comps.Second = date.Second;
            var value = calendar.DateFromComponents(comps);
            return value;
        }
    }
}

