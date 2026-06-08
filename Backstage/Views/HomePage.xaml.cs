using Microsoft.UI.Xaml.Controls;
using System;

namespace Backstage.Views; 
public sealed partial class HomePage : Page {
    public HomePage() {
        InitializeComponent();
        SetupHelloText();
    }

    private void SetupHelloText() {
        HelloText.Text = $"Hello, {UserName()}";
    }

    private string UserName() {
        return char.ToUpper(Environment.UserName[0]) + Environment.UserName[1..];
    }
}
