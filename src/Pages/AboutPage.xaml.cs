namespace Uroskur.Pages;

public partial class AboutPage
{
    public AboutPage(AboutViewModel aboutViewModel)
    {
        InitializeComponent();

        BindingContext = aboutViewModel;
    }
}