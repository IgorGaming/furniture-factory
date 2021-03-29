using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Customers;

namespace FurnitureFactoryApp.Views.Customers {
    public partial class CustomerAddWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="CustomerAddWindow"/> class.
        /// </summary>
        public CustomerAddWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="CustomerAddWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public CustomerAddWindow(CustomerAddWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
