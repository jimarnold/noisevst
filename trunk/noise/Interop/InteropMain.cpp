#include "stdafx.h"
#include "PluginBridge.h"
#include "HostBridge.h"

using namespace Noise::Interop;
using namespace System;
using namespace System::IO;
using namespace System::Diagnostics;
using namespace System::Reflection;

String^ getPluginPath();

AEffect* VSTMain (audioMasterCallback audioMaster)
{
		
	// Get VST Version
	if (!audioMaster || !audioMaster (0, audioMasterVersion, 0, 0, 0, 0))
		return 0;  // old version

	try 
	{
		gcroot<HostBridge^> host = gcnew HostBridge();
		gcroot<String^> configFile = Path::Combine(getPluginPath(), "Noise.config");
		gcroot<ConfigReader^> configReader = gcnew ConfigReader(configFile);
		gcroot<PluginLoader^> loader = gcnew PluginLoader();
		gcroot<INoisePlugin^> managedPlugin = loader->Load(configReader->AssemblyName, configReader->TypeName);

		managedPlugin->SetHost(host);

		PluginBridge* pluginBridge = new PluginBridge (audioMaster, managedPlugin);

		if (!pluginBridge)
			return 0;

		host->SetHost(pluginBridge);

		return pluginBridge->getAeffect();
	}
	catch(System::Exception^ e)
	{
		Debug::Fail(e->ToString());
		//TODO: return a dummy AEffect which can show errors.
		return 0;
	}
}

String^ getPluginPath()
{
	gcroot<String^> pathAsUri = Assembly::GetExecutingAssembly()->CodeBase;
	gcroot<Uri^> uri = gcnew Uri(pathAsUri);
	return Path::GetDirectoryName(uri->AbsolutePath);
}

#pragma unmanaged
void* hInstance;
BOOL WINAPI DllMain (HINSTANCE hInst, DWORD dwReason, LPVOID lpvReserved)
{
	hInstance = hInst;
	return 1;
}