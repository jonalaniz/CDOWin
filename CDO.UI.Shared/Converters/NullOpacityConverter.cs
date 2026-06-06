using Microsoft.UI.Xaml.Data;
using System;

namespace CDO.UI.Shared.Converters;

public partial class NullOpacityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        return value == null ? 0.5 : 1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}