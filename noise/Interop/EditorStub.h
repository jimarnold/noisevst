#pragma once
#include "stdafx.h"
#include "vstgui.h"
#include "AudioEffect.h"
#include <vcclr.h>

using namespace Noise::Framework;

namespace Noise
{
	namespace Interop
	{
		class EditorStub : public AEffGUIEditor, public CControlListener
		{
			public:
				EditorStub (AudioEffect* effect, INoisePlugin^ plugin);

				virtual bool open (void* ptr);
				virtual void close ();
				virtual void valueChanged (CDrawContext* context, CControl* control);
			private:
				bool bOpened;
				gcroot<INoisePlugin^> plugin;
				gcroot<INoiseEditor^> pluginEditor;
		};
	}
}