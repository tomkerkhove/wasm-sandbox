using Wasmtime;

namespace WebAssembly.Runtime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting runtime!");

            Console.WriteLine("Running official sample");
            RunOfficialSample();
            Console.WriteLine("Sample completed");
            
            Console.WriteLine("Running .NET scenario");
            RunOwnApp();
            Console.WriteLine("Eureka!");
        }

        private static void RunOwnApp()
        {
            var engineConfig = new Config();
            engineConfig.WithDebugInfo(true);
            using var engine = new Engine();
            using var store = new Store(engine);
            using var appModule = Module.FromFile(engine, @"C:\Code\GitHub\wasm-sandbox\src\WebAssembly.App\bin\Debug\net7.0\WebAssembly.App.wasm");
            
            var wasiConfiguration = new WasiConfiguration();
            // Customize access client app has:
            // wasiConfiguration.WithEnvironmentVariable()
            
            store.SetWasiConfiguration(wasiConfiguration);
            
            // Put limits on resources the client app gets access to:
            //store.SetLimits();
            
            using var linker = new Linker(engine);
            linker.DefineWasi();

            var appInstance = linker.Instantiate(store, appModule);
            
            // 💣 - It does not detect the function for now
            var action = appInstance.GetAction("Hello");
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