using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace BasicLibraryWinForm.PropertiesTemplates.TypeEditors
{
    public class FormTypeEditor<T, TV> : UITypeEditor where T : Form, IEditable<TV>, new()
    {
        public T Form { get; set; } = new();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService? editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (value is TV values && editorService != null)
            {
                Form.SetValues(values);
            }

            editorService.ShowDialog(Form);

            return Form.GetValues();
        }
    }
}
