//using radar;
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using platform;
using weapon;
using generics;
namespace gun;

public class Guns : Wepons
{
    public string id;
    public double elivation;
    public double azimuth;
    public Guns(int id, Platform platform, double Elivation, double Azimuth) : base(id, platform)
    {
        this.Id = id;
        this.elivation = Elivation;
        azimuth = Azimuth;
    }
}
public class AAA : Guns
{
    public double shell_fuses_delay;
    public AAA(int id, Platform platform, double shell_fuses_delay, double Elivation, double Azimuth) : base(id, platform, Elivation, Azimuth)
    {
        this.shell_fuses_delay = shell_fuses_delay;
    }
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

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("aaa is performing OnTick operation");
    }
}