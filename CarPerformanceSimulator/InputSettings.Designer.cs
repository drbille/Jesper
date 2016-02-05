namespace CarPerformanceSimulator
{
    partial class InputSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.InputSelect = new System.Windows.Forms.ComboBox();
            this.PollingTimer = new System.Windows.Forms.Timer(this.components);
            this.Joystick1Info = new System.Windows.Forms.Label();
            this.SteeringAxisSelect = new System.Windows.Forms.ComboBox();
            this.AcceleratorAxisSelect = new System.Windows.Forms.ComboBox();
            this.BrakeAxisSelect = new System.Windows.Forms.ComboBox();
            this.Xaxis = new System.Windows.Forms.Label();
            this.Yaxis = new System.Windows.Forms.Label();
            this.RzAxis = new System.Windows.Forms.Label();
            this.ReverseButtonSelect = new System.Windows.Forms.ComboBox();
            this.DriveButtonSelect = new System.Windows.Forms.ComboBox();
            this.Drive = new System.Windows.Forms.Label();
            this.Reverse = new System.Windows.Forms.Label();
            this.Hold = new System.Windows.Forms.CheckBox();
            this.AutoSteering = new System.Windows.Forms.Button();
            this.AutoAccelerator = new System.Windows.Forms.Button();
            this.AutoBrake = new System.Windows.Forms.Button();
            this.AutoDrive = new System.Windows.Forms.Button();
            this.AutoReverse = new System.Windows.Forms.Button();
            this.JoyPanel = new System.Windows.Forms.Panel();
            this.PedalSelect = new System.Windows.Forms.ComboBox();
            this.WheelSelect = new System.Windows.Forms.ComboBox();
            this.Joystick2Info = new System.Windows.Forms.Label();
            this.KeyboardOutput = new System.Windows.Forms.Label();
            this.JoyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputSelect
            // 
            this.InputSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InputSelect.FormattingEnabled = true;
            this.InputSelect.Items.AddRange(new object[] {
            "Joystick",
            "Keyboard"});
            this.InputSelect.Location = new System.Drawing.Point(15, 12);
            this.InputSelect.Name = "InputSelect";
            this.InputSelect.Size = new System.Drawing.Size(121, 21);
            this.InputSelect.TabIndex = 0;
            this.InputSelect.SelectedIndexChanged += new System.EventHandler(this.InputSelect_SelectedIndexChanged);
            // 
            // PollingTimer
            // 
            this.PollingTimer.Enabled = true;
            this.PollingTimer.Tick += new System.EventHandler(this.pollingTimer_Tick);
            // 
            // Joystick1Info
            // 
            this.Joystick1Info.AutoSize = true;
            this.Joystick1Info.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Joystick1Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Joystick1Info.Location = new System.Drawing.Point(11, 169);
            this.Joystick1Info.MaximumSize = new System.Drawing.Size(300, 300);
            this.Joystick1Info.MinimumSize = new System.Drawing.Size(250, 100);
            this.Joystick1Info.Name = "Joystick1Info";
            this.Joystick1Info.Padding = new System.Windows.Forms.Padding(1);
            this.Joystick1Info.Size = new System.Drawing.Size(250, 100);
            this.Joystick1Info.TabIndex = 0;
            this.Joystick1Info.Text = "No Joystick";
            this.Joystick1Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Joystick1Info.Visible = false;
            // 
            // SteeringAxisSelect
            // 
            this.SteeringAxisSelect.DisplayMember = "int";
            this.SteeringAxisSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SteeringAxisSelect.FormattingEnabled = true;
            this.SteeringAxisSelect.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z",
            "Rx",
            "Ry",
            "Rz"});
            this.SteeringAxisSelect.Location = new System.Drawing.Point(153, 37);
            this.SteeringAxisSelect.Name = "SteeringAxisSelect";
            this.SteeringAxisSelect.Size = new System.Drawing.Size(121, 21);
            this.SteeringAxisSelect.TabIndex = 1;
            this.SteeringAxisSelect.ValueMember = "int";
            this.SteeringAxisSelect.SelectedIndexChanged += new System.EventHandler(this.SteeringAxisSelect_SelectedIndexChanged);
            // 
            // AcceleratorAxisSelect
            // 
            this.AcceleratorAxisSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AcceleratorAxisSelect.FormattingEnabled = true;
            this.AcceleratorAxisSelect.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z",
            "Rx",
            "Ry",
            "Rz"});
            this.AcceleratorAxisSelect.Location = new System.Drawing.Point(430, 39);
            this.AcceleratorAxisSelect.Name = "AcceleratorAxisSelect";
            this.AcceleratorAxisSelect.Size = new System.Drawing.Size(121, 21);
            this.AcceleratorAxisSelect.TabIndex = 2;
            this.AcceleratorAxisSelect.SelectedIndexChanged += new System.EventHandler(this.AcceleratorAxisSelect_SelectedIndexChanged);
            // 
            // BrakeAxisSelect
            // 
            this.BrakeAxisSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BrakeAxisSelect.FormattingEnabled = true;
            this.BrakeAxisSelect.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z",
            "Rx",
            "Ry",
            "Rz"});
            this.BrakeAxisSelect.Location = new System.Drawing.Point(430, 66);
            this.BrakeAxisSelect.Name = "BrakeAxisSelect";
            this.BrakeAxisSelect.Size = new System.Drawing.Size(121, 21);
            this.BrakeAxisSelect.TabIndex = 3;
            this.BrakeAxisSelect.SelectedIndexChanged += new System.EventHandler(this.BrakeAxisSelect_SelectedIndexChanged);
            // 
            // Xaxis
            // 
            this.Xaxis.AutoSize = true;
            this.Xaxis.Location = new System.Drawing.Point(79, 37);
            this.Xaxis.Name = "Xaxis";
            this.Xaxis.Size = new System.Drawing.Size(68, 13);
            this.Xaxis.TabIndex = 5;
            this.Xaxis.Text = "Steering Axis";
            // 
            // Yaxis
            // 
            this.Yaxis.AutoSize = true;
            this.Yaxis.Location = new System.Drawing.Point(341, 42);
            this.Yaxis.Name = "Yaxis";
            this.Yaxis.Size = new System.Drawing.Size(83, 13);
            this.Yaxis.TabIndex = 6;
            this.Yaxis.Text = "Accelerator Axis";
            // 
            // RzAxis
            // 
            this.RzAxis.AutoSize = true;
            this.RzAxis.Location = new System.Drawing.Point(367, 69);
            this.RzAxis.Name = "RzAxis";
            this.RzAxis.Size = new System.Drawing.Size(57, 13);
            this.RzAxis.TabIndex = 7;
            this.RzAxis.Text = "Brake Axis";
            // 
            // ReverseButtonSelect
            // 
            this.ReverseButtonSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReverseButtonSelect.FormattingEnabled = true;
            this.ReverseButtonSelect.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.ReverseButtonSelect.Location = new System.Drawing.Point(153, 92);
            this.ReverseButtonSelect.Name = "ReverseButtonSelect";
            this.ReverseButtonSelect.Size = new System.Drawing.Size(121, 21);
            this.ReverseButtonSelect.TabIndex = 8;
            this.ReverseButtonSelect.SelectedIndexChanged += new System.EventHandler(this.ReverseButtonSelect_SelectedIndexChanged);
            // 
            // DriveButtonSelect
            // 
            this.DriveButtonSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DriveButtonSelect.FormattingEnabled = true;
            this.DriveButtonSelect.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.DriveButtonSelect.Location = new System.Drawing.Point(153, 65);
            this.DriveButtonSelect.Name = "DriveButtonSelect";
            this.DriveButtonSelect.Size = new System.Drawing.Size(121, 21);
            this.DriveButtonSelect.TabIndex = 9;
            this.DriveButtonSelect.SelectedIndexChanged += new System.EventHandler(this.DriveButtonSelect_SelectedIndexChanged);
            // 
            // Drive
            // 
            this.Drive.AutoSize = true;
            this.Drive.Location = new System.Drawing.Point(81, 68);
            this.Drive.Name = "Drive";
            this.Drive.Size = new System.Drawing.Size(66, 13);
            this.Drive.TabIndex = 11;
            this.Drive.Text = "Drive Button";
            // 
            // Reverse
            // 
            this.Reverse.AutoSize = true;
            this.Reverse.Location = new System.Drawing.Point(66, 95);
            this.Reverse.Name = "Reverse";
            this.Reverse.Size = new System.Drawing.Size(81, 13);
            this.Reverse.TabIndex = 12;
            this.Reverse.Text = "Reverse Button";
            // 
            // Hold
            // 
            this.Hold.AutoSize = true;
            this.Hold.Location = new System.Drawing.Point(280, 96);
            this.Hold.Name = "Hold";
            this.Hold.Size = new System.Drawing.Size(82, 17);
            this.Hold.TabIndex = 13;
            this.Hold.Text = "Hold Button";
            this.Hold.UseVisualStyleBackColor = true;
            // 
            // AutoSteering
            // 
            this.AutoSteering.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.AutoSteering.Location = new System.Drawing.Point(3, 34);
            this.AutoSteering.Name = "AutoSteering";
            this.AutoSteering.Size = new System.Drawing.Size(62, 23);
            this.AutoSteering.TabIndex = 14;
            this.AutoSteering.Text = "Auto Detect";
            this.AutoSteering.UseVisualStyleBackColor = true;
            this.AutoSteering.Click += new System.EventHandler(this.AutoSteering_Click);
            // 
            // AutoAccelerator
            // 
            this.AutoAccelerator.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.AutoAccelerator.Location = new System.Drawing.Point(280, 37);
            this.AutoAccelerator.Name = "AutoAccelerator";
            this.AutoAccelerator.Size = new System.Drawing.Size(62, 23);
            this.AutoAccelerator.TabIndex = 15;
            this.AutoAccelerator.Text = "Auto Detect";
            this.AutoAccelerator.UseVisualStyleBackColor = true;
            this.AutoAccelerator.Click += new System.EventHandler(this.AutoAccelerator_Click);
            // 
            // AutoBrake
            // 
            this.AutoBrake.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.AutoBrake.Location = new System.Drawing.Point(280, 64);
            this.AutoBrake.Name = "AutoBrake";
            this.AutoBrake.Size = new System.Drawing.Size(62, 23);
            this.AutoBrake.TabIndex = 16;
            this.AutoBrake.Text = "Auto Detect";
            this.AutoBrake.UseVisualStyleBackColor = true;
            this.AutoBrake.Click += new System.EventHandler(this.AutoBrake_Click);
            // 
            // AutoDrive
            // 
            this.AutoDrive.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.AutoDrive.Location = new System.Drawing.Point(3, 63);
            this.AutoDrive.Name = "AutoDrive";
            this.AutoDrive.Size = new System.Drawing.Size(62, 23);
            this.AutoDrive.TabIndex = 17;
            this.AutoDrive.Text = "Auto Detect";
            this.AutoDrive.UseVisualStyleBackColor = true;
            this.AutoDrive.Click += new System.EventHandler(this.AutoDrive_Click);
            // 
            // AutoReverse
            // 
            this.AutoReverse.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.AutoReverse.Location = new System.Drawing.Point(3, 90);
            this.AutoReverse.Name = "AutoReverse";
            this.AutoReverse.Size = new System.Drawing.Size(62, 23);
            this.AutoReverse.TabIndex = 18;
            this.AutoReverse.Text = "Auto Detect";
            this.AutoReverse.UseVisualStyleBackColor = true;
            this.AutoReverse.Click += new System.EventHandler(this.AutoReverse_Click);
            // 
            // JoyPanel
            // 
            this.JoyPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.JoyPanel.Controls.Add(this.PedalSelect);
            this.JoyPanel.Controls.Add(this.WheelSelect);
            this.JoyPanel.Controls.Add(this.AutoSteering);
            this.JoyPanel.Controls.Add(this.AutoReverse);
            this.JoyPanel.Controls.Add(this.Hold);
            this.JoyPanel.Controls.Add(this.Yaxis);
            this.JoyPanel.Controls.Add(this.SteeringAxisSelect);
            this.JoyPanel.Controls.Add(this.RzAxis);
            this.JoyPanel.Controls.Add(this.Xaxis);
            this.JoyPanel.Controls.Add(this.AutoDrive);
            this.JoyPanel.Controls.Add(this.Reverse);
            this.JoyPanel.Controls.Add(this.AutoAccelerator);
            this.JoyPanel.Controls.Add(this.AcceleratorAxisSelect);
            this.JoyPanel.Controls.Add(this.ReverseButtonSelect);
            this.JoyPanel.Controls.Add(this.DriveButtonSelect);
            this.JoyPanel.Controls.Add(this.BrakeAxisSelect);
            this.JoyPanel.Controls.Add(this.AutoBrake);
            this.JoyPanel.Controls.Add(this.Drive);
            this.JoyPanel.Location = new System.Drawing.Point(12, 39);
            this.JoyPanel.Name = "JoyPanel";
            this.JoyPanel.Size = new System.Drawing.Size(557, 127);
            this.JoyPanel.TabIndex = 19;
            // 
            // PedalSelect
            // 
            this.PedalSelect.DisplayMember = "int";
            this.PedalSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PedalSelect.FormattingEnabled = true;
            this.PedalSelect.Location = new System.Drawing.Point(280, 7);
            this.PedalSelect.Name = "PedalSelect";
            this.PedalSelect.Size = new System.Drawing.Size(121, 21);
            this.PedalSelect.TabIndex = 20;
            this.PedalSelect.ValueMember = "int";
            this.PedalSelect.SelectedIndexChanged += new System.EventHandler(this.PedalSelect_SelectedIndexChanged);
            // 
            // WheelSelect
            // 
            this.WheelSelect.DisplayMember = "int";
            this.WheelSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WheelSelect.FormattingEnabled = true;
            this.WheelSelect.Location = new System.Drawing.Point(3, 7);
            this.WheelSelect.Name = "WheelSelect";
            this.WheelSelect.Size = new System.Drawing.Size(121, 21);
            this.WheelSelect.TabIndex = 19;
            this.WheelSelect.ValueMember = "int";
            this.WheelSelect.SelectedIndexChanged += new System.EventHandler(this.WheelSelect_SelectedIndexChanged);
            // 
            // Joystick2Info
            // 
            this.Joystick2Info.AutoSize = true;
            this.Joystick2Info.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Joystick2Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Joystick2Info.Location = new System.Drawing.Point(318, 169);
            this.Joystick2Info.MaximumSize = new System.Drawing.Size(300, 300);
            this.Joystick2Info.MinimumSize = new System.Drawing.Size(250, 100);
            this.Joystick2Info.Name = "Joystick2Info";
            this.Joystick2Info.Padding = new System.Windows.Forms.Padding(1);
            this.Joystick2Info.Size = new System.Drawing.Size(250, 100);
            this.Joystick2Info.TabIndex = 0;
            this.Joystick2Info.Text = "No 2nd Joystick";
            this.Joystick2Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Joystick2Info.Visible = false;
            // 
            // KeyboardOutput
            // 
            this.KeyboardOutput.AutoSize = true;
            this.KeyboardOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.KeyboardOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.KeyboardOutput.Location = new System.Drawing.Point(161, 169);
            this.KeyboardOutput.MaximumSize = new System.Drawing.Size(300, 300);
            this.KeyboardOutput.MinimumSize = new System.Drawing.Size(250, 100);
            this.KeyboardOutput.Name = "KeyboardOutput";
            this.KeyboardOutput.Padding = new System.Windows.Forms.Padding(1);
            this.KeyboardOutput.Size = new System.Drawing.Size(250, 100);
            this.KeyboardOutput.TabIndex = 20;
            this.KeyboardOutput.Text = "Keyboard Keys";
            this.KeyboardOutput.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KeyboardOutput.Visible = false;
            // 
            // InputSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(577, 278);
            this.Controls.Add(this.KeyboardOutput);
            this.Controls.Add(this.Joystick2Info);
            this.Controls.Add(this.JoyPanel);
            this.Controls.Add(this.Joystick1Info);
            this.Controls.Add(this.InputSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InputSettings";
            this.Text = "Input Settings";
            this.JoyPanel.ResumeLayout(false);
            this.JoyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InputSelect;
        private System.Windows.Forms.Timer PollingTimer;
        private System.Windows.Forms.Label Joystick1Info;
        private System.Windows.Forms.ComboBox SteeringAxisSelect;
        private System.Windows.Forms.ComboBox AcceleratorAxisSelect;
        private System.Windows.Forms.ComboBox BrakeAxisSelect;
        private System.Windows.Forms.Label Xaxis;
        private System.Windows.Forms.Label Yaxis;
        private System.Windows.Forms.Label RzAxis;
        private System.Windows.Forms.ComboBox ReverseButtonSelect;
        private System.Windows.Forms.ComboBox DriveButtonSelect;
        private System.Windows.Forms.Label Drive;
        private System.Windows.Forms.Label Reverse;
        private System.Windows.Forms.CheckBox Hold;
        private System.Windows.Forms.Button AutoSteering;
        private System.Windows.Forms.Button AutoAccelerator;
        private System.Windows.Forms.Button AutoBrake;
        private System.Windows.Forms.Button AutoDrive;
        private System.Windows.Forms.Button AutoReverse;
        private System.Windows.Forms.Panel JoyPanel;
        private System.Windows.Forms.Label Joystick2Info;
        private System.Windows.Forms.ComboBox PedalSelect;
        private System.Windows.Forms.ComboBox WheelSelect;
        public System.Windows.Forms.Label KeyboardOutput;
    }
}