using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Customers;

namespace FurnitureFactoryApp.Views.Customers {
    /// <summary>
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="CustomerWindow"/> class.
        /// </summary>
        public CustomerWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="CustomerWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public CustomerWindow(CustomerWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.Close) {
            InitializeComponent();
        }
    }
}
