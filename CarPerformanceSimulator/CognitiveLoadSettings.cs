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
        Display main;
        public CognitiveLoadSettings(Display mainDisplay)
        {
            InitializeComponent();
            main = mainDisplay;

            int i = 0;
            foreach(bool b in mainDisplay.cogLoad)
            {
                OptionsList.SetSelected(i, mainDisplay.cogLoad[i]);
                i++;
            }

            initialized = true;
        }

        private void OptionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
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
}
