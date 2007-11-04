--------------------
Prerequisites:
--------------------

Noise requires Visual Studio.Net 2005 to build correctly (although you could hack a build with just the .Net 2 SDK).  A NAnt build file is planned.

Before using Noise you will need to download the 2.4 VST Plug-Ins SDK from www.steinberg.net (I'm not allowed to distribute it with this project).

Once you have the SDK, copy the whole thing (keeping paths intact) into the solution directory of Noise.

To use the example plugins, or to test your own, you will probably want another plugin which you can take output from.  I can recommend Synth1
from http://www.geocities.jp/daichi1969/softsynth/index.html#down - it's free and it rocks in every way.

--------------------
Development notes:
--------------------

In the solution directory there is a program called VstHost.exe which can be used to test plugins (as yet I have successfully tested
Noise with VstHost and Sony's ACID 5 Pro).  It should be set as the debug target for the HostPlugin project 
(HostPlugin Property Pages->Configuration properties->Debugging->Command should point to $(SolutionDir)\vsthost.exe), 
so you can just press [ctrl+]F5 to launch it.  To open a Noise plugin from VstHost, select New Effect from
the File menu, browse to the directory containing the Noise HostPlugin.dll (by default, this is the Build directory) and open it.  To insert the
plugin after another plugin (for example, a synthesizer), highlight it, select 'Chain After...' from the Effect menu, and select the plugin to chain
after.  VstHost remembers your settings from the last time it was run, so you shouldn't have to do that too many times.

VstHost is very easy to use, and support can be found here: http://www.hermannseib.com/english/vsthost.htm

Thanks to Hermann Seib for letting me include VstHost with Noise.  Please make sure to read and abide by the VstHost licence in vsthost-license.txt.


There are a few example plugins in the ManagedVst project.  DelayPlugin, ReverbPlugin and CompressorPlugin use the VST host's default 
UI, while FilterPlugin has its own - very lame - UI.  The custom UI support is very experimental and buggy, and involves hijacking an unmanaged window.

The common interface for these plugins, which any managed plugin must implement, is Noise.Framework.INoisePlugin.

There is also an abstract class, Plugin, which implements some mundane properties and methods for you.  You can use this as a base for your
own plugins, and override anything that doesn't suit your needs.

There is an example VSTi (instrument) called Cynth - it just outputs random noise, but shows you how to handle MIDI events via ProcessEvents (only basic 
MIDI events are supported currently - no SYSEX).

The other important thing is the Noise.config file.  This tells the C++ adaptor which managed plugin to load.  The first line is the full name
of the assembly containing the plugin, eg:

Noise.ManagedVst, Version=0.6.0.0, Culture=neutral, PublicKeyToken=39b9427163d6d640

This must be a fully qualified assembly name.  The second line is the name of the plugin class, eg:

Noise.ManagedVst.Plugins.ReverbPlugin

Again, this must be the full name, including namespaces.


Pleeeeease, if you make any changes to Noise, contribute back via the project site at http://gforge.public.thoughtworks.org/projects/noise, and let
me know if you have any problem building or running it.

--------------------
Limitations:
--------------------

At the moment, Noise is pretty rough and ready.  There is very little error handling, and only some features from the VST SDK are enabled.

Three DLLs are created - Noise.dll (this is the mixed-mode C++ library which loads into the host), Noise.Framework.dll (defines interfaces for
managed plugins and managed GUIs) and Noise.ManagedVst.dll (contains managed plugins).  Noise.Framework and Noise.ManagedVst are both installed in the 
GAC at build time (Visual Studio should take care of this).  If you want to create your own plugin assembly, it must be strong-named and must be 
installed in the GAC, otherwise the host won't be able to load it.  Future versions will hopefully produce one merged DLL which can reside anywhere.

If installation to the GAC fails, make sure you have gacutil.exe in your path (it should be in the .Net Framework SDK's \bin directory).

Be VERY CAREFUL when using VstHost to test your plugins - if your plugin throws an exception, VstHost can hang so badly it can't be shutdown for a
few minutes.  If you run through the debugger (or attach as sooon as an exception is thrown), however, it will usually exit gracefully if errors occur.

--------------------
Future directions:
--------------------

Obviously I would like to support all the features of the unmanaged VST SDK and provide full support for managed GUIs.
It would also be nice to be able to merge everything into one DLL for distribution, and get rid of the config file.


Any questions:

jarnold@thoughtworks.com or use the forums on the project site.