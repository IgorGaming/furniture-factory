namespace FurnitureFactoryApp.ViewModels.Products {
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using FurnitureFactoryApp.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TypeUpdateWindowViewModel : ViewModelBase {
        public TypeUpdateWindowViewModel(Type type) {
            Type = type;
        }

        #region Properties
        [Model]
        [Expose("Name")]
        public Type Type { get; set; }

        public override string Title => "Изменения типа продукта.";
        #endregion

        protected override void ValidateFields(List<IFieldValidationResult> validationResults) {
            if (string.IsNullOrWhiteSpace(Type.Name)) {
                validationResults.Add(FieldValidationResult.CreateError("Type.Name", "Поле Наименование обязательно для заполнения!"));
            } else {
                if (Type.Name.Length > 30 || Type.Name.Length < 3) {
                    validationResults.Add(FieldValidationResult.CreateError("Type.Name", "Поле Наименование должно быть от 3 до 30 символов!"));
                }
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
