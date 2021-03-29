using System.Linq;
using System.Reflection;

using Catel.IoC;
using Catel.Reflection;

/// <summary>
/// Used by the ModuleInit. All code inside the InitializeResourceGroups method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer {
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize() {
        // Code added here will be executed as soon as the assembly is loaded by the .net runtime. This
        // is a great opportunity to register any services in the service locator:

        // var serviceLocator = ServiceLocator.Default;

        /*
        |--------------------------------------------------------------------------
        | Управление сборками
        |--------------------------------------------------------------------------
        |
        | catelExcludeAssemblies - данные сборки будут добавлены в исключения constructor cache clear от Catel.
        | Это нужно, чтобы при подключении во время выполнения программы не было торомозов.
        | 
        | loadAssemblies - сборки, которые следует загрузить при старте приложения.
        |
        */
        string[] catelExcludeAssemblies = new string[] {
            "Windows.UI",
        };
        TypeCacheEvaluator.AssemblyEvaluators.Add((assembly) => catelExcludeAssemblies.Contains<string>(assembly.GetName().Name));


        string[] loadAssemblies = new string[] {
            "System.Runtime.WindowsRuntime",
        };
        foreach (string assembly in loadAssemblies) {
            try {
                Assembly.Load(assembly);
            } catch { }
        }
    }
}