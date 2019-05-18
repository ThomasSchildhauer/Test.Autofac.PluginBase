using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.TypeLoader.Autofac;
using Test.Autofac.PluginBase.TypeLoader.Loader;

namespace Test.Autofac.PluginBase.TypeLoader
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
