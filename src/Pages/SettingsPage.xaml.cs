namespace Uroskur.Pages;

public partial class SettingsPage
{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();

        BindingContext = _settingsViewModel = settingsViewModel;
    }
}