namespace FurnitureFactoryApp.ViewModels {
    using Catel.MVVM;
    using System.Threading.Tasks;
    using Catel.Services;
    using Catel.IoC;
    using FurnitureFactoryApp.ViewModels.Customers;
    using FurnitureFactoryApp.ViewModels.Products;
    using FurnitureFactoryApp.ViewModels.Orders;

    public class MainWindowViewModel : ViewModelBase {

        private readonly IUIVisualizerService _uiVisualizerService;

        public MainWindowViewModel(IUIVisualizerService uiVisualizerService) {
            //Команда закрытия приложения
            CloseApplicationCommand = new Command(OnCloseApplicationCommandExecute);

            //Команды открытия окон
            OpenCustomerWindowCommand = new TaskCommand(OnOpenCustomerWindowCommandExecuteAsync);
            OpenProductWindowCommand = new TaskCommand(OnOpenProductWindowCommandExecuteAsync);
            OpenOrderWindowCommand = new TaskCommand(OnOpenOrderWindowCommandExecuteAsync);

            _uiVisualizerService = uiVisualizerService;
        }

        #region Properties
        public override string Title => "Приложение для мебельной фабрики.";
        #endregion

        #region Commands
        /*
         * Закрытие приложения
         */
        public Command CloseApplicationCommand { get; private set; }
        private void OnCloseApplicationCommandExecute() => App.Current.Shutdown();

        /*
         * Открытие окна "Заказчики"
         */
        public TaskCommand OpenCustomerWindowCommand { get; private set; }
        private async Task OnOpenCustomerWindowCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var customerViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<CustomerWindowViewModel>();
            await _uiVisualizerService.ShowAsync(customerViewModel);
        }

        /*
         * Открытие окна "Продукты"
         */
        public TaskCommand OpenProductWindowCommand { get; private set; }
        private async Task OnOpenProductWindowCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var productViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ProductWindowViewModel>();
            await _uiVisualizerService.ShowAsync(productViewModel);
        }

        /*
         * Открытие окна "Заказы"
         */
        public TaskCommand OpenOrderWindowCommand { get; private set; }
        private async Task OnOpenOrderWindowCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var orderViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<OrderWindowViewModel>();
            await _uiVisualizerService.ShowAsync(orderViewModel);
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync() {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync() {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
        #endregion
    }
}
