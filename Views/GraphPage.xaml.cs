using Microcharts;
using SkiaSharp;
using CommunityToolkit.Maui.Views;
using CrossHealthX.Models;
using CrossHealthX.ViewModels;

namespace CrossHealthX.Views;

public partial class GraphPage : ContentPage
{
    private readonly ActivityGraphModelView _viewModel;

    public GraphPage()
    {
        InitializeComponent();
        _viewModel = new ActivityGraphModelView(App.ActivityDatabase);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Populate();
    }

    private async void DataHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (((CollectionView)sender).SelectedItem is Activity selectedActivity)
        {
            var shouldRefresh = await this.ShowPopupAsync(new DetailedActivityView(selectedActivity));
            if (shouldRefresh != null && (bool)shouldRefresh)
            {
                _viewModel.Populate();
            }

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
