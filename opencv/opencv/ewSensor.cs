using System.Collections.Generic;
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using battleFrameWork;
using platform;
using sensor;
namespace ewSensor;


public class Esm : Sensor
{
    public double NoOfAntenna;
    public double Pwds;
    public double EmmiterRecords;
    public double AntennaConfigaration;
    public double ReceptionBand;

    public Esm(int id, platform.Platform platform, double noOfAntenna, double pwds, double emmiterRecords, double antennaConfigaration, double receptionBand)
         : base(id, platform)
    {
        this.NoOfAntenna = noOfAntenna;
        this.Pwds = pwds;
        this.EmmiterRecords = emmiterRecords;
        this.AntennaConfigaration = antennaConfigaration;
        this.ReceptionBand = receptionBand;
    }
    public void GeneratePwd()
    {

    }
    public void EmmiterRecordGenerator()
    {

    }
    public void DueToCountermeasure()
    {

    }
}
public class Rwr : Sensor
{
    public double NoOfAntenna;
    public double Pwds;
    public double EmmiterRecords;
    public double AntennaConfigaration;
    public double ReceptionBand;

    public Rwr(int id, platform.Platform platform, double noOfAntenna, double pwds, double emmiterRecords, double antennaConfigaration, double receptionBand)
         : base(id, platform)
    {
        this.NoOfAntenna = noOfAntenna;
        this.Pwds = pwds;
        this.EmmiterRecords = emmiterRecords;
        this.AntennaConfigaration = antennaConfigaration;
        this.ReceptionBand = receptionBand;
    }
    public void GeneratePwd()
    {

    }
    public void EmmiterRecordGenerator()
    {

    }
    public void DueToCountermeasure()
    {

    }
}

