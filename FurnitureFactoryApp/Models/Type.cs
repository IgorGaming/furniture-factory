using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Catel.Data;

namespace FurnitureFactoryApp.Models {
    public class Type : ModelBase {
        public Type() {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Информация об ошибке, произошедшей во время удаления.
        /// </summary>
        public string RemoveErrorInfo { get; private set; }

        /**Получение всех типов товара.*/
        public static ObservableCollection<Type> All() {
            var collection = new ObservableCollection<Type>();

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM types", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return collection;
                }

                while (reader.Read()) {
                    collection.Add(new Type() {
                        Id = (int)reader["type_id"],
                        Name = (string)reader["name"],
                    });
                }
            }

            return collection;
        }

        /// <summary>
        /// Сохранение типа продукта в БД.
        /// </summary>
        /// <remarks>Если тип продукта уже существует - просто обновит его, в ином случае произведёт вставку.</remarks>
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
        /// Удаление типа продукта из БД.
        /// </summary>
        public bool Remove() {
            if (Id < 1) {
                RemoveErrorInfo = "Невалидный Id типа.";
                return false;
            }

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                var command = new SqlCommand($"SELECT COUNT(*) FROM products WHERE type_id = @id", connection);
                command.Parameters.Add(new SqlParameter("@id", Id));
                int count = (int)command.ExecuteScalar();

                if (count == 0) {
                    command = new SqlCommand($"DELETE FROM types WHERE type_id = @id", connection);
                    command.Parameters.Add(new SqlParameter("@id", Id));
                    command.ExecuteNonQuery();

                    Trace.WriteLine("Успешное удаление типа продукта!");
                    return true;
                } else {
                    RemoveErrorInfo = "Удаление данного типа невозможно, т.к он указан для некоторых продуктов.";
                    return false;
                }
            }
        }

        protected void Insert(SqlConnection connection) {
            //Создаём команду с хранимой процедурой
            SqlCommand cmd = new SqlCommand("sp_addProductType", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddRange(new[] {
                new SqlParameter("@name", Name),

                //Возвращаемое значение
                new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output },
            });

            //Выполняем
            cmd.ExecuteNonQuery();

            //Устанавливаем ID
            Id = (int)cmd.Parameters["@id"].Value;

            Trace.WriteLine("Продукт успешно добавлен!");
        }

        protected void Update(SqlConnection connection) {
            //Создаём команду и выполняем
            SqlCommand cmd = new SqlCommand($"UPDATE types SET name = @name WHERE type_id = @id", connection);
            cmd.Parameters.AddRange(new[] {
                new SqlParameter("@name", Name),
                new SqlParameter("@id", Id)
            });
            cmd.ExecuteNonQuery();

            Trace.WriteLine("Успешное изменение типа продукта!");
        }

        /// <summary>
        /// Проверяет, существует ли данный тип.
        /// </summary>
        /// <returns>Возвращает true, если данный тип уже существует.</returns>
        public bool Exists() {
            if (Name == null) {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM types WHERE name = @name", connection);
                command.Parameters.Add(new SqlParameter("@name", Name));
                int number = (int)command.ExecuteScalar();

                return number != 0;
            }
        }
    }
}
