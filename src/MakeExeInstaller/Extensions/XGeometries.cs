using System.Windows.Media;

namespace MakeExeInstaller.Extensions
{
    public static class XGeometries
    {
        /// <summary>
        /// close(✕)
        /// </summary>
        public static readonly PathGeometry Close = new PathGeometry(PathFigureCollection.Parse("M0 0 L100 100 M100 0 L0 100 M0 0Z"));

        /// <summary>
        /// success(✓)
        /// </summary>
        public static readonly PathGeometry Success = new PathGeometry(PathFigureCollection.Parse("M0 40 L35 100 L100 0 M0 40Z"));

        /// <summary>
        /// error
        /// </summary>
        public static readonly PathGeometry Error = new PathGeometry(PathFigureCollection.Parse("M0 0 M100 0 M100 100 M0 100 M50 0 A50 50 0 1 1 0 50 A50 50 0 0 1 50 0 M25 25 L40 40 M40 25 L25 40 M60 25 L75 40 M75 25 L60 40 M35 70 A30 30 0 0 1 70 75 M35 70Z"));
    }
}

