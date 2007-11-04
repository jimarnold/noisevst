using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Noise.Test.System
{
	[TestFixture]
	public class NativePluginTest
	{
		[Test]
		public void LoadsTheManagedPlugin()
		{
            //This test is a bit dodgy.  It relies on the configured plugin having 2 inputs!
            IntPtr pluginLibraryHandle = LoadLibrary("..\\..\\..\\build\\Noise.dll");

            try
            {
                AEffect effect = GetEffect(pluginLibraryHandle);
                Assert.AreEqual(2, effect.numInputs);
            }
            finally
            {
                FreeLibrary(pluginLibraryHandle);
            }
		}

        static AEffect GetEffect(IntPtr pluginLibraryHandle)
        {
            Assert.IsFalse(pluginLibraryHandle == IntPtr.Zero, "Plugin handle was null");
            IntPtr pMain = GetProcAddress(pluginLibraryHandle, "main");
            main mainFunction = (main)Marshal.GetDelegateForFunctionPointer(pMain, typeof(main));
            IntPtr pEffect = mainFunction(audioMaster); //Inline delegate may get me in trouble
            
            if(pEffect == IntPtr.Zero)
            {
                Assert.Fail("No plugin returned");
            }
            return (AEffect)Marshal.PtrToStructure(pEffect, typeof(AEffect));
        }

        //This is a dummy host callback
		static int audioMaster(IntPtr effect, AudioMasterOpCode opcode, int index, int value, IntPtr ptr, float opt)
		{
			if(opcode == AudioMasterOpCode.audioMasterVersion)
			{
				return 1;
			}

			return 0;
		}

        [DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);
		[DllImport("kernel32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        delegate IntPtr main(audioMasterCallback audioMaster);
		delegate int audioMasterCallback(IntPtr effect, AudioMasterOpCode opcode, int index, int value, IntPtr ptr, float opt);
	}

	public struct AEffect
	{
		public int magic;
		public IntPtr dispatcher;
		public IntPtr process;
		public IntPtr setParameter;
		public IntPtr getParameter;
		public int numPrograms;
		public int numParams;
		public int numInputs;
		public int numOutputs;
		public int flags;
		public int resvd1;
		public int resvd2;
		public int initialDelay;
		public int realQualities;
		public int offQualities;
		public float ioRatio;
		public IntPtr @object;
		public IntPtr user;
		public int uniqueID;
		public int version;
		public IntPtr processReplacing;
		public string future;
	}

	enum AudioMasterOpCode
	{
		audioMasterAutomate = 0,		// index, value, returns 0
		audioMasterVersion,				// VST Version supported (for example 2200 for VST 2.2)
		audioMasterCurrentId,			// Returns the unique id of a plug that's currently
		// loading
		audioMasterIdle,				// Call application idle routine (this will
		// call effEditIdle for all open editors too) 
		audioMasterPinConnected			// Inquire if an input or output is beeing connected;
		// index enumerates input or output counting from zero,
		// value is 0 for input and != 0 otherwise. note: the
		// return value is 0 for <true> such that older versions
		// will always return true.	
	};
}