using System.ComponentModel;
using System.Globalization;
using BasicLibraryWinForm.PropertiesTemplates;

namespace Visualization
{
    public class ExpandableSectionsConverter : CollectionConverter
    {
        public static List<IndexValue> Objects = new();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Objects);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string)
            {
                return IndexValue.ConvertFromString(value.ToString());
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
