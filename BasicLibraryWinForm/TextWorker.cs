using BasicLibraryWinForm;
using System.Globalization;
using System.Text;

namespace BasicLibraryWinForm
{
    public static class TextWorker
    {
        public static double Parse(string? text, double defaultValue = 0.0)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            if (double.TryParse(text.ToString(CultureInfo.InvariantCulture), out var value1))
                return value1;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            return double.TryParse(text.Replace('.', ',').ToString(CultureInfo.InvariantCulture), out var value2)
                ? value2
                : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns">массив столбцов формата: заголовок, значения </param>
        /// <returns></returns>
        public static string GetTableData(params (string, IEnumerable<double>)[] columns)
        {
            var sb = new StringBuilder();

            sb.AppendJoin("\t", columns.Select(v => v.Item1));
            sb.AppendLine();

            var length = columns.Min(c => c.Item2.Count());
            var arrs = columns.Select(a => a.Item2.ToArray()).ToArray();

            for (var i = 0; i < length; i++)
            {
                var row = new List<string>();

                for (var j = 0; j < columns.Length; j++)
                {
                    row.Add(
                        i < length
                        ? $"{arrs[j][i]}"
                        : "");
                }

                sb.AppendJoin("\t", row);
                if (i < length - 1)
                    sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
