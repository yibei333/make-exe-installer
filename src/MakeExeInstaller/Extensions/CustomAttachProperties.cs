using System.Windows;

namespace MakeExeInstaller.Extensions
{
    public static class CustomAttachProperties
    {
        public static DependencyProperty RadiusProperty = DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(CustomAttachProperties));

        public static CornerRadius GetRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(RadiusProperty);

        public static void SetRadius(DependencyObject obj, object value) => obj.SetValue(RadiusProperty, value);
    }
}
