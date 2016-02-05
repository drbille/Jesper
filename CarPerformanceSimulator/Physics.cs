using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace CarPerformanceSimulator
{
    class Physics
    {
        private int reverseGear;
        private int driveGear;
        private int selectedGear;
        
        private static double DRAG = 0.4257;
        private static double ROLLING_DRAG = 12.8;

        private double brakeValue = 0; //Current brake magnitude
        private double velocity; //Current velocity
        private int carMass = 1639; //IN KILOGRAMS, NEEDS TO BE PART OF CLASS
        private double throttle;
       
        private double accelValue; //Accelerator magnitude
        private int[] joyAxisMax; //Maximum joystick axis value
        private bool isAccel = false; //Is the acclerator engaged
        private bool isBrake = false; //Is brake engaged

        private double wheelRadius = 0.45; // IN METERS, NEEDS TO BE PART OF CLASS
        private int[] torqueCurve;
        private double[] gearRatio;
        private bool neutral;

        private int pedalJoy;

        internal double transmissionEfficiency;

        private Display mainDisplay;
        private UserDisplay dataReadout;
        private Car car = new _2015FordFusion();

        int rpm;
        int oldRpm;
        SoundEffectInstance soundInstance;
        SoundEffect sound;
        bool soundInit;

        public Physics(Display display, UserDisplay readout)
        {

            dataReadout = readout;
            mainDisplay = display;

            inputChanged();

            carMass = 1347;
            transmissionEfficiency = display.transmissionEfficiency;
            oldRpm = 0;

            torqueCurve = car.torqueCurve;
            gearRatio = car.gearRatios;

            selectedGear = 1; // CAUSES STARTUP INPUT BUGS *ToDo
            neutral = true;
            soundInit = false;
            try
            {
                sound = SoundEffect.FromStream(TitleContainer.OpenStream("engineIdleLoopAmp1.wav"));
                soundInstance = sound.CreateInstance();
                soundInstance.IsLooped = true;

                soundInstance.Volume = 0.5F;
                soundInstance.Play();
                soundInit = true;
            }
            catch (Exception err)
            {
            }
        }

        public void inputChanged()
        {
                pedalJoy = mainDisplay.getPedalJoy();
                joyAxisMax = new int[mainDisplay.getNumJoys()];
                for (int i = 0; i < mainDisplay.getNumJoys(); i++)
                {
                    joyAxisMax[i] = mainDisplay.joyAxisMax[i];
                }
        }

        public void physicsTick(double physicsCallFreq)
        {
            updateValues();

            if (soundInit && oldRpm != rpm)
            {
                soundInstance.Pause();
                soundInstance.Pitch = (float)((double)rpm / 6500.0);
                soundInstance.Volume = (float)(0.5 * (90 + (double)velocity) / 180.0);
                soundInstance.Play();
            }

            if (reverseGear == 0 && driveGear == 0)
            {
                neutral = true;
                dataReadout.Gear.Text = "N";
            }
            if (driveGear != 0)
            {
                if (selectedGear == 1 && throttle < 0.1 && velocity < 2.2 && brakeValue < 0.1)
                {
                    throttle = 0.5;
                    driveGear = 0;
                }
                else if ((selectedGear == 0 || selectedGear == 1) && rpm <= 1100 && neutral == true) // SIMPLIFY LOGIC *ToDo
                {
                    selectedGear = 1;
                    neutral = false;
                    driveGear = 0;
                }
                else if (selectedGear > 0 && selectedGear < 8)
                {
                    autoShift();
                    driveGear = 0;
                }
            }
            if ((reverseGear != 0 && rpm <= 1100 && selectedGear == 1 && neutral == true))
            {
                neutral = false;
                selectedGear = 0;
                reverseGear = 0;
                dataReadout.Gear.Text = "R";
            }
            double vel = calcVelocity (physicsCallFreq);

            if (Math.Abs(velocity) < 0.01 || (brakeValue > 0.1 && Math.Abs(velocity) < 0.5))
            {
                mainDisplay.velocity = 0;
            }
            else
            {
                mainDisplay.velocity = vel;
            }
        }

        private double calcVelocity(double physicsCallFreq)
        {
            velocity += ((0.001) * physicsCallFreq) * (calcAcceleration(rpm));
            return velocity;
        }

        private double calcAcceleration(int rpm)
        {
            double force = (calcTraction(rpm) + calcDrag()) / carMass;
            dataReadout.Force.Text = force.ToString();
            return force;
        }

        private double calcTraction(int rpm)
        {
            double traction = ((calcEngineTorque(rpm) * gearRatio[selectedGear] * gearRatio[7] * transmissionEfficiency) / wheelRadius);
            dataReadout.Traction.Text = traction.ToString();
            return traction;
        }

        private double calcDrag()
        {
            int sign = Math.Sign(velocity);
            double drag = (sign * -1 * (brakeValue + ((DRAG * Math.Abs(velocity) + ROLLING_DRAG) * Math.Abs(velocity))));
            dataReadout.Drag.Text = drag.ToString();
            return drag;
        }

        private double lookupTorque(int rpm)
        {
            if (neutral == true)
            {
                return 0;
            }
            double index = (22.00 * rpm) / 6500.00; // make max rpm and max index interface *ToDo
            if (index <= 22 && index > 0)
            {
                if ((Math.Round(index) - (index)) != 0)
                {
                    int upperIndex = (int)Math.Ceiling(index);
                    int lowerIndex = (int)Math.Floor(index);
                    return (torqueCurve[lowerIndex] + (torqueCurve[upperIndex] - torqueCurve[lowerIndex]) * (index - lowerIndex) / (upperIndex - lowerIndex));
                }
                else
                {
                    return torqueCurve[(int)index];
                }
            }
            else
                return 0;
        }

        private double calcEngineTorque(int rpm)
        {
            double maxTorque = lookupTorque(rpm);
            return (throttle * maxTorque);
        }

        private int calcRPM()
        {
                int rpm = (int)((velocity / wheelRadius) * gearRatio[selectedGear] * gearRatio[7] * (60 / (2 * Math.PI)));
                if (rpm < 1000)
                {
                    rpm = 1000;
                }
                dataReadout.RPM.Text = rpm.ToString();
                mainDisplay.rpm = rpm;
                return rpm;
        }

        private void autoShift() //FINISH SHIFT SCHEDULE *ToDo
        {
            if (throttle > 0.2 && rpm > 4500 && selectedGear < 4 && selectedGear > 0)
            {
                selectedGear++;
            }
            else if (throttle > 0.2 && rpm > 3000 && selectedGear < 7 && selectedGear > 3)
            {
                selectedGear++;
            }
            else if (throttle <= 0.2 && rpm < 1900 && selectedGear > 1)
            {
                selectedGear--;
            }

            dataReadout.Gear.Text = selectedGear.ToString();
        }

        private void updateValues()
        {
            reverseGear = mainDisplay.reverseGear;
            driveGear = mainDisplay.driveGear;
            isAccel = mainDisplay.isAccel;
            isBrake = mainDisplay.isBrake;
            velocity = mainDisplay.velocity;
            accelValue = mainDisplay.accelValue;
            brakeValue = mainDisplay.brakeValue;

            throttle = accelValue / joyAxisMax[pedalJoy];
            oldRpm = rpm;
            rpm = calcRPM();
        }

        public void stopSound()
        {
            soundInstance.Stop();
        }
    }
}
