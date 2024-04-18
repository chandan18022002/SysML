using System.Data;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using radar;         // here im using radar namespace of other ( claseses) file 

class radareqn
{
    static void Main(string[] args)
    {

        //Create an empty list of pulses,radarbase,aircraft
        // List<Pulse> pulselist = new List<Pulse>();
        List<RadarBase> radarbaselist = new List<RadarBase>();
        List<Aircraft> aircraftlist = new List<Aircraft>();

        //Dictionary of the pulse

        Dictionary<int, Pulse> pulse_dictionary = new Dictionary<int, Pulse>();

        // radAR BASE class initialsization

        RadarBase radarbase = new RadarBase("0", new Vector(100, 300), 0, 0, [], []);


        //radAR  class initialsization
        Pulsed_radar pulse_radar = new Pulsed_radar("0", radarbase, "operating_mode", "antenna_type", "none", 0, 0, 1.5, 500, 1.5, "antenna_scan_pattern", 100, 200, 1, 1, 100, 10);
        radarbase.onboardSensor.Add(pulse_radar);// assigning the onboardsensor to a radar
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
            Mat image_visual = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image_visual.SetTo(new Bgr(0, 0, 0).MCvScalar);

            Mat image_actual = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image_actual.SetTo(new Bgr(0, 0, 0).MCvScalar);

            for (int i = pulse_dictionary.Count - 1; i >= 0; i--)
            {
                //Green Aircraft

                CvInvoke.Circle(image_visual, new Point((int)radarbaselist[i].position.X, (int)radarbaselist[i].position.Y), 3, new MCvScalar(pul_index, 0, 255), -1);
                CvInvoke.Circle(image_actual, new Point((int)radarbaselist[i].position.X, (int)radarbaselist[i].position.Y), 3, new MCvScalar(pul_index, 0, 255), -1);

                int currentKey = pulse_dictionary.Keys.ElementAt(i);
                Pulse pulse = pulse_dictionary[currentKey];
                pulse.Move();


            }
            Platform pt=new Platform("0", new Vector(100, 300), 0, 0, [], []);
            
            for (int i = aircraftlist.Count - 1; i >= 0; i--)
            {

                Bgr actual_pixelValue = GetPixelBgr(image_actual, (int)aircraft.position.X, (int)aircraft.position.Y);

                if (actual_pixelValue.Red == 255)
                {
                    pul_index = (int)actual_pixelValue.Blue;

                    if (pulse_dictionary.ContainsKey(pul_index))
                    {
                        Pulse pulse = pulse_dictionary[pul_index];
                        //pulse.Collided_Target(aircraft.heading, RadarCrossSection);
                        pulse.Collided_Target(aircraft.heading,radarbase);
                    }
                }

                //Green Aircraft
                CvInvoke.Circle(image_actual, new Point((int)aircraftlist[i].position.X + 100, (int)aircraftlist[i].position.Y), 3, new MCvScalar(0, 255, 0), -1);
            }
            //Trigger pulse.collided_radar(tick, latest_radar_transmit_tick,radar.host_platform,radar) function

            for (int i = radarbaselist.Count - 1; i >= 0; i--)
            {
                Bgr actual_pixelValue = GetPixelBgr(image_actual, (int)radarbase.position.X, (int)radarbase.position.Y);

                if (actual_pixelValue.Red == 255)

                    pul_index = (int)actual_pixelValue.Blue;

                if (pulse_dictionary.ContainsKey(pul_index))
                {
                    Pulse pulse = pulse_dictionary[pul_index]; // public  void Collide_radar(int tick, int latest_radar_transmit_tick, RadarBase rb,Radar radar)
                    pulse.Collide_radar(tick, latest_radar_transmission_tick,  radarbase, pulse_radar); ;
                }// public void Collided_Target(double target_azimuth, List<List<double>> RadarCrossSection,Platform pt)
                pulse_dictionary.Remove(i);
            }

            static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
            {
                return degrees * (Math.PI / 180);
            }



            for (int i = radarbaselist.Count - 1; i >= 0; i--)
            {
                //Blue Radar
                CvInvoke.Circle(image_actual, new Point((int)radarbaselist[i].position.X, (int)radarbaselist[i].position.Y), 3, new MCvScalar(255, 0, 0), -1);

                if (tick % pulse_radar.Pri == 0)
                //if (tick == 0)
                {
                    //Pulse position should be equal to radar base
                    //create a pulse
                    pul_index += 1;
                    double temp_velocity = 1;
                    double vel_x = temp_velocity * Math.Cos(DegreesToRadians(((Radar)radarbase.onboardSensor[0]).azimuth));
                    double vel_y = temp_velocity * Math.Sin(DegreesToRadians(((Radar)radarbase.onboardSensor[0]).azimuth));


                    //creating pulse                                                                                                                           radar.peak_transmit_power*radar.gain_table[radar.frequency,0]

                    Pulse pulse = new Pulse(current_pulse_id, new Vector(radarbaselist[i].position.X, radarbaselist[i].position.Y), new Vector(vel_x, vel_y),( pulse_radar.peak_transmission_power *(int)pulse_radar.Gain_table[pulse_radar.frequency][0]), pulse_radar, pulse_radar.Pwd, pulse_radar.frequency, 0.0, 0.01, 0.0, 0.0, pulse_radar.azimuth);
                    //pulse position should be rdar position so it achieve by .x and .y individually 

                    //// add that pulse in list of pulses
                    pulse_dictionary.Add(pul_index, value: pulse);
                    latest_radar_transmission_tick = tick;
                    current_pulse_id += 1;
                    if (current_pulse_id >= 250)
                    {
                        current_pulse_id = 0;
                    }
                }
                CvInvoke.Circle(image_visual, new Point((int)radarbaselist[i].position.X, (int)radarbaselist[i].position.Y), 3, new MCvScalar(0, 0, 255), -1);

            }
            
                tick += 1;

                CvInvoke.Imshow("Visualise", image_visual);
                CvInvoke.Imshow("Visualisation", image_actual);

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


     
}



