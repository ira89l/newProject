using WeightTracker.Services;

namespace WeightTracker;

public partial class App : Application
{
    public static ActivityDatabase ActivityDatabase { get; private set; }

    public App()
    {
        InitializeComponent();

        // Ініціалізація бази даних для всього додатка
        ActivityDatabase = new ActivityDatabase();

        MainPage = new AppShell();
    }
}
