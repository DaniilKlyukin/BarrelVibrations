using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BarrelVibrations.Solvers.OutletBallisticFolder;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    [Serializable]
    [DataContract(Name = "Окружающая среда")]
    public class Environment
    {
        [Browsable(true)]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Показатель адиабаты воздуха")]
        [DataMember(Name = "Показатель адиабаты воздуха")]
        public double k { get; set; } = 1.41;

        [Browsable(true)]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Коэф. теплопередачи, Вт/(м·К)")]
        [DataMember(Name = "Коэф. теплопередачи, Вт/(м·К)")]
        public double HeatTransfer { get; set; } = 23;

        [Browsable(true)]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Таблица метеоданных")]
        [DataMember(Name = "Таблица метеоданных")]
        [Editor(typeof(FormTypeEditor<MeteoTableForm, MeteoTable>), typeof(UITypeEditor))]
        public MeteoTable MeteoTable { get; set; } = new();

        [Browsable(true)]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Карта высот")]
        [DataMember(Name = "Карта высот")]
        [Editor(typeof(FormTypeEditor<TerrainForm, Terrain>), typeof(UITypeEditor))]
        public Terrain Terrain { get; set; } = new();

        [Browsable(true)]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Карта ветра")]
        [DataMember(Name = "Карта ветра")]
        [Editor(typeof(FormTypeEditor<WindForm, Wind>), typeof(UITypeEditor))]
        public Wind Wind { get; set; } = new();

        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Давление, Па")]
        public double Pressure => MeteoTable.Pressure(0);

        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Температура, К")]
        public double Temperature => MeteoTable.Temperature(0);

        [ReadOnly(true)]
        [IgnoreDataMember]
        [Category("Параметры окружающей среды")]
        [Description("")]
        [DisplayName("Плотность, кг/м³")]
        public double Density => MeteoTable.Density(0);
    }
}
