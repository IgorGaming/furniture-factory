using System.ComponentModel;
using System.Diagnostics;

using Catel.Windows;
using Catel.Windows.Controls;

using FurnitureFactoryApp.ViewModels.Orders;

namespace FurnitureFactoryApp.Views.Orders {
    public partial class OrderProductsAddWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="OrderProductsAddWindow"/> class.
        /// </summary>
        public OrderProductsAddWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="OrderProductsAddWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public OrderProductsAddWindow(OrderProductsAddWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
