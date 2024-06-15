using ScottPlot;

namespace BarrelVibrations.ViewForms.DataTables
{
    public interface ITableData
    {
        public string Name { get; }
        public int Size1 { get; }
        public int Size2 { get; }
        public string ArgumentsColumnName { get; }
        public IEnumerable<double> ArgumentsValues { get; }
        public string ValuesColumnName { get; }
        public string GetString();
        public IEnumerable<double> GetValues();
        public Action<double, FormsPlot> AdditionalDrawMethod { get; set; }
    }
}
