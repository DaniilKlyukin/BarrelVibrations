using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;
using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BarrelVibrations.ModelingObjects.EnvironmentFolder;
using BarrelVibrations.ModelingObjects.ShotFolder;
using BasicLibraryWinForm.PropertiesTemplates;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;

namespace BarrelVibrations.ModelingObjects
{
    [Serializable]
    [DataContract(Name = "Модель")]
    public class ModelProperties
    {
        #region Сохранение
        private int stepsToSaveResults = 10;
        /// <summary>
        /// Шагов для сохранения результатов
        /// </summary>
        [Browsable(true)]
        [Category("Сохранение")]
        [Description("")]
        [DisplayName("Шагов для сохранения результатов")]
        [DataMember(Name = "Шагов для сохранения результатов")]
        public int StepsToSaveResults
        {
            get => stepsToSaveResults;
            set => stepsToSaveResults = Math.Max(1, value);
        }

        private int stepsToShowProgress = 200;
        /// <summary>
        /// Шагов для отображения прогресса
        /// </summary>
        [Browsable(true)]
        [Category("Сохранение")]
        [Description("")]
        [DisplayName("Шагов для отображения прогресса")]
        [DataMember(Name = "Шагов для отображения прогресса")]
        public int StepsToShowProgress
        {
            get => stepsToShowProgress;
            set => stepsToShowProgress = Math.Max(1, value);
        }

        /// <summary>
        /// Сохранять распределения параметров в файлы
        /// </summary>
        [Browsable(true)]
        [Category("Сохранение")]
        [Description("")]
        [DisplayName("Сохранять распределения параметров в файлы")]
        [DataMember(Name = "Сохранять распределения параметров в файлы")]
        public bool SaveDistributionsInFiles { get; set; }
        #endregion

        #region Время

        private double endTime = 20e-3;
        /// <summary>
        /// Время окончания расчета, сек
        /// </summary>
        [Browsable(true)]
        [Category("Время")]
        [Description("")]
        [DisplayName("Время окончания расчета, сек")]
        [DataMember(Name = "Время окончания расчета, сек")]
        public double EndTime
        {
            get => endTime;
            set => endTime = Math.Max(1e-6, value);
        }

        private double mainTimeStep = 1e-6;

        /// <summary>
        /// Шаг по времени, сек
        /// </summary>
        [Browsable(true)]
        [Category("Время")]
        [Description("Шаг по времени для задачи внутренней баллистики и последействия")]
        [DisplayName("Шаг по времени, сек")]
        [DataMember(Name = "Шаг по времени, сек")]
        public double MainTimeStep
        {
            get => mainTimeStep;
            set => mainTimeStep = Math.Max(1e-10, value);
        }

        private double outletBallisticTimeStep = 1e-4;
        /// <summary>
        /// Шаг по времени для задачи внешней баллистики, сек
        /// </summary>
        [Browsable(true)]
        [Category("Время")]
        [Description("")]
        [DisplayName("Шаг по времени для задачи внешней баллистики, сек")]
        [DataMember(Name = "Шаг по времени для задачи внешней баллистики, сек")]
        public double OutletBallisticTimeStep
        {
            get => outletBallisticTimeStep;
            set => outletBallisticTimeStep = Math.Max(1e-10, value);
        }

        private int vibrationsStepMultiplier = 2;
        /// <summary>
        /// Множитель шага для колебаний
        /// </summary>
        [Browsable(true)]
        [Category("Время")]
        [Description("Множитель шага по времени для задачи теплопроводности и колебаний и т.п.")]
        [DisplayName("Множитель шага для колебаний")]
        [DataMember(Name = "Множитель шага для колебаний")]
        public int VibrationsStepMultiplier
        {
            get => vibrationsStepMultiplier;
            set => vibrationsStepMultiplier = Math.Max(1, value);
        }
        #endregion

        #region Допущения

        /// <summary>
        /// Задача начальных деформаций?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Задача начальных деформаций?")]
        [DataMember(Name = "Задача начальных деформаций?")]
        public bool CalculateDeformations { get; set; } = true;

        /// <summary>
        /// Задача теплопроводность?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Задача теплопроводности?")]
        [DataMember(Name = "Задача теплопроводности?")]
        public bool CalculateTemperatures { get; set; } = true;

        /// <summary>
        /// Продольно-поперечные колебания?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Продольно-поперечные колебания?")]
        [DataMember(Name = "Продольно-поперечные колебания?")]
        public bool CalculateVibrations { get; set; } = true;

        /// <summary>
        /// Радиальные колебания?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Радиальные колебания?")]
        [DataMember(Name = "Радиальные колебания?")]
        public bool CalculateRadialVibrations { get; set; } = false;

        /// <summary>
        /// Учитывать противодавление
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Учитывать противодавление")]
        [DataMember(Name = "Учитывать противодавление")]
        public bool IsBackPressure { get; set; }

        /// <summary>
        /// Учитывать силу тяжести для снаряда в стволе?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Учитывать силу тяжести для снаряда в стволе?")]
        [DataMember(Name = "Учитывать силу тяжести для снаряда в стволе?")]
        public bool CalculateMissileGravity { get; set; } = true;

        /// <summary>
        /// Коэффициент трения снаряда
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Коэффициент трения снаряда")]
        [DataMember(Name = "Коэффициент трения снаряда")]
        public double MissileFrictionCoefficient { get; set; }

        /// <summary>
        /// Учитывать колебания снаряда?
        /// </summary>
        [Browsable(true)]
        [Category("Допущения")]
        [Description("")]
        [DisplayName("Учитывать колебания снаряда?")]
        [DataMember(Name = "Учитывать колебания снаряда?")]
        public bool MissileVibrations { get; set; }

        #endregion

        #region Стрельба

        /// <summary>
        /// Угол стрельбы, град
        /// </summary>
        [Browsable(true)]
        [Category("Стрельба")]
        [Description("")]
        [DisplayName("Угол стрельбы, град")]
        [DataMember(Name = "Угол стрельбы, град")]
        public double FiringAngle { get; set; }

        private double forcePressure = 30;
        /// <summary>
        /// Давление форсирования, МПа
        /// </summary>
        [Browsable(true)]
        [Category("Стрельба")]
        [Description("")]
        [DisplayName("Давление форсирования, МПа")]
        [DataMember(Name = "Давление форсирования, МПа")]
        public double ForcePressure
        {
            get => forcePressure;
            set => forcePressure = Math.Max(0, value);
        }

        /// <summary>
        /// Выстрелы
        /// </summary>
        [Browsable(true)]
        [Category("Стрельба")]
        [Description("")]
        [DisplayName("Выстрелы")]
        [DataMember(Name = "Выстрелы")]
        [Editor(typeof(FormTypeEditor<ShotForm, List<Shot>>), typeof(UITypeEditor))]
        public List<Shot> Shots { get; set; } = new();
        #endregion

    }
}
