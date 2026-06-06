using Microsoft.UI.Xaml.Data;
using System;

namespace CDO.UI.Shared.Converters;

public partial class BoolCompletedConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is not bool b) return string.Empty;
        return b == true ? "Mark Incomplete" : "Mark Complete";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}