namespace ModManager
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            title = new Label();
            statuslbl = new Label();
            togglebtn = new Button();
            provcombobox = new ComboBox();
            provlbl = new Label();
            versioncombobox = new ComboBox();
            versionlbl = new Label();
            SuspendLayout();
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            title.Location = new Point(0, 0);
            title.Name = "title";
            title.Size = new Size(257, 25);
            title.TabIndex = 0;
            title.Text = "Kingdom Hearts Mod Interface";
            // 
            // statuslbl
            // 
            statuslbl.AutoSize = true;
            statuslbl.ForeColor = Color.Red;
            statuslbl.Location = new Point(75, 50);
            statuslbl.Name = "statuslbl";
            statuslbl.Size = new Size(113, 25);
            statuslbl.TabIndex = 1;
            statuslbl.Text = "Not Running";
            // 
            // togglebtn
            // 
            togglebtn.Enabled = false;
            togglebtn.Location = new Point(50, 100);
            togglebtn.Name = "togglebtn";
            togglebtn.Size = new Size(148, 45);
            togglebtn.TabIndex = 2;
            togglebtn.Text = "Start";
            togglebtn.UseVisualStyleBackColor = true;
            togglebtn.Click += togglebtn_Click;
            // 
            // provcombobox
            // 
            provcombobox.FormattingEnabled = true;
            provcombobox.Location = new Point(412, 22);
            provcombobox.Name = "provcombobox";
            provcombobox.Size = new Size(103, 33);
            provcombobox.TabIndex = 3;
            provcombobox.SelectedIndexChanged += provcombobox_SelectedIndexChanged;
            // 
            // provlbl
            // 
            provlbl.AutoSize = true;
            provlbl.Location = new Point(328, 25);
            provlbl.Name = "provlbl";
            provlbl.Size = new Size(78, 25);
            provlbl.TabIndex = 4;
            provlbl.Text = "Provider";
            // 
            // versioncombobox
            // 
            versioncombobox.Enabled = false;
            versioncombobox.FormattingEnabled = true;
            versioncombobox.Location = new Point(412, 61);
            versioncombobox.Name = "versioncombobox";
            versioncombobox.Size = new Size(103, 33);
            versioncombobox.TabIndex = 5;
            versioncombobox.Visible = false;
            versioncombobox.SelectedIndexChanged += versioncombobox_SelectedIndexChanged;
            // 
            // versionlbl
            // 
            versionlbl.AutoSize = true;
            versionlbl.Location = new Point(336, 61);
            versionlbl.Name = "versionlbl";
            versionlbl.Size = new Size(70, 25);
            versionlbl.TabIndex = 6;
            versionlbl.Text = "Version";
            versionlbl.Visible = false;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(527, 161);
            Controls.Add(versionlbl);
            Controls.Add(versioncombobox);
            Controls.Add(provlbl);
            Controls.Add(provcombobox);
            Controls.Add(togglebtn);
            Controls.Add(statuslbl);
            Controls.Add(title);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainWindow";
            Text = "KHMI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label title;
        private Label statuslbl;
        private Button togglebtn;
        private ComboBox provcombobox;
        private Label provlbl;
        private ComboBox versioncombobox;
        private Label versionlbl;
    }
}
