using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Noise.Framework;
using Noise.ManagedVst.Processes;

namespace Noise.ManagedVst.Plugins
{
	public class FilterPlugin : Plugin
	{
		private Filter leftFilter;
		private Filter rightFilter;
		private FilterEditor editor;
		private const int FREQUENCY_PARAM = 0;
		private const int BANDWIDTH_PARAM = 1;
		private const int FILTER_PARAM = 2;

		public FilterPlugin() : base("Net Filter", 3)
		{
			parameterNames[0] = "Frequency";
			parameterNames[1] = "Bandwidth";
			parameterNames[2] = "Type";

			parameterLabels[0] = "Hz";
			parameterLabels[1] = "Octaves";
			parameterLabels[2] = "";
		}

		public override void Resume()
		{
			leftFilter = new Filter(FilterType.LPF, 10000, 8, sampleRate);
			rightFilter = new Filter(FilterType.LPF, 10000, 8, sampleRate);
		}

		public float Frequency
		{
			get
			{
				return leftFilter.Frequency;
			}
			set
			{
				leftFilter.Frequency = value;
				rightFilter.Frequency = value;
			}
		}

		public float Bandwidth
		{
			get
			{
				return leftFilter.Bandwidth;
			}
			set
			{
				leftFilter.Bandwidth = value;
				rightFilter.Bandwidth = value;
			}
		}

		public FilterType FilterType
		{
			get
			{
				return leftFilter.FilterType;
			}
			set
			{
				leftFilter.FilterType = value;
				rightFilter.FilterType = value;
			}
		}

        public override void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames)
        {
			for(int i = 0; i < sampleFrames; i++)
			{
				outputs.GetChannel(0)[i] = leftFilter.Process(inputs.GetChannel(0)[i]);
				outputs.GetChannel(1)[i] = leftFilter.Process(inputs.GetChannel(1)[i]);
			}
		}

		public override void SetParameter(int index, float value)
		{
			switch (index)
			{
				case FREQUENCY_PARAM:
					Frequency = value*10000f;
					break;
				case BANDWIDTH_PARAM:
					Bandwidth = value*8f;
					break;
				case FILTER_PARAM:
					FilterType = (value == 0f) ? FilterType.LPF : FilterType.HPF;
					break;
			}
		}

		public override float GetParameter(int index)
		{
			switch (index)
			{
				case FREQUENCY_PARAM:
					return Frequency/10000f;
				case BANDWIDTH_PARAM:
					return Bandwidth/8f;
				case FILTER_PARAM:
					return (FilterType == FilterType.LPF) ? 0f : 1f;
				default:
					return 0f;
			}
		}

		public override string GetParameterDisplay(int index)
		{
			switch (index)
			{
				case FREQUENCY_PARAM:
					return leftFilter.Frequency.ToString();
				case BANDWIDTH_PARAM:
					return leftFilter.Bandwidth.ToString();
				case FILTER_PARAM:
					return leftFilter.FilterType.ToString();
				default:
					return String.Empty;
			}
		}

		public override bool HasEditor
		{
			get { return true; }
		}

	    public override int NumberOfInputs
	    {
	        get { return 2; }
	    }

	    public override int NumberOfOutputs
	    {
	        get { return 2; }
	    }

	    public override INoiseEditor OpenEditorWindow(IntPtr window)
		{
			editorWindow = window;
			editor = new FilterEditor(this);
			editor.CreateControl();
			SetParent(editor.Handle, window);
			editor.Location = new Point(0, 0);
			editor.Show();
			return editor;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);
	}
}