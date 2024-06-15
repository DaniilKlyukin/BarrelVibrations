using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace BasicLibraryWinForm.PropertiesTemplates
{
    public class MultiArrayConverter<T> : CollectionConverter
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

                var multiArr = (T[,])arr;

                return $"Значений: {multiArr.GetLength(0)} x {multiArr.GetLength(1)} (строк и столбцов)";
            }

            return "Пустая коллекция";
        }
    }
}
