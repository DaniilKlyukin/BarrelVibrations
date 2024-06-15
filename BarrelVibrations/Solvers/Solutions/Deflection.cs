using System.Runtime.Serialization;
using BasicLibraryWinForm;

namespace BarrelVibrations.Solvers.Solutions
{
    [Serializable]
    [DataContract(Name = "Данные решения задачи начального прогиба")]
    public class Deflection
    {
        /// <summary>
        /// Сохранить результаты
        /// </summary>
        /// <param name="deflectionX">Локальные перемещения по Ox, связанной со стволом, м</param>
        /// <param name="deflectionY">Локальные перемещения по Oy, связанной со стволом, м</param>
        /// <param name="deflectionZ">Локальные перемещения по Oz, связанной со стволом, м</param>
        public void WriteResults(
            double[] deflectionX,
            double[] deflectionY,
            double[] deflectionZ)
        {
            DeflectionX = deflectionX.Copy();
            DeflectionY = deflectionY.Copy();
            DeflectionZ = deflectionZ.Copy();         
        }

        /// <summary>
        /// Локальные перемещения по Ox, м
        /// </summary>
        [DataMember(Name = "Локальные перемещения по Ox, м")]
        public double[] DeflectionX { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Локальные перемещения по Oy, м
        /// </summary>
        [DataMember(Name = "Перемещения по Oy, м")]
        public double[] DeflectionY { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Локальные перемещения по Oz, м
        /// </summary>
        [DataMember(Name = "Локальные перемещения по Oz, м")]
        public double[] DeflectionZ { get; set; } = Array.Empty<double>();
    }
}
