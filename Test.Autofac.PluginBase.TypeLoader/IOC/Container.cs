using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;
using Test.Autofac.PluginBase.TypeLoader.Loader;

namespace Test.Autofac.PluginBase.TypeLoader.IOC
{
    public class Container
    {
        private List<Type> _types = new List<Type>();

        public IContainer Config()
        {
            var builder = new ContainerBuilder();

            // Register Types
            builder.RegisterType<PluginLoader>().As<IPluginLoader>();

            //######################//
            // Dynamic registration //
            //######################//

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Plugin Registration
            var myType = typeof(IGamesPlugin);

            // Check if Directory exists
            if (Directory.Exists(path))
            {
                // get all filenames in directory, take all dlls and only search top directory. Search options could be changed to all directories
                IEnumerable<string> fileNames = Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

                foreach (var fileName in fileNames)
                {
                    // load all assemblies
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(fileName);

                    // export all types from the current assembly
                    IEnumerable<Type> types = assembly.ExportedTypes;

                    foreach (var t in types)
                    {
                        // check if the type meets the criterias: type is a class and type is inherited from myType
                        if (t.IsClass && myType.IsAssignableFrom(t))
                        {
                            // add types that meet all criterias to a list
                            _types.Add(t);
                        }
                    }
                }

                // register all types in the List
                foreach (var t in _types)
                {
                    builder.RegisterType(t)
                        .WithMetadata(myType.ToString(), t.Name)
                        .As<IGamesPlugin>()
                        .SingleInstance();
                }
            }

            // Plugin Autofac Modul registration
            // for testing purposes i implement this again to find bugs
            var moduleType = typeof(Module);

            // check if directory exists
            if (Directory.Exists(path))
            {
                // get all filenames in the directory, that are dlls and which are located in top directory only
                IEnumerable<string> fileNames = Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

                foreach (var fileName in fileNames)
                {
                    // loads all assemblies from file
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(fileName);

                    // exports all types from a assembly
                    IEnumerable<Type> types = assembly.ExportedTypes;

                    // check if the type meets the criterias: type is a class and type is inherited from myType
                    // the inheritance has to be checked because only the types of a assembly that holds a class that inherits from myType have be loaded
                    var isPlugin = types.Where(t => t.IsClass && myType.IsAssignableFrom(t));

                    // if the there is a type in the assembly that inherits from myType the check for Autofac.Module goes on
                    if (isPlugin.Count() > 0)
                    {
                        foreach (var t in types)
                        {
                            // only classes that inherit from Autofac.Module have to be registered
                            if (t.IsClass && moduleType.IsAssignableFrom(t))
                            {
                                // the RegisterModule only takes a instance of an object
                                var instance = Activator.CreateInstance(t);
                                builder.RegisterModule(instance as Module);
                            }
                        }
                    }
                }
            }

            return builder.Build();
        }
    }
}
