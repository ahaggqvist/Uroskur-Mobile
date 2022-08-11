namespace Uroskur.Pages;

public partial class PreferencesPage
{
    private readonly PreferencesViewModel _preferencesViewModel;

    public PreferencesPage(PreferencesViewModel preferencesViewModel)
    {
        InitializeComponent();

        BindingContext = _preferencesViewModel = preferencesViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _preferencesViewModel.LoadPreferences();
    }
}