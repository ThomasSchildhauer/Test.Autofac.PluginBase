using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Autofac;
using Test.Autofac.PluginBase.Interfaces;
using Test.Autofac.PluginBase.Loader;

namespace Test.Autofac.PluginBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var Container = new Autofac.Container();
            var scope = Container.CreateScope();

            using (scope)
            {
                var loader = scope.Resolve<IPluginLoader>();

                loader.LoadPlugins();

                Console.ReadKey();
            }
        }
    }
}
