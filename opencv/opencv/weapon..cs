
//using radar;
using System.Collections.Generic;
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using battle_frame_work;
using platform;
using generics;
namespace weapon;

public class Wepons : BattleSystem
{
    public Platform HostPlatform;
    public Wepons(int id, Platform platform) : base(id)
    {
        this.HostPlatform = platform;
    }
    public override void Set(List<Pair<string, string>> param)
    {
    }

    public override List<Pair<string, string>> Get()
    {
        List<Pair<string, string>> tmp_list = new List<Pair<string, string>>();
        Pair<string, string> tmp_pair = new Pair<string, string>("", "");
        tmp_list.Add(tmp_pair);
        return tmp_list;//this.position;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("wepons is performing OnTick operation");
    }
}