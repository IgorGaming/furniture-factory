namespace FurnitureFactoryApp {
    using System.Diagnostics;
    using System.Windows;

    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Catel.Windows;
    using Catel.Windows.Controls;

    using FurnitureFactoryApp.ViewModels.Customers;
    using FurnitureFactoryApp.Views.Customers;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e) {
#if DEBUG
            LogManager.AddDebugListener();
#endif

            Log.Info("Starting application");

            // Want to improve performance? Uncomment the lines below. Note though that this will disable
            // some features. 
            //
            // For more information, see http://docs.catelproject.com/vnext/faq/performance-considerations/

            // Log.Info("Improving performance");
            // Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            // Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            // TODO: Register custom types in the ServiceLocator


            /*
            |--------------------------------------------------------------------------
            | Регистрация типов для Dependency Injection
            |--------------------------------------------------------------------------
            | 
            | Пример: serviceLocator.RegisterType<IMyInterface, IMyClass>();
            | 
            */
            //Log.Info("Registering custom types");
            //var serviceLocator = ServiceLocator.Default;

            /*
             * Регистрация окон
             */
            //var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();

            //Заказчик
            //uiVisualizerService.Register(typeof(CustomerWindowViewModel), typeof(CustomerWindow));
            //uiVisualizerService.Register(typeof(CustomerAddWindowViewModel), typeof(CustomerAddWindow));
            //uiVisualizerService.Register(typeof(CustomerUpdateWindowViewModel), typeof(CustomerUpdateWindow));


            /*
            |--------------------------------------------------------------------------
            | Регистрация новых NamingConventions
            |--------------------------------------------------------------------------
            |
            | Они будут использованы для автоматического поиска view по viewModel
            |
            */
            Log.Info("Registering naming conventions");
            var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();
            viewLocator.NamingConventions.Add("[AS].Views.Customers.[VM]");
            viewLocator.NamingConventions.Add("[AS].Views.Products.[VM]");
            viewLocator.NamingConventions.Add("[AS].Views.Orders.[VM]");


            /*
            |--------------------------------------------------------------------------
            | Другие настройки
            |--------------------------------------------------------------------------
            */
            Log.Info("Setup other settings");
            InfoBarMessageControl.DefaultTextPropertyValue = "Обнаружены ошибки. Наведите сюда, чтобы посмотреть их.";

            // To auto-forward styles, check out Orchestra (see https://github.com/wildgums/orchestra)
            // StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);
        }
    }
}