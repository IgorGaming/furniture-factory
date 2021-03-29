namespace FurnitureFactoryApp.ViewModels.Orders {
    using Catel.Collections;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using FurnitureFactoryApp.Models;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderAddWindowViewModel : ViewModelBase {

        public OrderAddWindowViewModel(Order order) {
            Order = order;
            Customers = Customer.All();

            StartDate = DateTime.Now;
        }

        #region Properties
        /*
        |--------------------------------------------------------------------------
        | Связывание свойств
        |--------------------------------------------------------------------------
        |
        | Здесь мы связываем свойства VM со свойствами в переданной модели, чтобы потом получить их в другом окне.
        | Expose - краткая запись(более подробно в док. Catel).
        | Для полной записи нужно объявить свойство, а затем написать [ViewModelToModel([ModelName])]
        |
        */

        [Model]
        [Expose("Customer")]
        [Expose("ShipDate")]
        public Order Order { get; set; }

        public ObservableCollection<Customer> Customers { get; set; }

        public ObservableCollection<Customer> FilteredCustomers { get; set; } = new ObservableCollection<Customer>();

        public DateTime StartDate { get; set; }

        public string SearchText { get; set; }

        public override string Title => "Создание нового заказа.";
        #endregion

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) {
            if (Order == null) {
                return;
            }

            if (Order.Customer == null) {
                validationResults.Add(FieldValidationResult.CreateError("Order.Customer", "Требуется выбрать заказчика!"));
            }

            if (Order.ShipDate.Date < StartDate.Date) {
                validationResults.Add(FieldValidationResult.CreateError("Order.ShipDate", "Некорректная дата доставки!"));
            }
        }

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
        private void OnCustomersChanged() => FilterCustomers();
        private void OnSearchTextChanged() => FilterCustomers();
        private void FilterCustomers() {
            if (string.IsNullOrWhiteSpace(SearchText)) {
                FilteredCustomers.ReplaceRange(Customers);
            } else {
                FilteredCustomers.ReplaceRange(Customers.Where((c) => c.Fio.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }
        }
        #endregion
    }
}
