namespace CarPerformanceSimulator
{
    partial class CognitiveLoadSettings
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
            this.OptionsList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // OptionsList
            // 
            this.OptionsList.CheckOnClick = true;
            this.OptionsList.FormattingEnabled = true;
            this.OptionsList.Items.AddRange(new object[] {
            "Move Left",
            "Move Right",
            "Brake",
            "Move Left + Brake",
            "Move Right + Brake"});
            this.OptionsList.Location = new System.Drawing.Point(0, 0);
            this.OptionsList.Name = "OptionsList";
            this.OptionsList.Size = new System.Drawing.Size(221, 79);
            this.OptionsList.TabIndex = 0;
            this.OptionsList.SelectedIndexChanged += new System.EventHandler(this.OptionsList_SelectedIndexChanged);
            // 
            // CognitiveLoadSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 79);
            this.Controls.Add(this.OptionsList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CognitiveLoadSettings";
            this.Text = "Cognitive Load Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox OptionsList;

    }
}