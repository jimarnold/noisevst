using System;
using System.Reflection;

namespace Noise.Framework
{
    public class PluginLoader
    {
        public INoisePlugin Load(AssemblyName assemblyName, string typeName)
        {
            Assembly ass = Assembly.Load(assemblyName);
            Type pluginType = ass.GetType(typeName);

            if(pluginType == null)
            {
                throw new TypeLoadException(
                    string.Format("Could not find type '{0}' in assembly '{1}'.  Is your configuration correct?",
                                  typeName, assemblyName.FullName));
            }

            if(pluginType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new TypeLoadException(
                    string.Format("The type '{0}' in assembly '{1}' does not have a default constructor!  Is your configuration correct?",
                                  typeName, assemblyName.FullName));
            }

            if (!typeof(INoisePlugin).IsAssignableFrom(pluginType))
            {
                throw new TypeLoadException(
                    string.Format("The type '{0}' in assembly '{1}' is not compatible with Noise!  Is your configuration correct?",
                                  typeName, assemblyName.FullName));
            }

            INoisePlugin plugin = Activator.CreateInstance(pluginType) as INoisePlugin;

            return plugin;
        }
    }
}