using BarrelVibrations.Optimization.TargetCalculators;
using BasicLibraryWinForm.Optimization;
using BasicLibraryWinForm.PropertiesTemplates;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace BarrelVibrations.Optimization
{
    public class BarrelRandomDescend : RandomDescend, IBarrelOptimizer
    {
        [Editor(typeof(FormTypeEditor<BarrelOptimizationConstraintForm, BarrelOptimizationConstraint>), typeof(UITypeEditor))]
        public override OptimizationConstraint Constraint { get; set; } = new BarrelOptimizationConstraint();

        /// <summary>
        ///     Метод расчета целевой функции
        /// </summary>
        [Browsable(true)]
        [CustomSortedCategory("Расчет", 1, CATEGORIESCOUNT)]
        [Description("")]
        [DisplayName("Метод расчета целевой функции")]
        [DataMember(Name = "Метод расчета целевой функции")]
        [Editor(typeof(FormTypeEditor<OptimizationTargetForm, OptimizationTargetCalculator>), typeof(UITypeEditor))]
        public OptimizationTargetCalculator OptimizationTargetCalculator { get; set; }
            = new VibrationsAmplitudeOptimizationTarget();
    }
}
