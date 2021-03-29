namespace FurnitureFactoryApp.ViewModels.Products {
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using FurnitureFactoryApp.Models;

    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductWindowViewModel : ViewModelBase {

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        public ProductWindowViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService) {
            //Продукты
            AddProductCommand = new TaskCommand(OnAddProductCommandExecuteAsync);
            UpdateProductCommand = new TaskCommand(OnUpdateProductCommandExecuteAsync, OnUpdateProductCommandCanExecute);
            DeleteProductCommand = new Command(OnDeleteProductCommandExecute, OnDeleteProductCommandCanExecute);
            RefreshProductCommand = new Command(OnRefreshProductCommandExecute);

            //Типы продуктов
            AddTypeCommand = new TaskCommand(OnAddTypeCommandExecuteAsync);
            UpdateTypeCommand = new TaskCommand(OnUpdateTypeCommandExecuteAsync, OnUpdateTypeCommandCanExecute);
            DeleteTypeCommand = new Command(OnDeleteTypeCommandExecute, OnDeleteTypeCommandCanExecute);
            RefreshTypeCommand = new Command(OnRefreshTypeCommandExecute);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            //Бинд на фильтр
            Products.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => FilterProducts();
            Types.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => FilterProductsType();
        }

        #region Properties
        public ObservableCollection<Product> Products { get; set; } = Product.All();

        public ObservableCollection<Product> FilteredProducts { get; set; } = new ObservableCollection<Product>();

        public Product SelectedProduct { get; set; }

        public ObservableCollection<Type> Types { get; set; } = Type.All();

        public ObservableCollection<Type> FilteredTypes { get; set; } = new ObservableCollection<Type>();

        public Type SelectedType { get; set; }

        public string SearchProductType { get; set; }

        public string SearchProduct { get; set; }

        public override string Title => "Список товаров.";
        #endregion

        #region Commands
        /*
         * Добавление продукта
         */
        public TaskCommand AddProductCommand { get; private set; }
        private async Task OnAddProductCommandExecuteAsync() {
            var product = new Product();

            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ProductAddWindowViewModel>(product);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                product.Save();
                Products.Add(product);
            }
        }

        /*
         * Изменение продукта
         */
        public TaskCommand UpdateProductCommand { get; private set; }
        private bool OnUpdateProductCommandCanExecute() => SelectedProduct != null;
        private async Task OnUpdateProductCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ProductUpdateWindowViewModel>(SelectedProduct);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                SelectedProduct.Save();
            }
        }

        /*
         * Удаление продукта 
         */
        public Command DeleteProductCommand { get; private set; }
        private bool OnDeleteProductCommandCanExecute() => SelectedProduct != null;
        private void OnDeleteProductCommandExecute() {
            SelectedProduct.Remove();
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
        }

        /*
         * Обновление таблицы
         */
        public Command RefreshProductCommand { get; private set; }
        private void OnRefreshProductCommandExecute() {
            Products.ReplaceRange(Product.All());
        }


        /*
         * Добавление типа продукта
         */
        public TaskCommand AddTypeCommand { get; private set; }

        private async Task OnAddTypeCommandExecuteAsync() {
            var type = new Type();

            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TypeAddWindowViewModel>(type);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                type.Save();
                Types.Add(type);
            }
        }

        /*
         * Изменение типа продукта
         */
        public TaskCommand UpdateTypeCommand { get; private set; }
        private bool OnUpdateTypeCommandCanExecute() => SelectedType != null;

        private async Task OnUpdateTypeCommandExecuteAsync() {
            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TypeUpdateWindowViewModel>(SelectedType);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                SelectedType.Save();
            }
        }

        /*
         * Удаление типа продукта
         */
        public Command DeleteTypeCommand { get; private set; }

        private bool OnDeleteTypeCommandCanExecute() => SelectedType != null;

        private void OnDeleteTypeCommandExecute() {
            if (SelectedType.Remove()) {
                Types.Remove(SelectedType);
                SelectedType = null;
            } else {
                _messageService.ShowErrorAsync(SelectedType.RemoveErrorInfo);
            }
        }


        /*
         * Обновление таблицы
         */
        public Command RefreshTypeCommand { get; private set; }
        private void OnRefreshTypeCommandExecute() {
            Types.ReplaceRange(Type.All());
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
        private void OnProductsChanged() => FilterProducts();
        private void OnSearchProductChanged() => FilterProducts();
        private void FilterProducts() {
            if (string.IsNullOrWhiteSpace(SearchProduct)) {
                FilteredProducts.ReplaceRange(Products);
            } else {
                FilteredProducts.ReplaceRange(Products.Where((c) => c.Name.Contains(SearchProduct, System.StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void OnTypesChanged() => FilterProductsType();
        private void OnSearchProductTypeChanged() => FilterProductsType();
        private void FilterProductsType() {
            if (string.IsNullOrWhiteSpace(SearchProductType)) {
                FilteredTypes.ReplaceRange(Types);
            } else {
                FilteredTypes.ReplaceRange(Types.Where((c) => c.Name.Contains(SearchProductType, System.StringComparison.OrdinalIgnoreCase)));
            }
        }
        #endregion
    }
}
