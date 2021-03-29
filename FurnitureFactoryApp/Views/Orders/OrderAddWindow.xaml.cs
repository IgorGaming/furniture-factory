using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Orders;

namespace FurnitureFactoryApp.Views.Orders {
    public partial class OrderAddWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="OrderAddWindow"/> class.
        /// </summary>
        public OrderAddWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="OrderAddWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public OrderAddWindow(OrderAddWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
