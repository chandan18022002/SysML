
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using platform;
using generics;
namespace battleFrameWork;

public abstract class BattleSystem
{

    public int Id;
    public BattleSystem(int id)
    {
        this.Id = id;
    }
    public abstract void Set(List<Pair<string, string>> param);
    public abstract List<Pair<string,string>> Get();
    // Abstract method for OnTick
    public abstract void OnTick();
}

public class DataAnalyser
{

}

public class BattleSimulationEngine
{

    public void Register(BattleSystem battleSystem)
    {

    }
    public void Unregister(BattleSystem battleSystem)
    { 

    }
    public void RunScinario(BattleSystem battleSystem)
    {

    }

}
public class Scinario
{
    BattleSystem BattleSystem;
    double SequenceSteps;
}
