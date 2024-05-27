using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using generics;
using LanguageExt;
using platform;
using sensor;

namespace aircraft;
public class Aircraft : Platform
{

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
    public Aircraft(string id, double speed, double heading, List<Vector> waypoints, List<Sensor> onboardSensor /*,double radar_cross_section*/)
        : base(id, speed, heading, waypoints, onboardSensor)
    {


    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Aircraft
        Console.WriteLine("Aircraft is performing OnTick operation");
    }
}