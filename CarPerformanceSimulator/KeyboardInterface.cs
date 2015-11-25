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
