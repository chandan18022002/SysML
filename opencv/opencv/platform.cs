
//using radar;
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using battleFrameWork;
using sensor;
using generics;
namespace platform;

public class Platform : BattleSystem
{
    public Vector position;
    public double speed;
    public double heading;
    public List<Vector> waypoints;
    public int next_waypoint = 1;
    public List<Sensor> onboardSensor;
    public Dictionary<double, double[]> RadarCrossSection;

    public Platform(string id, double speed, double heading, List<Vector> waypoints, List<Sensor> OnBoardsensor)
        : base(id)
    {
        this.speed = speed;
        this.heading = heading;
        this.waypoints = waypoints;
        this.position = waypoints[0];
        this.onboardSensor = OnBoardsensor;
        RadarCrossSection = CreateRadarCrossSection();
    }

    public void MovePlatform()
    {
        if (waypoints.Count > 1)
        {
            double dis = Math.Sqrt(Math.Pow((this.position.X - this.waypoints[this.next_waypoint].X), 2) + Math.Pow((this.position.Y - this.waypoints[this.next_waypoint].Y), 2));
            if (dis > this.speed)
            {
                this.position.X += this.speed * (this.waypoints[this.next_waypoint].X - this.position.X) / dis;
                this.position.Y += this.speed * (this.waypoints[this.next_waypoint].Y - this.position.Y) / dis;
                this.heading = Math.Acos((this.waypoints[this.next_waypoint].X - this.position.X) / dis);
            }
            else
            {
                this.next_waypoint += 1;
                if (this.next_waypoint >= waypoints.Count)
                {
                    this.next_waypoint = 0;
                }
            }
        }
    }

    public Dictionary<double, double[]> CreateRadarCrossSection()
    {
        Dictionary<double, double[]> RadarCrossSection = new Dictionary<double, double[]>();

        // Populate the dictionary
        for (double keys = 0.5; keys <= 18.5; keys += 0.5)
        {
            double[] cosineValues = new double[360];
            for (int azimuth = 0; azimuth < 360; azimuth++)
            {
                cosineValues[azimuth] = 1; // Set the value to 1 for each azimuth angle
            }
            RadarCrossSection.Add(keys, cosineValues);
        }

        return RadarCrossSection; // Return the populated dictionary
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
        // Implement OnTick logic specific to Platform
        Console.WriteLine("Platform is performing OnTick operation");
    }
}

public class RadarBase : Platform
{
    public string id;


    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>() {
            new Pair<string, string>("Position_x", this.position.X.ToString()),
            new Pair<string, string>("Position_y", this.position.Y.ToString()),
        };
        return tmp_list;//this.position;
    }
    public RadarBase(string id, double Speed, double Heading, List<Vector> Waypoints, List<Sensor> OnboardSensor)
        : base(id, Speed, Heading, Waypoints, OnboardSensor)
    {

    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to RadarBase
        Console.WriteLine("RadarBase is performing OnTick operation");
    }
}