using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDOWin.Converters;

class BoolActiveConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is not bool b) return string.Empty;
        return b == true ? "Mark Inactive" : "Mark Active";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
