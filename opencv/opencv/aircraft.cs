using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace aircraft;
using radar;
public class Aircraft : Platform
{
    private string id;

    public override void Set(int id)
    {
        this.Id = id;
    }

    public override int Get()
    {
        return Id;
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