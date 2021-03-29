using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Customers;

namespace FurnitureFactoryApp.Views.Customers {
    public partial class CustomerUpdateWindow : DataWindow {
        public CustomerUpdateWindow()
            : this(null) { }

        public CustomerUpdateWindow(CustomerUpdateWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
