using MakeExeInstaller.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MakeExeInstaller.Extensions
{
    public class BoolReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = bool.TryParse(value?.ToString(), out var result) && result;
            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = bool.TryParse(value?.ToString(), out var result) && result;
            return !boolValue;
        }
    }

    public class DependencyStateImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = Enum.TryParse<DependencyState>(value?.ToString(), out var result) ? result : DependencyState.Waiting;
            if (state == DependencyState.Running) return new XLoading { Size = LoadingSize.Small };
            if (state == DependencyState.Completed) return new Path { Data = XGeometries.Success, Height = 12, Width = 12, Stroke = (SolidColorBrush)Application.Current.FindResource("green"), StrokeThickness = 1, Stretch = Stretch.Uniform };
            if (state == DependencyState.Error) return new Path { Data = XGeometries.Error, Height = 12, Width = 12, Stroke = (SolidColorBrush)Application.Current.FindResource("red"), StrokeThickness = 1, Stretch = Stretch.Uniform };
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
