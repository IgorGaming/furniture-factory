using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Catel.Data;

namespace FurnitureFactoryApp.Models {
    public class Order : ModelBase {
        public Order() {
        }

        public int Id { get; set; }

        public Customer Customer { get; protected set; }

        public ObservableCollection<Product> Products { get; protected set; } = new ObservableCollection<Product>();

        /**Дата создания заказа.*/
        public DateTime OrderDate { get; set; }

        /**Дата доставки заказа.*/
        public DateTime ShipDate { get; set; } = DateTime.Now;

        /**Дата последнего изменения заказа.*/
        public DateTime? UpdatedAt { get; set; } = null;

        /**Получение всех заказов.*/
        public static ObservableCollection<Order> All() {
            var collection = new ObservableCollection<Order>();

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT orders.*, customers.fio, customers.address, customers.telephone FROM orders LEFT JOIN customers ON orders.customer_id = customers.customer_id", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return collection;
                }

                while (reader.Read()) {
                    var order = new Order() {
                        Id = (int)reader["order_id"],
                        OrderDate = (DateTime)reader["order_date"],
                        ShipDate = (DateTime)reader["ship_date"],
                        Customer = new Customer() {
                            Id = (int)reader["customer_id"],
                            Fio = (string)reader["fio"],
                            Address = (string)reader["address"],
                            Telephone = (string)reader["telephone"],
                        },
                    };

                    if (reader["updated_at"] != System.DBNull.Value) {
                        order.UpdatedAt = (DateTime)reader["updated_at"];
                    }

                    collection.Add(order);
                }
            }

            return collection;
        }

        /// <summary>
        /// Сохранение заказа в БД.
        /// </summary>
        public void Save() {
            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                if (Id < 1) {
                    Insert(connection);
                }
            }
        }

        /// <summary>
        /// Удаление заказа из БД.
        /// </summary>
        public void Remove() {
            if (Id < 1) {
                return;
            }

            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();

                var command = new SqlCommand($"DELETE FROM orders WHERE order_id = @id", connection);
                command.Parameters.Add(new SqlParameter("@id", Id));
                var reader = command.ExecuteReader();

                Trace.WriteLine("Успешное удаление продукта!");
            }
        }

        protected void Insert(SqlConnection connection) {
            //Создаём команду с хранимой процедурой
            SqlCommand cmd = new SqlCommand("sp_addOrder", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //Добавляем параметры
            cmd.Parameters.AddRange(new[] {
                 new SqlParameter("@customer_id", Customer.Id),
                 new SqlParameter("@order_date", OrderDate),
                 new SqlParameter("@ship_date", ShipDate),
                 
                 //Возвращаемое значение
                 new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output }
            });

            //Выполняем
            cmd.ExecuteNonQuery();

            //Устанавливаем ID
            Id = (int)cmd.Parameters["@id"].Value;

            Trace.WriteLine("Заказ успешно добавлен!");
        }

        /**Загрузка продуктов в данном заказе.*/
        public void LoadProducts() {
            using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM products p RIGHT JOIN(SELECT COUNT(product_id) AS item_count, product_id AS pr_id FROM order_product WHERE order_product.order_id = @id GROUP BY product_id) products ON p.product_id = pr_id", connection);
                command.Parameters.Add(new SqlParameter("@id", Id));
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) {
                    return;
                }

                Products.Clear();

                while (reader.Read()) {
                    Products.Add(new Product() {
                        Id = (int)reader["product_id"],
                        Name = (string)reader["name"],
                        Length = (string)reader["length"],
                        Width = (string)reader["width"],
                        Height = (string)reader["height"],
                        Color = (string)reader["color"],
                        Cost = (decimal)reader["cost"],
                        Count = (int)reader["item_count"],
                    });
                }
            }
        }
    }
}
