using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.AssemblyLoader.Autofac;
using Test.Autofac.PluginBase.AssemblyLoader.Interfaces;
using Test.Autofac.PluginBase.AssemblyLoader.Loader;

namespace Test.Autofac.PluginBase.AssemblyLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var Container = new Autofac.Container();

            var contaier = Container.Config();

            using (var scope = contaier.BeginLifetimeScope())
            {
                var loader = scope.Resolve<IPluginLoader>();

                loader.LoadPlugins();

                Console.ReadKey();
            }
        }
    }
}
