using System.IO;
using Noise.Framework;
using Noise.ManagedVst.Plugins;
using NUnit.Framework;

namespace Noise.Test.Framework
{
    [TestFixture]
    public class ConfigReaderTest
    {
        private string configFile = "Noise.config";
        private string assemblyName;
        string typeName;

        [SetUp]
        public void SetUp()
        {
            assemblyName = typeof(DelayPlugin).Assembly.FullName;
            typeName = typeof(DelayPlugin).FullName;

            CreateTemporaryConfigFile();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTemporaryConfigFile();
        }

        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ThrowsIfConfigFileNotFound()
        {
            new ConfigReader("Not\\In\\Here");
        }

        [Test]
        public void GetsAssemblyNameAndTypeFromConfigFile()
        {
            ConfigReader reader = new ConfigReader(configFile);
            Assert.AreEqual(assemblyName, reader.AssemblyName.FullName);
            Assert.AreEqual(typeName, reader.TypeName);
        }
        
        private void CreateTemporaryConfigFile()
        {
            using (StreamWriter writer = File.CreateText(configFile))
            {
                writer.WriteLine(assemblyName);
                writer.WriteLine(typeName);
            }
        }

        private void DeleteTemporaryConfigFile()
        {
            if (File.Exists(configFile))
            {
                File.Delete(configFile);
            }
        }
    }
}