using System;
using System.Reflection;

namespace WarmUp
{
    class Program
    {
        static int Main(string[] args)
        {
            RegisterAssemblyResolve();

            return Application.Start(args);
        }

        private static void RegisterAssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var resourceName = "WarmUp." + new AssemblyName(args.Name).Name + ".dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new Byte[stream.Length];

                    stream.Read(assemblyData, 0, assemblyData.Length);

                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
}
