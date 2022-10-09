using Microsoft.Maui.Handlers;

namespace BymseBooks.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;
            
            MainPage = new AppShell();
        }
    }
}