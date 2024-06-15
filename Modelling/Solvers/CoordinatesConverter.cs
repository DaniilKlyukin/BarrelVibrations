using BasicLibraryWinForm.PointFolder;
using System;

namespace Modelling.Solvers
{
    public class CoordinatesConverter
    {
        private int ix, iy, iz;

        public CoordinatesConverter(int ix, int iy, int iz)
        {
            this.ix = ix;
            this.iy = iy;
            this.iz = iz;
        }

        public double[] GetCoordinates(Point point)
        {
            var coords = point.ToArray();

            return new double[] 
            { 
                Math.Sign(ix) * coords[Math.Abs(ix)],
                Math.Sign(iy) * coords[Math.Abs(iy)],
                Math.Sign(iz) * coords[Math.Abs(iz)]
            };
        }

        public Point GetPoint(Point point)
        {
            var coords = point.ToArray();

            return new Point(
                Math.Sign(ix) * coords[Math.Abs(ix)],
                Math.Sign(iy) * coords[Math.Abs(iy)],
                Math.Sign(iz) * coords[Math.Abs(iz)]);
        }
    }
}
