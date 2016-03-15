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
using System.Linq;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;

namespace CarPerformanceSimulator
{
    class KeyboardInterface
    {
        private Device keyboard;
        DeviceList keyboardList = Manager.GetDevices(DeviceClass.Keyboard, EnumDevicesFlags.AttachedOnly);

        public KeyboardInterface(Form form)
        {
            if (keyboardList.Count > 0)
            {
                keyboardList.MoveNext();
                DeviceInstance deviceInstance = (DeviceInstance)keyboardList.Current;
                keyboard = new Device(deviceInstance.InstanceGuid);
                keyboard.SetCooperativeLevel(form, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            }
            keyboard.SetDataFormat(DeviceDataFormat.Keyboard);
            keyboard.Acquire();
        }
        public Key[] poll()
        {
            Key[] keys = keyboard.GetPressedKeys();
            return keys;
        }
    }    
}
