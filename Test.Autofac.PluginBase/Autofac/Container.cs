using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;
using Test.Autofac.PluginBase.Loader;

namespace Test.Autofac.PluginBase.Autofac
{
    public class Container
    {
        private List<Type> _types = new List<Type>();

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
                var assembly = System.Reflection.Assembly.LoadFile(dll);
                assembly
                    .GetTypes()
                    .Where(t => !t.IsInterface && myType.IsAssignableFrom(t))
                    .ToList()
                    .ForEach(t => _types.Add(t));
            }

            // register all types
            _types
                .ForEach(t => builder.RegisterType(t)
                .WithMetadata(myType.ToString(), t.Name)
                .As<IGamesPlugin>()
                .SingleInstance());

            return builder.Build();
        }
    }
}
