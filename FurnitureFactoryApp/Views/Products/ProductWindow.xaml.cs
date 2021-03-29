using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Products;

namespace FurnitureFactoryApp.Views.Products {
    public partial class ProductWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="ProductWindow"/> class.
        /// </summary>
        public ProductWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="ProductWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public ProductWindow(ProductWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.Close) {
            InitializeComponent();
        }
    }
}
