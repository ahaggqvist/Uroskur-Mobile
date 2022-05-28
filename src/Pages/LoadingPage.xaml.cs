namespace Uroskur.Pages;

public partial class LoadingPage
{
    private readonly LoadingViewModel _loadingViewModel;

    public LoadingPage(LoadingViewModel loadingViewModel)
    {
        InitializeComponent();

        BindingContext = _loadingViewModel = loadingViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(500);

        _loadingViewModel.NavigateTo();
    }
}