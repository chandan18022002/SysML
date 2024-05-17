using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace aircraft;

using generics;
using LanguageExt;
using platform;
using sensor;


//using radar;
public class Aircraft : Platform
{
    private string id;

    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        Pair<string, string> tmp_pair = new Pair<string, string>("", "");
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>();
        tmp_list.Add(tmp_pair);
        return tmp_list;//this.position;
    }
    public Aircraft(int id, double Speed, double Heading, List<Vector> Waypoints, List<Sensor> OnboardSensor /*,double radar_cross_section*/)
        : base(id, Speed, Heading, Waypoints, OnboardSensor)
    {


    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Aircraft
        Console.WriteLine("Aircraft is performing OnTick operation");
    }
}