using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.Solutions.InletBallistic
{
    [Serializable]
    [DataContract(Name = "Данные решения задачи внутренней баллистики")]
    public class InletBallistic
    {
        public void Clear()
        {
            TimeMoments.Clear();
            Xsn.Clear();
            Vsn.Clear();
            P.Clear();
            Psn.Clear();
            Pkn.Clear();
            Ppr.Clear();
            Density.Clear();
            T.Clear();
            W.Clear();
            J0.Clear();
            J1.Clear();
            J2.Clear();
            Psi.Clear();
            Z.Clear();
            Sigma.Clear();
            U.Clear();
        }

        /// <summary>
        /// Моменты времени, сек
        /// </summary>
        [DataMember(Name = "Моменты времени, сек")]
        public List<double> TimeMoments { get; set; } = new();

        /// <summary>
        /// Координаты снаряда, м
        /// </summary>
        [DataMember(Name = "Координата снаряда, м")]
        public List<double> Xsn { get; set; } = new();

        /// <summary>
        /// Скорости снаряда, м/с
        /// </summary>
        [DataMember(Name = "Скорость снаряда, м/с")]
        public List<double> Vsn { get; set; } = new();

        /// <summary>
        /// Средние давления, Па
        /// </summary>
        [DataMember(Name = "Среднее давление, Па")]
        public List<double> P { get; set; } = new();

        /// <summary>
        /// Давления на дно снаряда, Па
        /// </summary>
        [DataMember(Name = "Давление на дно снаряда, Па")]
        public List<double> Psn { get; set; } = new();

        /// <summary>
        /// Давления на дно канала, Па
        /// </summary>
        [DataMember(Name = "Давление на дно канала, Па")]
        public List<double> Pkn { get; set; } = new();

        /// <summary>
        /// Средняя температура, К
        /// </summary>
        [DataMember(Name = "Средняя температура, К")]
        public List<double> T { get; set; } = new();

        /// <summary>
        /// Заснарядный объем, м^3
        /// </summary>
        [DataMember(Name = "Заснарядный объем, м^3")]
        public List<double> W { get; set; } = new();

        /// <summary>
        /// J0
        /// </summary>
        [DataMember(Name = "J0")]
        public List<double> J0 { get; set; } = new();

        /// <summary>
        /// J1
        /// </summary>
        [DataMember(Name = "J1")]
        public List<double> J1 { get; set; } = new();

        /// <summary>
        /// J2
        /// </summary>
        [DataMember(Name = "J2")]
        public List<double> J2 { get; set; } = new();

        /// <summary>
        /// Доля сгоревшего пороха
        /// </summary>
        [DataMember(Name = "Доля сгоревшего пороха")]
        public List<double[]> Psi { get; set; } = new();

        /// <summary>
        /// Относительная толщина сгоревшего свода
        /// </summary>
        [DataMember(Name = "Относительная толщина сгоревшего свода")]
        public List<double[]> Z { get; set; } = new();

        /// <summary>
        /// Относительная площадь поверхности горения
        /// </summary>
        [DataMember(Name = "Относительная площадь поверхности горения")]
        public List<double[]> Sigma { get; set; } = new();

        /// <summary>
        /// Скорость горения, м³/(Н с)
        /// </summary>
        [DataMember(Name = "Скорость горения, м³/(Н с)")]
        public List<double[]> U { get; set; } = new();

        /// <summary>
        /// Противодавление, Па
        /// </summary>
        [DataMember(Name = "Противодавление, Па")]
        public List<double> Ppr { get; set; } = new();

        /// <summary>
        /// Плотность газопороховой смеси, кг/м³
        /// </summary>
        [DataMember(Name = "Плотность газопороховой смеси, кг/м³")]
        public List<double> Density { get; set; } = new();
    }
}
