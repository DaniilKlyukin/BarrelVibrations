using BasicLibraryWinForm;

namespace BarrelVibrations.ViewForms.DataTables
{
    public class FileData
    {
        public string FilePath { get; }
        public string Header { get; }
        public string ArgumentsHeader { get; }
        public string DataHeader { get; }
        public double DataMultiplier { get; }

        public FileData(string filePath, string header, string argumentsHeader, string dataHeader, string ext = ".txt", double dataMultiplier = 1)
        {
            FilePath = filePath + ext;
            Header = header;
            ArgumentsHeader = argumentsHeader;
            DataHeader = dataHeader;
            DataMultiplier = dataMultiplier;
        }

        public double[] GetFileDataArray(int row)
        {
            return FileWorker.ReadFileRow(FilePath, row).Mult(DataMultiplier).ToArray();
        }
    }
}
