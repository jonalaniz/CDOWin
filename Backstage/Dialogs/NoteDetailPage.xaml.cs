using CDO.Core.DTOs.Admin;
using Microsoft.UI.Xaml.Controls;

namespace Backstage.Dialogs;

public sealed partial class NoteDetailPage : Page {

    // =========================
    // Dependencies
    // =========================
    private readonly AdminClientNote _note;

    // =========================
    // Constructor
    // =========================
    public NoteDetailPage(AdminClientNote note) {
        _note = note;
        InitializeComponent();
    }
}
