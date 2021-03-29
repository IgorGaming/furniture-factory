using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Catel.Data;

namespace FurnitureFactoryApp.Models {
    public class Product : ModelBase {
        public Product() {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Length { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string Color { get; set; }

        public decimal Cost { get; set; }

        public int Count { get; set; }

        public Type Type { get; set; } = new Type();

        /**Получение всех продуктов.*/
        public static ObservableCollection<Product> All() {
            var collection = new ObservableCollection<Product>();

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT products.*, types.name AS type_name FROM products LEFT JOIN types on products.type_id = types.type_id;", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return collection;
                }

                while (reader.Read()) {
                    collection.Add(new Product() {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Length = (string)reader["length"],
                        Width = (string)reader["width"],
                        Height = (string)reader["height"],
                        Color = (string)reader["color"],
                        Cost = (decimal)reader["cost"],
                        Count = (int)reader["count"],
                        Type = new Type() {
                            Id = (int)reader["type_id"],
                            Name = (string)reader["type_name"],
                        },
                    });
                }
            }

            return collection;
        }

        /**Получение доступных для заказа продуктов.*/
        public static ObservableCollection<Product> Available() {
            var collection = new ObservableCollection<Product>();

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT products.*, types.name AS type_name FROM products LEFT JOIN types on products.type_id = types.type_id WHERE products.count > 0", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return collection;
                }

                while (reader.Read()) {
                    collection.Add(new Product() {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Length = (string)reader["length"],
                        Width = (string)reader["width"],
                        Height = (string)reader["height"],
                        Color = (string)reader["color"],
                        Cost = (decimal)reader["cost"],
                        Count = (int)reader["count"],
                        Type = new Type() {
                            Id = (int)reader["type_id"],
                            Name = (string)reader["type_name"],
                        },
                    });
                }
            }

            return collection;
        }

        /// <summary>
        /// Сохранение продукта в БД.
        /// </summary>
        /// <remarks>Если продукт уже существует - просто обновит его, в ином случае произведёт вставку.</remarks>
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
        /// Удаление продукта из БД.
        /// </summary>
        public void Remove() {
            if (Id < 1) {
                return;
            }

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                var command = new SqlCommand($"DELETE FROM products WHERE product_id = @id", connection);
                command.Parameters.Add(new SqlParameter("@id", Id));
                command.ExecuteNonQuery();

                Trace.WriteLine("Успешное удаление продукта!");
            }
        }

        protected void Insert(SqlConnection connection) {
            //Создаём команду с хранимой процедурой
            SqlCommand cmd = new SqlCommand("sp_addProduct", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //Добавляем параметры
            cmd.Parameters.AddRange(new[] {
                 new SqlParameter("@type_id", Type.Id),
                 new SqlParameter("@name", Name),
                 new SqlParameter("@length", Length),
                 new SqlParameter("@width", Width),
                 new SqlParameter("@height", Height),
                 new SqlParameter("@color", Color),
                 new SqlParameter("@cost", Cost),
                 new SqlParameter("@count", Count),

                 //Возвращаемое значение
                 new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output }
            });

            //Выполняем
            cmd.ExecuteNonQuery();

            //Устанавливаем ID
            Id = (int)cmd.Parameters["@id"].Value;

            Trace.WriteLine("Продукт успешно добавлен!");
        }

        protected void Update(SqlConnection connection) {
            //Создаём команду и выполняем
            SqlCommand cmd = new SqlCommand($"UPDATE products SET type_id = @type_id, name = @name, length = @length, width = @width, height = @height, color = @color, cost = @cost, count = @count WHERE product_id = @id", connection);
            cmd.Parameters.AddRange(new[] {
                new SqlParameter("@type_id", Type.Id),
                new SqlParameter("@name", Name),
                new SqlParameter("@length", Length),
                new SqlParameter("@width", Width),
                new SqlParameter("@height", Height),
                new SqlParameter("@color", Color),
                new SqlParameter("@count", Count),
                new SqlParameter("@id", Id),
            });
            cmd.Parameters.AddWithValue("@cost", Cost); //чтобы decimal не конвертировался в строку

            cmd.ExecuteNonQuery();

            Trace.WriteLine("Успешное изменение продукта!");
        }
    }
}
