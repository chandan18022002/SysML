using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
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
    public List<Sensor> OnboardSensor;
    public List<List<double>> RadarCrossSection;

    public Platform(string id, Vector position, double speed, double heading, List<Waypoint> waypoints, List<Sensor> OnBoardsensor) : base(id)
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

    private List<List<double>> CreateRadarCrossSection()
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
    public double Azimuth;
    public double Frequency;
    public int Pri; // Pulse Repetition Interval
    public double Pwd; // Pulse Width Duration
    public string AntennaScanPattern;
    public List<object> Detected;
    public int Detection_Range;
    public int Detectability_Range ;
    public int Resolution_Cell ;
    public int Minimum_Range; 
    public int Max_Unambiguous_Range; 

    public List<List<double>> Gain_table;
    private int resolution_cell;

    public Radar(string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, int detection_range,int detectability_range,int resenution_cell, int minimum_range,int  max_unamb_range/*, List<List<double>> gain_table*/) : base(id, platform)
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
    public string id;
    public int Pri; // Pulse Repetition Interval
    public double Pwd;
    public int prf;
    // public Pulsed_radar(string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, int pwd, string antennaScanPattern/* List<List<double>> Gain_table*/) : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern /*Gain_table*/)
    public Pulsed_radar (string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, int detection_range, int detectability_range, int resolution_cell, int minimum_range, int max_unamb_range) : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    
    {
        Pri = pri; // Pulse Repetition Interval
        Pwd = pwd;
        prf = prf;
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
    public Continous_wave(string id, Platform platform, double transmitted_frequency, double received_frequency, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, int detection_range, int detectability_range, int resolution_cell, int minimum_range, int max_unamb_range)
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

    public Pulse_Doppler(string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern,int  detection_range,int detectability_range,int resolution_cell, int minimum_range,int  max_unamb_range) /*List<List<double>> Gain_table*/ : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
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
    public Aircraft(string id, Vector position, double Speed, double Heading, List<Waypoint> Waypoints, List<Sensor> OnboardSensor /*,double radar_cross_section*/) : base(id, position, Speed, Heading, Waypoints, OnboardSensor)
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
    public RadarBase(string id, Vector position, double Speed, double Heading, List<Waypoint> Waypoints, List<Sensor> OnboardSensor) : base(id, position, Speed, Heading, Waypoints, OnboardSensor)
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
    public Vector Position;
    public Vector Velocity;
    public double energy;
    public string source;
    public double pwd;
    public double frequency;
    public double beam_width;
    public double beam_width_vel;



    public Pulse(int id, Vector position, Vector Velocity, double pwd, double frequency, double beam_width, double beam_width_vel)
    {
        Id = id;
        Position = position;
        this.Velocity = Velocity;
        pwd = pwd;
        frequency = frequency;
        beam_width = beam_width;
        beam_width_vel = beam_width_vel;
    }
    public void Move()
    {
        Position.X += Velocity.X;
        Position.Y += Velocity.Y;
    }
    public void reverse()
    {
        Velocity.X = -Velocity.X;
        Velocity.Y = -Velocity.Y;
    }

   public void colloid_radar(int tick,int latest_radar_transmission_tick)
    {
       double time_diff = latest_radar_transmission_tick - tick;
        double Target_distance=Magnitude(Velocity)*time_diff/2;
        Console.WriteLine(Target_distance);
    }

    private double Magnitude(Vector velocity)
    {
        throw new NotImplementedException();
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











