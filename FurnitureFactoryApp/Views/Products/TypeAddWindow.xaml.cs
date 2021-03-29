using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Products;

namespace FurnitureFactoryApp.Views.Products {
    public partial class TypeAddWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="TypeAddWindow"/> class.
        /// </summary>
        public TypeAddWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="TypeAddWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public TypeAddWindow(TypeAddWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
