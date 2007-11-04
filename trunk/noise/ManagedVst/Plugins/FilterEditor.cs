using System;
using System.ComponentModel;
using System.Windows.Forms;
using Noise.ManagedVst.Processes;
using Noise.Framework;
namespace Noise.ManagedVst.Plugins
{
	public class FilterEditor : UserControl, INoiseEditor
	{
		private readonly FilterPlugin plugin;
		private RadioButton lpfButton;
		private RadioButton hpfButton;
		private Label freqLabel;
		private Label bandwidthLabel;
		private HScrollBar frequencyScroll;
		private HScrollBar bandwidthScroll;
		private Container components = null;

		public FilterEditor(FilterPlugin plugin)
		{
			this.plugin = plugin;
			InitializeComponent();
			lpfButton.Checked = (plugin.FilterType == FilterType.LPF);
			hpfButton.Checked = (plugin.FilterType == FilterType.HPF);
			frequencyScroll.Value = (int)plugin.Frequency;
			bandwidthScroll.Value = (int)plugin.Bandwidth * 10;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.frequencyScroll = new System.Windows.Forms.HScrollBar();
			this.bandwidthScroll = new System.Windows.Forms.HScrollBar();
			this.lpfButton = new System.Windows.Forms.RadioButton();
			this.hpfButton = new System.Windows.Forms.RadioButton();
			this.freqLabel = new System.Windows.Forms.Label();
			this.bandwidthLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// frequencyScroll
			// 
			this.frequencyScroll.CausesValidation = false;
			this.frequencyScroll.LargeChange = 1000;
			this.frequencyScroll.Location = new System.Drawing.Point(8, 8);
			this.frequencyScroll.Maximum = 10000;
			this.frequencyScroll.Name = "frequencyScroll";
			this.frequencyScroll.Size = new System.Drawing.Size(200, 17);
			this.frequencyScroll.SmallChange = 10;
			this.frequencyScroll.TabIndex = 0;
			this.frequencyScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnFrequencyScroll);
			// 
			// bandwidthScroll
			// 
			this.bandwidthScroll.CausesValidation = false;
			this.bandwidthScroll.LargeChange = 8;
			this.bandwidthScroll.Location = new System.Drawing.Point(8, 40);
			this.bandwidthScroll.Maximum = 80;
			this.bandwidthScroll.Name = "bandwidthScroll";
			this.bandwidthScroll.Size = new System.Drawing.Size(200, 17);
			this.bandwidthScroll.TabIndex = 1;
			this.bandwidthScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnBandwidthScroll);
			// 
			// lpfButton
			// 
			this.lpfButton.CausesValidation = false;
			this.lpfButton.Location = new System.Drawing.Point(8, 64);
			this.lpfButton.Name = "lpfButton";
			this.lpfButton.Size = new System.Drawing.Size(48, 24);
			this.lpfButton.TabIndex = 2;
			this.lpfButton.Text = "LPF";
			this.lpfButton.CheckedChanged += new System.EventHandler(this.lpfButton_CheckedChanged);
			// 
			// hpfButton
			// 
			this.hpfButton.CausesValidation = false;
			this.hpfButton.Location = new System.Drawing.Point(8, 88);
			this.hpfButton.Name = "hpfButton";
			this.hpfButton.Size = new System.Drawing.Size(48, 24);
			this.hpfButton.TabIndex = 3;
			this.hpfButton.Text = "HPF";
			this.hpfButton.CheckedChanged += new System.EventHandler(this.hpfButton_CheckedChanged);
			// 
			// freqLabel
			// 
			this.freqLabel.Location = new System.Drawing.Point(216, 8);
			this.freqLabel.Name = "freqLabel";
			this.freqLabel.Size = new System.Drawing.Size(56, 23);
			this.freqLabel.TabIndex = 5;
			this.freqLabel.Text = "Hz";
			// 
			// bandwidthLabel
			// 
			this.bandwidthLabel.Location = new System.Drawing.Point(216, 40);
			this.bandwidthLabel.Name = "bandwidthLabel";
			this.bandwidthLabel.Size = new System.Drawing.Size(56, 23);
			this.bandwidthLabel.TabIndex = 6;
			this.bandwidthLabel.Text = "Octaves";
			// 
			// FilterEditor
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.bandwidthLabel);
			this.Controls.Add(this.freqLabel);
			this.Controls.Add(this.hpfButton);
			this.Controls.Add(this.lpfButton);
			this.Controls.Add(this.bandwidthScroll);
			this.Controls.Add(this.frequencyScroll);
			this.Name = "FilterEditor";
			this.Size = new System.Drawing.Size(280, 120);
			this.ResumeLayout(false);

		}

		#endregion

		private void OnFrequencyScroll(object sender, ScrollEventArgs e)
		{
			plugin.SetParameterAutomated(0, (float)e.NewValue / 10000f);
		}

		private void OnBandwidthScroll(object sender, ScrollEventArgs e)
		{
			plugin.SetParameterAutomated(1, (float)e.NewValue / 10f);
		}

		private void lpfButton_CheckedChanged(object sender, EventArgs e)
		{
			if (lpfButton.Checked)
			{
				plugin.SetParameterAutomated(2, 0f);
			}
		}

		private void hpfButton_CheckedChanged(object sender, EventArgs e)
		{
			if (hpfButton.Checked)
			{
				plugin.SetParameterAutomated(2, 1f);
			}
		}
	}
}