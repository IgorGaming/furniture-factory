namespace FurnitureFactoryApp.ViewModels.Customers {
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using FurnitureFactoryApp.Models;
    using FurnitureFactoryApp.ViewModels.Orders;

    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Data;

    public class CustomerWindowViewModel : ViewModelBase {

        private readonly IUIVisualizerService _uiVisualizerService;

        public CustomerWindowViewModel(IUIVisualizerService uiVisualizerService) {
            //Команды управления заказчиками
            AddCustomerCommand = new TaskCommand(OnAddCustomerCommandExecuteAsync);
            UpdateCustomerCommand = new TaskCommand(OnUpdateCustomerCommandExecuteAsync, CanExecuteAction);
            DeleteCustomerCommand = new Command(OnDeleteCustomerCommandExecute, CanExecuteAction);
            RefreshCustomerCommand = new Command(OnRefreshCustomerCommandExecute);

            _uiVisualizerService = uiVisualizerService;

            //Загрузка всех заказчиков
            Customers = Customer.All();

            //Бинд на фильтрацию
            Customers.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => FilterCustomers();
        }

        #region Properties
        public override string Title => "Список заказчиков.";

        public ObservableCollection<Customer> Customers { get; set; }

        public ObservableCollection<Customer> FilteredCustomers { get; set; } = new ObservableCollection<Customer>();

        public Customer SelectedCustomer { get; set; }

        public string SearchCustomer { get; set; }
        #endregion


        #region Commands
        /*
         * Добавление заказчика
         */
        public TaskCommand AddCustomerCommand { get; private set; }
        private async Task OnAddCustomerCommandExecuteAsync() {
            var customer = new Customer();

            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<CustomerAddWindowViewModel>(customer);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                customer.Save();
                Customers.Add(customer);
            }
        }

        /*
         * Изменение заказчика
         */
        public TaskCommand UpdateCustomerCommand { get; private set; }
        private async Task OnUpdateCustomerCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<CustomerUpdateWindowViewModel>(SelectedCustomer);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                SelectedCustomer.Save();
            }
        }

        /*
         * Удаление заказчика 
         */
        public Command DeleteCustomerCommand { get; private set; }
        private void OnDeleteCustomerCommandExecute() {
            SelectedCustomer.Remove();
            Customers.Remove(SelectedCustomer);
            SelectedCustomer = null;
        }

        /*
         * Можно ли выполнять команду
         */
        private bool CanExecuteAction() => SelectedCustomer != null;


        /*
         * Обновление таблицы
         */
        public Command RefreshCustomerCommand { get; private set; }
        private void OnRefreshCustomerCommandExecute() {
            Customers.ReplaceRange(Customer.All());
        }
        #endregion

        #region Methods

        protected override async Task InitializeAsync() {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync() {

            await base.CloseAsync();
        }

        /*
         * Фильтрация текста
         */
        private void OnCustomersChanged() => FilterCustomers();
        private void OnSearchCustomerChanged() => FilterCustomers();
        private void FilterCustomers() {
            if (string.IsNullOrWhiteSpace(SearchCustomer)) {
                FilteredCustomers.ReplaceRange(Customers);
            } else {
                FilteredCustomers.ReplaceRange(Customers.Where((c) => c.Fio.Contains(SearchCustomer, System.StringComparison.OrdinalIgnoreCase)));
            }
        }
        #endregion
    }
}
