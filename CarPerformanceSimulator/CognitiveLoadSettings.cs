/*
 * This file is part of NeuRRoDrive
 *
 * Copyright 2014-2016 University of Michigan NeuRRo Lab. All Rights Reserved.
 * <http://www.neurro-lab.engin.umich.edu/>
 *
 * NeuRRoDrive is free software: you may redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * NeuRRoDrive is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * <http://www.gnu.org/licenses>
 *
 * Managed by: Jakob Rodseth (Jrodseth12@gmail.com)
 * 
 */

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
