using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;

namespace Test.Autofac.PluginBase.Autofac
{
    public class PluginInstantiation
    {
        private List<Type> _types = new List<Type>();

        public Dictionary<string, IGamesPlugin> Plugins { get; private set; } = new Dictionary<string, IGamesPlugin>();

        public ILifetimeScope CreateScope()
        {
            var builder = new ContainerBuilder();

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

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                var assembly = System.Reflection.Assembly.LoadFile(dll);
                assembly
                    .GetTypes()
                    .Where(t => !t.IsInterface && myType.IsAssignableFrom(t))
                    .ToList()
                    .ForEach(t => _types.Add(t));
            }

            // register all types
            _types.ForEach(t => builder.RegisterType(t).SingleInstance());

            var scope = builder.Build();

            // Fill Dict with Typename as Key and Plugin Object cast to IGamesPlugin
            _types.ForEach(t => { Plugins.Add(t.Name, (IGamesPlugin)scope.Resolve(t)); });

            return scope;
        }
    }
}
