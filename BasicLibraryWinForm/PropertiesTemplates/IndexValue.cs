namespace BasicLibraryWinForm.PropertiesTemplates
{
    public class IndexValue
    {
        public int Index { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return $"{Index} | {Value}";
        }

        public static IndexValue ConvertFromString(string str)
        {
            var data = str.Split(' ');

            return new IndexValue((int)TextWorker.Parse(data[0]), TextWorker.Parse(data[2]));
        }

        public IndexValue()
        {
            Index = 0;
            Value = 0;
        }

        public IndexValue(int index, double value)
        {
            Index = index;
            Value = value;
        }
    }
}
