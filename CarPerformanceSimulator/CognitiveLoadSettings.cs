using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarPerformanceSimulator
{
    public partial class CognitiveLoadSettings : Form
    {
        bool initialized = false;
        FormView main;
        public CognitiveLoadSettings(FormView mainDisplay)
        {
            InitializeComponent();
            main = mainDisplay;
            initialized = true;
        }

        private void OptionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            bool cogMode = false;
            foreach (bool b in main.cogLoad)
            {
                bool s = OptionsList.GetItemChecked(i);
                main.cogLoad[i] = s;
                i++;
                if (s == true)
                {
                    cogMode = true;
                    main.cognitiveLoadTest = true;
                }
            }
            if (!cogMode)
            {
                main.cognitiveLoadTest = false;
            }
            main.Restart();
        }
    }
}
