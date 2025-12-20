using Coursework.Data;

namespace Coursework
{
    public partial class App : Application
    {
        private readonly DatabaseService _database;

        public App(DatabaseService database)
        {
            InitializeComponent();
            _database = database;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _database.Initialize();
            return new Window(new MainPage()) { Title = "Coursework" };
        }
    }
}
