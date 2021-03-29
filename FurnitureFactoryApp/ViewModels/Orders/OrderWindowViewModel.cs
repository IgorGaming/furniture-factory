namespace FurnitureFactoryApp.ViewModels.Orders {
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using FurnitureFactoryApp.Models;

    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderWindowViewModel : ViewModelBase {

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        public OrderWindowViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService) {
            AddOrderCommand = new TaskCommand(OnAddOrderCommandExecuteAsync);
            ShowProductsInOrderCommand = new TaskCommand(OnShowProductsInOrderCommandExecuteAsync, OnShowProductsInOrderCommandCanExecute);
            DeleteOrderCommand = new Command(OnDeleteOrderCommandExecute, OnDeleteOrderCommandCanExecute);
            RefreshOrderCommand = new Command(OnRefreshOrderCommandExecute);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            Orders = Order.All();

            //Бинд на фильтрацию
            Orders.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => FilterOrders();
        }

        #region Properties
        public ObservableCollection<Order> Orders { get; set; }

        public ObservableCollection<Order> FilteredOrders { get; set; } = new ObservableCollection<Order>();

        public Order SelectedOrder { get; set; }

        public string SearchOrder { get; set; }

        public override string Title => "Заказы.";
        #endregion

        #region Commands
        /*
         * Создание заказа
         */
        public TaskCommand AddOrderCommand { get; private set; }
        private async Task OnAddOrderCommandExecuteAsync() {
            var order = new Order();

            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<OrderAddWindowViewModel>(order);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                order.OrderDate = DateTime.Now;

                order.Save();
                Orders.Add(order);

                SelectedOrder = order;
                ShowProductsInOrderCommand.Execute();
            }
        }

        /*
         * Просмотр содержимого
         */
        public TaskCommand ShowProductsInOrderCommand { get; private set; }
        private bool OnShowProductsInOrderCommandCanExecute() => SelectedOrder != null;

        private async Task OnShowProductsInOrderCommandExecuteAsync() {
            var order = SelectedOrder;
            order.LoadProducts();
            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<OrderProductsWindowViewModel>(order);

            await _uiVisualizerService.ShowDialogAsync(viewModel);
        }

        /*
         * Удаление заказа
         */
        public Command DeleteOrderCommand { get; private set; }

        private bool OnDeleteOrderCommandCanExecute() => SelectedOrder != null;

        private void OnDeleteOrderCommandExecute() {
            SelectedOrder.Remove();
            Orders.Remove(SelectedOrder);
            SelectedOrder = null;
        }

        /*
         * Обновление таблицы
         */
        public Command RefreshOrderCommand { get; private set; }
        private void OnRefreshOrderCommandExecute() {
            Orders.ReplaceRange(Order.All());
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

        /*
         * Фильтрация текста
         */
        private void OnOrdersChanged() => FilterOrders();
        private void OnSearchOrderChanged() => FilterOrders();
        private void FilterOrders() {
            if (string.IsNullOrWhiteSpace(SearchOrder)) {
                FilteredOrders.ReplaceRange(Orders);
            } else {
                FilteredOrders.ReplaceRange(Orders.Where((c) => c.Customer.Fio.Contains(SearchOrder, StringComparison.OrdinalIgnoreCase)));
            }
        }
        #endregion
    }
}
