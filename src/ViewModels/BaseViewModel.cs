namespace Uroskur.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty] private bool _isRefreshing;


    [ObservableProperty] private string? _title = string.Empty;

    public bool IsNotBusy => !IsBusy;
}