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
        }

        private void togglebtn_Click(object sender, EventArgs e)
        {
            running = !running;
            if (running)
            {
                statuslbl.Text = "Running";
                statuslbl.ForeColor = Color.Green;
                modLoader = new ModLoader(Provider.EPIC, "1.0.0.9");
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
                    statuslbl.Text = "Process not found. Try again";
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
    }
}
