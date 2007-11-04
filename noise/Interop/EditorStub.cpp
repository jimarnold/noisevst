#include "EditorStub.h"
using namespace Noise::Interop;
using namespace System;


EditorStub::EditorStub (AudioEffect *effect, INoisePlugin^ plugin) : AEffGUIEditor (effect) 
{
	// We don't know what size we should be yet
	rect.left   = 0;
	rect.top    = 0;
	rect.right  = 0;
	rect.bottom = 0;

	this->plugin = plugin;
}

bool EditorStub::open (void *ptr)
{
	AEffGUIEditor::open (ptr);
	pluginEditor = plugin->OpenEditorWindow(IntPtr(systemWindow));

	RECT bigRect = *((RECT*)&pluginEditor->Bounds);

	rect.left = (short)bigRect.left;
	rect.top = (short)bigRect.top;
	rect.right = (short)bigRect.right;
	rect.bottom = (short)bigRect.bottom;

	return true;
}

void EditorStub::close ()
{
}

void EditorStub::valueChanged (CDrawContext* context, CControl* control)
{
}