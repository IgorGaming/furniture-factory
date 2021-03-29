using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace FurnitureFactoryApp {
    static class AppConfig {
        public static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
