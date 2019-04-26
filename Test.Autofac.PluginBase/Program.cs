using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Autofac;
using Test.Autofac.PluginBase.Interfaces;

namespace Test.Autofac.PluginBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var registration = new PluginInstantiation();
            var scope = registration.CreateScope();

            using (scope)
            {
                registration.Plugins.Values.ToList().ForEach(p => p.OnStartup());
                Console.ReadKey();
            }
        }
    }
}
