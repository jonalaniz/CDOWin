using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CDOWin.Controls;

public sealed partial class LabeledMultiLinePair : UserControl {
    public TextBox innerTextBox => this.InnerTextBox;

    public string Label {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledMultiLinePair), new PropertyMetadata(""));

    public Object Tag {
        get => (Object)GetValue(TagProperty);
        set => SetValue(TagProperty, value);
    }

    public static readonly DependencyProperty TagProperty = DependencyProperty.Register("TagProperty", typeof(Object), typeof(LabeledTextBox), new PropertyMetadata(""));

    public string Value {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(LabeledMultiLinePair), new PropertyMetadata(""));

    public LabeledMultiLinePair() {
        InitializeComponent();
    }

    public event TextChangedEventHandler TextChangedForwarded;

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        TextChangedForwarded?.Invoke(this, e);
    }
}