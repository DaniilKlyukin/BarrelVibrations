using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace BasicLibraryWinForm.PropertiesTemplates.TypeEditors
{
    public class PropertyFormTypeEditor<TV> : UITypeEditor
    {
        public PropertyForm<TV> Form { get; set; } = new();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService? editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (value is TV values && editorService != null)
            {
                Form.SetValue(values);
            }

            editorService.ShowDialog(Form);

            return Form.GetValue();
        }
    }
}
