using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace BasicLibraryWinForm.PropertiesTemplates
{
    public class CountArrayConverter : CollectionConverter
    {
#pragma warning disable CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
#pragma warning disable CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
#pragma warning disable CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
#pragma warning restore CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
#pragma warning restore CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
#pragma warning restore CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
        {
            if (destType == typeof(string) && value is ICollection)
            {
                if (value is not ICollection arr)
                    return "Пустая коллекция";

                switch (arr.Count)
                {
                    case > 1:
                        return $"Значений: {arr.Count}";
                    case 1:
                        {
                            var enumerator = arr.GetEnumerator();
                            enumerator.MoveNext();
                            return $"{enumerator.Current}";
                        }
                    case 0:
                        return "Пустая коллекция";
                }
            }

            return "Пустая коллекция";
        }
    }
}
