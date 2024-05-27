
//using radar;
using System.Collections.Generic;
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using battleFrameWork;
using platform;
using generics;
namespace weapon;

public class Wepons : BattleSystem
{
    public platform.Platform HostPlatform;
    public Wepons(string id, platform.Platform platform) : base(id)
    {
        this.HostPlatform = platform;
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
        Console.WriteLine("wepons is performing OnTick operation");
    }
}