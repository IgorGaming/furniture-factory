using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Orders;

namespace FurnitureFactoryApp.Views.Orders {
    public partial class OrderWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="OrderWindow"/> class.
        /// </summary>
        public OrderWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="OrderWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public OrderWindow(OrderWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.Close) {
            InitializeComponent();
        }
    }
}
