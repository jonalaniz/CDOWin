using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.Converters;

public class BoolToSolidBrushConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if ((bool)value)
            return new SolidColorBrush(Colors.Black);

        return Application.Current.Resources["AccentAAFillColorDefaultBrush"] as SolidColorBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}