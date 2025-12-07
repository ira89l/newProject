using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CrossHealthX.Models;
using CrossHealthX.Services;

namespace CrossHealthX.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Тут можна показати додаткові налаштування
        UnitText.ValueText = Preferences.Get("UnitSystem", "Metric");
        HeightText.ValueText = Preferences.Get("Height", "0");
        GenderText.ValueText = Preferences.Get("Gender", "UnSelected");
    }

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await App.ActivityDatabase.DeleteAllActivitiesAsync();

            Preferences.Remove("isAppInit");
            Preferences.Remove("UnitSystem");
            Preferences.Remove("Height");
            Preferences.Remove("Gender");

            var toast = Toast.Make("All activity data successfully deleted", ToastDuration.Short, 14);
            await toast.Show();
        }
        catch (Exception)
        {
            var toast = Toast.Make("Error deleting activity data", ToastDuration.Short, 14);
            await toast.Show();
        }
    }

    private async void ExportBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var activities = await App.ActivityDatabase.GetActivitiesAsync();
            activities = activities.OrderByDescending(a => a.Date).ToList();

            string csvFile = Path.Combine(FileSystem.CacheDirectory, "activity_history.csv");
            using (StreamWriter file = new StreamWriter(csvFile))
            {
                file.WriteLine("Date, Steps, Distance (km), Calories");

                foreach (var activity in activities)
                {
                    file.WriteLine($"{activity.Date:yyyy-MM-dd}, {activity.Steps}, {activity.Distance:F2}, {activity.Calories}");
                }
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Activity History",
                File = new ShareFile(csvFile)
            });
        }
        catch (Exception)
        {
            var toast = Toast.Make("Error exporting activity data", ToastDuration.Short, 14);
            await toast.Show();
        }
    }
}
