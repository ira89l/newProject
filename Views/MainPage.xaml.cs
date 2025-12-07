using CommunityToolkit.Maui.Views;
using CrossHealthX.ViewModels;

namespace CrossHealthX.Views;

public partial class MainPage : ContentPage
{
    private readonly ActivityMainModelView _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new ActivityMainModelView();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Якщо додаток не ініціалізовано – перенаправляємо на welcome page
        if (!Preferences.ContainsKey("isAppInit"))
            Navigation.PushAsync(new WelcomePage());
    }

    private async void NewBtn_Clicked(object sender, EventArgs e)
    {
        // Відкриваємо попап для додавання активності
        var shouldRefresh = await this.ShowPopupAsync(new NewActivityView());
        if (shouldRefresh != null && (bool)shouldRefresh)
        {
            _viewModel.Populate();
        }
    }

    private async void GraphBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GraphPage());
    }

    private async void SettingsBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}
