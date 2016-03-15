/*
 * This file is part of
 *
 * Copyright 2014-2016 University of Michigan NeuRRo Lab. All Rights Reserved.
 * <http://www.neurro-lab.engin.umich.edu/>
 *
 * is free software: you may redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published
 * by the Free Software Foundation, version 2 of the License
 *
 * is distributed in the hope that it will be useful but
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
using System.Drawing; // drawing and painting
using System.Windows.Forms; // form
using System.Diagnostics; // to use stopwatch
using Microsoft.DirectX.DirectInput;
using System.Runtime.InteropServices;


namespace CarPerformanceSimulator
{
    public partial class Display : Form
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        private UserDisplay UserDisplay; //LOOKS LIKE OTHER CLASSES OR STRUCTS COULD SIMPLIFY *ToDo

        private Device[] joystick; //Joystick
        private JoystickInterface joystickInterface; //The Joystick Interface. Handles aquiring and polling of Joystick
        private JoystickState[] joystickState; //The current Joystick state returned from the interface
        private KeyboardInterface keyboardInterface;
        private Key[] pressedKeys;
        internal int[] joyAxisMax; //Maximum joystick axis value
        internal int[] joyAxisMin; //Minimum joystick axis value

        private static readonly int AXIS_MAX = 65535;

        private readonly double METRIC_CONVERSION = 2.236936;
        enum axisSelection { X = 0, Y = 1, Z = 2, Rx = 3, Ry = 4, Rz = 5 };

        private string drivingAxis;
        private string accelAxis;
        private string brakeAxis;
        private int drive;
        private int reverse;

        internal int reverseGear;
        internal int driveGear;

        private double Car_x; // User Car X coordinate
        private double Car_y; // User Car Y coordinate
        private double leadCar_X;
        private double leadCar_y; //Lead Car Y coordinate

        private double centerLinesY; // Lane line 1 y coordinate
        private double stationaryObjectYTotal;
        private double stationaryObjectYRTstart;
        private double stationaryObjectY;
        private int walkX;

        public int rTriggerMax;
        public int rTriggerMin;

        private Stopwatch stopwatch = new Stopwatch(); //Stopwatch
        private Stopwatch trialTime = new Stopwatch();
        bool isHighPrecision = Stopwatch.IsHighResolution; //ADD CONTROL OVER POLLING TIME AND DO LATENCY CALUCLATIONS

        private bool RTtest; // Reaction Time Test start controller
        private double gasMomentReleaseTime, gasTotalReleaseTime, gasMomentReleaseToBrakeTouchTime, gasTotalReleaseToBrakeTouchTime, trialStartToBrakeTime, wheelTurnTime, totalStopTime; //


        public bool cognitiveLoadTest;

        public bool[] cogLoad;

        private Bitmap carBrakeBitmap;

        internal double acceleration = 0; //Current accleration
        internal double velocity = 0; //Current velocity
        internal int rpm = 0;

        internal double brakeValue = 0; //Brake magnitude
        internal double brakeMax = 20000; //Brake Maximum
        internal int accelValue; //Accelerator magnitude

        private double degree = 0; // Steering wheel position from 0 to 180
        private double degreeR = 0.0; //Steering wheel position converted to radians
        internal int steeringSensitivity;

        internal double transmissionEfficiency;
        internal System.Action cogLoadAction;


        private enum inputSelection { joystick = 0, keyboard = 1 };
        private int selectedInput;

        internal bool isAccel = false; //Is the acclerator engaged
        internal bool isBrake = false; //Is brake engaged
        private bool rtStartGas = false; //Is accelerator engaged at start of reaction test
        private bool rtStartBrake = false; //Is brake engaged at start of reaction test
        private double rtStartSpeed; //Speed when reaction time test was started
        private bool rtWheelPos;
        private double rtStartWheelPos;
        private double leadCarStartSpeed;
        private double rtStartAccelPosition;
        internal int xTrials;
        internal int xSpeed;

        private enum cogLoadActionsEnum { left, right, brake, brakeAndLeft, brakeAndRight };
        private System.Action[] cogLoadActions;

        bool RTstart = false;
        bool RTgas = false;
        bool RTbrake = false;

        private int cogLoadIndex;

        int randomNum;
        bool trigger;

        int currentTrial;

        internal int triggerType;
        int collision;

        bool initialized = false;

        bool driveStart = false;
        bool validSpeed;
        private int object_iD;
        int xScale;

        private int pedalJoy;
        private int wheelJoy;

        private double oldHeight;
        private double oldWidth;
        string fileName;
        string fileNameRaw;
        string fileNameDumpPerTrial;
        int trialCount;
        bool filePathSet;
        int FPS;
        double frameTime;
        float speedRotate;
        float rpmRotate;

        private FormWindowState LastWindowState;
        internal Physics physicsEngine;

        bool tick;

        Bitmap tachometer;
        Bitmap speedometer;
        Bitmap car;
        Bitmap deer;
        Bitmap pedestrian;
        Bitmap stopSign;
        Bitmap leadCar;

        Pen pen;
        int lineCenter;
        public int msPerUpdate = 16;

        Rectangle path;
        private enum choices : int { moveLeft, moveRight, brake, moveLeftAndBrake, moveRightAndBrake };

        private int objectScale; //Hardcode ALL values to avoid calculation overhead *ToDo

        double previous;
        double lag;

        public Display()
        {
            InitializeComponent();
            filePathSet = false;
            LastWindowState = WindowState;
            oldWidth = this.Size.Width;
            oldHeight = this.Size.Height;
            this.DoubleBuffered = true;
            degree = 0;
            degreeR = (degree * Math.PI) / 180;
            reverse = 0;
            driveGear = 0;
            accelValue = 0;
            brakeValue = 0;
            triggerType = 0;
            object_iD = 0;
            rTriggerMax = 10;
            rTriggerMin = 2;
            cognitiveLoadTest = false;
            carBrakeBitmap = new Bitmap("frontCar_Braking.png"); ;

            cogLoadIndex = -1;

            cogLoadActions = new System.Action[5] { cogLeft, cogRight, cogBrake, cogBrakeAndLeft, cogBrakeAndRight };

            cogLoad = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                cogLoad[i] = false;
            }

            transmissionEfficiency = 0.75;
            steeringSensitivity = 50;

            currentTrial = -1;

            setInputSource((int)inputSelection.keyboard); //Set Keyboard as input

            trigger = false;
            randomNum = -1;

            trialCount = 0;

            UserDisplay = new UserDisplay(this);
            UserDisplay.Show();

            if (!isHighPrecision)
                UserDisplay.HighPrecision.Text = "WARNING: Stopwatch is not high resolution";
            else
            {
                long frequency = Stopwatch.Frequency;
                long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
                UserDisplay.HighPrecision.Text = "  Timer reported " + nanosecPerTick + " ns";
            }

            Restart();

            physicsEngine = new Physics(this, UserDisplay);


            if (UserDisplay.saveData.ShowDialog() == DialogResult.Cancel)
            {
                Environment.Exit(0);
                return;
            }
            //fileName = DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DateTime.Today.Year + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "Trial.txt";
            System.IO.File.WriteAllText(fileName, "Trail started at " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + " on " + DateTime.Today.Month + "/" + DateTime.Today.Day + "/" + DateTime.Today.Year + "\r\n" + UserDisplay.HighPrecision.Text + "\r\n");
            //fileNameRaw = DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DateTime.Today.Year + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "Trial_RAW.txt";
            System.IO.File.WriteAllText(fileNameRaw, "Trial\tType\tStartSpeed\tGasMRel\tGasTRel\tGasMBrake\tGasTBrake\tTrialTBrake\tTotalStop\tWheelTurn\tError\t\r\n");
            System.IO.File.WriteAllText(fileNameDumpPerTrial, "Thrtl\tBrke\tWjlDeg\tDGear\tRGear\tVel\tLaneDev\tRT\tCol\tTime\r\n");
            filePathSet = true;

            speedometer = new Bitmap("SpeedometerBlankScaled.png");
            tachometer = new Bitmap("TachometerBlankScaled.png");
            stopSign = new Bitmap("stop_sign.jpg");
            deer = new Bitmap("deer.jpg");
            pedestrian = new Bitmap("pedestrian.jpg");
            car = new Bitmap("Car.png");
            leadCar = new Bitmap("frontCar.png");
            Brush brush = new SolidBrush(Color.Green);
            pen = new Pen(brush, 5);

            initialized = true;
            trialTime.Start();
            tick = true;
            Application.Idle += HandleApplicationIdle;
        }

        void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (isApplicationIdle())
            {
                double current = trialTime.ElapsedMilliseconds;
                double elapsed = current - previous;
                previous = current;
                lag += elapsed;

                if (triggerType == 1 && velocity >= xSpeed && !validSpeed)
                {
                    validSpeed = true;
                    Console.WriteLine("validSpeed");
                }

                if (randomNum > 0 && !trigger && validSpeed)
                {
                    randomNum -= (int)elapsed;
                    if (randomNum <= 0)
                    {
                        trigger = true;
                        RT_btn_Click();
                    }
                }

                while (lag >= msPerUpdate)//set MS PER UPDATE
                {
                    pollingTick();
                    data_tmr_Tick();
                    if (tick)
                    {
                        physicsEngine.physicsTick(msPerUpdate);
                        updateMovement();
                    }
                    if (driveStart)
                        saveSnapshot();
                    lag -= msPerUpdate;
                }

                if (tick && current - frameTime >= 1)
                {
                    Update();
                    frameTime = current;
                    FPS++;
                }
            }
        }

        bool isApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        // paint function ( all the figures that are not in the design page
        private void Display_Paint(object sender, PaintEventArgs e)
        {
            int spacing = (int)((double)Size.Height * 0.20);
            int size = (int)((double)Size.Height * 0.10);
            lineCenter = (int)((double)Size.Width * 0.45);

            e.Graphics.FillRectangle(Brushes.Gray, LpathBound.Location.X, 0, RpathBound.Location.X - LpathBound.Location.X, Size.Height);
            e.Graphics.DrawImage(speedometer, RpathBound.Location.X + 2 * RpathBound.Size.Width, this.Height - 275, 300, 250);
            e.Graphics.DrawImage(tachometer, LpathBound.Location.X - LpathBound.Size.Width - 300, this.Height - 275, 300, 250);

            speedRotate += (float)((((251 * Math.Abs(velocity) / 54 - 50) * Math.PI) / 180.0) - speedRotate);
            rpmRotate += (float)((((137 * rpm / 6500 - 30) * Math.PI) / 180.0) - rpmRotate);

            Point speedOrigin = new Point(RpathBound.Location.X + 2 * RpathBound.Size.Width + 150, this.Height - 150);
            Point speedEnd = new Point((int)(speedOrigin.X - Math.Cos(-speedRotate) * 125), (int)(speedOrigin.Y + Math.Sin(-speedRotate) * 125));
            e.Graphics.DrawLine(pen, speedOrigin, speedEnd);

            Point rpmOrigin = new Point(LpathBound.Location.X - LpathBound.Size.Width - 150, this.Height - 150);
            Point rpmEnd = new Point((int)(rpmOrigin.X - Math.Cos(-rpmRotate) * 125), (int)(rpmOrigin.Y + Math.Sin(-rpmRotate) * 125));
            e.Graphics.DrawLine(pen, rpmOrigin, rpmEnd);

            if (RTstart)
            {
                if (object_iD == 1)
                    e.Graphics.DrawImage(stopSign, RpathBound.Location.X + 11, (float)(stationaryObjectY - objectScale), objectScale / 2, objectScale / 2); // The stop sign
                else if (object_iD == 2)
                    e.Graphics.DrawImage(deer, walkX, (float)(stationaryObjectY - objectScale), xScale, objectScale); // The deer
                else if (object_iD == 3)
                    e.Graphics.DrawImage(pedestrian, walkX, (float)(stationaryObjectY - objectScale), xScale, objectScale); // The pedestrian
            }


            for (int i = -15; i <= Size.Height * .15; i++)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, lineCenter, (float)centerLinesY + (i * spacing), 6, size); // the moving lines
            }
            e.Graphics.DrawImage(car, (float)Car_x, (float)Car_y, xScale, objectScale); // The user car
            if (object_iD == 0)
            {
                e.Graphics.DrawImage(leadCar, (float)leadCar_X, (float)leadCar_y, xScale, objectScale); // The lead car
            }

        }

        //Handles trial restarts
        public void Restart()
        {
            tick = false;

            leadCar = new Bitmap("frontCar.png");

            velocity = 0;
            RTgas = false;
            RTbrake = false;
            RTtest = false;
            RTstart = false;
            rtStartGas = false;
            rtStartBrake = false;

            cogLoadIndex = -1;

            accelValue = 0;
            collision = 0;
            gasMomentReleaseTime = 0;
            gasMomentReleaseToBrakeTouchTime = 0;
            gasTotalReleaseTime = 0;
            gasTotalReleaseToBrakeTouchTime = 0;
            trialStartToBrakeTime = 0;
            wheelTurnTime = 0;
            totalStopTime = 0;
            objectScale = (int)((double)this.Size.Width * 0.08);
            rtStartAccelPosition = 0;
            rtStartWheelPos = 0;
            xScale = objectScale / 2;

            validSpeed = false;

            stationaryObjectYTotal = 0;
            stationaryObjectYRTstart = 0;
            stationaryObjectY = 0;

            //if (object_iD == 0) //&& !cognitiveLoadTest)
            // {
            Car_x = (int)this.Size.Width - (int)(this.Size.Width * .5);
            Car_y = (int)this.Size.Height - (int)(this.Size.Height * .25);
            leadCar_X = Car_x;
            leadCar_y = (int)this.Size.Height - (int)(this.Size.Height * .95);
            centerLinesY = (int)this.Size.Height - (int)(this.Size.Height * .3); // initial y_position
            //}
            //else
            //{
            //    Car_x = (int)(lineCenter) - (int)(0.5 * xScale);
            //    Car_y = (int)this.Size.Height - (int)(this.Size.Height * .25);
            //    leadCar_X = Car_x;
            //    leadCar_y = (int)this.Size.Height - (int)(this.Size.Height * .95);
            //    centerLinesY = (int)this.Size.Height - (int)(this.Size.Height * .3);
            //}
            if (RTtest)
            {
                stopwatch.Stop();
                totalStopTime = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                RTtest = false;
            }
            else
            {
                UserDisplay.TrialBox.Text = "Trial " + trialCount;
            }

            if (initialized)
            {
                tick = true;
            }

            Invalidate();

        }

        // controls the car's speed, by updating location every certain amount of millisecond
        private void updateMovement()
        {

            if (velocity > 0 && !driveStart)
            {
                driveStart = true;
            }

            // speed control
            stationaryObjectYTotal += velocity;
            centerLinesY += velocity;

            double x = Car_x;
            x += velocity * Math.Sin(degreeR);

            if (x + xScale < RpathBound.Location.X + RpathBound.Size.Width / 2 && x > LpathBound.Location.X + LpathBound.Size.Width)
            {
                Car_x += velocity * Math.Sin(degreeR);
            }
            else if (!cognitiveLoadTest)
            {
                collision = 2;
            }

            // Makes lane lines return to page
            if (centerLinesY > this.Size.Height / 2) centerLinesY -= 2 * this.Size.Height;
            if (centerLinesY < this.Size.Height / -2) centerLinesY += 2 * this.Size.Height;

            if (object_iD == 0)
            {
                checkForCollisionWithLeadCar();
            }

            if (RTstart)
            {
                switch (object_iD)
                {
                    case 0:
                        if (!cognitiveLoadTest)
                        {
                            leadCarBrake();
                        }
                        else //if (leadCar_X + xScale < RpathBound.Location.X && leadCar_X > LpathBound.Location.X + LpathBound.Size.Width)
                        {
                            cogLoadAction();
                        }
                        break;
                    case 1:
                        {
                            if (Car_y < stationaryObjectY - objectScale)
                            {
                                collision = 1;
                            }
                            stationaryObjectY = stationaryObjectYTotal - stationaryObjectYRTstart;
                            break;
                        }
                    case 2:
                    case 3:
                        {
                            checkForCollisionWithObject();
                            stationaryObjectY = stationaryObjectYTotal - stationaryObjectYRTstart;
                            walkX -= 2;
                            break;
                        }
                }
                if(collision != 0)
                {
                    endTrial();
                }
            }
            else if (collision != 0)
            {
                tick = false;
                physicsEngine.stopSound();
            }

            if (RTtest == true)
            {
                RT_tmr_Tick();
            }

            Rectangle path = new Rectangle(LpathBound.Location.X - xScale, 0, RpathBound.Location.X + RpathBound.Size.Width + objectScale - LpathBound.Location.X, this.Height);
            Invalidate(path);
            Rectangle refreshDisplay = new Rectangle(0, this.Height - 325, this.Width, this.Height - 325);
            Invalidate(refreshDisplay);


        }

        private void checkForCollisionWithLeadCar()
        {
            if (Car_x < leadCar_X + xScale &&
                    Car_x + xScale > leadCar_X &&
                    Car_y < leadCar_y + objectScale &&
                    Car_y + objectScale > leadCar_y)
            {
                collision = 1;
            }
        }

        private void checkForCollisionWithObject()
        {
            if (Car_x < walkX + xScale &&
                            Car_x + xScale > walkX &&
                            Car_y < stationaryObjectY &&
                            Car_y + objectScale > stationaryObjectY - objectScale)
            {
                collision = 1;
            }
        }

        //causes lead car to brake
        private void leadCarBrake()
        {
            leadCarStartSpeed *= 0.99;
            leadCar_y += (int)(velocity - leadCarStartSpeed);
        }

        // Reaction time button control (temp)
        public void RT_btn_Click()
        {
            if (triggerType == 1 && currentTrial < 0)
            {
                currentTrial = xTrials - 1;
                UserDisplay.progressBar.Value = 0;
            }
            if (!RTtest)
            {
                if (triggerType == 1 && !trigger)
                {
                    Random numGen = new Random();
                    randomNum = numGen.Next(rTriggerMin * 1000, rTriggerMax * 1000);
                }
                if (triggerType == 0 || trigger)
                {
                    RTtest = !RTtest;
                    rtStartSpeed = velocity;
                    rtStartWheelPos = degree;
                    leadCarStartSpeed = rtStartSpeed;
                    stationaryObjectYRTstart = stationaryObjectYTotal;
                    walkX = RpathBound.Location.X;

                    if (isAccel)
                    {
                        rtStartGas = true;
                        rtStartAccelPosition = accelValue;
                    }
                    else
                    {
                        gasMomentReleaseTime = -1;
                    }
                    if (isBrake)
                    {
                        rtStartBrake = true;
                        gasMomentReleaseToBrakeTouchTime = -1;
                        gasTotalReleaseToBrakeTouchTime = -1;
                        trialStartToBrakeTime = -1;
                    }
                    if (cognitiveLoadTest)
                    {
                        int i = 0,j = 0,t = 0;
                        foreach (bool b in cogLoad)
                        {
                            if (b)
                            {
                                i++;
                                j = t;
                            }
                            t++;
                        }
                        if (i == 1)
                        {
                            cogLoadAction = cogLoadActions[j];
                        }
                        else if (i > 1)
                        {
                            bool randomChoice = false;
                            Random ran = new Random();
                            cogLoadIndex = ran.Next() % t;
                            while (!randomChoice)
                            {
                                if (cogLoad[cogLoadIndex])
                                {
                                    cogLoadAction = cogLoadActions[cogLoadIndex];
                                    randomChoice = true;
                                }
                                cogLoadIndex = cogLoadIndex + 1 % t;
                            }
                        }
                        else
                        {
                            throw new Exception();                            //exception
                        }
                        if (((cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndLeft] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.left]) && (degree > -2))
                         || ((cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.right] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndRight]) && (degree < 2)))
                        {
                            rtWheelPos = true;
                        }
                        else
                        {
                            rtWheelPos = false;
                        }
                    }

                    RTstart = true;
                    stopwatch.Start();
                }
            }
        }

        // reaction time timer (set start)
        private void RT_tmr_Tick()
        {
            //bool brake;
            if (!cognitiveLoadTest || (cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brake] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndLeft] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndRight]))
            {
                leadCarBrakeLights();

                if (rtStartGas && !RTgas)
                {
                    if(gasMomentReleaseTime == 0)
                    {
                        checkIfGasMomentTimersTriggered();
                    }
                    if(gasTotalReleaseTime == 0)
                    {
                        checkIfGasTotalTimersTriggered();
                    }
                }

                if (isBrake && !RTbrake && !rtStartBrake)
                {
                    if (gasMomentReleaseToBrakeTouchTime == 0)
                    {
                        checkIfGasMomentToBrakeTimersTriggered();
                    }
                    if (gasTotalReleaseToBrakeTouchTime == 0)
                    {
                        checkIfGasTotalToBrakeTimersTriggered();
                    }
                    if (trialStartToBrakeTime == 0)
                    {
                        checkIfTrialToBrakeTimersTriggered();
                    }
                }
            }

            if (cognitiveLoadTest && cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.brake] && wheelTurnTime == 0)
            {
                checkIfWheelTurnTimerTriggered();
            }

            if (velocity == 0 || (cognitiveLoadTest && (cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.left] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.right]) && (wheelTurnTime != 0)))
            {
                endTrial();
            }
        }

        private void leadCarBrakeLights()
        {
            if (leadCar != carBrakeBitmap)
            {
                leadCar = carBrakeBitmap;
            }
        }

        private void checkIfGasMomentTimersTriggered()
        {
            if (gasMomentReleaseTime == 0 && (rtStartAccelPosition - accelValue) > ((double)joyAxisMax[pedalJoy] * 0.1))
            {
                gasMomentReleaseTime = stopwatch.ElapsedMilliseconds;
            }
        }

        private void checkIfGasTotalTimersTriggered()
        {
            if (gasTotalReleaseTime == 0 && !isAccel)
            {
                gasTotalReleaseTime = stopwatch.ElapsedMilliseconds;
                RTgas = true;
            }
        }

        private void checkIfGasMomentToBrakeTimersTriggered()
        {
            if (gasMomentReleaseTime > 0)
            {
                gasMomentReleaseToBrakeTouchTime = stopwatch.ElapsedMilliseconds - gasMomentReleaseTime;
            }
            else
            {
                gasMomentReleaseToBrakeTouchTime = -1;
            }
        }

        private void checkIfGasTotalToBrakeTimersTriggered()
        {
            if (gasTotalReleaseTime > 0)
            {
                gasTotalReleaseToBrakeTouchTime = stopwatch.ElapsedMilliseconds - gasTotalReleaseTime;
            }
            else
            {
                gasTotalReleaseToBrakeTouchTime = -1;
            }
            RTbrake = true;
        }

        private void checkIfTrialToBrakeTimersTriggered()
        {
            if (trialStartToBrakeTime == 0)
            {
                trialStartToBrakeTime = stopwatch.ElapsedMilliseconds;
            }
            rtStartBrake = true;
        }

        private void checkIfWheelTurnTimerTriggered()
        {
            if (rtWheelPos)
            {
                if (((cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndLeft] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.left]) && (Math.Abs(degree) - Math.Abs(rtStartWheelPos) > 3))
                 || ((cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.right] || cogLoadAction == cogLoadActions[(int)cogLoadActionsEnum.brakeAndRight]) && (degree - rtStartWheelPos > 3)))
                {
                    wheelTurnTime = stopwatch.ElapsedMilliseconds;
                }
            }
            else
            {
                wheelTurnTime = -1;
            }
        }

        private void endOfTrialInvalidateUntriggeredValues()
        {
            if (rtStartGas && !RTgas && (!cognitiveLoadTest || (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.left] && cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.right])))
            {
                gasMomentReleaseTime = -1;
                gasTotalReleaseTime = -1;
            }

            if (!rtStartBrake && !RTbrake && (!cognitiveLoadTest || (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.left] && cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.right])))
            {
                gasMomentReleaseToBrakeTouchTime = -1;
                gasTotalReleaseToBrakeTouchTime = -1;
            }

            if (cognitiveLoadTest && cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.brake] && (wheelTurnTime <= 0 || !rtWheelPos))
            {
                wheelTurnTime = -1;
                collision = 3;
            }
        }

        private void endOfTrialWrite()
        {
            if (filePathSet == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                {
                    string output;
                    if (!cognitiveLoadTest)
                    {
                        output = "Trial " + trialCount +
                           ": \r\n\tType: " + object_iD +
                           "\r\n\tStart Speed: " + Math.Round(rtStartSpeed, 1) +
                           " Meters/Sec\r\n\tTrial start to gas moment release time: " + gasMomentReleaseTime +
                           "ms\r\n\tTrial start to gas total release time: " + gasTotalReleaseTime +
                           "ms\r\n\tGas moment release to brake time: " + gasMomentReleaseToBrakeTouchTime +
                           "ms\r\n\tGas total release to brake time: " + gasTotalReleaseToBrakeTouchTime +
                           "ms\r\n\tTrial start to brake time: " + trialStartToBrakeTime +
                           "ms\r\n\tTotal stop time: " + totalStopTime +
                           "ms\r\n\tCollision Detected: " + collision + "\r\n";
                    }
                    else
                    {
                        output = output = "Trial " + trialCount +
                            ": \r\n\tType: " + cogLoadIndex +
                            "\r\n\tStart Speed: " + Math.Round(rtStartSpeed, 1) + " Meters/Sec";
                        if (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.left] || cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.right])
                        {
                            output += "\r\n\tTrial start to gas moment release time: " + gasMomentReleaseTime +
                           "\r\n\tTrial start to gas total release time: " + gasTotalReleaseTime +
                           "ms\r\n\tGas moment release to brake time: " + gasMomentReleaseToBrakeTouchTime +
                           "ms\r\n\tGas total release to brake time: " + gasTotalReleaseToBrakeTouchTime + 
                           "ms\r\n\tTrial start to brake time: " + trialStartToBrakeTime +
                           "ms\r\n\t" + "Total stop time: " + totalStopTime;
                        }
                        if (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.brake])
                        {
                            output += "\r\n\tTWheel turn time: " + wheelTurnTime + "ms\r\n\t";
                        }
                        output += "ms\r\n\tErrors: " + collision + "\r\n";
                    }
                    file.WriteLine(output);
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameRaw, true))
                {
                    string output;
                    if (!cognitiveLoadTest)
                    {
                        output = trialCount + "\t" + object_iD + "\t" + Math.Round(rtStartSpeed, 1) +
                        "\t" + gasMomentReleaseTime + "\t" + gasTotalReleaseTime + "\t" + gasMomentReleaseToBrakeTouchTime + "\t" + gasTotalReleaseToBrakeTouchTime + "\t" + trialStartToBrakeTime +"\t" + totalStopTime + "\t" + "-2" + "\t" + collision;
                    }
                    else
                    {
                        output = trialCount + "\t" + object_iD;
                        output += " CL-" + cogLoadIndex;
                        output += "\t" + Math.Round(rtStartSpeed, 1) + "\t";
                        if (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.right] && cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.left])
                        {
                            output += gasMomentReleaseTime + "\t" + gasTotalReleaseTime + "\t" + gasMomentReleaseToBrakeTouchTime + "\t" + gasTotalReleaseToBrakeTouchTime + "\t" + trialStartToBrakeTime + "\t" + totalStopTime;
                        }
                        else
                        {
                            output += "-2" + "\t" + "-2" + "\t" + "-2" + "\t" + "-2" + "\t" + "-2" + "\t" + "-2";
                        }
                        if (cogLoadAction != cogLoadActions[(int)cogLoadActionsEnum.brake])
                        {
                            output += "\t" + wheelTurnTime;
                        }
                        else
                        {
                            output += "\t" + "-2";
                        }

                        output += "\t" + collision;
                    }
                    file.WriteLine(output);
                }
            }
            trialCount++;
            fileNameDumpPerTrial = fileName.Insert(fileName.Length - 4, "_" + trialCount + "TrialDump");
            System.IO.File.WriteAllText(fileNameDumpPerTrial, "Thrtl\tBrke\tWjlDeg\tDGear\tRGear\tVel\tLaneDev\tRT\tCol\tTime\r\n");
        }

        private void trialSeriesResetOrTerminate()
        {
            if (triggerType == 1 && currentTrial > 0)
            {
                UserDisplay.progressBar.Increment(1);
                currentTrial--;
                Restart();
                RT_btn_Click();
            }
            else
            {
                UserDisplay.progressBar.Increment(1);
                currentTrial = -1;
            }
        }

        private void endTrial()
        {
            endOfTrialInvalidateUntriggeredValues();

            //stop timers, reset, and stop sound
            stopwatch.Stop();
            totalStopTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            RTtest = false;
            tick = false;
            physicsEngine.stopSound();

            endOfTrialWrite();

            trigger = false;

            trialSeriesResetOrTerminate();
        }

        private void pollingTick()
        {
            if (selectedInput == (int)inputSelection.keyboard) //FIX *ToDo
            {
                pressedKeys = keyboardInterface.poll();

                bool[] pressed = new bool[3];
                for (int i = 0; i < 3; i++)
                {
                    pressed[i] = false;
                }

                foreach (Key key in pressedKeys)
                {
                    if (key.Equals(Key.R))
                    {
                        driveGear = 0;
                        reverseGear = 1;
                        pressed[0] = true;
                        continue;
                    }
                    else if (key.Equals(Key.D))
                    {
                        driveGear = 1;
                        reverseGear = 0;
                        pressed[0] = true;
                        continue;
                    }

                    if ((key.Equals(Key.DownArrow) || key.Equals(Key.Down)) && accelValue < AXIS_MAX + 500)
                    {
                        accelValue -= 500;
                        continue;
                    }
                    else if ((key.Equals(Key.UpArrow) || key.Equals(Key.Up)) && velocity < 120 && accelValue < AXIS_MAX - 500)
                    {
                        accelValue += 500;
                        continue;
                    }

                    if (key.Equals(Key.Space))
                    {
                        accelValue = 0;
                        brakeValue = brakeMax;
                        pressed[1] = true;
                        continue;
                    }

                    if (key.Equals(Key.LeftArrow) || key.Equals(Key.Left))
                    {
                        degree = -10 * ((double)steeringSensitivity / 50.0);
                        pressed[2] = true;
                        continue;
                    }
                    else if (key.Equals(Key.RightArrow) || key.Equals(Key.Right))
                    {
                        degree = 10 * ((double)steeringSensitivity / 50.0);
                        pressed[2] = true;
                        continue;
                    }
                }
                if (!pressed[0])
                {
                    driveGear = 0;
                    reverseGear = 0;
                }
                if (!pressed[1])
                {
                    brakeValue = 0;
                }
                if (!pressed[2])
                {
                    degree = 0;
                }

            }
            else if (selectedInput == (int)inputSelection.joystick)
            {
                joystickInterface.poll(wheelJoy);
                joystickInterface.poll(pedalJoy);

                joystickState[wheelJoy] = joystickInterface.getState(wheelJoy);
                joystickState[pedalJoy] = joystickInterface.getState(pedalJoy);

                degree = ((double)steeringSensitivity / 50.0) * (((((int)joystickState[wheelJoy].GetType().GetProperty(drivingAxis).GetValue(joystickState[wheelJoy], null) - (joyAxisMax[wheelJoy] / 2)) * 60.0)) / joyAxisMax[wheelJoy]);//SO MUCH OVERHEAD, EXPLORE ALTERNATIVES *ToDo
                byte[] buttons = joystickState[wheelJoy].GetButtons();
                reverseGear = buttons[reverse];
                driveGear = buttons[drive]; //IMPLEMENT BUTTON HOLD CODE *ToDo

                accelValue = (joyAxisMax[pedalJoy] - (int)joystickState[pedalJoy].GetType().GetProperty(accelAxis).GetValue(joystickState[pedalJoy], null));


                brakeValue = ((brakeMax * (joyAxisMax[pedalJoy] - (int)joystickState[pedalJoy].GetType().GetProperty(brakeAxis).GetValue(joystickState[pedalJoy], null))) / joyAxisMax[pedalJoy]);

            }

            degreeR = (degree * Math.PI) / 180.0;
            if (accelValue > 0)
            {
                isAccel = true;
            }
            else
            {
                isAccel = false;
            }
            if (brakeValue > 0)
            {
                isBrake = true;
            }
            else
                isBrake = false;
        }

        // Collect and show data periodically
        private void data_tmr_Tick()
        {
            speed_lb.Text = Convert.ToString(METRIC_CONVERSION * velocity);
            UserDisplay.speed_lb.Text = Convert.ToString(METRIC_CONVERSION * velocity);
            UserDisplay.acceleration_lb.Text = Convert.ToString(acceleration);
            UserDisplay.angle_lb.Text = Convert.ToString(degree);
            UserDisplay.x_lb.Text = Convert.ToString(Car_x);

            // show statstics of direction (up or down)
            if (velocity >= 0)
            {
                UserDisplay.direction_lb.Text = "Forward";
            }
            else
            {
                UserDisplay.direction_lb.Text = "Backward";
            }

            UserDisplay.break_lb.Text = Convert.ToString(brakeValue);
            UserDisplay.isBreak_lb.Text = Convert.ToString(isBrake);

            if (RTtest)
            {
                UserDisplay.RT_lb1.Text = "Yes";
                UserDisplay.RT_lb2.Text = "Yes";
                UserDisplay.RT_lb.Text = "Test Is Running";
            }
            else
            {
                UserDisplay.RT_lb1.Text = "No";
                UserDisplay.RT_lb2.Text = "No";
                UserDisplay.RT_lb.Text = "Start Reaction Test";
            }
            if (RTgas)
            {
                UserDisplay.gas_lb1.Text = "Yes";
                UserDisplay.gas_lb2.Text = Convert.ToString(gasMomentReleaseTime);
            }
            else
            {
                UserDisplay.gas_lb1.Text = "No";
                if (RTtest)
                {
                    UserDisplay.gas_lb2.Text = "Testing";
                }
            }
            if (RTbrake)
            {
                UserDisplay.g2b_lb1.Text = "Yes";
                UserDisplay.g2b_lb2.Text = Convert.ToString(gasMomentReleaseToBrakeTouchTime);
            }
            else
            {
                UserDisplay.g2b_lb1.Text = "No";
                if (RTtest)
                {
                    UserDisplay.g2b_lb2.Text = "Testing";
                }
            }

            if ((int)(velocity) == 0)
            {
                UserDisplay.stop_lb1.Text = "Yes";
                UserDisplay.stop_lb2.Text = Convert.ToString(totalStopTime);
            }
            else
            {
                UserDisplay.stop_lb1.Text = "No";
                UserDisplay.stop_lb2.Text = "Moving";
            }
        }

        //Resizing
        private void FormView_ResizeBegin(object sender, System.EventArgs e)
        {
            oldWidth = this.Size.Width;
            oldHeight = this.Size.Height;

            Console.Write("Resize Begin\n"); //debug
        }

        private void FormView_ResizeEnd(object sender, System.EventArgs e)
        {
            Control window = (Control)sender;
            try
            {
                resizeEnd(oldWidth, oldHeight, window);
            }
            catch (NullReferenceException ex)
            {
                resizeEnd(300, 300, window);
            }
            Invalidate();

            Console.Write("Resize end\n"); //debug
        }

        private void resizeEnd(double width, double height, Control window)
        {
            Car_x = ((int)window.Size.Width * Car_x) / (int)width;
            Car_y = ((int)window.Size.Height * Car_y) / (int)height;
            leadCar_X = ((int)window.Size.Width * leadCar_X) / (int)width;
            leadCar_y = ((int)window.Size.Height * leadCar_y) / (int)height;
            centerLinesY = ((int)window.Size.Height * centerLinesY) / (int)height;

            sizeStaticObjects(window);

            objectScale = (int)((double)window.Size.Width * 0.08);
            xScale = objectScale / 2;

            oldWidth = window.Size.Width;
            oldHeight = window.Size.Height;
            Console.Write("Resize end helper\n"); //debug
        }

        private void FormView_Resize(object sender, EventArgs e)
        {
            Control window = (Control)sender;
            fullResize(oldWidth, oldHeight, window);
                 objectScale = (int)((double)this.Size.Width * 0.08);
                xScale = objectScale / 2;
            
            Invalidate();
        }

        private void fullResize(double width, double height, Control window)
        {
            Car_x = ((int)window.Size.Width * Car_x) / (int)width;
            Car_y = ((int)window.Size.Height * Car_y) / (int)height;
            leadCar_X = ((int)window.Size.Width * leadCar_X) / (int)width;
            leadCar_y = ((int)window.Size.Height * leadCar_y) / (int)height;

            sizeStaticObjects(window);

            oldWidth = window.Size.Width;
            oldHeight = window.Size.Height;

            Console.Write("Full Resize MAX"); //debug
        }

        private void sizeStaticObjects(Control window)
        {
            LpathBound.Location = new Point((int)((double)window.Size.Width * 0.3), 0);
            RpathBound.Location = new Point((int)((double)window.Size.Width * 0.6), 0);
            LpathBound.Size = new Size(11, window.Size.Height);
            RpathBound.Size = LpathBound.Size;
            objectScale = (int)((double)window.Size.Width * 0.08);
            path = new Rectangle(LpathBound.Location.X, 0, RpathBound.Location.X - LpathBound.Location.X, window.Height);
        }
        //Control settings methods
        public void setInputSource(int input)
        {
            if ((int)inputSelection.joystick == input)
            {

                drivingAxis = "X";
                accelAxis = "Y";
                brakeAxis = "Rz";
                drive = 9;
                reverse = 8;
                wheelJoy = 0;
                pedalJoy = 0;
                //this.PollingTimer.Enabled = false;
                joystickInterface = new JoystickInterface(this);
                joystickState = new JoystickState[joystickInterface.getNumJoysticks()];
                joystick = joystickInterface.getJoystick();
                joyAxisMax = new int[joystickInterface.getNumJoysticks()];
                joyAxisMin = new int[joystickInterface.getNumJoysticks()];
                for (int i = 0; i < joystickInterface.getNumJoysticks(); i++)
                {
                    joyAxisMax[i] = 65535;
                }
                //this.PollingTimer.Enabled = true;
                selectedInput = (int)inputSelection.joystick;

                if (initialized)
                    physicsEngine.inputChanged();
                //this.PollingTimer.Enabled = true;
            }
            else if ((int)inputSelection.keyboard == input)
            {
                //this.PollingTimer.Enabled = false;
                selectedInput = (int)inputSelection.keyboard;

                try
                {
                    for (int i = 0; i < joystickInterface.getNumJoysticks(); i++)
                    {
                        joystick[i].Unacquire();
                        joystick[i].Dispose();
                    }
                }
                catch
                {
                    //joystick wasn't aquired in the first place, this is ok ADD ERROR *ToDo
                }

                keyboardInterface = new KeyboardInterface(this);

                wheelJoy = 0;
                pedalJoy = 0;
                joyAxisMax = new int[1];
                joyAxisMin = new int[1];
                joyAxisMax[0] = AXIS_MAX;
                joyAxisMin[0] = 0;

                degree = 0; // TEMP
                degreeR = 0;
                reverse = 0;
                driveGear = 0;
                accelValue = 0;
                brakeValue = 0;

                if (initialized)
                    physicsEngine.inputChanged();
                //this.PollingTimer.Enabled = true;
            }

        }

        public int getInputSource()
        {
            int source = selectedInput;
            return source;
        }

        public JoystickState[] getJoystickState()
        {
            JoystickState[] state = joystickState;
            state[wheelJoy] = joystickInterface.getState(wheelJoy);
            state[pedalJoy] = joystickInterface.getState(pedalJoy);
            return state;
        }

        public Device[] getJoystick()
        {
            Device[] joy = joystick;
            return joy;
        }

        public int getDrivingAxis()
        {
            return getAxis(drivingAxis);
        }

        public void setDrivingAxis(string axis)
        {
            drivingAxis = axis;
        }

        public int getAccelAxis()
        {
            return getAxis(accelAxis);
        }

        public void setAccelAxis(string axis)
        {
            accelAxis = axis;
        }

        public int getBrakeAxis()
        {
            return getAxis(brakeAxis);
        }

        public void setBrakeAxis(string axis)
        {
            brakeAxis = axis;
        }

        public int getDriveButton()
        {
            int d = drive;
            return d;
        }

        public void setDriveButton(int d)
        {
            drive = d;
        }

        public int getReverseButton()
        {
            int r = reverse;
            return reverse;
        }

        public void setReverseButton(int r)
        {
            reverse = r;
        }

        public int getObjectId()
        {
            int id = object_iD;
            return id;
        }

        public void setObjectId(int id)
        {
            object_iD = id;
        }

        public int getNumJoys()
        {
            if (selectedInput == (int)inputSelection.joystick)
            {
                int joys = joystickInterface.getNumJoysticks();
                return joys;
            }
            else
                return 1;
        }

        private int getAxis(string axis)
        {

            if (axis.Equals("X"))
                return (int)axisSelection.X;
            else if (axis.Equals("Y"))
                return (int)axisSelection.Y;
            else if (axis.Equals("Z"))
                return (int)axisSelection.Z;
            else if (axis.Equals("Rx"))
                return (int)axisSelection.Rx;
            else if (axis.Equals("Ry"))
                return (int)axisSelection.Ry;
            else if (axis.Equals("Rz"))
                return (int)axisSelection.Rz;
            else
                return 0;
        }

        public int getPedalJoy()
        {
            int joy = pedalJoy;
            return joy;
        }

        public void setPedalJoy(int joy)
        {
            pedalJoy = joy;
        }

        public int getWheelJoy()
        {
            int joy = wheelJoy;
            return joy;
        }

        public void setWheelJoy(int joy)
        {
            wheelJoy = joy;
        }

        public Key[] getPressedKeys()
        {
            Key[] key = pressedKeys;
            return key;
        }

        private void LatencyTimer_Tick(object sender, EventArgs e)
        {
            //FPS *= 2;
            UserDisplay.Latency.Text = "FPS: " + FPS.ToString();
            FPS = 0;
        }

        //File writing methods
        public void setFileName(string name)
        {
            fileName = name;
            fileNameRaw = fileName.Insert(fileName.Length - 4, "_Raw");
            fileNameDumpPerTrial = fileName.Insert(fileName.Length - 4, "_" + trialCount + "TrialDump");
        }

        public string getFileName()
        {
            return fileName;
        }

        private void saveSnapshot()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameDumpPerTrial, true)) //leave stream open? *ToDo
            {
                file.WriteLine(Math.Round(((double)accelValue / (double)joyAxisMax[pedalJoy]) * 100, 3) +
                    "\t" + Math.Round(((brakeValue / brakeMax) * 100), 3) +
                    "\t" + Math.Round(degree, 3) +
                    "\t" + driveGear +
                    "\t" + reverseGear +
                    "\t" + Math.Round(velocity, 3) +
                    "\t" + Math.Round((Car_x + ((double)xScale / 2)) - (((double)RpathBound.Location.X - (double)lineCenter) / 2), 3) +
                    "\t" + collision +
                    "\t" + RTtest.ToString() +
                    "\t" + trialTime.ElapsedMilliseconds);
            }
        }

        //Cognitive load methods
        private void cogBrake()
        {
            //leadCarBrake();
        }

        private void cogLeft()
        {
            leadCar_X += velocity * Math.Sin(-0.1);
        }

        private void cogRight()
        {
            leadCar_X += velocity * Math.Sin(0.1);
        }

        private void cogBrakeAndLeft()
        {
            cogBrake();
            cogLeft();
        }

        private void cogBrakeAndRight()
        {
            cogBrake();
            cogRight();
        }

        private void Display_Load(object sender, EventArgs e)
        {

        }

    }
}
