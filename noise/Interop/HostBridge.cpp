#include "HostBridge.h"

using namespace Noise::Interop;

HostBridge::HostBridge()
{
}

void HostBridge::SetHost(AudioEffectX* host)
{
	this->host = host;
}

void HostBridge::SetParameterAutomated(VstInt32 index, float value)
{
	host->setParameterAutomated(index, value);
}

VstInt32 HostBridge::GetMasterVersion()
{
	return host->getMasterVersion();
}

void HostBridge::SetUniqueID(VstInt32 id)
{
	host->setUniqueID(id);
}

void HostBridge::SetInitialDelay(VstInt32 delay)
{
	host->setInitialDelay(delay);
}

int HostBridge::GetCurrentUniqueId()
{
	return host->getCurrentUniqueId();
}

void HostBridge::MasterIdle()
{
	host->masterIdle();
}

float HostBridge::GetSampleRate()
{
	return host->getSampleRate();
}

VstInt32 HostBridge::GetBlockSize()
{
	return host->getBlockSize();
}