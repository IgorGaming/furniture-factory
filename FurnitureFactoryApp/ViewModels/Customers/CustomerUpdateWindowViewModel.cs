namespace FurnitureFactoryApp.ViewModels.Customers {
    using Catel.Fody;
    using Catel.MVVM;

    using FurnitureFactoryApp.Models;

    using System.Threading.Tasks;

    public class CustomerUpdateWindowViewModel : ViewModelBase {
        public CustomerUpdateWindowViewModel(Customer customer) {
            Customer = customer;
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
        [Expose("Fio")]
        [Expose("Address")]
        [Expose("Telephone")]
        public Customer Customer { get; set; }

        public override string Title => "Редактирование заказчика.";
        #endregion

        #region Methods
        protected override async Task InitializeAsync() {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync() {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
        #endregion
    }
}
