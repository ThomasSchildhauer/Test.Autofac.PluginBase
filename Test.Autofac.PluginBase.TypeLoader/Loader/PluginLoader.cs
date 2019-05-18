using Autofac.Features.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;

namespace Test.Autofac.PluginBase.TypeLoader.Loader
{
    public class PluginLoader : IPluginLoader
    {
        private IEnumerable<Meta<IGamesPlugin>> _plugins;
        public PluginLoader(IEnumerable<Meta<IGamesPlugin>> plugins)
        {
            _plugins = plugins;
        }

        public void LoadPlugins()
        {
            foreach (var plugin in _plugins)
            {
                plugin.Value.OnStartup();
            }
        }
    }
}
