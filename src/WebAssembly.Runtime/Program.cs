using Wasmtime;

namespace WebAssembly.Runtime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting runtime!");

            RunOfficialSample();
            Console.WriteLine("Sample completed");
            
            RunOwnApp();
            Console.WriteLine("Eureka!");
        }

        private static void RunOwnApp()
        {
            var engineConfig = new Config();
            engineConfig.WithDebugInfo(true);
            using var engine = new Engine();
            using var store = new Store(engine);
            using var appModule = Module.FromFile(engine,
                @"C:\Code\GitHub\wasm-sandbox\src\WebAssembly.App\bin\Debug\net7.0\WebAssembly.App.wasm");

            var wasiConfiguration = new WasiConfiguration();
            // IEnumerable<(string, string)> wasiEnvVars = new List<(string, string)>()
            // {
            //     { "foo", "bar" }
            // };
            // wasiConfiguration.WithEnvironmentVariables(wasiEnvVars);
            store.SetWasiConfiguration(wasiConfiguration);
            using var linker = new Linker(engine);
            linker.DefineWasi();
            linker.Define(
                "",
                "Main",
                Function.FromCallback(store, () => Console.WriteLine("Hello from C#!"))
            );
            linker.Define(
                "",
                "Foo",
                Function.FromCallback(store, () => Console.WriteLine("Hello from C#!"))
            );

            var appInstance = linker.Instantiate(store, appModule);
            var action = appInstance.GetAction("Main");
            action();
        }

        private static void RunOfficialSample()
        {
            using var engine = new Engine();

            using var module = Module.FromText(
                engine,
                "hello",
                "(module (func $hello (import \"\" \"hello\")) (func (export \"run\") (call $hello)))"
            );

            using var store = new Store(engine);

            using var linker = new Linker(engine);
            linker.Define(
                "",
                "hello",
                Function.FromCallback(store, () => Console.WriteLine("Hello from C#!"))
            );

            var sampleInstance = linker.Instantiate(store, module);
            var run = sampleInstance.GetAction("run")!;
            run();
        }
    }
}