using System;
using System.IO;
using System.Reflection;

namespace Noise.Framework
{
    public class ConfigReader
    {
        AssemblyName assemblyName;
        string typeName;
        
        public ConfigReader(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                assemblyName = new AssemblyName(reader.ReadLine());
                typeName = reader.ReadLine();
            }
        }

        public AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public string TypeName
        {
            get { return typeName; }
        }
    }
}