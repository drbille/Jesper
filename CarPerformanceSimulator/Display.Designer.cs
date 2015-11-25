namespace CarPerformanceSimulator
{
    partial class FormView
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
            this.LpathBound = new System.Windows.Forms.Label();
            this.RpathBound = new System.Windows.Forms.Label();
            this.speed_lb = new System.Windows.Forms.Label();
            this.speed_lbHelper = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LatencyTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LpathBound
            // 
            this.LpathBound.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LpathBound.BackColor = System.Drawing.Color.Black;
            this.LpathBound.Location = new System.Drawing.Point(195, 0);
            this.LpathBound.Name = "LpathBound";
            this.LpathBound.Size = new System.Drawing.Size(11, 612);
            this.LpathBound.TabIndex = 0;
            // 
            // RpathBound
            // 
            this.RpathBound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RpathBound.BackColor = System.Drawing.Color.Black;
            this.RpathBound.ForeColor = System.Drawing.Color.Black;
            this.RpathBound.Location = new System.Drawing.Point(390, 0);
            this.RpathBound.Name = "RpathBound";
            this.RpathBound.Size = new System.Drawing.Size(11, 612);
            this.RpathBound.TabIndex = 0;
            // 
            // speed_lb
            // 
            this.speed_lb.BackColor = System.Drawing.Color.Black;
            this.speed_lb.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speed_lb.ForeColor = System.Drawing.Color.White;
            this.speed_lb.Location = new System.Drawing.Point(116, 0);
            this.speed_lb.Name = "speed_lb";
            this.speed_lb.Size = new System.Drawing.Size(81, 24);
            this.speed_lb.TabIndex = 12;
            this.speed_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // speed_lbHelper
            // 
            this.speed_lbHelper.BackColor = System.Drawing.Color.Black;
            this.speed_lbHelper.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speed_lbHelper.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.speed_lbHelper.Location = new System.Drawing.Point(3, 0);
            this.speed_lbHelper.Name = "speed_lbHelper";
            this.speed_lbHelper.Size = new System.Drawing.Size(109, 24);
            this.speed_lbHelper.TabIndex = 13;
            this.speed_lbHelper.Text = "speed";
            this.speed_lbHelper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.speed_lbHelper.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.speed_lbHelper);
            this.panel1.Controls.Add(this.speed_lb);
            this.panel1.Location = new System.Drawing.Point(432, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 26);
            this.panel1.TabIndex = 14;
            this.panel1.Visible = false;
            // 
            // LatencyTimer
            // 
            this.LatencyTimer.Enabled = true;
            this.LatencyTimer.Interval = 500;
            this.LatencyTimer.Tick += new System.EventHandler(this.LatencyTimer_Tick);
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 612);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RpathBound);
            this.Controls.Add(this.LpathBound);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "FormView";
            this.Text = "Car Performance Simulator";
            this.ResizeBegin += new System.EventHandler(this.FormView_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormView_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormView_Paint);
            this.Resize += new System.EventHandler(this.FormView_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LpathBound;
        private System.Windows.Forms.Label RpathBound;
        private System.Windows.Forms.Label speed_lb;
        private System.Windows.Forms.Label speed_lbHelper;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer LatencyTimer;
    }
}

