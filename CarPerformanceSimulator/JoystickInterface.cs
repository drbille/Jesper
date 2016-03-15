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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace CarPerformanceSimulator
{
    class JoystickInterface
    {
        private Device[] joystick;
        DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
        private JoystickState[] state;

        public void poll(int joy)
        {
            try
                { 
                    // poll the joystick
                    joystick[joy].Poll();
                // update the joystick state field
                UpdateJoystick(joy);
                }
                catch (Exception err)
                {
                    // we probably lost connection to the joystick
                    // was it unplugged or locked by another application?
                    //Debug.WriteLine("Joystick Connection Lost");
                }
        }

        private void UpdateJoystick(int index)
        {
            //Get Joystick State.
            state[index] = joystick[index].CurrentJoystickState;
        }

        //constructor
        public JoystickInterface(Form form)
        {
            joystick = new Device[gameControllerList.Count];
            state = new JoystickState[gameControllerList.Count];
            if (gameControllerList.Count > 0)
            {
                for (int i = 0; i < gameControllerList.Count; i++)
                {
                    {
                        // Move to the next device
                        gameControllerList.MoveNext();
                        DeviceInstance deviceInstance = (DeviceInstance)gameControllerList.Current;

                        // create a device from this controller.
                        joystick[i] = new Device(deviceInstance.InstanceGuid);
                        joystick[i].SetCooperativeLevel(form, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                    }

                    // Tell DirectX that this is a Joystick.
                    joystick[i].SetDataFormat(DeviceDataFormat.Joystick);
                    // Finally, acquire the device.
                    joystick[i].Acquire();
                }
            }   
        }


        public JoystickState getState(int joystick)
        {
            JoystickState stateR = state[joystick];
            return stateR;
        }

        public Device[] getJoystick()
        {
            Device[] joystickR = joystick;
            return joystickR;
        }

        public int getNumJoysticks()
        {
            return gameControllerList.Count;
        }
    }
}
