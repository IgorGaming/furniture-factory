using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Catel.Data;
using Catel.MVVM.Converters;

namespace FurnitureFactoryApp.Converters {
    class DecimalToStringConverter : ValueConverterBase {

        protected override object Convert(object value, Type targetType, object parameter) {
            return value != null ? value.ToString() : string.Empty;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter) {
            decimal tmp = 0.0M;
            decimal? result = null;

            if (decimal.TryParse(value as string, out tmp)) {
                result = tmp;
            }

            return BoxingCache.GetBoxedValue(result);
        }
    }
}
