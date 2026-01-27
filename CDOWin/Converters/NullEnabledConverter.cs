using Microsoft.UI.Xaml.Data;
using System;

namespace CDOWin.Converters;

class NullToBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        return value == null ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
