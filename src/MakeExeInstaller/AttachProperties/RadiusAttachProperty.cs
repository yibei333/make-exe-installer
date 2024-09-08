using System.Windows;

namespace MakeExeInstaller.AttachProperties
{
    public static class RadiusAttachProperty
    {
        public static DependencyProperty RadiusProperty = DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(RadiusAttachProperty));

        public static CornerRadius GetRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(RadiusProperty);

        public static void SetRadius(DependencyObject obj, object value) => obj.SetValue(RadiusProperty, value);
    }
}
