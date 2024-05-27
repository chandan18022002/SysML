//using radar;
using System.Collections.Generic;
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using weapon;
using platform;
using generics;
namespace missile;
public class Missiles : Wepons
{

    public Vector position;
    public double speed;
    public double heading;
    public List<Vector> waypoints;
    public bool released;
    public Vector target_coordinates;
    public Missiles(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, bool released, Vector target_coardinates)
        : base(id, platform)
    {
        this.position = position;
        this.speed = speed;
        this.heading = heading;
        this.waypoints = waypoints;
        this.released = released;
        this.target_coordinates = target_coardinates;
    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("Sensor is performing OnTick operation");
    }

    // Define targetCoordinates using integer coordinates
  
    public void move_missile()
    {
        if (this.released && (this.target_coordinates.X != this.position.X || this.target_coordinates.Y != this.position.Y))
        {
            double temp_dis = Math.Sqrt(Math.Pow(target_coordinates.X - this.position.X, 2) + Math.Pow(this.target_coordinates.Y - this.position.Y, 2));
            this.position.X += this.speed * (this.target_coordinates.X - this.position.X) / temp_dis;
            this.position.Y += this.speed * (this.target_coordinates.Y - this.position.Y) / temp_dis;
        }
    }
}
public class RadarGuided : Missiles
{


    public RadarGuided(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {

    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class ActiveGuider : Missiles
{
    double onboard_radar;
    public ActiveGuider(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, double onboard_radar, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {
        this.onboard_radar = onboard_radar;
    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class ImageGuidence : Missiles
{
    double onboard__image_sensor;
    public ImageGuidence(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, double onboard_image_sensor, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {
        this.onboard__image_sensor = onboard_image_sensor;
    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }
    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class GpsGuidence : Missiles
{

    public GpsGuidence(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {

    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }
    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class SemiaciveGuidence : Missiles
{
    double onboard_receiver;
    public SemiaciveGuidence(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, double onboard_receiver, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {
        this.onboard_receiver = onboard_receiver;
    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }
    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class PassiveGuidence : Missiles
{

    public PassiveGuidence(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {

    }
    public override void Set(List<Pair<string, string>> param)
    {
    }
    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}
public class TerrianFollowingGuidence : Missiles
{
    public TerrianFollowingGuidence(string id, platform.Platform platform, Vector position, double speed, double heading, List<Vector> waypoints, bool released, Vector target_coardinates) : base(id, platform, position, speed, heading, waypoints, released, target_coardinates)

    {


    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("", ""),
        };
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("radar guided is performing OnTick operation");
    }
}