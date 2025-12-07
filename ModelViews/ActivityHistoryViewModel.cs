using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossHealthX.Models;
using CrossHealthX.Services;

namespace CrossHealthX.ViewModels;

public partial class ActivityHistoryViewModel : ObservableObject
{
    private readonly ActivityDatabase _database;

    [ObservableProperty]
    private List<Activity> activities = new List<Activity>();

    [ObservableProperty]
    private string selectedInterval = "All"; // "All", "Week", "Month", "Year"

    public ActivityHistoryViewModel(ActivityDatabase database)
    {
        _database = database;
        LoadActivitiesCommand = new AsyncRelayCommand(LoadActivitiesAsync);
    }

    public IAsyncRelayCommand LoadActivitiesCommand { get; }

    [RelayCommand]
    public async Task LoadActivitiesAsync()
    {
        var allActivities = await _database.GetActivities();
        if (allActivities == null || allActivities.Count == 0)
        {
            Activities = new List<Activity>();
            return;
        }

        // сортування по даті (останнє зверху)
        allActivities = allActivities.OrderByDescending(a => a.Date).ToList();

        DateTime filterDate = DateTime.MinValue;
        switch (SelectedInterval)
        {
            case "Week": filterDate = DateTime.Today.AddDays(-7); break;
            case "Month": filterDate = DateTime.Today.AddMonths(-1); break;
            case "Year": filterDate = DateTime.Today.AddYears(-1); break;
            case "All":
            default: filterDate = DateTime.MinValue; break;
        }

        Activities = allActivities.Where(a => a.Date >= filterDate).ToList();
    }
}
