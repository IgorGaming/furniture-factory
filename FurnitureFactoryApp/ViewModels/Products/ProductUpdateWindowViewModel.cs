namespace FurnitureFactoryApp.ViewModels.Products {
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using FurnitureFactoryApp.Models;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class ProductUpdateWindowViewModel : ViewModelBase {
        public ProductUpdateWindowViewModel(Product product) {
            Product = product;
            Types = Type.All();
        }


        #region Properties
        [Model]
        [Expose("Name")]
        [Expose("Length")]
        [Expose("Width")]
        [Expose("Height")]
        [Expose("Color")]
        [Expose("Cost")]
        [Expose("Count")]
        [Expose("Type")]
        public Product Product { get; set; }

        public ObservableCollection<Type> Types { get; set; }

        public override string Title => "Изменение продукта.";
        #endregion

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) {

            if (Product == null) {
                return;
            }

            if (string.IsNullOrWhiteSpace(Product.Name)) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Name", "Поле Наименование обязательно для заполнения!"));
            } else {
                if (Product.Name.Length > 300 || Product.Name.Length < 3) {
                    validationResults.Add(FieldValidationResult.CreateError("Product.Name", "Поле Наименование должно быть от 3 до 150 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Product.Length)) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Length", "Поле Длина обязательно для заполнения!"));
            } else {
                if (Product.Length.Length > 50) {
                    validationResults.Add(FieldValidationResult.CreateError("Product.Length", "Поле Длина должно быть до 50 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Product.Width)) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Width", "Поле Ширина обязательно для заполнения!"));
            } else {
                if (Product.Width.Length > 50) {
                    validationResults.Add(FieldValidationResult.CreateError("Product.Width", "Поле Ширина должно быть до 50 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Product.Height)) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Height", "Поле Высота обязательно для заполнения!"));
            } else {
                if (Product.Height.Length > 50) {
                    validationResults.Add(FieldValidationResult.CreateError("Product.Height", "Поле Высота должно быть до 50 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Product.Color)) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Color", "Поле Цвет обязательно для заполнения!"));
            } else {
                if (Product.Color.Length > 30) {
                    validationResults.Add(FieldValidationResult.CreateError("Product.Color", "Поле Цвет должно быть до 30 символов!"));
                }
            }

            if (Product.Cost == 0.0M) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Cost", "Поле Стоимость обязательно для заполнения и должно быть в формате 100,00!"));
            }

            if (Product.Count == 0) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Count", "Поле Количество обязательно для заполнения!"));
            }

            if (Product.Type.Id == 0) {
                validationResults.Add(FieldValidationResult.CreateError("Product.Type", "Необходимо выбрать тип для продукта!"));
            }
        }


        #region Methods
        protected override async Task InitializeAsync() {
            await base.InitializeAsync();
            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync() {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
        #endregion
    }
}
