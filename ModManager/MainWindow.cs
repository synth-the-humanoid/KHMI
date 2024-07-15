using KHMI;

namespace ModManager
{
    public partial class MainWindow : Form
    {
        private bool running = false;
        private ModLoader modLoader;
        private Task t;

        public MainWindow()
        {
            InitializeComponent();
            foreach (string s in VersionInfo.SupportedProviders)
            {
                provcombobox.Items.Add(s);
            }
        }

        private void togglebtn_Click(object sender, EventArgs e)
        {
            running = !running;
            if (running)
            {
                statuslbl.Text = "Running";
                statuslbl.ForeColor = Color.Green;
                modLoader = new ModLoader(VersionInfo.StringToProvider(provcombobox.Text), versioncombobox.Text);
                if (modLoader.attach())
                {
                    togglebtn.Text = "Stop";
                    t = Task.Run(() =>
                    {
                        while (modLoader.Runnable)
                        {
                            modLoader.runEvents();
                            Thread.Sleep(1);
                        }
                    });
                }
                else
                {
                    statuslbl.Text = "Process not found. Retry";
                    statuslbl.ForeColor = Color.Red;
                    running = !running;
                }
            }
            else
            {
                statuslbl.Text = "Not Running";
                statuslbl.ForeColor = Color.Red;
                modLoader.close();
                togglebtn.Text = "Start";
            }
        }

        private void provcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (provcombobox.Text != "")
            {
                Provider p = VersionInfo.StringToProvider(provcombobox.Text);
                string[] versions = VersionInfo.GetSupportedVersions(p);
                foreach (string v in versions)
                {
                    versioncombobox.Items.Add(v);
                }
                versionlbl.Visible = true;
                versioncombobox.Enabled = true;
                versioncombobox.Visible = true;
            }
        }

        private void versioncombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (versioncombobox.Text != "")
            {
                togglebtn.Enabled = true;
            }
        }
    }
}
