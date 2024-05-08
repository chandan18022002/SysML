using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using battle_frame_work;
using sensor;
using platform;
using radar;
using missile;
using aircraft;
using pulse;
//using radar;         // here im using radar namespace of other ( claseses) file 


class main_class
{
    static void Main(string[] args)
    {
        //Create an empty list of pulses,radarbase,aircraft
        // List<Pulse> pulselist = new List<Pulse>();
        List<RadarBase> radarbaselist = new List<RadarBase>();
        List<Aircraft> aircraftlist = new List<Aircraft>();
        List<Pulsed_radar> pulse_radar_list = new List<Pulsed_radar>();

        //Dictionary of the pulse
        Dictionary<int, Pulse> pulse_dictionary = new Dictionary<int, Pulse>();
        Dictionary<int, Missiles> missile_dictionary = new Dictionary<int, Missiles>();

        // RADAR BASE class initialsization

        RadarBase radarbase = new RadarBase(0, 0, 0, [new Vector(100, 300)], []);
        radarbaselist.Add(radarbase);


        //radAR  class initialsization
        Pulsed_radar pulse_radar = new Pulsed_radar(0, radarbase, "operating_mode", "antenna_type", "none", 0, 0, 1.5, 500, 1.5, "antenna_scan_pattern", 100, 200, 1, 1, 100, 10);
        radarbase.onboardSensor.Add(pulse_radar);// assigning the onboardsensor to a radar
        pulse_radar_list.Add(pulse_radar);

        //aircraft class initialsization
        Aircraft aircraft = new Aircraft(0, .1, 0, [new Vector(500, 300), new Vector(500, 500), new Vector(500, 100)], []);
        aircraftlist.Add(aircraft);                      //here set aircraft pos as same as radar pos bz max unamb range is =500 so that aircraft pos is radarpos+unamgious rangee

        Aircraft aircraft1 = new Aircraft(1, .1, 0, [new Vector(100, 100)], []);
        aircraftlist.Add(aircraft1);

        Radar_guided rdr_guided = new Radar_guided(1, aircraftlist[1], aircraftlist[1].position, 0.05, 0.0, [], true, aircraftlist[1].position);
        //missile_dictionary.Add(rdr_guided);
        int mis_id = rdr_guided.Id;
        missile_dictionary.Add(mis_id, rdr_guided);



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

            Mat image_missile = new Mat(800, 1000, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            image_missile.SetTo(new Bgr(0, 0, 0).MCvScalar);

            for (int i = pulse_dictionary.Count - 1; i >= 0; i--)
            {
                //Red Pulse
                int currentKey = pulse_dictionary.Keys.ElementAt(i);
                Pulse pulse = pulse_dictionary[currentKey];
                int x1 = (int)(pulse.position.X + (pulse.beam_width * Math.Cos(DegreesToRadians(pulse.azimuth + 90))));
                int y1 = (int)(pulse.position.Y + (pulse.beam_width * Math.Sin(DegreesToRadians(pulse.azimuth + 90))));
                int x2 = (int)(pulse.position.X + (pulse.beam_width * Math.Cos(DegreesToRadians(pulse.azimuth - 90))));
                int y2 = (int)(pulse.position.Y + (pulse.beam_width * Math.Sin(DegreesToRadians(pulse.azimuth - 90))));
                CvInvoke.Line(image_actual, new Point(x1, y1), new Point(x2, y2), new MCvScalar(currentKey, 0, 255), 3);
                CvInvoke.Line(image_visual, new Point(x1, y1), new Point(x2, y2), new MCvScalar(currentKey, 0, 255), 3);
                //CvInvoke.Circle(image_visual, new Point((int)pulse.position.X, (int)pulse.position.Y), 3, new MCvScalar(currentKey, 0, 255), -1);
                //CvInvoke.Circle(image_actual, new Point((int)pulse.position.X, (int)pulse.position.Y), 3, new MCvScalar(currentKey, 0, 255), -1);
                pulse.Move();
            }

            for (int i = aircraftlist.Count - 1; i >= 0; i--)
            {
                Bgr actual_pixelValue = GetPixelBgr(image_actual, (int)aircraftlist[i].position.X, (int)aircraftlist[i].position.Y);
                if (actual_pixelValue.Red == 255)
                {
                    pul_index = (int)actual_pixelValue.Blue;
                    if (pulse_dictionary.ContainsKey(pul_index))
                    {
                        Pulse pulse = pulse_dictionary[pul_index];
                        //pulse.Collided_Target(aircraft.heading, RadarCrossSection);
                        pulse.Collided_Target(aircraftlist[i]);
                    }
                }
                //Green Aircraft
                aircraftlist[i].MovePlatform();
                CvInvoke.PutText(image_visual, Math.Round(aircraftlist[i].position.X, 2).ToString() + "," + Math.Round(aircraftlist[i].position.Y, 2).ToString(), new Point((int)aircraftlist[i].position.X + 10, (int)aircraftlist[i].position.Y + 10), FontFace.HersheySimplex, 1.0, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Circle(image_visual, new Point((int)aircraftlist[i].position.X, (int)aircraftlist[i].position.Y), 3, new MCvScalar(0, 255, 0), -1);
                CvInvoke.Circle(image_missile, new Point((int)aircraftlist[i].position.X, (int)aircraftlist[i].position.Y), 3, new MCvScalar(0, 255, 0), -1);

            }
            for (int i = missile_dictionary.Count - 1; i >= 0; i--)
            {
                Bgr actual_pixelValue = GetPixelBgr(image_missile, (int)aircraftlist[i].position.X, (int)aircraftlist[i].position.Y);
                if (actual_pixelValue.Green == 255)
                {
                    mis_id = (int)actual_pixelValue.Blue;
                    if (missile_dictionary.ContainsKey(mis_id))
                    {
                        Aircraft air = aircraftlist[mis_id];
                        //print(missile of id - {it’s id} has collided with an aircraft of id - {it’s id} )
                        //pulse.Collided_Target(aircraft.heading, RadarCrossSection);
                        Console.WriteLine($"Missile of id - {mis_id} has collided with an aircraft of id - {aircraftlist[i].Id}");
                        aircraftlist.Remove(aircraftlist[i]);
                        missile_dictionary.Remove(mis_id);

                    }
                    missile_dictionary.Remove(i);
                }


                CvInvoke.Circle(image_visual, new Point((int)missile_dictionary[i].position.X, (int)missile_dictionary[i].position.Y), 3, new MCvScalar(0, 255, 255), -1);
                //CvInvoke.Crcle(image_visual, new Point((int)aircraftlist[i].position.X, (int)aircraftlist[i].position.Y), 3, new MCvScalar(0, 255, 0), -1);
            }




            for (int i = pulse_radar_list.Count - 1; i >= 0; i--)
            {
                RadarBase current_radarbase = ((RadarBase)(pulse_radar_list[i].hostPlatform));
                Bgr actual_pixelValue_0 = GetPixelBgr(image_actual, (int)current_radarbase.position.X, (int)current_radarbase.position.Y);

                if (actual_pixelValue_0.Red == 255)
                {

                    pul_index = (int)actual_pixelValue_0.Blue;

                    if (pulse_dictionary.ContainsKey(pul_index))
                    {
                        Pulse pulse = pulse_dictionary[pul_index];

                        Vector target_pos;
                        double[] collided_pos = collide_radar(tick, latest_radar_transmission_tick, radarbaselist, pulse_radar_list);
                        target_pos = new Vector((float)collided_pos[0], (float)collided_pos[1]);


                        // Assign values to target_pos
                        // target_pos.X = target_x_coordinate;
                        // target_pos.Y = target_y_coordinate;
                        //  double[] target_pos= double target_x_coordinate, double target_y_coordinate;
                        //(double target_x_coordinate, double target_y_coordinate) = Collide_radar(tick, latest_radar_transmit_tick, radar, pulsar);
                        //
                        //pulse_radar_list[i].azimuth= pulse.Collide_radar(tick, latest_radar_transmission_tick, radarbase, pulse_radar_list[i]);
                        for (int j = missile_dictionary.Count - 1; j >= 0; i--)
                        {
                            // (double target_x_coordinate, double target_y_coordinate) = missile_dictionary[j].target_coordinates;

                            target_pos = missile_dictionary[j].target_coordinates;
                        }
                        pulse_dictionary.Remove(pul_index); // Remove the pulse from the dictionary after processing


                    }
                    pulse_dictionary.Remove(i);
                }
                //RadarBase current_radar__base = pulse_radar_list[i];
                //current radar base is onboard sensor in pulse_radar_list[i]
                // Use current radar base position for circle
                // CvInvoke.Circle(image_actual, new Point((int)current_radarbase.position.X, (int)current_radarbase.position.Y), 3, new MCvScalar(255, 0, 0), -1);
                // use pulse_radar_list[i] instead of pulse_radar
                if (tick % pulse_radar_list[i].pri == 0)
                //if (tick == 0)
                {
                    //Pulse position should be equal to radar base
                    //create a pulse
                    pul_index = current_pulse_id; ;
                    double temp_velocity = 4;
                    double vel_x = temp_velocity * Math.Cos(DegreesToRadians((pulse_radar_list[i].azimuth)));
                    double vel_y = temp_velocity * Math.Sin(DegreesToRadians((pulse_radar_list[i].azimuth)));


                    //creating pulse                                                                                                                          

                    // Pulse pulse = new Pulse(current_pulse_id, new Vector(radarbaselist[i].position.X, radarbaselist[i].position.Y), new Vector(vel_x, vel_y),
                    //  ( pulse_radar.peak_transmission_power *(int)pulse_radar.Gain_table[pulse_radar.frequency][0]), pulse_radar, pulse_radar.Pwd, pulse_radar.frequency, 0.0, 0.01, 0.0, 0.0, pulse_radar.azimuth);
                    //pulse position should be rdar position so it achieve by .x and .y individually 
                    // pulse_radar.frequency convert to index


                    double power = pulse_radar_list[i].peak_transmission_power * pulse_radar_list[i].Gain_table[pulse_radar_list[i].frequency][0];     //36 beacause it is index value of 18.5 frequency
                    //int powerInt = (int)power; // Convert double to int, truncating any fractional part

                    Pulse pulse = new Pulse(
                        current_pulse_id,
                        new Vector((int)current_radarbase.position.X, (int)current_radarbase.position.Y),
                        new Vector(vel_x, vel_y),
                        power, // Use the converted int value
                        pulse_radar_list[i],
                        pulse_radar_list[i].Pwd,
                        pulse_radar_list[i].frequency,
                        0.0,
                        0.08 * temp_velocity,
                        0.0,
                        0.0,
                        pulse_radar_list[i].azimuth
                    );

                    /*  for (int j = 0; j < 11; j++)
                      {
                          pulse.Move();
                      }*/
                    //// add that pulse in list of pulses
                    pulse_dictionary.Add(pul_index, pulse);

                    latest_radar_transmission_tick = tick;
                    current_pulse_id += 1;
                    if (current_pulse_id >= 255)
                    {
                        current_pulse_id = 0;
                    }
                }
                //RADAR
                //CvInvoke.PutText(image_visual, Math.Round(pulse_radar_list[i].azimuth,2).ToString(), new Point((int)radarbaselist[i].position.X+10, (int)radarbaselist[i].position.Y+10), FontFace.HersheySimplex, 1.0, new MCvScalar(255, 255, 255), 2);
                CvInvoke.Circle(image_visual, new Point((int)radarbaselist[i].position.X, (int)radarbaselist[i].position.Y), 3, new MCvScalar(255, 0, 0), -1);
            }


            tick += 1;

            CvInvoke.Imshow("Visual", image_visual);
            //CvInvoke.Imshow("actual", image_actual);
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




