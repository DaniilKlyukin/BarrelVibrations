using System.ComponentModel;

namespace BasicLibraryWinForm.PropertiesTemplates
{
    public class CustomSortedCategoryAttribute : CategoryAttribute
    {
        private const char NonPrintableChar = '\t';

        public CustomSortedCategoryAttribute(string category,
            ushort categoryPos,
            ushort totalCategories)
            : base(category.PadLeft(category.Length + (totalCategories - categoryPos),
                NonPrintableChar))
        {
        }
    }
}
