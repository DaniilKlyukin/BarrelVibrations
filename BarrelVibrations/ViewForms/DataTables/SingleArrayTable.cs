using BasicLibraryWinForm;
using ScottPlot;

namespace BarrelVibrations.ViewForms.DataTables
{
    public class SingleArrayTable : ITableData
    {
        public int Size1 { get; }
        public int Size2 { get; }
        public string ArgumentsColumnName { get; protected set; }
        public IEnumerable<double> ArgumentsValues { get; protected set; }
        public string ValuesColumnName { get; protected set; }
        public IEnumerable<double> Values { get; protected set; }
        public string Name { get; protected set; }
        public Action<double, FormsPlot> AdditionalDrawMethod { get; set; } = (t, plot) => { };

        public SingleArrayTable(string name, string argumentsColumnName, IEnumerable<double> argumentsValues, string valuesColumnName, IEnumerable<double> values)
        {
            if (argumentsValues.Count() != values.Count())
            {
                var n = Math.Min(argumentsValues.Count(), values.Count());
                Name = name;
                ArgumentsColumnName = argumentsColumnName;
                ValuesColumnName = valuesColumnName;
                ArgumentsValues = argumentsValues.Take(n);
                Values = values.Take(n);
                Size1 = n;
                Size2 = 1;
                Logger.Log($"Ошибка при создании таблицы данных. Разные размеры массивов \"{Name}\"");
                return;
            }

            Name = name;
            ArgumentsColumnName = argumentsColumnName;
            ArgumentsValues = argumentsValues;
            ValuesColumnName = valuesColumnName;
            Values = values;
            Size1 = Values.Count();
            Size2 = 1;
        }

        public string GetString()
        {
            return TextWorker.GetTableData((ArgumentsColumnName, ArgumentsValues), (ValuesColumnName, Values));
        }

        public IEnumerable<double> GetValues()
        {
            return Values;
        }
    }
}
