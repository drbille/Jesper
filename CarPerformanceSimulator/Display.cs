/*
 * Authors: Ali M. Al Haddad (alhaddad@umich.edu), Jakob Rodseth (Jakob.Rodseth12@kzoo.edu)
 * 
 * Changelog:
 *          
 * 6/16/2015, Jakob Rodseth
 *          Removed some unused code.
 *          Migrated variable display fields to UserDisplay form.
 *          Added Joystick Interface.
 * 
 * 6/17/2015, Jakob Rodseth
 *          Implemented accelerator and brake joystick functionality.
 *          
 * 6/18/2015 and 6/19/2015, Jakob Rodseth
 *          Updated and revised variable names and comments.         
 *          Changed hardcoded joystick axis max and min values to be read automatically.
 *          Implemented joystick steering control.
 *          
 * 6/19/2015, Jakob Rodseth
 *          Revised stopwatches and reaction timer tick code.
 *          Revised reaction time test implementation.
 *          Added user control over input selection (keyboard or joystick)
 *          Added autodetect axis
 *          
 * 6/22/2015, Jakob Rodseth
 *          Added restart function
 *          Added resizing support
 *          Added simple gear select and creep
 * 
 * 6/23/2015, Jakob Rodseth
 *          Simplified stopwatch and reaction test code
 *          Added gear select mapping and auto detect
 *          Added low resolution stopwatch warning
 *          Added UI for controlling lead object and behaviour
 *          
 * 6/24/2015, Jakob Rodseth
 *          Began rewrite of physics engine
 *          
 * 6/26/2015, Jakob Rodseth
 *          Merged Timers
 *          Implemented UI control for changing scenario
 *          Increased framerate
 *          
 * 6/29/2015, Jakob Rodseth
 *          Migrated physics engine to new class
 *          Created stubs for complete physics simulation
 *          Variables and methods for full simulation
 *          
 *          
 * Purpose: UROP project 2014-2015 in the NeuRRo Lab
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; // drawing and painting
using System.Linq;
using System.Text; // text
using System.Windows.Forms; // form
using System.Diagnostics; // to use stopwatch
using System.Threading.Tasks;
using System.Threading; // to use stopwatch
using Microsoft.DirectX.DirectInput;
using System.Runtime.InteropServices;


namespace CarPerformanceSimulator
{
    public partial class FormView : Form
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

        private static double METRIC_CONVERSION = 2.236936;
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
        private double gasMomentReleaseTime, gasTotalReleaseTime, gasMomentReleaseToBrakeTouchTime, gasTotalReleaseToBrakeTouchTime, wheelTurnTime, totalStopTime; //

        private int speed_choice; // NEEDS CORRECT IMPLEMENTATION, ONLY KEYBOARD?  *ToDo
        
        public bool cognitiveLoadTest;
        private bool cogLoadMoveLeft;
        private bool cogLoadMoveRight;
        private bool cogLoadBrake;
        private bool cogLoadMoveRightAndBrake;
        private bool cogLoadMoveLeftAndBrake;

        public bool[] cogLoad;

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

        private enum inputSelection { joystick = 0, keyboard = 1 };
        private int selectedInput;

        internal bool isAccel = false; //Is the acclerator engaged
        internal bool isBrake = false; //Is brake engaged
        private bool rtStartGas = false; //Is accelerator engaged at start of reaction test
        private bool rtStartBrake = false; //Is brake engaged at start of reaction test
        private double rtStartSpeed; //Speed when reaction time test was started
        private bool rtWheelPos;
        private double leadCarStartSpeed;
        private double rtStartAccelPosition;
        internal int xTrials;
        internal int xSpeed;

        bool RTstart = false;
        bool RTgas = false;
        bool RTbrake = false;

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

        private int objectScale; //Hardcode ALL values to avoid calculation overhead *ToDo

        double previous;
        double lag;


        public FormView()
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
            speed_choice = 0; // a person's choice of speed
            rTriggerMax = 10;
            rTriggerMin = 2;
            cognitiveLoadTest = false;
            cogLoadMoveLeft = false;
            cogLoadMoveRight = false;
            cogLoadBrake = false;
            cogLoadMoveLeftAndBrake = false;
            cogLoadMoveRightAndBrake = false;

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
            System.IO.File.WriteAllText(fileNameRaw, "Trial\tType\tStartSpeed\tGasMRel\tGasTRel\tGasMBrake\tGasTBrake\tWheelTurn\tTotalStop\tCollision\t\r\n");
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
                if (tick)
                    Update();
                FPS++;
            }
        }

        bool isApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        public void Restart()
        {
            tick = false;

            leadCar = new Bitmap("frontCar.png");

            

            velocity = 0;
            speed_choice = 0;
            RTgas = false;
            RTbrake = false;
            RTtest = false;
            RTstart = false;
            rtStartGas = false;
            rtStartBrake = false;

            collision = 0;
            gasMomentReleaseTime = 0;
            gasMomentReleaseToBrakeTouchTime = 0;
            gasTotalReleaseTime = 0;
            gasTotalReleaseToBrakeTouchTime = 0;
            wheelTurnTime = 0;
            totalStopTime = 0;
            objectScale = (int)((double)this.Size.Width * 0.08);
            rtStartAccelPosition = 0;
            xScale = objectScale / 2;

            validSpeed = false;

            stationaryObjectYTotal = 0;
            stationaryObjectYRTstart = 0;
            stationaryObjectY = 0;

            if (object_iD == 0 && !cognitiveLoadTest)
            {
                Car_x = (int)this.Size.Width - (int)(this.Size.Width * .5);
                Car_y = (int)this.Size.Height - (int)(this.Size.Height * .25);
                leadCar_X = Car_x;
                leadCar_y = (int)this.Size.Height - (int)(this.Size.Height * .95);
                centerLinesY = (int)this.Size.Height - (int)(this.Size.Height * .3); // initial y_position
            }
            else
            {
                Car_x = (int)(lineCenter) - (int)(0.5 * xScale);
                Car_y = (int)this.Size.Height - (int)(this.Size.Height * .25);
                leadCar_X = Car_x;
                leadCar_y = (int)this.Size.Height - (int)(this.Size.Height * .95);
                centerLinesY = (int)this.Size.Height - (int)(this.Size.Height * .3);

                int i = 0;
                foreach (bool b in cogLoad)
                {
                    if(b)
                    {
                        switch(i)
                        {
                            case 0:
                                {
                                    cogLoadMoveLeft = true;
                                    break; 
                                }
                            case 1:
                                {
                                    cogLoadMoveRight = true;
                                    break;
                                }
                            case 2:
                                {
                                    cogLoadBrake = true;
                                    break;
                                }
                            case 3:
                                {
                                    cogLoadMoveRightAndBrake = true;
                                    break;
                                }
                            case 4:
                                {
                                    cogLoadMoveLeftAndBrake = true;
                                    break;
                                }
                        }
                    }
                    i++;
                }
            }


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

        // paint function ( all the figures that are not in the design page
        private void FormView_Paint(object sender, PaintEventArgs e)
        {
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

            e.Graphics.DrawImage(car, (float)Car_x, (float)Car_y, xScale, objectScale); // The user car
            if (object_iD == 0)
            {
                e.Graphics.DrawImage(leadCar, (float)leadCar_X, (float)leadCar_y, xScale, objectScale); // The lead car
            }
            if (RTstart)
            {
                if (object_iD == 1)
                    e.Graphics.DrawImage(stopSign, RpathBound.Location.X + 11, (float)(stationaryObjectY - objectScale), objectScale / 2, objectScale / 2); // The stop sign
                else if (object_iD == 2)
                    e.Graphics.DrawImage(deer, walkX, (float)(stationaryObjectY - objectScale), xScale, objectScale); // The deer
                else if (object_iD == 3)
                    e.Graphics.DrawImage(pedestrian, walkX, (float)(stationaryObjectY - objectScale), xScale, objectScale); // The pedestrian
            }
            int spacing = (int)((double)Size.Height * 0.20);
            int size = (int)((double)Size.Height * 0.10);
            lineCenter = (int)((double)Size.Width * 0.45);
            for (int i = -15; i <= Size.Height * .15; i++)
            {
                e.Graphics.FillRectangle(Brushes.Black, lineCenter, (float)centerLinesY + (i * spacing), 6, size); // the moving lines
            }
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
            else if(!cognitiveLoadTest)
            {
                collision = 2;
            }

            // Makes lane lines return to page
            if (centerLinesY > this.Size.Height / 2) centerLinesY -= 2 * this.Size.Height;
            if (centerLinesY < this.Size.Height / -2) centerLinesY += 2 * this.Size.Height;

            if (object_iD == 0)
            {
                if (Car_x < leadCar_X + xScale &&
                    Car_x + xScale > leadCar_X &&
                    Car_y < leadCar_y + objectScale &&
                    Car_y + objectScale > leadCar_y)
                {
                    collision = 1;
                }
            }

            if (RTstart)
            {
                switch (object_iD)
                {
                    case 0:
                        if (!cognitiveLoadTest)
                        {
                            leadCarStartSpeed *= 0.99;
                            leadCar_y += (int)(velocity - leadCarStartSpeed);
                        }
                        else
                        {
                            bool choiceReaction = false;
                            

                            if (leadCar_X + xScale < RpathBound.Location.X && leadCar_X > LpathBound.Location.X + LpathBound.Size.Width)
                            {
                                if (cogLoadMoveLeft && !cogLoadMoveRight)
                                {
                                    leadCar_X += velocity * Math.Sin(-0.1);
                                }
                                else if (cogLoadMoveRight && !cogLoadMoveLeft)
                                {
                                    leadCar_X += velocity * Math.Sin(0.1);
                                }
                                else if (cogLoadMoveLeft && cogLoadMoveRight)
                                {
                                    //cogLeftOrRight = true;
                                    Random ran = new Random();
                                    if (ran.Next() % 2 == 0)
                                    {
                                        leadCar_X += velocity * Math.Sin(0.17);
                                        cogLoadMoveLeft = false;
                                    }
                                    else
                                    {
                                        leadCar_X += velocity * Math.Sin(-0.17);
                                        cogLoadMoveRight = false;
                                    }
                                }
                            }
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
                            if (Car_x < walkX + xScale &&
                            Car_x + xScale > walkX &&
                            Car_y < stationaryObjectY &&
                            Car_y + objectScale > stationaryObjectY - objectScale)
                            {
                                collision = 1;
                            }
                            stationaryObjectY = stationaryObjectYTotal - stationaryObjectYRTstart;
                            walkX -= 2;
                            break;
                        }
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
                    }
                    if (((cogLoadMoveLeft && cogLoadMoveRight) && (degree < -2 || degree > 2)) || (cogLoadMoveLeft && (degree < -2)) || (cogLoadMoveRight && (degree > 2)))
                    {
                        rtWheelPos = false;
                    }
                    else
                        rtWheelPos = true;

                    RTstart = true;
                    stopwatch.Start();
                }
            }
        }

        // reaction time timer (set start)
        private void RT_tmr_Tick()
        {
            //bool brake;
            if ((!cognitiveLoadTest || (cognitiveLoadTest && cogLoadBrake)))
            {
                //if (cogLoadMoveLeft || cogLoadMoveRight)
                //{
                //    Random ran = new Random();
                //    int num = ran.Next();
                //    if (num % 2 == 0)
                //    {
                //        brake = true;
                //        leadCar = new Bitmap("frontCar_Braking.png");
                //    }
                //    else
                //    {
                //        brake = false;
                //    }

                //}
                //else    
                //{
                    leadCar = new Bitmap("frontCar_Braking.png");
                    //brake = true;
                //}


                if (rtStartGas && !RTgas)
                {
                    if (gasMomentReleaseTime == 0 && (rtStartAccelPosition - accelValue) > ((double)joyAxisMax[pedalJoy] * 0.1))
                    {
                        gasMomentReleaseTime = stopwatch.ElapsedMilliseconds;
                    }
                    if (gasTotalReleaseTime == 0 && !isAccel)
                    {
                        gasTotalReleaseTime = stopwatch.ElapsedMilliseconds;
                        RTgas = true;
                    }
                }

                if (isBrake && !RTbrake && !rtStartBrake)
                {
                    if (gasMomentReleaseToBrakeTouchTime == 0)
                    {
                        if (rtStartGas)
                        {
                            gasMomentReleaseToBrakeTouchTime = stopwatch.ElapsedMilliseconds - gasMomentReleaseTime;
                        }
                        else if (!rtStartGas)
                        {
                            gasMomentReleaseToBrakeTouchTime = -1;
                        }
                    }
                    if (gasTotalReleaseToBrakeTouchTime == 0)
                    {
                        if (rtStartGas)
                        {
                            gasTotalReleaseToBrakeTouchTime = stopwatch.ElapsedMilliseconds - gasTotalReleaseTime;
                            RTbrake = true;
                        }
                        if (!rtStartGas)
                        {
                            gasTotalReleaseToBrakeTouchTime = -1;
                            RTbrake = true;
                        }
                    }
                }
            }
            
            if (cognitiveLoadTest && (cogLoadMoveLeft || cogLoadMoveRight) && rtWheelPos)
            {
                if (cogLoadMoveLeft && degree > -2)
                {
                    wheelTurnTime = stopwatch.ElapsedMilliseconds;
                }
                if (cogLoadMoveRight && (degree < 2))
                {
                    wheelTurnTime = stopwatch.ElapsedMilliseconds;
                }
            }

            if (velocity == 0 || (collision != 0 && (!cognitiveLoadTest || (wheelTurnTime != 0 && cognitiveLoadTest && !cogLoadBrake))))
            {
                if (RTtest)
                {
                    if (rtStartGas && !RTgas)
                    {
                        gasMomentReleaseTime = -1;
                        gasTotalReleaseTime = -1;
                    }
                    if (!rtStartBrake && !RTbrake)
                    {
                        gasMomentReleaseToBrakeTouchTime = -1;
                        gasTotalReleaseToBrakeTouchTime = -1;
                    }
                    if ((cogLoadMoveLeft || cogLoadMoveRight) && (wheelTurnTime == 0 || !rtWheelPos))
                    {
                        wheelTurnTime = -1;
                        collision = 3;
                    }
                    stopwatch.Stop();
                    totalStopTime = stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();
                    RTtest = false;
                }
                tick = false;
                physicsEngine.stopSound();
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
                               "\r\n\tTrial start to gas total release time: " + gasTotalReleaseTime +
                               "ms\r\n\tGas moment release to brake time: " + gasMomentReleaseToBrakeTouchTime +
                               "ms\r\n\tGas total release to brake time: " + gasTotalReleaseToBrakeTouchTime +
                               "ms\r\n\tTotal stop time: " + totalStopTime +
                               "ms\r\n\tCollision Detected: " + collision + "\r\n";
                        }
                        else
                        {
                            output = output = "Trial " + trialCount +
                               ": \r\n\tType: " + object_iD;

                            if (cogLoadBrake)
                                output += "B";
                            if (cogLoadMoveLeft)
                                output += "L";
                            if (cogLoadMoveRight)
                                output += "R";
                            output += "\r\n\tStart Speed: " + Math.Round(rtStartSpeed, 1) + " Meters/Sec";
                            if(cogLoadBrake)
                            {
                                output += "\r\n\tTrial start to gas moment release time: " + gasMomentReleaseTime +
                             "\r\n\tTrial start to gas total release time: " + gasTotalReleaseTime +
                             "ms\r\n\tGas moment release to brake time: " + gasMomentReleaseToBrakeTouchTime +
                             "ms\r\n\tGas total release to brake time: " + gasTotalReleaseToBrakeTouchTime + "ms\r\n\t";
                            }
                             if(cogLoadMoveLeft || cogLoadMoveRight)
                             {
                             output += "tWheel turn time: " + wheelTurnTime + "ms\r\n\t";
                             }
                             output += "Total stop time: " + totalStopTime +
                             "ms\r\n\tCollision Detected: " + collision + "\r\n";
                        }
                        file.WriteLine(output);
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameRaw, true))
                    {
                        string output;
                        if (!cognitiveLoadTest)
                        {
                            output = trialCount + "\t" + object_iD + "\t" + Math.Round(rtStartSpeed, 1) +
                            "\t\t" + gasMomentReleaseTime + "\t\t" + gasTotalReleaseTime + "\t\t" + gasMomentReleaseToBrakeTouchTime + "\t\t" + gasTotalReleaseToBrakeTouchTime + "\t\t\t" + totalStopTime + "\t\t" + collision + "\r\n";
                        }
                        else
                        {
                            output = trialCount + "\t" + object_iD;
                            if (cogLoadBrake)
                                output += "B";
                            if (cogLoadMoveLeft)
                                output += "L";
                            if (cogLoadMoveRight)
                                output += "R";
                            output += "\t" + Math.Round(rtStartSpeed, 1) + "\t\t";
                            if (cogLoadBrake)
                            {
                                output += gasMomentReleaseTime + "\t\t" + gasTotalReleaseTime + "\t\t" + gasMomentReleaseToBrakeTouchTime + "\t\t" + gasTotalReleaseToBrakeTouchTime;
                            }
                            if (cogLoadMoveRight || cogLoadMoveRight)
                                output += "\t\t" + wheelTurnTime;
                            output+= "\t\t" + totalStopTime + "\t\t" + collision + "\r\n";
                        }
                        file.WriteLine(output);
                    }
                }
                trialCount++;
                fileNameDumpPerTrial = fileName.Insert(fileName.Length - 4, "_" + trialCount + "TrialDump");
                System.IO.File.WriteAllText(fileNameDumpPerTrial, "Thrtl\tBrke\tWjlDeg\tDGear\tRGear\tVel\tLaneDev\tRT\tCol\tTime\r\n");

                trigger = false;
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
        }
        public void setInputSource(int input)
        {
            if ((int)inputSelection.joystick == input)
            {
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

                drivingAxis = "X";
                accelAxis = "Y";
                brakeAxis = "Rz";
                drive = 9;
                reverse = 8;
                wheelJoy = 0;
                pedalJoy = 0;
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
                joyAxisMax[0] = 1;
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
        private void pollingTick()
        {
            if (selectedInput == (int)inputSelection.keyboard) //FIX *ToDo
            {
                pressedKeys = keyboardInterface.poll();
                foreach (Key key in pressedKeys)
                {
                    if ((key.Equals(Key.Up) || key.Equals(Key.UpArrow)) && speed_choice < 150)
                    {
                        speed_choice += 1;
                    }
                    else if ((key.Equals(Key.Down) || key.Equals(Key.DownArrow)) && speed_choice > 0)
                    {
                        speed_choice -= 1;
                    }
                    if (key.Equals(Key.Space))
                    {
                        speed_choice = 0;
                        brakeValue = brakeMax;
                    }
                    else
                    {
                        brakeValue = 0;
                    }
                    if (velocity <= speed_choice)
                    {
                        accelValue = 1;
                    }

                    if (key.Equals(Key.LeftArrow) || key.Equals(Key.Left))
                    {
                        degree = -20;
                    }
                    else if (key.Equals(Key.RightArrow) || key.Equals(Key.Right))
                    {
                        degree = 20;
                    }
                    else
                    {
                        degree = 0;
                    }

                    if (key.Equals(Key.D1))
                    {
                        driveGear = 0;
                        reverseGear = 1;
                    }
                    else if (key.Equals(Key.D2))
                    {
                        driveGear = 0;
                        reverseGear = 0;
                    }
                    else if (key.Equals(Key.D3))
                    {
                        driveGear = 1;
                        reverseGear = 0;
                    }
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
            if (velocity >= 0) UserDisplay.direction_lb.Text = "Up";
            else UserDisplay.direction_lb.Text = "Down";

            UserDisplay.break_lb.Text = Convert.ToString(brakeValue);
            UserDisplay.isBreak_lb.Text = Convert.ToString(isBrake);

            if (RTtest)
            {
                UserDisplay.RT_lb1.Text = "Yes";
                UserDisplay.RT_lb.Text = "Test Is Running";
            }
            else
            {
                UserDisplay.RT_lb1.Text = "No";
                UserDisplay.RT_lb.Text = "Start Reaction Test";
            }
            if (RTstart)
                UserDisplay.RT_lb2.Text = "Yes";
            else
                UserDisplay.RT_lb2.Text = "No";

            if (RTgas)
            {
                UserDisplay.gas_lb1.Text = "Yes";
                UserDisplay.gas_lb2.Text = Convert.ToString(gasMomentReleaseTime);
            }
            else
            {
                UserDisplay.gas_lb1.Text = "No";
                if (RTtest)
                    UserDisplay.gas_lb2.Text = "Testing";
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
                    UserDisplay.g2b_lb2.Text = "Testing";
            }

            if (!RTtest && ((int)((double)velocity / 10)) == 0)
            {
                UserDisplay.stop_lb1.Text = "Yes";
                UserDisplay.stop_lb2.Text = Convert.ToString(totalStopTime);
            }
            else
            {
                UserDisplay.stop_lb1.Text = "No";
                if (RTtest)
                    UserDisplay.stop_lb2.Text = "Moving";
            }
        }

        private void FormView_ResizeBegin(object sender, System.EventArgs e)
        {
            oldWidth = this.Size.Width;
            oldHeight = this.Size.Height;

        }

        private void FormView_ResizeEnd(object sender, System.EventArgs e)
        {
            Car_x = ((int)this.Size.Width * Car_x) / (int)oldWidth;
            Car_y = ((int)this.Size.Height * Car_y) / (int)oldHeight;
            leadCar_X = ((int)this.Size.Width * leadCar_X) / (int)oldWidth;
            leadCar_y = ((int)this.Size.Height * leadCar_y) / (int)oldHeight;
            centerLinesY = ((int)this.Size.Height * centerLinesY) / (int)oldHeight;
            sizeStaticObjects();
            objectScale = (int)((double)this.Size.Width * 0.08);
            xScale = objectScale / 2;
            oldWidth = this.Size.Width;
            oldHeight = this.Size.Height;
            Invalidate();
        }

        private void FormView_Resize(object sender, EventArgs e)
        {

            // When window state changes
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;
                if (WindowState == FormWindowState.Maximized)
                {
                    Car_x = ((int)Screen.PrimaryScreen.Bounds.Width * Car_x) / (int)oldWidth;
                    Car_y = ((int)Screen.PrimaryScreen.Bounds.Height * Car_y) / (int)oldHeight;
                    leadCar_X = ((int)Screen.PrimaryScreen.Bounds.Width * leadCar_X) / (int)oldWidth;
                    leadCar_y = ((int)Screen.PrimaryScreen.Bounds.Height * leadCar_y) / (int)oldHeight;
                    sizeStaticObjects();
                }
                else if (WindowState == FormWindowState.Normal)
                {

                    Car_x = ((int)oldWidth * Car_x) / (int)Screen.PrimaryScreen.Bounds.Width;
                    Car_y = ((int)oldHeight * Car_y) / (int)Screen.PrimaryScreen.Bounds.Height;
                    leadCar_X = ((int)oldWidth * leadCar_X) / (int)Screen.PrimaryScreen.Bounds.Width;
                    leadCar_y = ((int)oldHeight * leadCar_y) / (int)Screen.PrimaryScreen.Bounds.Height;
                    sizeStaticObjects();
                    oldWidth = this.Size.Width;
                    oldHeight = this.Size.Height;
                }
                objectScale = (int)((double)this.Size.Width * 0.08);
                xScale = objectScale / 2;
            }
            Invalidate();

        }

        private void sizeStaticObjects()
        {
            LpathBound.Location = new Point((int)((double)Size.Width * 0.3), 0);
            RpathBound.Location = new Point((int)((double)Size.Width * 0.6), 0);
            LpathBound.Size = new Size(11, Size.Height);
            RpathBound.Size = LpathBound.Size;
            objectScale = (int)((double)this.Size.Width * 0.08);
            path = new Rectangle(LpathBound.Location.X, 0, RpathBound.Location.X - LpathBound.Location.X, this.Height);
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
            FPS *= 2;
            UserDisplay.Latency.Text = "FPS: " + FPS.ToString();
            FPS = 0;
        }

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
    }
}
