using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using CrossHealthX.Models;

namespace CrossHealthX.ViewModels;

public partial class ActivityGraphModelView : ObservableObject
{
    [ObservableProperty]
    private string selectedInterval = "Week";

    [ObservableProperty]
    private LineChart chartData;

    [ObservableProperty]
    private List<Activity> activitiesHistory = new List<Activity>();

    public ActivityGraphModelView()
    {
        Populate();
    }

    partial void OnSelectedIntervalChanged(string value)
    {
        Populate();
    }

    public async void Populate()
    {
        int intervalDays = -7;
        if (SelectedInterval == "Month") intervalDays = -30;
        if (SelectedInterval == "Year") intervalDays = -365;

        DateTime dateLimit = DateTime.Today.AddDays(intervalDays);
        ActivitiesHistory = (await App.Database.GetActivities())
                                .Where(a => a.Date >= dateLimit)
                                .OrderBy(a => a.Date)
                                .ToList();

        if (ActivitiesHistory.Count == 0)
        {
            ChartData = new LineChart
            {
                Entries = new List<ChartEntry> { new ChartEntry(0) { Label = "-", ValueLabel = "0", Color = SKColors.Black } }
            };
            return;
        }

        List<ChartEntry> entries = ActivitiesHistory.Select(a =>
            new ChartEntry(a.Steps)
            {
                Label = a.Date.ToString("MM/dd"),
                ValueLabel = a.Steps.ToString(),
                Color = SKColor.Parse("#77d065")
            }).ToList();

        ChartData = new LineChart { Entries = entries };
    }
}
