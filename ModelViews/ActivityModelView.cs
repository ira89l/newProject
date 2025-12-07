using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossHealthX.Models;
using CrossHealthX.Services;

namespace CrossHealthX.ViewModels;

public partial class ActivityViewModel : ObservableObject
{
    private readonly ActivityDatabase _db;
    private readonly LocationService _locationService;

    [ObservableProperty]
    private int steps;

    [ObservableProperty]
    private int calories;

    [ObservableProperty]
    private double distance;

    public ActivityViewModel()
    {
        _db = new ActivityDatabase();
        _locationService = new LocationService();

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        AddStepsCommand = new RelayCommand(AddSteps);
    }

    public IAsyncRelayCommand LoadDataCommand { get; }
    public IRelayCommand AddStepsCommand { get; }

    private async Task LoadDataAsync()
    {
        // Отримати кроки з сенсора
        Steps = (int)await _locationService.GetStepCountAsync();

        // Отримати GPS для дистанції
        var location = await _locationService.GetCurrentLocationAsync();
        if (location != null)
        {
            // Простий приклад: дистанція = кроки * середня довжина кроку (0.762 м)
            Distance = Steps * 0.762 / 1000; // км
        }

        // Калорії: приблизно 0.04 ккал на крок
        Calories = (int)(Steps * 0.04);

        // Зберегти у базу
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
