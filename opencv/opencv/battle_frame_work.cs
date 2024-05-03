
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
namespace battle_frame_work;

public abstract class BattleSystem
{

    public BattleSystem(int id)
    {
        this.Id = id;
    }
    public int Id;
    public abstract void Set(int id);
    public abstract int Get();

    // Abstract method for OnTick
    public abstract void OnTick();
}

public class data_analyser
{

}

public class battle_simulation_engine
{
    public List<(double x, double y, int tick)> latest_five_target_coordinates { get; private set; }

    public void register(BattleSystem battle_system)
    {

    }
    public void unregister(BattleSystem battle_system)
    { 

    }
    public void run_scinario(BattleSystem battle_system)
    {

    }

}
public class scinario
{
    BattleSystem battle_system;
    double sequence_steps;
}
