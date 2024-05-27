using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using battleFrameWork;
using platform;
using aircraft;
using radar;
using pulse;
using generics;
using System.Collections.Generic;
class MainClass
{
    static void Main(string[] args)
    {
        //Create an empty list of pulses,radarbase,aircraft
        // List<Pulse> pulselist = new List<Pulse>();
        List<RadarBase> RadarBaseList = new List<RadarBase>();
        List<Aircraft> AircraftList = new List<Aircraft>();
        List<PulsedRadar> PulseRadarList = new List<PulsedRadar>();
        List<BattleSystem> BattleSystems = new List<BattleSystem>();


        // RADAR BASE class initialsization
        RadarBase RadarBase1 = new RadarBase("rb1", 0, 0, [new Vector(100, 300)], []);
        RadarBaseList.Add(RadarBase1);

        //radAR  class initialsization
        PulsedRadar PulseRadar1 = new PulsedRadar("r1", RadarBase1, "operating_mode", "antenna_type", "none", 0, 0, 1.5, 300, 1.5, "antenna_scan_pattern", 100, 200, 1, 1, 100, 10);
        RadarBase1.onboardSensor.Add(PulseRadar1);// assigning the onboardsensor to a radar
        PulseRadarList.Add(PulseRadar1);
        BattleSystems.Add(PulseRadar1);

        //aircraft class initialsization
        Aircraft Aircraft1 = new Aircraft("a1", 1, 1, [new Vector(500, 300), new Vector(500, 500), new Vector(500, 100)], []);
        AircraftList.Add(Aircraft1);
        BattleSystems.Add(Aircraft1);
        
        //List<List<Pair<string, string>>> GlobalMatrix = new List<List<Pair<string, string>>>;
        List<Pair<string, string>>[,] GlobalMatrix = new List<Pair<string, string>>[BattleSystems.Count, BattleSystems.Count];
        int tick = 0;
        while (true)
        {
            Dictionary<string, List<Pair<string, string>>> Awareness = new Dictionary<string, List<Pair<string, string>>>();

            Mat image = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image.SetTo(new Bgr(0, 0, 0).MCvScalar);

            //Aircraft visualize, move and pulse.collide
            for (int i = AircraftList.Count - 1; i >= 0; i--)
            {
                AircraftList[i].MovePlatform();
                //Green Aircraft Visualize
                CvInvoke.PutText(image, Math.Round(AircraftList[i].position.X, 2).ToString() + "," + Math.Round(AircraftList[i].position.Y, 2).ToString(), new Point((int)AircraftList[i].position.X + 10, (int)AircraftList[i].position.Y + 10), FontFace.HersheySimplex, 1.0, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Circle(image, new Point((int)AircraftList[i].position.X, (int)AircraftList[i].position.Y), 3, new MCvScalar(0, 255, 0), -1);
            }

            //Radar visualize, move, pulse.collide, remove_pulse and create_and_transmit_pulse
            for (int i = PulseRadarList.Count - 1; i >= 0; i--)
            {
                //RADAR Visualize
                CvInvoke.Circle(image, new Point((int)((RadarBase)(PulseRadarList[i].hostPlatform)).position.X, (int)((RadarBase)(PulseRadarList[i].hostPlatform)).position.Y), 3, new MCvScalar(255, 0, 0), -1);
            }
            foreach (var BS in BattleSystems)
            {
                Awareness.Add(BS.Id,BS.Get());
            }
            int m = 0;
            int n = 0;
            foreach(var a1 in Awareness)
            {
                n = 0;
                foreach (var a2 in Awareness)
                {
                    if(a1.Key == a2.Key)
                    {
                        GlobalMatrix[m,n] = new List<Pair<string, string>>() {new Pair<string, string>("id", a1.Key) };
                    }
                    else
                    {
                        List<double> x_comp = new List<double>();
                        List<double> y_comp = new List<double>();
                        foreach (var a3 in a1.Value.Concat(a2.Value))
                        {
                            if (a3.First == "Position_x")
                            {
                                x_comp.Add(Double.Parse(a3.Second));
                            }
                            else if (a3.First == "Position_y")
                            {
                                y_comp.Add(Double.Parse(a3.Second));
                            }
                        }
                        if(x_comp.Count == 2 && y_comp.Count == 2)
                        {
                            GlobalMatrix[m, n] = new List<Pair<string, string>>() { new Pair<string, string>("distance", Math.Sqrt(Math.Pow((x_comp[0] - x_comp[1]), 2) + Math.Pow((y_comp[0] - y_comp[1]), 2)).ToString()) };
                        }
                    }
                    n++;
                }
                m++;
            }
            //Position_x,Position_y
            tick += 1;

            //CvInvoke.Imshow("Visual", image);
            //Console.WriteLine(tick);
            int key = CvInvoke.WaitKey(10);

            if (key == 113 || key == 81)
            {
                break;
            }
        }
        CvInvoke.DestroyAllWindows();
    }

    static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
    {
        return degrees * (Math.PI / 180);
    }

    static Bgr GetPixelValue(Mat image, int x, int y)
    {
        // Get the data pointer for the image
        IntPtr ptr = image.DataPointer;

        // Calculate the byte index corresponding to the pixel at (x, y)
        int pixelSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Bgr));
        int step = image.Step;
        int byteIndex = y * step + x * pixelSize;

        // Read the BGR values from the image data
        byte blue = System.Runtime.InteropServices.Marshal.ReadByte(ptr, byteIndex);
        byte green = System.Runtime.InteropServices.Marshal.ReadByte(ptr, byteIndex + 1);
        byte red = System.Runtime.InteropServices.Marshal.ReadByte(ptr, byteIndex + 2);

        // Create a Bgr structure with the pixel values
        return new Bgr(blue, green, red);
    }
    static Bgr GetPixelBgr(Mat image, int x, int y)
    {
        // Access the BGR values of the pixel at the specified coordinates
        Image<Bgr, byte> img = image.ToImage<Bgr, byte>();
        return img[y, x];
    }



}



