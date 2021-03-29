using System;
using System.Collections.Generic;
using System.Text;

using Catel.Data;

using FurnitureFactoryApp.Models;

namespace FurnitureFactoryApp.Repositories {
    public class AddProductInOrderRepository : ModelBase {
        public AddProductInOrderRepository(Product product, int count) {
            Product = product;
            ProductCount = count;
        }

        public Product Product { get; set; }

        public int ProductCount { get; set; }
    }
}
