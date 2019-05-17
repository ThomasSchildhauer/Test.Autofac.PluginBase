using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.AssemblyLoader.Interfaces;
using Test.Autofac.PluginBase.AssemblyLoader.Loader;

namespace Test.Autofac.PluginBase.AssemblyLoader.Autofac
{
    public class Container
    {
        private List<Type> _types = new List<Type>();

        private List<System.Reflection.Assembly> _assemblies = new List<System.Reflection.Assembly>();

        public IContainer Config()
        {
            var builder = new ContainerBuilder();

            // Register Types
            builder.RegisterType<PluginLoader>().As<IPluginLoader>();

            // Plugin Registration
            var myType = typeof(IGamesPlugin);

            // this is only for the assembly that holds this code
            GetType()
                .Assembly
                .GetTypes()
                .Where(t => !t.IsInterface && myType.IsAssignableFrom(t))
                .ToList()
                .ForEach(t => _types.Add(t));

            // this reads all dlls in the folder which holds the executing assembly
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                _assemblies.Add( System.Reflection.Assembly.LoadFile(dll));
            }

            // register all types
            builder.RegisterAssemblyTypes(_assemblies.ToArray())
                .Where(t => t.GetInterfaces().Contains(myType) && !t.IsAbstract)
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
