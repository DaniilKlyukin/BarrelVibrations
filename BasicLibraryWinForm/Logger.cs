namespace BasicLibraryWinForm
{
    public static class Logger
    {
        private const string fileName = "Logs.txt";

        public static void Log(string message)
        {
            var text = $"{Environment.NewLine}{DateTime.Now}{Environment.NewLine}{message}{Environment.NewLine}";
            File.AppendAllText(fileName, text);
        }
    }
}
