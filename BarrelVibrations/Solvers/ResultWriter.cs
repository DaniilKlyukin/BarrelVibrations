namespace BarrelVibrations.Solvers
{
    public class ResultWriter
    {
        private FileStream fs;
        private StreamWriter sw;
        private Func<string> getStringToWrite;

        public ResultWriter(string fileName, Func<string> getStringToWrite)
        {
            if (!Directory.Exists(Resource.CalculationFilesFolder))
                Directory.CreateDirectory(Resource.CalculationFilesFolder);

            var filePath = Path.Combine(Resource.CalculationFilesFolder, fileName + ".txt");

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Create(filePath).Close();

            fs = File.OpenWrite(filePath);
            sw = new StreamWriter(fs);
            this.getStringToWrite = getStringToWrite;
        }

        public void Write()
        {
            sw.WriteLine(getStringToWrite());
        }

        public void Close()
        {
            sw.Close();
            fs.Close();
        }
    }
}
