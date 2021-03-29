using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Products;

namespace FurnitureFactoryApp.Views.Products {
    public partial class TypeUpdateWindow : DataWindow {
        public TypeUpdateWindow()
            : this(null) { }

        public TypeUpdateWindow(TypeUpdateWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel) {
            InitializeComponent();
        }
    }
}
