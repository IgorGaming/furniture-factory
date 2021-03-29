namespace FurnitureFactoryApp.ViewModels.Orders {
    using Catel.Collections;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using FurnitureFactoryApp.Models;
    using FurnitureFactoryApp.Repositories;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderProductsAddWindowViewModel : ViewModelBase {

        public OrderProductsAddWindowViewModel(AddProductInOrderRepository productRepository) {
            ProductRepository = productRepository;
            Products = Product.Available();
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
        [Expose("Product")]
        [Expose("ProductCount")]
        public AddProductInOrderRepository ProductRepository { get; set; }

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; } = new ObservableCollection<Product>();

        public string SearchText { get; set; }

        public override string Title => "Добавление нового продукта в заказ.";
        #endregion

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) {
            if (ProductRepository == null) {
                return;
            }

            if (ProductRepository.Product != null) {
                if (ProductRepository.Product.Name == null) {
                    validationResults.Add(FieldValidationResult.CreateError("ProductRepository.Product", "Требуется выбрать продукт!"));
                }

                if (ProductRepository.ProductCount < 1) {
                    validationResults.Add(FieldValidationResult.CreateError("ProductRepository.ProductCount", "Количество товара не может быть меньше 1!"));
                }

                if (ProductRepository.Product.Id != 0) {
                    if (Products.Single((p) => p.Id == ProductRepository.Product.Id).Count < ProductRepository.ProductCount) {
                        validationResults.Add(FieldValidationResult.CreateError("ProductRepository.ProductCount", "У нас нет такого количества товара!"));
                    }
                }
            } else {
                validationResults.Add(FieldValidationResult.CreateError("ProductRepository.Product", "Требуется выбрать продукт!"));
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
        private void OnProductsChanged() => FilterProducts();
        private void OnSearchTextChanged() => FilterProducts();
        private void FilterProducts() {
            if (string.IsNullOrWhiteSpace(SearchText)) {
                FilteredProducts.ReplaceRange(Products);
            } else {
                FilteredProducts.ReplaceRange(Products.Where((c) => c.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));
            }
        }
        #endregion
    }
}
