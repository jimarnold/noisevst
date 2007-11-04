using System;
using System.IO;
using System.Reflection;
using Noise.Framework;
using Noise.ManagedVst.Plugins;
using NUnit.Framework;

namespace Noise.Test.Framework
{
    [TestFixture]
    public class PluginLoaderTest
    {
        [Test]
        public void LoadsPluginGivenAssemblyNameAndType()
        {
            PluginLoader loader = new PluginLoader();
            AssemblyName assemblyName = typeof(DelayPlugin).Assembly.GetName();
            string typeName = typeof(DelayPlugin).FullName;
            INoisePlugin plugin = loader.Load(assemblyName, typeName);

            Assert.IsTrue(plugin is DelayPlugin);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowsIfAssemblyNotFound()
        {
            PluginLoader loader = new PluginLoader();
            AssemblyName assemblyName = new AssemblyName("OMG u deleted teh asembly u nubcake");
            string typeName = typeof(DelayPlugin).FullName;
            loader.Load(assemblyName, typeName);
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void ThrowsIfPluginTypeNotFoundInAssembly()
        {
            PluginLoader loader = new PluginLoader();
            AssemblyName assemblyName = typeof(DelayPlugin).Assembly.GetName();
            string typeName = "Sorry, you're not my type";
            loader.Load(assemblyName, typeName);
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void ThrowsIfTypeIsNotCompatibleWithNoise()
        {
            PluginLoader loader = new PluginLoader();
            AssemblyName assemblyName = GetType().Assembly.GetName();
            string typeName = GetType().FullName;
            loader.Load(assemblyName, typeName);
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void ThrowsIfTypeDoesNotHaveDefaultConstructor()
        {
            PluginLoader loader = new PluginLoader();
            AssemblyName assemblyName = GetType().Assembly.GetName();
            string typeName = typeof(PluginWithNoDefaultConstructor).FullName;
            loader.Load(assemblyName, typeName);
        }

        public class PluginWithNoDefaultConstructor
        {
            public PluginWithNoDefaultConstructor(string someParameter)
            {
                
            }
        }
    }
}