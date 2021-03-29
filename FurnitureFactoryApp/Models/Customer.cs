using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Catel.Data;


namespace FurnitureFactoryApp.Models {
    public class Customer : ValidatableModelBase {
        public Customer() {
        }

        public int Id { get; set; }

        public string Fio { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        /// <summary>
        /// Загрузка заказчиков из БД.
        /// </summary>
        /// <returns>Возвращает коллекцию загруженных заказчиков.</returns>
        public static ObservableCollection<Customer> All() {
            var collection = new ObservableCollection<Customer>();

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM customers", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return collection;
                }

                while (reader.Read()) {
                    collection.Add(new Customer() {
                        Id = (int)reader["customer_id"],
                        Fio = (string)reader["fio"],
                        Address = (string)reader["address"],
                        Telephone = (string)reader["telephone"],
                    });
                }
            }

            return collection;
        }

        /// <summary>
        /// Сохранение заказчика в БД.
        /// </summary>
        /// <remarks>Если заказчик уже существует - просто обновит его, в ином случае произведёт вставку.</remarks>
        public void Save() {
            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                if (Id < 1) {
                    Insert(connection);
                } else {
                    Update(connection);
                }
            }
        }

        /// <summary>
        /// Удаление заказчика из БД.
        /// </summary>
        public void Remove() {
            if (Id < 1) {
                return;
            }

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                var command = new SqlCommand($"DELETE FROM customers WHERE customer_id = @id", connection);
                command.Parameters.Add(new SqlParameter("@id", Id));
                command.ExecuteNonQuery();

                Trace.WriteLine("Успешное удаление заказчика!");
            }
        }

        protected void Insert(SqlConnection connection) {
            //Создаём команду с хранимой процедурой
            SqlCommand cmd = new SqlCommand("sp_addCustomer", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //Добавляем параметры
            cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@fio", Fio),
                    new SqlParameter("@address", Address),
                    new SqlParameter("@telephone", Telephone),

                    //Возвращаемое значение
                    new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                });

            //Выполняем
            cmd.ExecuteNonQuery();

            //Устанавливаем ID
            Id = (int)cmd.Parameters["@id"].Value;

            Trace.WriteLine("Заказчик успешно добавлен!");
        }

        protected void Update(SqlConnection connection) {
            //Создаём команду и выполняем
            SqlCommand cmd = new SqlCommand($"UPDATE customers SET fio = @fio, address = @address, telephone = @phone WHERE customer_id = @id", connection);
            cmd.Parameters.AddRange(new[] {
                new SqlParameter("@fio", Fio),
                new SqlParameter("@address", Address),
                new SqlParameter("@phone", Telephone),
                new SqlParameter("@id", Id),
            });

            cmd.ExecuteNonQuery();

            Trace.WriteLine("Успешное изменение заказчика!");
        }

        //Валидация
        protected override void ValidateFields(List<IFieldValidationResult> validationResults) {
            if (string.IsNullOrWhiteSpace(Fio)) {
                validationResults.Add(FieldValidationResult.CreateError("Fio", "Поле ФИО обязательно для заполнения!"));
            } else {
                if (Fio.Length > 150 || Fio.Length < 5) {
                    validationResults.Add(FieldValidationResult.CreateError("Fio", "Поле ФИО должно быть от 5 до 150 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Address)) {
                validationResults.Add(FieldValidationResult.CreateError("Address", "Поле Адрес обязательно для заполнения!"));
            } else {
                if (Address.Length > 150 || Address.Length < 5) {
                    validationResults.Add(FieldValidationResult.CreateError("Address", "Поле Адрес должно быть от 5 до 150 символов!"));
                }
            }

            if (string.IsNullOrWhiteSpace(Telephone)) {
                validationResults.Add(FieldValidationResult.CreateError("Telephone", "Поле Телефон обязательно для заполнения!"));
            } else {
                if (Telephone.Length > 20) {
                    validationResults.Add(FieldValidationResult.CreateError("Telephone", "Поле Телефон должно быть до 20 символов!"));
                }
            }
        }
    }
}



