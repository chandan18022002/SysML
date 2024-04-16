using System.Data;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using radar;         // here im using radar namespace of other ( claseses) file 

class radareqn
{
    static void Main(string[] args)
    {

        double initial_time = 0;

        //Create an empty list of pulses,radarbase,aircraft
        // List<Pulse> pulselist = new List<Pulse>();
        List<RadarBase> radarbaselist = new List<RadarBase>();
        List<Aircraft> aircraftlist = new List<Aircraft>();

        //Dictionary of the pulse

        Dictionary<int, Pulse> pulse_dictionary = new Dictionary<int, Pulse>();

        // radAR BASE class initialsization

        RadarBase radarbase = new RadarBase("0", new Vector(100, 300), 0, 0, [], []);
     

        //radAR  class initialsization
        Radar radar = new Radar("0", radarbase, "operating_mode", "antenna_type", "none", 0, 0, 1.5, 500, 1.5, "antenna_scan_pattern",100,200,1,1,100);
        radarbase.OnboardSensor.Add(radar);// assigning the onboardsensor to a radar
        radarbaselist.Add(radarbase);

        //aircraft class initialsization
        Aircraft aircraft = new Aircraft("0", new Vector(500, 300), 0, 0, [], []);
        aircraftlist.Add(aircraft);                      //here set aircraft pos as same as radar pos bz max unamb range is =500 so that aircraft pos is radarpos+unamgious rangee


        int tick = 0;
        int current_pulse_id = 0;
        int latest_radar_transmission_tick = 0;
        int pul_index = 0;
        while (true)
        {
            Mat image_virtual = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image_virtual.SetTo(new Bgr(0, 0, 0).MCvScalar);

            Mat image_actual = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image_actual.SetTo(new Bgr(0, 0, 0).MCvScalar);

            for (int i = pulse_dictionary.Count - 1; i >= 0; i--)
            {
                //Green Aircraft

                CvInvoke.Circle(image_virtual, new Point((int)radarbaselist[i].Position.X, (int)radarbaselist[i].Position.Y), 3, new MCvScalar(pul_index, 0, 255), -1);
                CvInvoke.Circle(image_actual, new Point((int)radarbaselist[i].Position.X, (int)radarbaselist[i].Position.Y), 3, new MCvScalar(pul_index, 0, 255), -1);

                int currentKey = pulse_dictionary.Keys.ElementAt(i);
                Pulse pulse = pulse_dictionary[currentKey];
                pulse.Move();


            }

            for (int i = aircraftlist.Count - 1; i >= 0; i--)
            {

                Bgr actual_pixelValue = GetPixelBgr(image_actual, (int)aircraft.Position.X, (int)aircraft.Position.Y);

                if (actual_pixelValue.Red == 255)
                {
                    pul_index = (int)actual_pixelValue.Blue;

                    if (pulse_dictionary.ContainsKey(pul_index))
                    {
                        Pulse pulse = pulse_dictionary[pul_index];
                        pulse.colloid_target(0);
                    }
                }

                //Green Aircraft
                CvInvoke.Circle(image_actual, new Point((int)aircraftlist[i].Position.X + 100, (int)aircraftlist[i].Position.Y), 3, new MCvScalar(0, 255, 0), -1);
            }                                                                //umambrange=c/(2f)  =>c is pulse velocity where pulsevelocity is vector quantity sotake magnitude of it (sqrt(velx square)+(vel.y square))   so we get only one value for range beacuse rngeis scalar quantity
                                                                             //f is prf which is 1/pri   ==> 500/(2*0.001)==500

            for (int i = radarbaselist.Count - 1; i >= 0; i--)
            {
                Bgr actual_pixelValue = GetPixelBgr(image_actual, (int)radarbase.Position.X, (int)radarbase.Position.Y);

                if (actual_pixelValue.Red == 255)

                    pul_index = (int)actual_pixelValue.Blue;

                if (pulse_dictionary.ContainsKey(pul_index))
                {
                    Pulse pulse = pulse_dictionary[pul_index];
                    pulse.colloid_radar(tick, latest_radar_transmission_tick);
                }
                pulse_dictionary.Remove(i);
            }

            static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
            {
                return degrees * (Math.PI / 180);
            }



            for (int i = radarbaselist.Count - 1; i >= 0; i--)
            {
                //Blue Radar
                CvInvoke.Circle(image_actual, new Point((int)radarbaselist[i].Position.X, (int)radarbaselist[i].Position.Y), 3, new MCvScalar(255, 0, 0), -1);

                if (tick % radar.Pri == 0)
                //if (tick == 0)
                {
                    //Pulse position should be equal to radar base
                    //create a pulse
                    pul_index += 1;
                    double vel_x = Math.Cos(DegreesToRadians(((Radar)radarbase.OnboardSensor[0]).Azimuth));
                    double vel_y = Math.Sin(DegreesToRadians(((Radar)radarbase.OnboardSensor[0]).Azimuth));

                    //creating pulse
                    Pulse pulse = new Pulse(current_pulse_id, new Vector(radarbaselist[i].Position.X, radarbaselist[i].Position.Y), new Vector(vel_x, vel_y), 85, 75, 22, 58,radar.Azimuth,radar.Frequency);//pulse position should be rdar position so it achieve by .x and .y individually 
                    initial_time = tick;
                    //// add that pulse in list of pulses
                    pulse_dictionary.Add(pul_index, value: pulse);
                }

            }

            //for each pulse in list of pulses{
            for (int i = pulse_dictionary.Count - 1; i >= 0; i--)
            {
                int currentKey = pulse_dictionary.Keys.ElementAt(i);
                Pulse pulse = pulse_dictionary[currentKey];
                pulse.Move();


                Bgr pulse_pixelValue = GetPixelBgr(image, (int)pulse.Position.X, (int)pulse.Position.Y);

                if (pulse_pixelValue.Green == 255)
                {
                    pulse.reverse();
                }

                if (pulse_pixelValue.Green == 0 && pulse_pixelValue.Blue == 255 && pulse_pixelValue.Red == 0)
                {
                    double time_diff = tick - initial_time;
                    double pul_vel = 0;
                    if (pulse.Velocity.X > 0)
                    {
                        pul_vel = pulse.Velocity.X;
                    }
                    else
                    {
                        pul_vel = -pulse.Velocity.X;
                    }
                    double distance = (pul_vel * time_diff) / 2;     // c is the pulse velocity

                    Console.WriteLine($"time_diff: {time_diff}, Distance: {distance + 7}, id: {pulse.Id}");

                    pulse_dictionary.Remove(i);
                }

                //   draw red circle for each pulse
                CvInvoke.Circle(image, new Point((int)pulse.Position.X, (int)pulse.Position.Y), 3, new MCvScalar(0, 0, 255), -1);

            }


            tick += 1;

            CvInvoke.Imshow("Visualise", image);
            int key = CvInvoke.WaitKey(10);



            if (key == 113 || key == 81)
            {
                break;

            }


        }
        CvInvoke.DestroyAllWindows();
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
    static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
    static double Magnitude(double[] vector)
    {
        double sumOfSquares = 0.0;
        foreach (double component in vector)
        {
            sumOfSquares += Math.Pow(component, 2);
        }
        return Math.Sqrt(sumOfSquares);
    }

}


