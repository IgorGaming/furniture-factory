using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Catel.MVVM;
using Catel.Windows;

using FurnitureFactoryApp.ViewModels.Orders;

namespace FurnitureFactoryApp.Views.Orders {
    /// <summary>
    /// Логика взаимодействия для OrderProductsWindow.xaml
    /// </summary>
    public partial class OrderProductsWindow : DataWindow {
        /// <summary>
        /// Initializes a new instance of the<see cref="OrderProductsWindow"/> class.
        /// </summary>
        public OrderProductsWindow()
            : this(null) { }

        ///<summary>
        ///Initializes a new instance of the<see cref="OrderProductsWindow"/> class.
        ///</summary>
        ///<param name = "viewModel" > The view model to inject.</param>
        ///<remarks>
        ///This constructor can be used to use view-model injection.
        ///</remarks>
        public OrderProductsWindow(OrderProductsWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom) {

            //Добавляем свою кнопку "Закрыть", которая будет сохранять изменения
            AddCustomButton(new DataWindowButton("Закрыть", "CloseWindowCommand"));

            InitializeComponent();
        }
    }
}
