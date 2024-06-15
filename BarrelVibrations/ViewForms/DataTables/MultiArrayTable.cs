using BasicLibraryWinForm;
using ScottPlot;

namespace BarrelVibrations.ViewForms.DataTables
{
    public class MultiArrayTable : ITableData
    {
        public int Size1 { get; }
        public int Size2 { get; }
        public string ArgumentsColumnName { get; protected set; }
        public IEnumerable<double> ArgumentsValues { get; protected set; }
        public string ValuesColumnName { get; protected set; }
        public double[,] Values { get; protected set; }

        public string Name { get; protected set; }
        public Action<double, FormsPlot> AdditionalDrawMethod { get; set; } = (t, plot) => { };

        public MultiArrayTable(string name, string argumentsColumnName, IEnumerable<double> argumentsValues, string valuesColumnName, double[,] values)
        {
            if (argumentsValues.Count() != values.GetLength(0))
            {
                var n = Math.Min(argumentsValues.Count(), values.GetLength(0));

                Name = name;
                ArgumentsColumnName = argumentsColumnName;
                ArgumentsValues = argumentsValues.Take(n);
                ValuesColumnName = valuesColumnName;
                Values = new double[n, values.GetLength(1)];

                for (int i = 0; i < Values.GetLength(0); i++)
                {
                    for (int j = 0; j < Values.GetLength(1); j++)
                    {
                        Values[i,j] = values[i, j];
                    }
                }

                Size1 = Values.GetLength(0);
                Size2 = Values.GetLength(1);

                Logger.Log($"Ошибка при создании таблицы данных. Разные размеры массивов \"{Name}\"");
                return;
            }

            Name = name;
            ArgumentsColumnName = argumentsColumnName;
            ArgumentsValues = argumentsValues;
            ValuesColumnName = valuesColumnName;
            Values = values;
            Size1 = values.GetLength(0);
            Size2 = values.GetLength(1);
        }

        public string GetString()
        {
            var data = new List<(string, IEnumerable<double>)> { (ArgumentsColumnName, ArgumentsValues) };

            for (var i = 0; i < Values.GetLength(1); i++)
            {
                data.Add(($"{ValuesColumnName} {i}", Values.GetColumnSlice(i).ToArray()));
            }

            return TextWorker.GetTableData(data.ToArray());
        }

        public IEnumerable<double> GetValues()
        {
            var values = new double[Values.Length];

            for (var j = 0; j < Values.GetLength(1); j++)
            {
                for (var i = 0; i < Values.GetLength(0); i++)
                {
                    values[i + j * Values.GetLength(0)] = Values[i, j];
                }
            }

            return values;
        }
    }
}
