using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GorgonLibrary;

namespace Beyond_Beyaan
{
	public partial class Configuration : Form
	{
		private List<VideoMode> videoModes;

		public Configuration()
		{
			InitializeComponent();
		}

		internal void FillResolutionList()
		{
			videoModes = new List<VideoMode>();
			foreach (var videoMode in Gorgon.CurrentDriver.VideoModes)
			{
				if (videoMode.Format == BackBufferFormats.BufferRGB888 && videoMode.Width >= 1066 && videoMode.Height > 600)
				{
					bool skip = false;
					foreach (var videoModeAlreadyAdded in videoModes)
					{
						if (videoModeAlreadyAdded.Width == videoMode.Width && videoModeAlreadyAdded.Height == videoMode.Height)
						{
							skip = true;
							break;
						}
					}
					if (skip)
					{
						continue;
					}
					videoModes.Add(videoMode);
				}
			}
			videoModes.Sort(delegate(VideoMode a, VideoMode b)
				                {
					                int diff = a.Width.CompareTo(b.Width);
					                return diff == 0 ? a.Height.CompareTo(b.Height) : diff;
				                });
			foreach (var videoMode in videoModes)
			{
				_resolutionComboBox.Items.Add(videoMode.Width + " x " + videoMode.Height);
				if (_resolutionComboBox.Items.Count > 0)
				{
					_resolutionComboBox.SelectedIndex = 0;
				}
			}
		}

		internal void FillDatasetList(List<DirectoryInfo> dataSets)
		{
			foreach (var ds in dataSets)
			{
				_datasetComboBox.Items.Add(ds.Name);
			}
			if (_datasetComboBox.Items.Count > 0)
			{
				_datasetComboBox.SelectedIndex = 0;
			}
		}

		internal int DataSetIndex { get { return _datasetComboBox.SelectedIndex; } }
		internal VideoMode VideoMode { get { return videoModes[_resolutionComboBox.SelectedIndex]; } }
		internal bool FullScreen { get { return _fullCB.Checked; } }
		internal bool ShowTutorial { get { return _showTutorialCB.Checked; } }

		private void _launchButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
