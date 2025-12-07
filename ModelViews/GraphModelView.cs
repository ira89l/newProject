using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using CrossHealthX.Models;

namespace CrossHealthX.ViewModels;

public partial class ActivityGraphModelView : ObservableObject
{
    [ObservableProperty] private string selectedInterval = "Week";
    [ObservableProperty] private LineChart chartData;
    [ObservableProperty] private List<Activity> activitiesHistory = new List<Activity>();

    public ActivityGraphModelView()
    {
        Populate();
    }

    partial void OnSelectedIntervalChanged(string value) => Populate();

    public async void Populate()
    {
        int intervalDays = SelectedInterval switch
        {
            "Month" => -30,
            "Year" => -365,
            _ => -7
        };

        var dateLimit = DateTime.Today.AddDays(intervalDays);
        ActivitiesHistory = (await App.ActivityDatabase.GetActivities())
                            .Where(a => a.Date >= dateLimit)
                            .OrderBy(a => a.Date)
                            .ToList();

        if (!ActivitiesHistory.Any())
        {
            ChartData = new LineChart
            {
                Entries = new List<ChartEntry> { new ChartEntry(0) { Label = "-", ValueLabel = "0", Color = SKColors.Black } }
            };
            return;
        }

        var entries = ActivitiesHistory.Select(a => new ChartEntry(a.Steps)
        {
            Label = a.Date.ToString("MM/dd"),
            ValueLabel = a.Steps.ToString(),
            Color = SKColor.Parse("#77d065")
        }).ToList();

        ChartData = new LineChart { Entries = entries };
    }
}
