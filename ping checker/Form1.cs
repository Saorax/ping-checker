using MetroFramework.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ping_checker.Properties;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Onova;
using Onova.Services;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace ping_checker
{
	
	public partial class Form1 : MetroForm
    {
		private readonly GithubPackageResolver _updateManager = new GithubPackageResolver(
			repositoryOwner: "Saorax",
			repositoryName: "ping-checker",
			assetNamePattern: "ping.checker.exe");
		public Form1()
        {
            InitializeComponent();
			CheckUpdate();
			this.metroStyleManager1.Theme = Settings.Default.theme;
			if (this.metroStyleManager1.Theme == MetroThemeStyle.Dark)
			{
				this.metroComboBox1.SelectedIndex = 0;
				metroComboBox1.Theme = MetroThemeStyle.Light;
				this.richTextBox1.BackColor = Color.FromArgb(12, 12, 12);
				this.richTextBox1.ForeColor = Color.FromArgb(255, 255, 255);
			}
			else
			{
				this.metroComboBox1.SelectedIndex = 1;
				metroComboBox1.Theme = MetroThemeStyle.Dark;
				this.richTextBox1.BackColor = Color.FromArgb(255, 255, 255);
				this.richTextBox1.ForeColor = Color.FromArgb(12, 12, 12);
			}
			this.metroComboBox2.SelectedIndex = Settings.Default.region;
			if (Settings.Default.top)
			{
				base.TopMost = true;
				this.metroCheckBox1.Checked = true;
			}
			if (base.TopMost)
			{
				this.metroCheckBox1.Checked = true;
			}
			this.metroComboBox2.SelectedIndex = Settings.Default.region;

		}

		private async 
		Task
CheckUpdate()
		{
			if (File.Exists(@"ping checker.bak1"))
			{
				File.Delete(@"ping checker.bak1");
			}
			var check = await _updateManager.GetPackageVersionsAsync();
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fileVersionInfo.ProductVersion;
			//_updateManager.DownloadPackageAsync();
			//Console.WriteLine(Int32.Parse(version));
			string currentVersion = version[0].ToString() + version[2].ToString();
			string latestVersion = check[0].ToString()[0].ToString() + check[0].ToString()[2].ToString();
			if (Int32.Parse(latestVersion) > Int32.Parse(currentVersion))
            {
				Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
				await _updateManager.DownloadPackageAsync(check[0], @"ping checker.bak");
				File.Move(@"ping checker.exe", @"ping checker.bak1");
				File.Move(@"ping checker.bak", @"ping checker.exe");
				MessageBox.Show("new update found, restarting");
				Application.Restart();
			};
			for (int i = 0; i < check.Count; i++)
            {
				Console.WriteLine(check[i]);
			}
			
			/* If there are none, notify user and return
			if (!check.CanUpdate)
			{
				MessageBox.Show("There are no updates available.");
				return;
			}
			MessageBox.Show("New update available, will automatically update");
			// Prepare the latest update
			await _updateManager.PrepareUpdateAsync(check.LastVersion);

			// Launch updater and exit
			_updateManager.LaunchUpdater(check.LastVersion);
			Application.Exit();
			*/
		}
		private List<string> ips = new List<string>
		{
			"pingtest-atl.brawlhalla.com",
			"pingtest-cal.brawlhalla.com",
			"pingtest-ams.brawlhalla.com",
			"pingtest-brs.brawlhalla.com",
			"pingtest-aus.brawlhalla.com",
			"pingtest-jpn.brawlhalla.com",
			"pingtest-sgp.brawlhalla.com",
			"pingtest-saf.brawlhalla.com",
			"pingtest-mde.brawlhalla.com",
		};
		private void Form1_Load(object sender, EventArgs e)
        {
			

		}

        private void metroButton1_Click(object sender, EventArgs e)
        {
			List<MetroLabel> list = new List<MetroLabel>
			{
				metroLabel5,
				metroLabel6,
				metroLabel10,
				metroLabel8,
				metroLabel14,
				metroLabel18,
				metroLabel12,
				metroLabel22,
				metroLabel16
			};
			using (List<string>.Enumerator enumerator = this.ips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string ip = enumerator.Current;
					int index = this.ips.FindIndex((string x) => x.StartsWith(ip));
					PingReply pingReply = new Ping().Send(ip);
					MetroColorStyle foreColor = MetroColorStyle.Green;
					if (pingReply.RoundtripTime >= 150L)
					{
						foreColor = MetroColorStyle.Red;
					}
					else if (pingReply.RoundtripTime >= 80L)
					{
						foreColor = MetroColorStyle.Yellow;
					}
					if (pingReply.Status == IPStatus.Success)
					{
						list[index].Style = foreColor;
						list[index].Text = pingReply.RoundtripTime.ToString();
					}
					else
					{
						list[index].Style = MetroColorStyle.Red;
						list[index].Text = "ERROR";
					}
				}
			}
		}

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
			if (this.metroCheckBox1.Checked)
			{
				base.TopMost = true;
                Settings.Default.top = true;
			}
			else
			{
				base.TopMost = false;
				Settings.Default.top = false;
			}
			Settings.Default.Save();
		}

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (this.metroComboBox1.SelectedItem.ToString() == "Light")
			{
				this.metroStyleManager1.Theme = MetroThemeStyle.Light;
				this.richTextBox1.ForeColor = Color.FromArgb(12, 12, 12);
				this.richTextBox1.BackColor = Color.FromArgb(255, 255, 255);
				metroComboBox1.Theme = MetroThemeStyle.Light;
				Settings.Default.theme = MetroThemeStyle.Light;
			}
			else
			{
				this.metroStyleManager1.Theme = MetroThemeStyle.Dark;
				this.richTextBox1.ForeColor = Color.FromArgb(255, 255, 255);
				this.richTextBox1.BackColor = Color.FromArgb(12, 12, 12);
				metroComboBox1.Theme = MetroThemeStyle.Dark;
				Settings.Default.theme = MetroThemeStyle.Dark;
			}
			Settings.Default.Save();
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
			PingReply pingReply = new Ping().Send(this.ips[Settings.Default.region]);
			MetroColorStyle color1 = MetroColorStyle.Green;
			Color color = Color.FromArgb(0, 177, 89);
			if (this.metroStyleManager1.Theme.ToString() == "Light")
			{
				if (pingReply.RoundtripTime >= 150L)
				{
					color = Color.FromArgb(209, 17, 65);
					color1 = MetroColorStyle.Red;
				}
				else if (pingReply.RoundtripTime >= 80L)
				{
					color = Color.FromArgb(255, 196, 37);
					color1 = MetroColorStyle.Yellow;
				}
			}
			else
			{
				color = Color.FromName("Lime");
				if (pingReply.RoundtripTime >= 150L)
				{
					color = Color.FromName("Red");
					color1 = MetroColorStyle.Red;
				}
				else if (pingReply.RoundtripTime >= 80L)
				{
					color = Color.FromName("Yellow");
					color1 = MetroColorStyle.Yellow;
				}
			};
			this.metroLabel2.Style = color1;
			this.metroLabel2.Text = pingReply.RoundtripTime.ToString();
			if (pingReply.Status == IPStatus.Success)
			{
				this.metroLabel2.Style = color1;
				this.metroLabel2.Text = pingReply.RoundtripTime.ToString();
				this.richTextBox1.AppendText(pingReply.RoundtripTime.ToString(), color);
				this.richTextBox1.AppendText(" ");
			}
		}

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
			Settings.Default.region = this.metroComboBox2.SelectedIndex;
			Settings.Default.Save();
		}

        private void metroTabPage4_Click(object sender, EventArgs e)
        {

        }
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
		private const int WM_VSCROLL = 277;
		private const int SB_PAGEBOTTOM = 7;

		internal static void ScrollToBottom(RichTextBox richTextBox)
		{
			SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
			richTextBox.SelectionStart = richTextBox.Text.Length;
		}
		private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
			ScrollToBottom(richTextBox1);
		}

        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
			if (this.metroCheckBox2.Checked)
			{
				this.richTextBox1.Clear();
				this.timer1.Enabled = true;
				return;
			}
			this.timer1.Enabled = false;
		}

        private void metroTextBox1_Click(object sender, EventArgs e)
        {
			try
			{
				int num = int.Parse(this.metroTextBox1.Text);
				this.metroTextBox1.Text = num.ToString();
			}
			catch
			{
				this.metroTextBox1.Text = "";
			}
			this.timer1.Interval = int.Parse(this.metroTextBox1.Text);
		}
    }
    public static class RichTextBoxExtensions
	{
		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}
	}
}
