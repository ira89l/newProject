using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossHealthX.Models;
using CrossHealthX.Services;

namespace CrossHealthX.ViewModels;

public partial class ActivityViewModel : ObservableObject
{
    private readonly ActivityDatabase _db;
    private readonly LocationService _locationService;

    [ObservableProperty] private int steps;
    [ObservableProperty] private int calories;
    [ObservableProperty] private double distance;

    public ActivityViewModel(ActivityDatabase db, LocationService locationService)
    {
        _db = db;
        _locationService = locationService;

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        AddStepsCommand = new RelayCommand(AddSteps);
    }

    public IAsyncRelayCommand LoadDataCommand { get; }
    public IRelayCommand AddStepsCommand { get; }

    private async Task LoadDataAsync()
    {
        Steps = await _locationService.GetStepCountAsync();
        var location = await _locationService.GetCurrentLocationAsync();
        Distance = Steps * 0.762 / 1000;
        Calories = (int)(Steps * 0.04);

        var activity = new Activity
        {
            Steps = Steps,
            Calories = Calories,
            Distance = Distance,
            Date = DateTime.Today
        };

        await _db.SaveActivityAsync(activity);
    }

    private void AddSteps()
    {
        Steps += 100;
        Calories += (int)(100 * 0.04);
        Distance += 100 * 0.762 / 1000;
    }
}
