using CrossHealthX.Models;
using CrossHealthX.ViewModels;
using CommunityToolkit.Maui.Views;

namespace CrossHealthX.Views;

public partial class ActivityHistoryPage : ContentPage
{
    private readonly ActivityHistoryViewModel _viewModel;

    public ActivityHistoryPage()
    {
        InitializeComponent();
        _viewModel = new ActivityHistoryViewModel(App.ActivityDatabase);
        BindingContext = _viewModel;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Activity selectedActivity)
        {
            var shouldRefresh = await this.ShowPopupAsync(new DetailedActivityView(selectedActivity));
            if (shouldRefresh != null && (bool)shouldRefresh)
                await _viewModel.LoadActivitiesCommand.ExecuteAsync(null);

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
