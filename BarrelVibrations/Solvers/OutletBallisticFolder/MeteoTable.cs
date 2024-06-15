using BasicLibraryWinForm;
using System.Runtime.Serialization;

namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    [Serializable]
    [DataContract(Name = "Таблица метеоданных")]
    public class MeteoTable
    {

        /// <summary>
        /// Список метеоданных
        /// </summary>
        [DataMember(Name = "Список метеоданных")]
        public List<MeteoData> MeteoDatas { get; private set; } = new();

        /// <summary>
        /// Функция температуры от высоты
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> Temperature { get; private set; } = _ => 288;

        /// <summary>
        /// Функция давления от высоты
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> Pressure { get; private set; } = _ => 101325;

        /// <summary>
        /// Функция плотности от высоты
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> Density { get; private set; } = _ => 1.28;

        /// <summary>
        /// Функция скорости звука от высоты
        /// </summary>
        [IgnoreDataMember]
        public Func<double, double> SoundSpeed { get; private set; } = _ => 340.3;


        public void UpdateFunctions()
        {
            var ordered = MeteoDatas.OrderBy(v => v.Altitude).ToArray();

            Temperature = Algebra.GetFunc(
                ordered.Select(v => v.Altitude).ToArray(),
                ordered.Select(v => v.Temperature).ToArray(),
                false);

            Pressure = Algebra.GetFunc(
                ordered.Select(v => v.Altitude).ToArray(),
                ordered.Select(v => v.Pressure).ToArray(),
                false);

            Density = Algebra.GetFunc(
                ordered.Select(v => v.Altitude).ToArray(),
                ordered.Select(v => v.Density).ToArray(),
                false);

            SoundSpeed = Algebra.GetFunc(
                ordered.Select(v => v.Altitude).ToArray(),
                ordered.Select(v => v.SoundSpeed).ToArray(),
                false);
        }

        public override string ToString()
        {
            return $"Метеоданные, {MeteoDatas.Count} значений";
        }
    }
}
