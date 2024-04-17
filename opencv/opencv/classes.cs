using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace radar;    // im giving this namespace name as radar

public abstract class BattleSystem
{
    protected BattleSystem(string id)
    {
        Id = id;
    }
    public string Id;
    public abstract void Set(string id);
    public abstract string Get();

    // Abstract method for OnTick
    public abstract void OnTick();
}

public class Platform : BattleSystem
{
    public Vector Position;
    public double Speed;
    public double Heading;
    public List<Waypoint> Waypoints;
    public static List<Sensor> OnboardSensor;
    public List<List<double>> RadarCrossSection;

    public Platform(string id, Vector position, double speed, double heading, List<Waypoint> waypoints, List<Sensor> OnBoardsensor) 
        : base(id)
    {
        Position = position;
        Speed = speed;
        Heading = heading;
        Waypoints = waypoints;
        OnboardSensor = OnBoardsensor;
        RadarCrossSection = CreateRadarCrossSection();
    }

    public void MovePlatform()
    {
        // Implement logic for moving the platform based on speed, heading, and waypoints
        Console.WriteLine("Platform is moving!");
    }

    public List<List<double>> CreateRadarCrossSection()
    {
        List<List<double>> table = new List<List<double>>();

        for (int i = 0; i <= 360; i++)
        {
            List<double> row = new List<double>();

            for (double elevation = 0.5; elevation <= 18.5; elevation += 0.5)
            {
                row.Add(1); // Add 1 to the current row (all values are 1)
            }

            table.Add(row);
        }

        return table;
    }

    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Platform
        Console.WriteLine("Platform is performing OnTick operation");
    }
}

public class Vector
{
    public double X;
    public double Y;
    public  Vector(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public class Waypoint
{
    public int Location;

    public Waypoint() { }

    public Waypoint(int location)
    {
        Location = location;
    }
}

public class Sensor : BattleSystem
{
    public Platform HostPlatform;
    public Sensor(string id, Platform platform) : base(id)
    {
        HostPlatform = platform;
    }

    public virtual List<object> Detect()
    {
        // Implement logic for detecting targets based on sensor type (e.g., radar)
        Console.WriteLine("Sensor is detecting...");
        return new List<object>(); // Placeholder, replace with actual detected objects
    }

    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("Sensor is performing OnTick operation");
    }
}

public class Radar : Sensor
{

    public string OperatingMode;
    public string Antenna;
    public string Modulation;
    public double Elevation;
    public  static double Azimuth;
    public  static double Frequency;
    public int Pri; // Pulse Repetition Interval
    public double Pwd; // Pulse Width Duration
    public string AntennaScanPattern;
    public List<object> Detected;
    public double Detection_Range;
    public double Detectability_Range ;
    public double Resolution_Cell ;
    public double Minimum_Range; 
    public double Max_Unambiguous_Range; 

    public List<List<double>> Gain_table;
    private int resolution_cell;

    public Radar(string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range,double detectability_range,double resenution_cell, double minimum_range,double  max_unamb_range/*, List<List<double>> gain_table*/) 
        : base(id, platform)
    {

        OperatingMode = operatingMode;
        Antenna = antenna;
        Modulation = modulation;
        Elevation = elevation;
        Azimuth = azimuth;
        Frequency = frequency;
        Pri = pri;
        Pwd = pwd;
        AntennaScanPattern = antennaScanPattern;
        Detected = new List<object>();
        Detection_Range = detection_range;
        Detectability_Range = detectability_range;
        Resolution_Cell = resolution_cell;
        Minimum_Range = minimum_range;
        Max_Unambiguous_Range = max_unamb_range;
     Gain_table = CreateGainTable();

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
    private List<List<double>> CreateGainTable()
    {
        List<List<double>> table = new List<List<double>>();

        for (int i = 0; i <= 360; i++)
        {
            // double azimuth = i;

            List<double> row = new List<double>();

            for (double frequency = 0.5; frequency <= 18.5; frequency += 0.5)
            {
                double gain = Math.Cos(DegreesToRadians(i));
                row.Add(gain);
            }

            table.Add(row);
        }

        return table;
    }
    static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

}


public class Pulsed_radar : Radar
{
  
    public int Pri; // Pulse Repetition Interval
    public double Pwd;
    public double prf;
    public double peak_transmission_power;

    public Pulsed_radar (string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range,  double peak_transmission_power) 
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    
    {
        Pri = pri; // Pulse Repetition Interval
        Pwd = pwd;
        prf = prf;
        peak_transmission_power = peak_transmission_power;

    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
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
    public Continous_wave(string id, Platform platform, double transmitted_frequency, double received_frequency, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range)
          : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    {
        TransmittedFrequency = transmitted_frequency; // Initialize Transmitted Frequency
        ReceivedFrequency = received_frequency; // Initialize Received Frequency
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
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

    public Pulse_Doppler(string id, Platform platform, string operatingMode,string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern,double detection_range,double detectability_range,double resolution_cell, double minimum_range,double  max_unamb_range) /*List<List<double>> Gain_table*/
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    { 

    }
public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("pulse-dopler is performing OnTick operation");
    }

}



public class Aircraft : Platform
{
    private string id;

    public override void Set(string id)
    {
        this.id = id;
    }

    public override string Get()
    {
        return id;
    }
    public Aircraft(string id, Vector position, double Speed, double Heading, List<Waypoint> Waypoints, List<Sensor> OnboardSensor /*,double radar_cross_section*/) 
        : base(id, position, Speed, Heading, Waypoints, OnboardSensor)
    {


    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Aircraft
        Console.WriteLine("Aircraft is performing OnTick operation");
    }
}
public class RadarBase : Platform
{
    public string id;


    public override void Set(string id)
    {
        this.id = id;
    }

    public override string Get()
    {
        return id;
    }
    public RadarBase(string id, Vector position, double Speed, double Heading, List<Waypoint> Waypoints, List<Sensor> OnboardSensor) 
        : base(id, position, Speed, Heading, Waypoints, OnboardSensor)
    {

    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to RadarBase
        Console.WriteLine("RadarBase is performing OnTick operation");
    }
}
public class Pulse
{
    public int Id;
    public Vector position;
    public Vector velocity;
    public double energy = 0;
    public string source;
    public double pwd;
    public double frequency;
    public double beam_width;
    public double beam_width_vel;
    public double distance_travelled;
    public double elevation;
    public static  double azimuth;
    

    public Pulse(int id, Vector position, Vector velocity,double energy,string source, double pwd, double frequency, double beam_width, double beam_width_vel,double Distance_travelled, double elevation,double azimuth)
    {
        Id = id;
        position = position;
        velocity = velocity;
        energy = energy;
        source = source;
        pwd = pwd;
        frequency = frequency;
        beam_width = beam_width;
        distance_travelled = distance_travelled;
        elevation = elevation;
        azimuth = azimuth;

    }
    public void Move()
    {
        double time_diff = 1.0;
        position.X += velocity.X;
        position.Y += velocity.Y;
        distance_travelled += Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) * time_diff;
        beam_width += beam_width_vel;

    }
    public void reverse()
    {
        velocity.X = -velocity.X;
        velocity.Y = -velocity.Y;
    }



    /* public void colloid_radar(int tick,int latest_radar_transmission_tick)
      {
         double time_diff = latest_radar_transmission_tick - tick;
          double Target_distance=Magnitude(Velocity)*time_diff/2;
          Console.WriteLine(Target_distance);
      }

      /* public void CollidedTarget(double targetAzimuth)
       {
           // Update velocity
           Velocity.X = -Velocity.X;
           Velocity.Y = -Velocity.Y;

           // Reset beam width
           beam_width = 0;

           // Calculate new energy
           double gain = Gain_table[radar.Frequency, 0];
           double radarCrossSection = RadarCrossSection[radar.Frequency, (int)(targetAzimuth - azimuth)];
           double pi = Math.PI;

           double Energy = Energy * (gain * radarCrossSection) / (4 * pi * Math.Pow(Distance_travelled, 2));

           // Update azimuth
           azimuth += 180;

           // Reset distance travelled
           Distance_travelled = 0;
       }*/
    /* public static double Magnitude(Vector Velocity.X, Vector Velocity.Y)
     {
         return Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2));
     }

     // Function to calculate energy based on the provided equation
     public static double CalculateEnergy(double energy, double[,] gain_table, double radar_frequency, double pulse_azimuth, double radar_azimuth, double distance_travelled, double velocityMagnitude)
     {
         double pi = Math.PI;
         double lambda = velocityMagnitude / radar_frequency;
         double energyFactor = energy * (gain_table[(int)radar_frequency, (int)(pulse_azimuth - radar_azimuth)] * Math.Pow(lambda, 2)) / Math.Pow(4 * pi * distance_travelled, 2);
         return energyFactor;
     }
     public void Collide_radar( int tick, int latest_radar_transmit_tick,)
     {


         double time_diff = latest_radar_transmit_tick - tick;
         double velocityMagnitude = Magnitude(vel_X, veL_Y);
         double target_distance = velocityMagnitude * time_diff / 2;

         double target_x = radar_base.x + (target_distance * Math.Cos(DegreesToRadians(((Radar)radarbase.OnboardSensor[0]).Azimuth)); 
         double target_y = radar_base_y + (target_distance * Math.Sin(DegreesToRadians(((Radar)radarbase.OnboardSensor[0]).Azimuth));

         Console.WriteLine("Target's x coordinate: " + target_x);
         Console.WriteLine("Target's y coordinate: " + target_y);

         double distance_travelled = target_distance; // Assuming distance travelled is the same as distance to target
         double energyFactor = CalculateEnergy(Energy, Gain_table, Radar.frequency, 0, Radar.azimuth, distance_travelled, velocityMagnitude);

     }
     public void Collided_Target(double target_azimuth)
     {
         double pi = Math.PI;
         Velocity.X = -Velocity.X;
         Velocity.Y = -Velocity.Y;
         beam_width = 0;
         Energy *= (Gain_table[(int)frequency, 0] * RadarCrossSection[(int)frequency, (int)(target_azimuth - azimuth)]) / (4 * pi * Math.Pow(Distance_travelled, 2));
        azimuth += 180;
         Distance_travelled = 0;
     }*/
    public void Collided_Target(double target_azimuth, List<List<double>> RadarCrossSection, List<double> table, List<double> row)
    {
        velocity.X = -velocity.X;
        velocity.Y = -velocity.Y;
        beam_width = 0;

        // energy = energy * (RadarCrossSection[frequency, (target_azimuth - table[azimuth]]) / (4 * Math.PI * Math.Pow(distance_travelled, 2));
        energy = energy * (RadarCrossSection[frequency, (target_azimuth - azimuth)]) / (4 * Math.PI * Math.Pow(distance_travelled, 2));
        azimuth += 180;
        distance_travelled = 0;
    }
    static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
    {
        return degrees * (Math.PI / 180);
    }


    public void Collide_radar(int tick, int latest_radar_transmit_tick, double frequency)
    {
       double time_diff = latest_radar_transmit_tick - tick;
        double target_distance = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) * time_diff / 2;
        //Target x coordinate = radar_base’s x coordinate + (target_distance * cosine (radar.azimuth)
        double target_x_coordinate = RadarBase.X + (target_distance * Math.Cos(DegreesToRadians((Radar.Azimuth);
        double target_y_coordinate = RadarBase.Y + (target_distance * Math.Sin(DegreesToRadians((Radar.Azimuth);

        Console.WriteLine("Target's x coordinate: " + target_x_coordinate);
        Console.WriteLine("Target's y coordinate: " + target_y_coordinate);

        double lambda = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) / Radar.Frequency;

    }




}

    public class Wepons : BattleSystem
{
    public Platform HostPlatform;
    public Wepons(string id, Platform platform) : base(id)
    {
        HostPlatform = platform;
    }


    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("wepons is performing OnTick operation");
    }
}
public class Guns : Wepons
{
    public string Id;
    public double elivation;
    public double azimuth;
    public Guns(string id, Platform platform, double Elivation, double Azimuth) : base(id, platform)
    {
        Id = id;
        elivation = Elivation;
        azimuth = Azimuth;
    }
}

public class Missiles : Wepons
{

    public Vector Position;
    public double Speed;
    public double Heading;
    public List<Waypoint> Waypoints;
    public Missiles(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints) : base(id, platform)
    {
        Position = Position;
        Speed = Speed;
        Heading = heading;
        Waypoints = waypoints;
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("Sensor is performing OnTick operation");
    }
}
public class AAA : Guns
{
    public double shell_fuses_delay;
    public AAA(string id, Platform platform, double shell_fuses_delay, double Elivation, double Azimuth) : base(id, platform, Elivation, Azimuth)
    {
        shell_fuses_delay = shell_fuses_delay;
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("aaa is performing OnTick operation");
    }
}
public class Radar_guided : Missiles
{

    public Radar_guided(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints) : base(id, platform, Position, Speed, heading, waypoints)

    {

    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Active_guider : Missiles
{
    double onboard_radar;
    public Active_guider(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints, double onboard_radar) : base(id, platform, Position, Speed, heading, waypoints)

    {
        onboard_radar = onboard_radar;
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Image_guidence : Missiles
{
    double onboard__image_sensor;
    public Image_guidence(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints, double onboard_image_sensor) : base(id, platform, Position, Speed, heading, waypoints)

    {
        onboard_image_sensor = onboard_image_sensor;
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Gps_guidence : Missiles
{

    public Gps_guidence(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints) : base(id, platform, Position, Speed, heading, waypoints)

    {

    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Semiacive_guidence : Missiles
{
    double onboard__image_sensor;
    public Semiacive_guidence(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints, double onboard_receiver) : base(id, platform, Position, Speed, heading, waypoints)

    {
        onboard_receiver = onboard_receiver;
    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Passive_guidence : Missiles
{

    public Passive_guidence(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints) : base(id, platform, Position, Speed, heading, waypoints)

    {

    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class Terrian_following_guidence : Missiles
{

    public Terrian_following_guidence(string id, Platform platform, Vector Position, double Speed, double heading, List<Waypoint> waypoints) : base(id, platform, Position, Speed, heading, waypoints)

    { 


    }
    public override void Set(string id)
    {
        Id = id;
    }

    public override string Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}











