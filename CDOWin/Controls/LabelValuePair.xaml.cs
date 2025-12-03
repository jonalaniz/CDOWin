using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace CDOWin.Controls;

public sealed partial class LabelValuePair : UserControl {
    public LabelValuePair() {
        InitializeComponent();
    }

    public string Label {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(LabelValuePair), new PropertyMetadata(""));

    public string Value {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(LabelValuePair), new PropertyMetadata("", OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = (LabelValuePair)d;
        control.UpdateOpacityBasedOnValue(e.NewValue);
    }

    private void UpdateOpacityBasedOnValue(object newValue) {
        var value = (string)newValue;

        if(string.IsNullOrEmpty(value)) {
            this.Opacity = 0.5;
        } else {
            this.Opacity = 1.0;
        }
    }
}
