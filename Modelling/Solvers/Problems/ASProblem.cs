using System;
using System.Collections.Generic;
using System.Linq;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PointFolder;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Modelling.Material;
using Modelling.Solvers.Elements;

namespace Modelling.Solvers.Problems
{
    public class ASProblem : IProblem
    {
        public List<Dictionary<int, double>> U { get; set; }
        public List<Dictionary<int, double>> V { get; set; }
        public List<Dictionary<int, double>> TotalVelocity;
        public List<Dictionary<int, double>> DeformationXX;
        public List<Dictionary<int, double>> DeformationYY;
        public List<Dictionary<int, double>> DeformationOO;
        public List<Dictionary<int, double>> DeformationXY;
        public List<Dictionary<int, double>> EquivalentDeformation;
        public List<Dictionary<int, double>> StressXX;
        public List<Dictionary<int, double>> StressYY;
        public List<Dictionary<int, double>> StressOO;
        public List<Dictionary<int, double>> StressXY;
        public List<Dictionary<int, double>> EquivalentStress;

        CoordinatesConverter coordinatesConverter;

        public ASProblem(CoordinatesConverter coordinatesConverter)
        {
            this.coordinatesConverter = coordinatesConverter;
        }

        public DenseMatrix GetD(double temperature, MaterialProperties materialProperties)
        {
            var nu = materialProperties.PoissonRatio.Function(temperature);
            var E = 1e9 * materialProperties.YoungModulus.Function(temperature);

            return E / ((1 + nu) * (1 - 2 * nu)) * DenseMatrix.OfArray(new[,]
            { { 1.0 - nu, nu, nu, 0.0},
                { nu, 1.0 - nu, nu, 0.0},
                { nu, nu, 1.0 - nu, 0.0},
                { 0.0, 0.0, 0.0, 0.5 - nu} });
        }

        public void InitializeElement(Element element, out Matrix<double> B, out Matrix<double> k, out Matrix<double> M)
        {
            var rho = element.MaterialProperties.Density.Function(300);
            var D = element.D;

            var dNNeutralPoly = new BiPolynomial[2, element.Points.Length];

            var coords = new DenseMatrix(element.Points.Length, 2);

            for (var i = 0; i < element.Points.Length; i++)
            {
                var pc = coordinatesConverter.GetPoint(element.Points[i]);

                coords[i, 0] = pc.X;
                coords[i, 1] = pc.Y;

                dNNeutralPoly[0, i] = element.N[i].Differentiate(1);
                dNNeutralPoly[1, i] = element.N[i].Differentiate(2);
            }

            k = new DenseMatrix(2 * element.Points.Length, 2 * element.Points.Length);
            B = new DenseMatrix(4, 2 * element.Points.Length);
            M = new DenseMatrix(2 * element.Points.Length, 2 * element.Points.Length);

            for (var t = 0; t < element.integration_xi.Length; t++)
            {
                var xi = element.integration_xi[t];
                var eta = element.integration_eta[t];
                var w = element.weights[t];

                var dNNeutral = new DenseMatrix(2, element.Points.Length);

                for (var i = 0; i < element.Points.Length; i++)
                {
                    dNNeutral[0, i] = dNNeutralPoly[0, i].Evaluate(xi, eta);
                    dNNeutral[1, i] = dNNeutralPoly[1, i].Evaluate(xi, eta);
                }

                var J = dNNeutral * coords;
                var JI = J.Inverse();

                var dN = JI * dNNeutral; //matrix: 2 x Points

                var BLocal = new DenseMatrix(4, 2 * element.Points.Length);

                var r = element.N.Select((poly, i) => poly.Evaluate(xi, eta) * coords[i, 1]).Sum();

                for (var i = 0; i < element.Points.Length; i++)
                {
                    BLocal[0, 2 * i] = dN[0, i];
                    BLocal[1, 2 * i + 1] = dN[1, i];

                    BLocal[2, 2 * i + 1] = element.N[i].Evaluate(xi, eta) / r;

                    BLocal[3, 2 * i] = dN[1, i];
                    BLocal[3, 2 * i + 1] = dN[0, i];
                }

                var detJ = J.Determinant();

                var kLocal = r * BLocal.Transpose() * D * BLocal * detJ;

                var N_vect = element.N.Select(n => n.Evaluate(xi, eta)).ToArray();

                var N = new DenseMatrix(2, 2 * element.Points.Length);

                for (var i = 0; i < element.Points.Length; i++)
                {
                    N[0, 2 * i] = N_vect[i];
                    N[1, 2 * i + 1] = N_vect[i];
                }

                var MLocal = r * N.Transpose() * rho * N * detJ;

                B += BLocal / element.integration_xi.Length;
                k += -2 * w * kLocal;
                M += -2 * w * MLocal;
            }
        }

        public Vector<double> CalculateLoad(Element element, double pressure, (int, int) edge)
        {
            var (pId1, pId2) = edge;
            var pIndex1 = 0;
            var pIndex2 = 0;

            for (var i = 0; i < element.Points.Length; i++)
            {
                if (element.Points[i].Id == pId1)
                    pIndex1 = i;

                if (element.Points[i].Id == pId2)
                    pIndex2 = i;
            }
            var pc1 = coordinatesConverter.GetPoint(element.LocalPoints[pIndex1]);
            var pc2 = coordinatesConverter.GetPoint(element.LocalPoints[pIndex2]);

            var (nx, ny) = Algebra.GetClockwisedNormal(
                pc1.X,
                pc1.Y,
                pc2.X,
                pc2.Y);

            var length = Point.GetDistance(
                pc1.X,
                pc1.Y,
                pc2.X,
                pc2.Y);

            Vector<double> load = new DenseVector(2 * element.Points.Length);
            Vector<double> f = DenseVector.OfArray(new[] { -nx * pressure * length, -ny * pressure * length });

            var dNNeutralPoly = new BiPolynomial[2, element.Points.Length];

            var coords = new DenseMatrix(element.Points.Length, 2);

            for (var i = 0; i < element.Points.Length; i++)
            {
                var pc = coordinatesConverter.GetPoint(element.Points[i]);

                coords[i, 0] = pc.X;
                coords[i, 1] = pc.Y;

                dNNeutralPoly[0, i] = element.N[i].Differentiate(1);
                dNNeutralPoly[1, i] = element.N[i].Differentiate(2);
            }

            for (var t = 0; t < element.integration_xi.Length; t++)
            {
                var xi = element.integration_xi[t];
                var eta = element.integration_eta[t];
                var w = element.weights[t];

                var dNNeutral = new DenseMatrix(2, element.Points.Length);

                for (var i = 0; i < element.Points.Length; i++)
                {
                    dNNeutral[0, i] = dNNeutralPoly[0, i].Evaluate(xi, eta);
                    dNNeutral[1, i] = dNNeutralPoly[1, i].Evaluate(xi, eta);
                }

                var J = dNNeutral * coords;
                var JI = J.Inverse();

                var dN = JI * dNNeutral; //matrix: 2 x Points

                var r = element.N.Select((poly, i) => poly.Evaluate(xi, eta) * coords[i, 1]).Sum();

                var detJ = J.Determinant();

                var N_vect = element.N.Select(n => n.Evaluate(xi, eta)).ToArray();

                var N = new DenseMatrix(2, 2 * element.Points.Length);

                for (var i = 0; i < element.Points.Length; i++)
                {
                    if (element.Points[i].Id != pId1 && element.Points[i].Id != pId2)
                    {
                        continue;
                    }

                    N[0, 2 * i] = N_vect[i];
                    N[1, 2 * i + 1] = N_vect[i];
                }

                load += -2 * w * r * N.Transpose() * f * detJ;
            }

            return load;
        }

        public string[] MeasureUnits { get; } = {
            "мм",
            "мм",
            "мм",

            "мм/м",
            "мм/м",
            "мм/м",
            "мм/м",
            "мм/м",

            "МПа",
            "МПа",
            "МПа",
            "МПа",
            "МПа"
        };

        public string[] VariableNames { get; } =
        {
            "Продольные перемещения",
            "Поперечные перемещения",
            "Результирующие перемещения",

            "Деформации ε_xx",
            "Деформации ε_yy",
            "Деформации ε_θθ",
            "Деформации ε_xy",
            "Эквивалентные деформации",

            "Напряжения σ_xx",
            "Напряжения σ_yy",
            "Напряжения σ_θθ",
            "Напряжения σ_xy",
            "Эквивалентные напряжения"
        };


        public double[] ValueMultipliers { get; } =
        {
            1e3,
            1e3,
            1e3,

            1e3,
            1e3,
            1e3,
            1e3,
            1e3,

            1e-6,
            1e-6,
            1e-6,
            1e-6,
            1e-6
        };

        public void CalculateParameters<TElement>(
            List<TElement> elements,
            Dictionary<int, double> u,
            Dictionary<int, double> v) where TElement : Element
        {
            var totalVelocity = new Dictionary<int, double>();
            var deformationXX = new Dictionary<int, double>();
            var deformationYY = new Dictionary<int, double>();
            var deformationOO = new Dictionary<int, double>();
            var deformationXY = new Dictionary<int, double>();
            var equivalentDeformation = new Dictionary<int, double>();
            var stressXX = new Dictionary<int, double>();
            var stressYY = new Dictionary<int, double>();
            var stressOO = new Dictionary<int, double>();
            var stressXY = new Dictionary<int, double>();
            var equivalentStress = new Dictionary<int, double>();

            var exx_arr = new double[u.Count];
            var eyy_arr = new double[u.Count];
            var eoo_arr = new double[u.Count];
            var exy_arr = new double[u.Count];

            var sxx_arr = new double[u.Count];
            var syy_arr = new double[u.Count];
            var soo_arr = new double[u.Count];
            var sxy_arr = new double[u.Count];

            var repeats = new int[u.Count];
            var nu_arr = new double[u.Count];

            foreach (var element in elements)
            {
                element.GetStrainStress(element.Points.Select(p => u[p.Id]).ToArray(), element.Points.Select(p => v[p.Id]).ToArray(), out var strain, out var stress);

                foreach (var point in element.Points)
                {
                    var pId = point.Id;

                    exx_arr[pId] += strain[0];
                    eyy_arr[pId] += strain[1];
                    eoo_arr[pId] += strain[2];
                    exy_arr[pId] += strain[3];

                    sxx_arr[pId] += stress[0];
                    syy_arr[pId] += stress[1];
                    soo_arr[pId] += stress[2];
                    sxy_arr[pId] += stress[3];

                    repeats[pId]++;
                    nu_arr[pId] += element.MaterialProperties.PoissonRatio.Function(300);
                }
            }

            var keys = u.Keys.ToArray();

            foreach (var id in keys)
            {
                totalVelocity.Add(id, Math.Sqrt(u[id] * u[id] + v[id] * v[id]));

                var nu = nu_arr[id] / repeats[id];

                var exx = exx_arr[id] / repeats[id];
                var eyy = eyy_arr[id] / repeats[id];
                var eoo = eoo_arr[id] / repeats[id];
                var exy = exy_arr[id] / repeats[id];

                deformationXX.Add(id, exx);
                deformationYY.Add(id, eyy);
                deformationOO.Add(id, eoo);
                deformationXY.Add(id, exy);
                equivalentDeformation.Add(id, Math.Sqrt(0.5 * (FastMath.Pow2(eyy - eoo) + FastMath.Pow2(eyy - exx) + FastMath.Pow2(exx - eoo)) + 0.75 * FastMath.Pow2(exy)) / (1 + nu));

                var sxx = sxx_arr[id] / repeats[id];
                var syy = syy_arr[id] / repeats[id];
                var soo = soo_arr[id] / repeats[id];
                var sxy = sxy_arr[id] / repeats[id];

                stressXX.Add(id, sxx);
                stressYY.Add(id, syy);
                stressOO.Add(id, soo);
                stressXY.Add(id, sxy);
                equivalentStress.Add(id, Math.Sqrt(0.5 * (FastMath.Pow2(syy - soo) + FastMath.Pow2(syy - sxx) + FastMath.Pow2(sxx - soo) + 6 * FastMath.Pow2(sxy))));
            }

            U.Add(u);
            V.Add(v);
            TotalVelocity.Add(totalVelocity);

            DeformationXX.Add(deformationXX);
            DeformationYY.Add(deformationYY);
            DeformationOO.Add(deformationOO);
            DeformationXY.Add(deformationXY);
            EquivalentDeformation.Add(equivalentDeformation);

            StressXX.Add(stressXX);
            StressYY.Add(stressYY);
            StressOO.Add(stressOO);
            StressXY.Add(stressXY);
            EquivalentStress.Add(equivalentStress);
        }

        public List<Dictionary<int, double>>[] Solutions { get; private set; } = new List<Dictionary<int, double>>[11];
        public void InitializeSolutions()
        {
            Solutions = new List<Dictionary<int, double>>[13];

            Solutions[0] = U = new List<Dictionary<int, double>>();
            Solutions[1] = V = new List<Dictionary<int, double>>();
            Solutions[2] = TotalVelocity = new List<Dictionary<int, double>>();
            Solutions[3] = DeformationXX = new List<Dictionary<int, double>>();
            Solutions[4] = DeformationYY = new List<Dictionary<int, double>>();
            Solutions[5] = DeformationOO = new List<Dictionary<int, double>>();
            Solutions[6] = DeformationXY = new List<Dictionary<int, double>>();
            Solutions[7] = EquivalentDeformation = new List<Dictionary<int, double>>();
            Solutions[8] = StressXX = new List<Dictionary<int, double>>();
            Solutions[9] = StressYY = new List<Dictionary<int, double>>();
            Solutions[10] = StressOO = new List<Dictionary<int, double>>();
            Solutions[11] = StressXY = new List<Dictionary<int, double>>();
            Solutions[12] = EquivalentStress = new List<Dictionary<int, double>>();
        }
    }
}
