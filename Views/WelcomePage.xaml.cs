using CrossHealthX.ViewModels;

namespace CrossHealthX.Views;

public partial class WelcomePage : ContentPage
{
    private readonly WelcomeModelView _viewModel;

    public WelcomePage()
    {
        InitializeComponent();
        _viewModel = new WelcomeModelView();
        BindingContext = _viewModel;
    }

    private async void NextBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.Increment())
            await Navigation.PopAsync();
    }
}
