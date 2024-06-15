using BarrelVibrations.ModelingObjects.MissileFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.Data;
using System.Text;

namespace BarrelVibrations.PropertyGridClasses.MissileFolder
{
    public partial class MissileRegressionsForm : Form, IEditable<MissileRegressions>
    {

        public MissileRegressionsForm()
        {
            InitializeComponent();

            CxDataGridView.RowCount = MissileRegressions.CMachs.Length - 1;
            CyDataGridView.RowCount = MissileRegressions.CMachs.Length - 1;
            CzDataGridView.RowCount = 1;
            MxDataGridView.RowCount = 1;
            MyDataGridView.RowCount = 1;
            MzDataGridView.RowCount = 1;

            for (int i = 0; i < MissileRegressions.CMachs.Length - 1; i++)
            {
                CxDataGridView.Rows[i].Cells[0].Value =
                    $"M[{MissileRegressions.CMachs[i]} : {MissileRegressions.CMachs[i + 1]}]";

                CyDataGridView.Rows[i].Cells[0].Value =
                    $"M[{MissileRegressions.CMachs[i]} : {MissileRegressions.CMachs[i + 1]}]";
            }
        }

        public void SetValues(MissileRegressions missileRegressions)
        {
            for (int i = 0; i < missileRegressions.MxRegression.Length; i++)
            {
                MxDataGridView.Rows[0].Cells[i].Value = missileRegressions.MxRegression[i];
                MyDataGridView.Rows[0].Cells[i].Value = missileRegressions.MyRegression[i];
                MzDataGridView.Rows[0].Cells[i].Value = missileRegressions.MzRegression[i];
            }

            for (int i = 0; i < missileRegressions.CzRegression.Length; i++)
            {
                CzDataGridView.Rows[0].Cells[i].Value = missileRegressions.CzRegression[i];
            }

            for (int i = 0; i < missileRegressions.CxRegression.GetLength(0); i++)
            {
                for (int j = 0; j < missileRegressions.CxRegression.GetLength(1); j++)
                {
                    CxDataGridView.Rows[i].Cells[j + 1].Value = missileRegressions.CxRegression[i, j];
                    CyDataGridView.Rows[i].Cells[j + 1].Value = missileRegressions.CyRegression[i, j];
                }
            }
        }

        public MissileRegressions GetValues()
        {
            var mx = new double[MxDataGridView.ColumnCount];
            var my = new double[MyDataGridView.ColumnCount];
            var mz = new double[MzDataGridView.ColumnCount];

            var cx = new double[CxDataGridView.RowCount, CxDataGridView.ColumnCount - 1];
            var cy = new double[CyDataGridView.RowCount, CyDataGridView.ColumnCount - 1];
            var cz = new double[CzDataGridView.ColumnCount];

            for (int i = 0; i < mx.Length; i++)
            {
                mx[i] = TextWorker.Parse(MxDataGridView.Rows[0].Cells[i].Value.ToString());
                my[i] = TextWorker.Parse(MyDataGridView.Rows[0].Cells[i].Value.ToString());
                mz[i] = TextWorker.Parse(MzDataGridView.Rows[0].Cells[i].Value.ToString());
            }

            for (int i = 0; i < cz.Length; i++)
            {
                cz[i] = TextWorker.Parse(CzDataGridView.Rows[0].Cells[i].Value.ToString());
            }

            for (int i = 0; i < cx.GetLength(0); i++)
            {
                for (int j = 0; j < cx.GetLength(1); j++)
                {
                    cx[i, j] = TextWorker.Parse(CxDataGridView.Rows[i].Cells[j + 1].Value.ToString());
                    cy[i, j] = TextWorker.Parse(CyDataGridView.Rows[i].Cells[j + 1].Value.ToString());
                }
            }

            return new MissileRegressions(cx, cy, cz, mx, my, mz);
        }

        private void PasteValuesToDataGrid(DataGridView dataGrid, double[,] data, int startRow, int startColumn)
        {
            for (int i = startRow; i < dataGrid.RowCount; i++)
            {
                for (int j = startColumn; j < dataGrid.ColumnCount; j++)
                {
                    var dataI = i - startRow;
                    var dataJ = j - startColumn;

                    if (dataI >= data.GetLength(0) || dataJ >= data.GetLength(1))
                        continue;

                    dataGrid.Rows[i].Cells[j].Value = data[i - startRow, j - startColumn];
                }
            }
        }

        private bool TryReadMatrixFromClipBoard(int rows, int columns, out double[,] matrix)
        {
            matrix = new double[rows, columns];

            var values = Clipboard.GetText()
                .Split('\n')
                .Select(row => row.Split('\t', ' ')
                    .Select(v => TextWorker.Parse(v))
                    .ToArray())
                .ToArray();

            if (values.Length != rows || values.Any(row => row.Length != columns))
                return false;

            matrix = values.Convert();
            return true;
        }

        private void CxDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(4, 5, out var m);

                if (isReaded)
                    PasteValuesToDataGrid(CxDataGridView, m, 0, 1);
            }
        }

        private void CyDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(4, 5, out var m);

                if (isReaded) 
                    PasteValuesToDataGrid(CyDataGridView, m, 0, 1);
            }
        }

        private void CzDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(1, 5, out var m);

                if (isReaded) 
                    PasteValuesToDataGrid(CzDataGridView, m, 0, 0);
            }
        }

        private void MxDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(1, 4, out var m);

                if (isReaded) 
                    PasteValuesToDataGrid(MxDataGridView, m, 0, 0);
            }
        }

        private void MyDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(1, 4, out var m);

                if (isReaded) 
                    PasteValuesToDataGrid(MyDataGridView, m, 0, 0);
            }
        }

        private void MzDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var isReaded = TryReadMatrixFromClipBoard(1, 4, out var m);

                if (isReaded) 
                    PasteValuesToDataGrid(MzDataGridView, m, 0, 0);
            }
        }
    }
}
