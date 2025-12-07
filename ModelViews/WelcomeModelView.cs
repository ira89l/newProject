using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CrossHealthX;

public partial class WelcomeModelView : ObservableObject
{
    private static string[] Colors = { "ee6002", "6200EE", "75e900", "FFDE03" };

    [ObservableProperty]
    private string title = Titles[0];

    private static string[] Titles = { "Hey There!", "Set Daily Goal", "Choose Unit", "Finish" };

    [ObservableProperty]
    private string pickerTitle = PickerTitles[0];

    private static string[] PickerTitles = { "", "Daily Step Goal", "Unit System", "" };

    [ObservableProperty]
    private List<string> pickerItems = new List<string>();

    public List<string>[] Items = {
        new List<string>(),
        new List<string>() {"5000", "8000", "10000", "15000"},
        new List<string>() {"Metric", "Imperial"},
        new List<string>()
    };

    [ObservableProperty]
    private string selectedItem = "";

    private int Index = 0;

    public WelcomeModelView() { }

    public bool Increment()
    {
        // обробка кроку
        if (Index == 1) 
        {
            if (string.IsNullOrWhiteSpace(SelectedItem))
            {
                var toast = Toast.Make("Please select daily step goal", ToastDuration.Short, 14);
                toast.Show();
                return false;
            }
            Preferences.Set("DailyStepGoal", int.Parse(SelectedItem));
        }
        else if (Index == 2)
        {
            if (string.IsNullOrWhiteSpace(SelectedItem))
            {
                var toast = Toast.Make("Please select unit system", ToastDuration.Short, 14);
                toast.Show();
                return false;
            }
            Preferences.Set("UnitSystem", SelectedItem);
        }

        Index++;
        if (Index == 4) return Save();

        // оновлення UI
        Title = Titles[Index];
        PickerTitle = PickerTitles[Index];
        PickerItems = Items[Index];
        SelectedItem = "";
        return false;
    }

    public bool Save()
    {
        Preferences.Set("isAppInit", DateTime.Now);
        return true;
    }
}
