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
    public Vector position;
    public double speed;
    public double heading;
    public List<Waypoint> waypoints;
    public List<Sensor> onboardSensor;
    public  List<List<double>> RadarCrossSection;

    public Platform(string id, Vector position, double speed, double heading, List<Waypoint> waypoints, List<Sensor> OnBoardsensor) 
        : base(id)
    {
        this.position = position;
        this.speed = speed;
        this.heading = heading;
        this.waypoints = waypoints;
        this.onboardSensor = OnBoardsensor;
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
        this.X = x;
        this.Y = y;
    }
}

public class Waypoint
{
    public int Location;

    public Waypoint() { }

    public Waypoint(int location)
    {
         this.Location = location;
    }
}

public class Sensor : BattleSystem
{
    public Platform hostPlatform;
    public Sensor(string id, Platform platform) : base(id)
    {
        this.hostPlatform = platform;
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

    public string operatingMode;
    public string antenna;
    public string modulation;
    public double elevation;
    public    double azimuth;
    public   double frequency;
    public int pri; // Pulse Repetition Interval
    public double pwd; // Pulse Width Duration
    public string antenna_scan_pattern;
    public List<object> detected;
    public double detection_Range;
    public double detectability_Range ;
    public double resolution_Cell ;
    public double minimum_Range; 
    public double max_Unambiguous_Range; 
   
    public List<List<double>> Gain_table;
    private int resolution_cell;

    public Radar(string id, Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range,double detectability_range, double resenution_cell, double minimum_range, double max_unamb_range/*, List<List<double>> gain_table*/)
        : base(id, platform)
    {

        this.operatingMode = operatingMode;
        this. antenna = antenna;
        this.modulation = modulation;
        this. elevation = elevation;
        this.azimuth = azimuth;
        this.frequency = frequency;
        this.pri = pri;
        this.pwd = pwd;
        this.antenna_scan_pattern = antennaScanPattern;
        this. detected = new List<object>();
        this. detection_Range = detection_range;
        this. detectability_Range = detectability_range;
        this. resolution_Cell = resolution_cell;
        this.minimum_Range = minimum_range;
        this.max_Unambiguous_Range = max_unamb_range;
        


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
        this.Pri = pri; // Pulse Repetition Interval
        this.Pwd = pwd;
        this.prf = prf;
        this.peak_transmission_power = peak_transmission_power;

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
        this.TransmittedFrequency = transmitted_frequency; // Initialize Transmitted Frequency
        this.ReceivedFrequency = received_frequency; // Initialize Received Frequency
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
    public Radar source;
    public double pwd;
    public double frequency;
    public double beam_width;
    public double beam_width_vel;
    public double distance_travelled;
    public double elevation;
    public  double azimuth;
    
    

    public Pulse(int id, Vector position, Vector velocity,double energy,Radar source, double pwd, double frequency, double beam_width, double beam_width_vel,double Distance_travelled, double elevation,double azimuth)
    {
        this.Id = id;
        this.position = position;
        this.velocity = velocity;
        this.energy = energy;
        this.source = source;
        this.pwd = pwd;
        this.frequency = frequency;
        this.beam_width = beam_width;
        this.distance_travelled = distance_travelled;
        this.elevation = elevation;
        this.azimuth = azimuth;
       
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
    
    public void Collided_Target(Aircraft aircraft)
    {
       velocity.X = -velocity.X;
       velocity.Y = -velocity.Y;
        beam_width = 0;
        this.energy = this.energy * (aircraft.RadarCrossSection[(int)frequency][(int)(aircraft.heading - this.azimuth)]) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));
        
        this.azimuth += 180;
        this.distance_travelled = 0;
    }
    static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
    {
        return degrees * (Math.PI / 180);
    }


    public  void Collide_radar(int tick, int latest_radar_transmit_tick, RadarBase rb,Pulsed_radar pradar)
    {
       double time_diff = latest_radar_transmit_tick - tick;
        double target_distance = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) * time_diff / 2;
        //Target x coordinate = radar_base’s x coordinate + (target_distance * cosine (radar.azimuth)
        double target_x_coordinate = rb.position.X + (target_distance * Math.Cos(DegreesToRadians(pradar.azimuth)));
        double target_y_coordinate = rb.position.Y+ (target_distance * Math.Sin(DegreesToRadians(pradar.azimuth)));
            
        Console.WriteLine("Target's x coordinate: " + target_x_coordinate);
        Console.WriteLine("Target's y coordinate: " + target_y_coordinate);

        double lambda = Math.Sqrt(Math.Pow(this.velocity.X, 2) + Math.Pow(this.velocity.Y, 2)) / pradar.frequency;
        this.energy = this.energy * (pradar.Gain_table[(int)frequency][(int)(this.azimuth - pradar.azimuth)]) * Math.Pow(lambda, 2) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));
      
    }




}

    public class Wepons : BattleSystem
{
    public Platform HostPlatform;
    public Wepons(string id, Platform platform) : base(id)
    {
      this.HostPlatform = platform;
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
    public string id;
    public double elivation;
    public double azimuth;
    public Guns(string id, Platform platform, double Elivation, double Azimuth) : base(id, platform)
    {
       this. Id = id;
       this. elivation = Elivation;
        azimuth = Azimuth;
    }
}

public class Missiles : Wepons
{

    public Vector position;
    public double speed;
    public double heading;
    public List<Waypoint> waypoints;
    public Missiles(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints) : base(id, platform)
    {
      this.position = position;
      this. speed = speed;
      this. heading = heading;
      this.waypoints = waypoints;
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
       this. shell_fuses_delay = shell_fuses_delay;
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

    public Radar_guided(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints) : base(id, platform, position, speed, heading, waypoints)

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
    public Active_guider(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints, double onboard_radar) : base(id, platform, position, speed, heading, waypoints)

    {
       this. onboard_radar = onboard_radar;
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
    public Image_guidence(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints, double onboard_image_sensor) : base(id, platform, position, speed, heading, waypoints)

    {
      this.onboard__image_sensor = onboard_image_sensor;
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

    public Gps_guidence(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints) : base(id, platform, position, speed, heading, waypoints)

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
    double onboard_receiver;
    public Semiacive_guidence(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints, double onboard_receiver) : base(id, platform, position, speed, heading, waypoints)

    {
       this. onboard_receiver = onboard_receiver;
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

    public Passive_guidence(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints) : base(id, platform, position, speed, heading, waypoints)

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

    public Terrian_following_guidence(string id, Platform platform, Vector position, double speed, double heading, List<Waypoint> waypoints) : base(id, platform, position, speed, heading, waypoints)

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
public class Esn :Sensor
{
    public double no_of_antenna;
    public double pwds;
    public double emmiter_records;
    public double antenna_configaration;
    public double reception_band;

    public Esn(string id, Platform platform, double no_of_antenna, double pwd, double emmiter_records, double antenna_configaration, double reception_band)
         : base(id, platform)
    {
        this.no_of_antenna = no_of_antenna;
        this.pwds = pwds;
        this. emmiter_records = emmiter_records;
        this.antenna_configaration = antenna_configaration;
        this.reception_band = reception_band;
    }
    public void generate_pwd()
    {

    }
    public void emmiter_record_generator ()
    {

    }
    public void due_to_countermeasure ()
    {

    }
}
public class Rwr : Sensor
{
    public double no_of_antenna;
    public double pwds;
    public double emmiter_records;
    public double antenna_configaration;
    public double reception_band;

    public Rwr(string id, Platform platform, double no_of_antenna, double pwd, double emmiter_records, double antenna_configaration, double reception_band)
         : base(id, platform)
    {
       this. no_of_antenna = no_of_antenna;
       this. pwds = pwds;
       this. emmiter_records = emmiter_records;
        this.antenna_configaration = antenna_configaration;
       this. reception_band = reception_band;
    }
    public void generate_pwd()
    {

    }
    public void emmiter_record_generator()
    {

    }
    public void due_to_countermeasure()
    {

    }
}












