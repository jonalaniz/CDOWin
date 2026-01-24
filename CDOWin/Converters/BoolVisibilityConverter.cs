using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace CDOWin.Converters;
public class BoolVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is bool b) return b == true ? Visibility.Visible : Visibility.Collapsed;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}