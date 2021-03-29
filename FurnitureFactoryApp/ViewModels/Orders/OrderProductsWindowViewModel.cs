namespace FurnitureFactoryApp.ViewModels.Orders {
    using Catel.Fody;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using FurnitureFactoryApp.Models;
    using FurnitureFactoryApp.Repositories;

    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderProductsWindowViewModel : ViewModelBase {

        private readonly IUIVisualizerService _uiVisualizerService;

        public OrderProductsWindowViewModel(Order order, IUIVisualizerService uiVisualizerService) {
            CloseWindowCommand = new TaskCommand(OnCloseWindowCommandExecute);

            AddProductsInOrderCommand = new TaskCommand(OnAddProductsInOrderCommandExecuteAsync);
            DeleteProductsFromOrderCommand = new Command(OnDeleteProductsFromOrderCommandExecute, OnDeleteProductsFromOrderCommandCanExecute);

            Order = order;

            _uiVisualizerService = uiVisualizerService;
        }

        #region Properties
        [Model]
        [Expose("Products")]
        public Order Order { get; set; }

        public Product SelectedProduct { get; set; }

        public override string Title => "Содержимое заказа.";
        #endregion


        #region Commands
        /*
         * Добавление продукта в заказ
         */
        public TaskCommand AddProductsInOrderCommand { get; private set; }
        private async Task OnAddProductsInOrderCommandExecuteAsync() {
            var productRepository = new AddProductInOrderRepository(new Product(), 1);
            var typeFactory = this.GetTypeFactory();
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<OrderProductsAddWindowViewModel>(productRepository);

            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false) {
                using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                    connection.Open();

                    //Количество добавляемого товара в данном заказе
                    var cmd = new SqlCommand($"SELECT COUNT(*) FROM order_product WHERE order_id = @order_id AND product_id = @product_id", connection);
                    cmd.Parameters.AddRange(new[] {
                        new SqlParameter("@order_id", Order.Id),
                        new SqlParameter("@product_id", productRepository.Product.Id),
                    });

                    int count = cmd.ExecuteScalar() == null ? 0 : (int)cmd.ExecuteScalar();

                    //Создаём команду на добавление и выполняем столько раз, сколько нужно
                    for (int i = 0; i < productRepository.ProductCount; i++) {
                        cmd = new SqlCommand($"INSERT INTO order_product (order_id, product_id) VALUES (@order_id, @product_id)", connection);
                        cmd.Parameters.AddRange(new[] {
                            new SqlParameter("@order_id", Order.Id),
                            new SqlParameter("@product_id", productRepository.Product.Id),
                        });

                        cmd.ExecuteNonQuery();
                    }

                    //Если товар добавляется в первый раз - добавляем в таблицу, иначе увеличиваем количество в таблице
                    if (count == 0) {
                        productRepository.Product.Count = productRepository.ProductCount;
                        Order.Products.Add(productRepository.Product);
                    } else {
                        Order.Products.Single((p) => p.Id == productRepository.Product.Id).Count = productRepository.ProductCount + count;
                    }

                    Order.UpdatedAt = DateTime.Now;
                    Trace.WriteLine("Успешное добавление товара в заказ!");
                }
            }
        }

        /*
         * Удаление продукта из заказа
         */
        public Command DeleteProductsFromOrderCommand { get; private set; }
        private bool OnDeleteProductsFromOrderCommandCanExecute() => SelectedProduct != null;
        private void OnDeleteProductsFromOrderCommandExecute() {
            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                int productId = SelectedProduct.Id;
                var cmd = new SqlCommand($"SELECT COUNT(*) FROM order_product WHERE order_id = @order_id AND product_id = @product_id", connection);
                cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@order_id", Order.Id),
                    new SqlParameter("@product_id", SelectedProduct.Id),
                });
                int count = cmd.ExecuteScalar() == null ? 0 : (int)cmd.ExecuteScalar();

                //Удаляем 1 ед. продукта из заказа
                cmd = new SqlCommand($"DELETE TOP(1) FROM order_product WHERE order_id = @order_id AND product_id = @product_id", connection);
                cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@order_id", Order.Id),
                    new SqlParameter("@product_id", SelectedProduct.Id),
                });
                cmd.ExecuteNonQuery();

                //Если товаров было больше 1, то уменьшаем количество в таблице, иначе удаляем строку из таблицы
                if (count == 1) {
                    Order.Products.Remove(SelectedProduct);
                } else {
                    Order.Products.Single((p) => p.Id == SelectedProduct.Id).Count = count - 1;
                }

                Order.UpdatedAt = DateTime.Now;
                Trace.WriteLine("Успешное удаление товара из заказа!");
            }
        }


        //Закрытие окна и его сохранение
        public TaskCommand CloseWindowCommand { get; private set; }
        async private Task OnCloseWindowCommandExecute() {
            await SaveViewModelAsync();
            await CloseViewModelAsync(true);
        }
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
