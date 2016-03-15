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
    public partial class UserDisplay : Form
    {
        InputSettings inputSettings;
        Display mainDisplay;
        bool initialized = false;
        bool boxSet = false;

        CognitiveLoadSettings cogSettings;

        public UserDisplay(Display display)
        {
            InitializeComponent();

            mainDisplay = display;
            cogSettings = new CognitiveLoadSettings(mainDisplay);
            inputSettings = new InputSettings(mainDisplay);

            ObjectSelect.SelectedIndex = mainDisplay.getObjectId();
            StationaryObjectsList.SelectedIndex = 0;
            AppearBehaviour.SelectedIndex = 0; //Implement setters for choices *ToDo
            CarActivation.SelectedIndex = 0;
            CarBehaviour.SelectedIndex = 0;

            SteeringWheelSenseSet.Value = mainDisplay.steeringSensitivity;
            SteeringWheelSensScroll.Value = (int)SteeringWheelSenseSet.Value;
            TransmissionEffSet.Value = (int)(mainDisplay.transmissionEfficiency * 100);
            TransmissionEffScroll.Value = (int)TransmissionEffSet.Value;
            BrakeForceSet.Value = (int)mainDisplay.brakeMax;
            BrakeForceScroll.Value = (int)BrakeForceSet.Value;
            SimStepSet.Value = (int)mainDisplay.msPerUpdate;
            SimStepScroll.Value = (int)SimStepSet.Value;
            RTriggerFactor.Value = (int)mainDisplay.rTriggerMax;

            saveData.InitialDirectory = mainDisplay.getFileName();
            initialized = true;
            this.BringToFront();
        }

        private void inputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputSettings.IsDisposed)
                inputSettings = new InputSettings(mainDisplay);
            inputSettings.Focus();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainDisplay.Restart();
        }

        private void RT_lb_Click(object sender, EventArgs e)
        {
            mainDisplay.RT_btn_Click();
        }

        private void ObjectSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectSelect.SelectedIndex == 0)
            {
                if (initialized)
                    mainDisplay.setObjectId(0);
                StationaryObjects.Hide();
                mainDisplay.triggerType = CarActivation.SelectedIndex;
                CarControls.Show();
                //Invalidate();
            }
            else if (ObjectSelect.SelectedIndex == 1)
            {
                if (initialized)
                    mainDisplay.setObjectId(1);
                CarControls.Hide();
                StationaryObjects.Show();
                mainDisplay.triggerType = AppearBehaviour.SelectedIndex;
                //Invalidate();
            }
        }

        private void StationaryObjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
                mainDisplay.setObjectId(StationaryObjectsList.SelectedIndex + 1);
        }


        private void saveData_FileOk(object sender, CancelEventArgs e)
        {
            mainDisplay.setFileName(saveData.FileName);
        }

        private void dataExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveData.ShowDialog();
        }

        private void AppearBehaviour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AppearBehaviour.SelectedIndex == 0)
            {
                RandomTrigger.Hide();
            }
            else if (AppearBehaviour.SelectedIndex == 1)
            {
                RandomTrigger.Show();
            }
            mainDisplay.triggerType = AppearBehaviour.SelectedIndex;
        }

        private void CarActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CarActivation.SelectedIndex == 0)
            {
                RandomTrigger.Hide();
            }
            else if (CarActivation.SelectedIndex == 1)
            {
                RandomTrigger.Show();
            }
            mainDisplay.triggerType = CarActivation.SelectedIndex;
            if(initialized)
            mainDisplay.Restart();
        }

        private void xTrialsSet_ValueChanged(object sender, EventArgs e)
        {
            mainDisplay.xTrials = (int)xTrialsSet.Value;
            progressBar.Maximum = (int)xTrialsSet.Value;
            if (initialized)
            mainDisplay.Restart();
        }

        private void xSpeedSet_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                mainDisplay.xSpeed = (int)((double)xSpeedSet.Value * 0.44704);
                mainDisplay.Restart();
            }

        }

        private void BrakeForceScroll_Scroll(object sender, EventArgs e)
        {
            if (initialized && !boxSet)
                BrakeForceSet.Value = BrakeForceScroll.Value;
            boxSet = false;
        }

        private void BrakeForceSet_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
                mainDisplay.brakeMax = (int)BrakeForceSet.Value;
            boxSet = true;
            BrakeForceScroll.Value = (int)BrakeForceSet.Value;
        }

        private void TransmissionEffScroll_Scroll(object sender, EventArgs e)
        {
            if (initialized && !boxSet)
                TransmissionEffSet.Value = TransmissionEffScroll.Value;
            boxSet = false;
        }

        private void TransmissionEffSet_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
                mainDisplay.physicsEngine.transmissionEfficiency = (double)TransmissionEffSet.Value / 100;
            boxSet = true;
            TransmissionEffScroll.Value = (int)TransmissionEffSet.Value;
        }

        private void SteeringWheelSensScroll_Scroll(object sender, EventArgs e)
        {
            if (initialized && !boxSet)
                SteeringWheelSenseSet.Value = SteeringWheelSensScroll.Value;
            boxSet = false;
        }

        private void SteeringWheelSenseSet_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
                mainDisplay.steeringSensitivity = (int)SteeringWheelSenseSet.Value;
            boxSet = true;
            SteeringWheelSensScroll.Value = (int)SteeringWheelSenseSet.Value;
        }

        private void SimStepScroll_Scroll(object sender, EventArgs e)
        {
            if (initialized && !boxSet)
                SimStepSet.Value = SimStepScroll.Value;
            boxSet = false;
        }

        private void SimStepSet_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
                mainDisplay.msPerUpdate = (int)SimStepSet.Value;
            boxSet = true;
            SimStepScroll.Value = (int)SimStepSet.Value;
        }

        private void RTriggerFactor_ValueChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                mainDisplay.rTriggerMax = (int)RTriggerFactor.Value;
            }
        }

        private void cognitaveLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(cogSettings.IsDisposed)
            {
                cogSettings = new CognitiveLoadSettings(mainDisplay);
                cogSettings.Show();
            }
            cogSettings.Show();
        }
    }
}
