using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MakeExeInstaller.Extensions
{
    /// <summary>
    /// card control
    /// </summary>
    public class XLoading : BaseControl
    {
        #region Constructors
        static XLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XLoading), new FrameworkPropertyMetadata(typeof(XLoading)));
            VisibilityProperty.OverrideMetadata(typeof(XLoading), new PropertyMetadata((d, _) =>
            {
                if (d is XLoading loading)
                {
                    loading.StopWhenHidden();
                }
            }));
        }

        /// <summary>
        /// create instance of type XLoading
        /// </summary>
        public XLoading()
        {
            RenderTransformOrigin = new Point(.5, .5);
            RenderTransform = new RotateTransform();
            Storyboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var animation = new DoubleAnimation { From = 0, To = 360 };
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath($"RenderTransform.Angle"));
            BindingOperations.SetBinding(animation, Timeline.DurationProperty, new Binding { Source = this, Path = new PropertyPath(DurationProperty.Name) });
            BindingOperations.SetBinding(animation, DoubleAnimation.EasingFunctionProperty, new Binding { Source = this, Path = new PropertyPath(EasingFunctionProperty.Name) });
            Storyboard.Children.Add(animation);
            Loaded += (_, e) => Start();
            StartCommand = new BuiltInCommand(_ => Start());
            StopCommand = new BuiltInCommand(_ => Stop());
        }
        #endregion

        #region Properties
        Storyboard Storyboard { get; }

        /// <summary>
        /// easingfunction
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(XLoading));

        /// <summary>
        /// easingfunction
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <summary>
        /// duration
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(XLoading));

        /// <summary>
        /// duration
        /// </summary>
        public Duration Duration
        {
            get => (Duration)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        /// <summary>
        /// stroke size
        /// </summary>
        public static readonly DependencyProperty StrokeSizeProperty = DependencyProperty.Register(nameof(StrokeSize), typeof(double), typeof(XLoading));

        /// <summary>
        /// stroke size
        /// </summary>
        public double StrokeSize
        {
            get => (double)GetValue(StrokeSizeProperty);
            set => SetValue(StrokeSizeProperty, value);
        }

        /// <summary>
        /// size
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(LoadingSize), typeof(XLoading), new PropertyMetadata(LoadingSize.Medium));

        /// <summary>
        /// size
        /// </summary>
        public LoadingSize Size
        {
            get => (LoadingSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        #endregion

        #region Commands
        /// <summary>
        /// start loading command
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// stop loading command
        /// </summary>
        public ICommand StopCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// start loading
        /// </summary>
        public void Start()
        {
            Storyboard.Begin();
        }

        /// <summary>
        /// stop loading
        /// </summary>
        public void Stop()
        {
            Storyboard.Stop();
        }

        /// <summary>
        /// if not stop,gpu always being running
        /// </summary>
        void StopWhenHidden()
        {
            if (Visibility != Visibility.Visible) Stop();
        }
        #endregion
    }

    /// <summary>
    /// loading size
    /// </summary>
    public enum LoadingSize
    {
        /// <summary>
        /// none
        /// </summary>
        None,
        /// <summary>
        /// small
        /// </summary>
        Small,
        /// <summary>
        /// medium
        /// </summary>
        Medium,
        /// <summary>
        /// big
        /// </summary>
        Big
    }
}
