using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Products;

namespace FurnitureFactoryApp.Views.Products {
    public partial class ProductUpdateWindow : DataWindow {
        public ProductUpdateWindow()
            : this(null) { }

        public ProductUpdateWindow(ProductUpdateWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
