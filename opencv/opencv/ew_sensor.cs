using radar;
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace ew_sensor;

public class Sensor : BattleSystem
{
    public Platform hostPlatform;
    public Sensor(int id, Platform platform) : base(id)
    {
        this.hostPlatform = platform;
    }

    public virtual List<object> Detect()
    {
        // Implement logic for detecting targets based on sensor type (e.g., radar)
        Console.WriteLine("Sensor is detecting...");
        return new List<object>(); // Placeholder, replace with actual detected objects
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
        Console.WriteLine("Sensor is performing OnTick operation");
    }
}
public class Esn : Sensor
{
    public double no_of_antenna;
    public double pwds;
    public double emmiter_records;
    public double antenna_configaration;
    public double reception_band;

    public Esn(int id, Platform platform, double no_of_antenna, double pwd, double emmiter_records, double antenna_configaration, double reception_band)
         : base(id, platform)
    {
        this.no_of_antenna = no_of_antenna;
        this.pwds = pwds;
        this.emmiter_records = emmiter_records;
        this.antenna_configaration = antenna_configaration;
        this.reception_band = reception_band;
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
public class Rwr : Sensor
{
    public double no_of_antenna;
    public double pwds;
    public double emmiter_records;
    public double antenna_configaration;
    public double reception_band;

    public Rwr(int id, Platform platform, double no_of_antenna, double pwd, double emmiter_records, double antenna_configaration, double reception_band)
         : base(id, platform)
    {
        this.no_of_antenna = no_of_antenna;
        this.pwds = pwds;
        this.emmiter_records = emmiter_records;
        this.antenna_configaration = antenna_configaration;
        this.reception_band = reception_band;
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

