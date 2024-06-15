using MathNet.Numerics.LinearAlgebra.Double;
using SharpGL.SceneGraph;

namespace Visualization.OpenGL
{
    public static class Math3D
    {
        public static Vertex RotateAround(Vertex axis, float angle, Vertex axisP, Vertex p)
        {
            axis.Normalize();

            var x = axis.X;
            var y = axis.Y;
            var z = axis.Z;

            var rad = MathF.PI * angle / 180;

            var q0 = MathF.Cos(rad / 2);
            var q1 = MathF.Sin(rad / 2) * x;
            var q2 = MathF.Sin(rad / 2) * y;
            var q3 = MathF.Sin(rad / 2) * z;

            var m = DenseMatrix.OfArray(new double[,]
            {
                {q0*q0+q1*q1-q2*q2-q3*q3, 2* (q1*q2-q0*q3), 2*(q1*q3+q0*q2)},
                {2*(q2*q1+q0*q3), q0*q0-q1*q1+q2*q2-q3*q3, 2*(q2*q3-q0*q1)},
                {2*(q3*q1-q0*q2), 2*(q3*q2+q0*q1), q0*q0-q1*q1-q2*q2+q3*q3}
            });

            var p1 = p - axisP;

            var p2 = m * new DenseVector(new double[] { p1.X, p1.Y, p1.Z });

            return new Vertex((float)p2.Values[0], (float)p2.Values[1], (float)p2.Values[2]) + axisP;
        }

        /// <summary>
        /// Rotates a vector using the Rodriguez rotation formula
        /// about an arbitrary axis.
        /// </summary>
        /// <param name="vector">The vector to be rotated.</param>
        /// <param name="axis">The rotation axis.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated vector</returns>
        public static Vertex RotateVector(Vertex vector, Vertex axis, float angle)
        {
            var vxp = axis.VectorProduct(vector);
            var vxvxp = axis.VectorProduct(vxp);
            return vector + vxp * MathF.Sin(angle) + vxvxp * (1 - MathF.Cos(angle));
        }

        /// <summary>
        /// Rotates a vector about a point in space.
        /// </summary>
        /// <param name="vector">The vector to be rotated.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="axis">The rotation axis.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated vector</returns>
        public static Vertex RotateVectorAboutPoint(Vertex vector, Vertex pivot, Vertex axis, float angle)
        {
            return pivot + RotateVector(vector - pivot, axis, angle);
        }

        public static Vertex GetNormalize(this Vertex vec)
        {
            var mag = (float)vec.Magnitude();

            if (mag == 0)
                return new Vertex(0, 0, 0);

            return vec / mag;
        }
    }
}
