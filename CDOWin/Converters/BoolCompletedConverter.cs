using Microsoft.UI.Xaml.Data;
using System;

namespace CDOWin.Converters;

class BoolCompletedConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is bool b) {
            return b == true ? "Mark Incomplete" : "Mark Complete";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
