using radar;
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace radars;

public class Radar : Sensor
{

    public string operatingMode;
    public string antenna;
    public string modulation;
    public double elevation;
    public double azimuth;
    public double frequency;
    public int pri; // Pulse Repetition Interval
    public double pwd; // Pulse Width Duration
    public string antenna_scan_pattern;
    public List<object> detected;
    public double detection_Range;
    public double detectability_Range;
    public double resolution_Cell;
    public double minimum_Range;
    public double max_Unambiguous_Range;

    public Dictionary<double, double[]> Gain_table;
    public List<(double x, double y, int tick)> latest_five_target_coordinates { get; private set; }

    public Radar(int id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resenution_cell, double minimum_range, double max_unamb_range/*, List<List<double>> gain_table*/)
        : base(id, platform)
    {

        this.operatingMode = operatingMode;
        this.antenna = antenna;
        this.modulation = modulation;
        this.elevation = elevation;
        this.azimuth = azimuth;
        this.frequency = frequency;
        this.pri = pri;
        this.pwd = pwd;
        this.antenna_scan_pattern = antennaScanPattern;
        this.detected = new List<object>();
        this.detection_Range = detection_range;
        this.detectability_Range = detectability_range;
        this.minimum_Range = minimum_range;
        this.max_Unambiguous_Range = max_unamb_range;

        Gain_table = CreateGainTable();
        latest_five_target_coordinates = new List<(double x, double y, int tick)>();
    }

    public void Transmit()
    {
        // Implement logic for transmitting a signal from the radar
        Console.WriteLine("Radar is transmitting...");
    }

    public object Receive()
    {
        // Implement logic for receiving a signal by the radar
        Console.WriteLine("Radar is receiving...");
        return new object(); // Placeholder for received signal
    }



    private Dictionary<double, double[]> CreateGainTable()
    {
        Dictionary<double, double[]> Gain_Table = new Dictionary<double, double[]>();

        // Populate the dictionary
        for (double keys = 0.5; keys <= 18.5; keys += 0.5)
        {
            double[] cosineValues = new double[360];
            for (int azimuth = 0; azimuth < 360; azimuth++)
            {
                double radians = DegreesToRadians(azimuth + 1); // Convert angle to radians
                cosineValues[azimuth] = Math.Cos(radians);
            }
            Gain_Table.Add(keys, cosineValues);
        }

        return Gain_Table; // Return the populated dictionary
    }

    static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
    public void UpdateLatestTargetCoordinates(double x, double y, int tick)
    {
        if (latest_five_target_coordinates.Count == 5)
        {
            latest_five_target_coordinates.RemoveAt(0); // Remove oldest entry if the list is full
        }
        latest_five_target_coordinates.Add((x, y, tick)); // Add new coordinates
    }
}

public class Pulsed_radar : Radar
{

    public int Pri; // Pulse Repetition Interval
    public double Pwd;
    public double prf;
    public double peak_transmission_power;

    public Pulsed_radar(int id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range, double peak_transmission_power)
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)

    {
        this.Pri = pri; // Pulse Repetition Interval
        this.Pwd = pwd;
        this.prf = prf;
        this.peak_transmission_power = peak_transmission_power;

    }
    public override void Set(int id)
    {
        Id = id;
    }

    public override int Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("pulse radar is performing OnTick operation");
    }

}
public class Continous_wave : Radar
{
    public double TransmittedFrequency;
    public double ReceivedFrequency;
    public Continous_wave(int id, Platform platform, double transmitted_frequency, double received_frequency, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range)
          : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    {
        this.TransmittedFrequency = transmitted_frequency; // Initialize Transmitted Frequency
        this.ReceivedFrequency = received_frequency; // Initialize Received Frequency
    }
    public override void Set(int id)
    {
        Id = id;
    }

    public override int Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("continious wave is performing OnTick operation");
    }

}

public class Pulse_Doppler : Radar
{

    public Pulse_Doppler(int id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range) /*List<List<double>> Gain_table*/
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    {

    }
    public override void Set(int id)
    {
        Id = id;
    }

    public override int Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("pulse-dopler is performing OnTick operation");
    }

}
public class Pulse
{
    public int Id;
    public Vector position;
    public Vector velocity;
    public double energy = 0;
    public Radar source;
    public double pwd;
    public double frequency;
    public double beam_width;
    public double beam_width_vel;
    public double distance_travelled;
    public double elevation;
    public double azimuth;



    public Pulse(int id, Vector position, Vector velocity, double energy, Radar source, double pwd, double frequency, double beam_width, double beam_width_vel, double Distance_travelled, double elevation, double azimuth)
    {
        this.Id = id;
        this.position = position;
        this.velocity = velocity;
        this.energy = energy;
        this.source = source;
        this.pwd = pwd;
        this.frequency = frequency;
        this.beam_width = beam_width;
        this.beam_width_vel = beam_width_vel;
        this.elevation = elevation;
        this.azimuth = azimuth;

    }

    public void Move()
    {
        double time_diff = 1.0;
        this.position.X += this.velocity.X;
        this.position.Y += this.velocity.Y;
        this.distance_travelled += Math.Sqrt(Math.Pow(this.velocity.X, 2) + Math.Pow(this.velocity.Y, 2)) * time_diff;
        this.beam_width += this.beam_width_vel;

    }
    public void reverse()
    {
        velocity.X = -velocity.X;
        velocity.Y = -velocity.Y;
    }

    public void Collided_Target(Aircraft target)
    {
        this.velocity.X = -this.velocity.X;
        this.velocity.Y = -this.velocity.Y;
        this.position.X = target.position.X;
        this.position.Y = target.position.Y;
        this.beam_width = 0;
        int temp_azimuth = (int)(target.heading - this.azimuth);
        while (temp_azimuth < 0)
        {
            temp_azimuth += 360;
        }
        if (temp_azimuth > 360)
        {
            temp_azimuth = temp_azimuth % 360;
        }
        double temp_val = target.RadarCrossSection[(int)this.frequency][temp_azimuth];
        this.energy = this.energy * (temp_val) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));
        this.azimuth += 180;
        this.distance_travelled = 0;
        for (int j = 0; j < 11; j++)
        {
            this.Move();
        }


    }
    static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
    {
        return degrees * (Math.PI / 180);
    }
    public double CalculateDistance(RadarBase radarbase)
    {
        return Math.Sqrt(Math.Pow(radarbase.position.X - this.position.X, 2) + Math.Pow(radarbase.position.Y - this.position.Y, 2));
    }



    public void Collide_radar(int tick, int latest_radar_transmit_tick, RadarBase rb, Pulsed_radar pradar)
    {
        double time_diff = tick + 20 - latest_radar_transmit_tick;
        double target_distance = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) * time_diff / 2;
        //Target x coordinate = radar_base’s x coordinate + (target_distance * cosine (radar.azimuth)
        double target_x_coordinate = rb.position.X + (target_distance * Math.Cos(DegreesToRadians(pradar.azimuth)));
        double target_y_coordinate = rb.position.Y + (target_distance * Math.Sin(DegreesToRadians(pradar.azimuth)));

        double lambda = Math.Sqrt(Math.Pow(this.velocity.X, 2) + Math.Pow(this.velocity.Y, 2)) / pradar.frequency;
        this.energy = this.energy * (pradar.Gain_table[(int)frequency][(int)(this.azimuth - pradar.azimuth)]) * Math.Pow(lambda, 2) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));

       /* Console.WriteLine("Target's x coordinate: " + target_x_coordinate);
        Console.WriteLine("Target's y coordinate: " + target_y_coordinate);*/
        pradar.UpdateLatestTargetCoordinates(position.X, position.Y, latest_radar_transmit_tick);
        // Console.Write(pradar.latest_five_target_coordinates);
        Console.WriteLine("Latest Five Target Coordinates: " + string.Join(", ", pradar.latest_five_target_coordinates));


        double temp_distance = CalculateDistance(rb);
        if (temp_distance > 0)
        {
            double temp_x1 = position.X + (temp_distance * Math.Cos(DegreesToRadians(pradar.azimuth + 90)));
            double temp_y1 = position.Y + (temp_distance * Math.Sin(DegreesToRadians(pradar.azimuth + 90)));
            double temp_x2 = position.X + (temp_distance * Math.Cos(DegreesToRadians(pradar.azimuth - 90)));
            double temp_y2 = position.Y + (temp_distance * Math.Sin(DegreesToRadians(pradar.azimuth - 90)));

            double distanceToTemp1 = Math.Sqrt(Math.Pow(temp_x1 - rb.position.X, 2) + Math.Pow(temp_y1 - rb.position.Y, 2));
            double distanceToTemp2 = Math.Sqrt(Math.Pow(temp_x2 - rb.position.X, 2) + Math.Pow(temp_y2 - rb.position.Y, 2));
            //Console.Write(", delta : " + Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI));
            if (distanceToTemp1 < distanceToTemp2)
            {
                //pradar.azimuth -= Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI);
                pradar.azimuth += temp_distance * .1;
            }
            else
            {
                //pradar.azimuth += Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI);
                pradar.azimuth -= temp_distance * .1;

            }
            int target_x_coordinate_int = (int)target_x_coordinate;
            int target_y_coordinate_int = (int)target_y_coordinate;

            // Define targetCoordinates using integer coordinates
            Point target_Coordinates = new Point(target_x_coordinate_int, target_y_coordinate_int);

            //return target_x_coordinate,target_y_coordinate;
            //Console.WriteLine(", after : " + pradar.azimuth);
            Console.WriteLine("------------------------------------------------------------");

            Console.WriteLine("target_x_coordinate"+target_x_coordinate);
            Console.WriteLine("target_y_coordinate"+target_y_coordinate);

        }

    }
    public void move_missile(Point target_Coordinates, Missiles missiles)
    {
        if (missiles.released && (target_Coordinates.X != position.X || target_Coordinates.Y != position.Y))
        {
            double temp_dis = Math.Sqrt(Math.Pow(target_Coordinates.X - position.X, 2) + Math.Pow(target_Coordinates.Y - position.Y, 2));
           this. position.X += missiles.speed * (target_Coordinates.X - this.position.X) / temp_dis;
           this. position.Y += missiles.speed * (target_Coordinates.Y -this. position.Y) / temp_dis;
        }
    }

}
