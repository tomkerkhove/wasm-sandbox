using System.Collections;
using System.Runtime.InteropServices;

namespace WebAssembly.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, let's start poking around!");

            Console.WriteLine("Let's look for environment variables 🕵️‍");

            // IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            // foreach (DictionaryEntry i in environmentVariables)
            // {
            //     Console.WriteLine("- {0}:{1}", i.Key, i.Value);
            // }

            Console.WriteLine("Bye, bye!");
        }
        
        public string Foo()
        {
            Console.WriteLine($"Hello, {RuntimeInformation.RuntimeIdentifier} {RuntimeInformation.OSArchitecture}!");
            return "Hey, bye!";
        }
    }
}