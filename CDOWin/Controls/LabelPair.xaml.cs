using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace CDOWin.Controls;

public sealed partial class LabelPair : UserControl {
    public string Label {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(LabelPair), new PropertyMetadata(""));

    public string Value {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(LabelPair), new PropertyMetadata("", OnValueChanged));

    public LabelPair() {
        InitializeComponent();
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = (LabelPair)d;
        control.UpdateOpacityBasedOnValue(e.NewValue);
    }

    private void UpdateOpacityBasedOnValue(object newValue) {
        var value = (string)newValue;

        if (string.IsNullOrEmpty(value)) {
            this.Opacity = 0.4;
        } else {
            this.Opacity = 1.0;
        }
    }

    private void StackPanel_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e) {
        if (string.IsNullOrWhiteSpace(Value))
            return;

        var data = new DataPackage();
        data.SetText(Value);
        Clipboard.SetContent(data);
        ShowCopied();
    }

    private async void ShowCopied() {
        var old = Label;
        Label = "Copied!";
        await Task.Delay(650);
        Label = old;
    }
}
