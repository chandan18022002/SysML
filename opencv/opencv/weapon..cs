
using radar;
using System.Collections.Generic;
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using battle_frame_work;
namespace weapon;

public class Wepons : BattleSystem
{
    public Platform HostPlatform;
    public Wepons(int id, Platform platform) : base(id)
    {
        this.HostPlatform = platform;
    }


    public override void Set(int id)
    {
        Id = id;
    }

    public override int Get()
    {
        return Id;
    }

    public override void OnTick()
    {
        // Implement OnTick logic specific to Sensor
        Console.WriteLine("wepons is performing OnTick operation");
    }
}