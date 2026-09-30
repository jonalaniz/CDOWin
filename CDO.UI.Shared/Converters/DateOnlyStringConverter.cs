using Microsoft.UI.Xaml.Data;
using System;

namespace CDO.UI.Shared.Converters;

public partial class DateOnlyStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is not DateOnly date) return string.Empty;
        return date == DateOnly.FromDateTime(DateTime.Today) ? "Today" : date.ToString("D");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
