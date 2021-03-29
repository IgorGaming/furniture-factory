using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Products;

namespace FurnitureFactoryApp.Views.Products {
    public partial class ProductAddWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="ProductAddWindow"/> class.
        /// </summary>
        public ProductAddWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="ProductAddWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public ProductAddWindow(ProductAddWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
