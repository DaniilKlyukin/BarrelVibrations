using BasicLibraryWinForm;
using Newtonsoft.Json;

namespace BasicLibraryWinForm
{
    public class FileWorker
    {
        public static IEnumerable<double> ReadFileRow(string path, int row, char[]? delimeters = null)
        {
            if (row < 0)
                return new List<double>();

            using var sr = new StreamReader(path);
            var currentRow = 0;

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (currentRow == row)
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                    return line
                        .Split(delimeters ?? new[] { ' ', '\t' })
                        .Select(x => TextWorker.Parse(x));
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

                currentRow++;
            }

            return new List<double>();
        }

        public static void SaveJsonNewton<T>(T data, string filePath)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(data);

                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception e)
            {
                File.WriteAllText(filePath + ".error", e.Message);
            }
        }

        public static T LoadJsonNewton<T>(string filePath)
        {
            var str = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<T>(str) ?? throw new ArgumentException();
        }
    }
}