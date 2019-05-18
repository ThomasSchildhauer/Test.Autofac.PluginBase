using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;
using Test.Autofac.PluginBase.TypeLoader.Loader;

namespace Test.Autofac.PluginBase.TypeLoader.Autofac
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

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (Directory.Exists(path))
            {
                IEnumerable<string> fileNames = Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

                foreach (var fileName in fileNames)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(fileName);

                    IEnumerable<Type> types = assembly.ExportedTypes;

                    foreach (var t in types)
                    {
                        if (t.IsClass && myType.IsAssignableFrom(t))
                        {
                            _types.Add(t);
                        }
                    }
                }

                foreach (var t in _types)
                {
                    builder.RegisterType(t)
                        .WithMetadata(myType.ToString(), t.Name)
                        .As<IGamesPlugin>()
                        .SingleInstance();
                }
            }

            return builder.Build();
        }
    }
}
