using CommunityToolkit.Mvvm.ComponentModel;
using CrossHealthX.Models;
using CrossHealthX.Services;

namespace CrossHealthX.ViewModels;

public partial class ActivityMainModelView : ObservableObject
{
    private readonly LocationService _locationService;

    public Activity? LastActivity = null;

    [ObservableProperty] private int steps;
    [ObservableProperty] private int calories;
    [ObservableProperty] private double distance;
    [ObservableProperty] private DateTime date;
    [ObservableProperty] private bool hasHistory = false;
    [ObservableProperty] private Color topBackgroundColor = Colors.Purple;
    [ObservableProperty] private Color bottomBackgroundColor = Colors.DarkPurple;
    [ObservableProperty] private double weekProgress;
    [ObservableProperty] private double monthProgress;
    [ObservableProperty] private double yearProgress;

    private CancellationTokenSource _trackingTokenSource;

    public ActivityMainModelView()
    {
        _locationService = new LocationService();
        Populate();
        StartTracking();
    }

    private void StartTracking()
    {
        _trackingTokenSource = new CancellationTokenSource();
        var token = _trackingTokenSource.Token;

        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                Steps = await _locationService.GetStepCountAsync();
                var currentLocation = await _locationService.GetCurrentLocationAsync();
                Distance = _locationService.TotalDistance;
                Calories = CalculateCalories(Steps, Distance);
                Date = DateTime.Now;

                await SaveCurrentActivityAsync();

                await Task.Delay(5000, token);
            }
        }, token);
    }

    private int CalculateCalories(int steps, double distance)
    {
        return (int)(steps * 0.04);
    }

    private async Task SaveCurrentActivityAsync()
    {
        var activity = new Activity
        {
            Steps = Steps,
            Calories = Calories,
            Distance = Distance,
            Date = Date
        };

        await App.ActivityDatabase.SaveActivityAsync(activity);
        Populate();
    }

    public async void Populate()
    {
        var activities = await App.ActivityDatabase.GetActivities();
        if (activities.Count == 0)
        {
            HasHistory = false;
            Steps = Calories = 0;
            Distance = 0;
            Date = DateTime.Now;
            WeekProgress = MonthProgress = YearProgress = 0;
            return;
        }

        activities = activities.OrderByDescending(a => a.Date).ToList();
        LastActivity = activities[0];

        Steps = LastActivity.Steps;
        Calories = LastActivity.Calories;
        Distance = LastActivity.Distance;
        Date = LastActivity.Date;
        HasHistory = activities.Count > 1;

        WeekProgress = CalculateProgress(activities, 7);
        MonthProgress = CalculateProgress(activities, 30);
        YearProgress = CalculateProgress(activities, 365);
    }

    private double CalculateProgress(List<Activity> activities, int days)
    {
        var cutoff = DateTime.Today.AddDays(-days);
        var filtered = activities.Where(a => a.Date >= cutoff).OrderBy(a => a.Date).ToList();
        if (filtered.Count < 2) return 0;
        return filtered.Last().Steps - filtered.First().Steps;
    }

    public void StopTracking()
    {
        _trackingTokenSource?.Cancel();
    }
}
