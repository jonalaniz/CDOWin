using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CDOWin.Controls;

public sealed partial class LabeledTextBox : UserControl {
    public event TextChangedEventHandler? TextChangedForwarded;
    public TextBox innerTextBox => this.InnerTextBox;

    public string Label {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(""));

    public string Value {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(""));

    public LabeledTextBox() {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
        TextChangedForwarded?.Invoke(this, e);
    }
}