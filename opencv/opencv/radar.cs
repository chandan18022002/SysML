//using radar;
using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using ewSensor;
using platform;
using sensor;
using missile;
using aircraft;
using generics;
namespace radar;

public class Radar : Sensor
{

    public string operatingMode;
    public string antenna;
    public string modulation;
    public double elevation;
    public double azimuth;
    public double frequency;
    public int pri; // Pulse Repetition Interval
    public double pwd; // Pulse Width Duration
    public string antenna_scan_pattern;
    public List<object> detected;
    public double detection_Range;
    public double detectability_Range;
    public double resolution_Cell;
    public double minimum_Range;
    public double max_Unambiguous_Range;

    public Dictionary<double, double[]> Gain_table;
    public List<(double x, double y, int tick)> latest_five_target_coordinates { get; private set; }

    public Radar(int id, platform.Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resenution_cell, double minimum_range, double max_unamb_range/*, List<List<double>> gain_table*/)
        : base(id, platform)
    {

        this.operatingMode = operatingMode;
        this.antenna = antenna;
        this.modulation = modulation;
        this.elevation = elevation;
        this.azimuth = azimuth;
        this.frequency = frequency;
        this.pri = pri;
        this.pwd = pwd;
        this.antenna_scan_pattern = antennaScanPattern;
        this.detected = new List<object>();
        this.detection_Range = detection_range;
        this.detectability_Range = detectability_range;
        this.minimum_Range = minimum_range;
        this.max_Unambiguous_Range = max_unamb_range;

        Gain_table = CreateGainTable();
        latest_five_target_coordinates = new List<(double x, double y, int tick)>();
    }

    public void Transmit()
    {
        // Implement logic for transmitting a signal from the radar
        Console.WriteLine("Radar is transmitting...");
    }

    public object Receive()
    {
        // Implement logic for receiving a signal by the radar
        Console.WriteLine("Radar is receiving...");
        return new object(); // Placeholder for received signal
    }



    private Dictionary<double, double[]> CreateGainTable()
    {
        Dictionary<double, double[]> Gain_Table = new Dictionary<double, double[]>();

        // Populate the dictionary
        for (double keys = 0.5; keys <= 18.5; keys += 0.5)
        {
            double[] cosineValues = new double[360];
            for (int azimuth = 0; azimuth < 360; azimuth++)
            {
                double radians = DegreesToRadians(azimuth + 1); // Convert angle to radians
                cosineValues[azimuth] = Math.Cos(radians);
            }
            Gain_Table.Add(keys, cosineValues);
        }

        return Gain_Table; // Return the populated dictionary
    }

    static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
    public void UpdateLatestTargetCoordinates(double x, double y, int tick)
    {
        if (latest_five_target_coordinates.Count == 5)
        {
            latest_five_target_coordinates.RemoveAt(0); // Remove oldest entry if the list is full
        }
        latest_five_target_coordinates.Add((x, y, tick)); // Add new coordinates
    }
}

public class PulsedRadar : Radar
{

    public int Pri; // Pulse Repetition Interval
    public double Pwd;
    public double prf;
    public double peak_transmission_power;

    public PulsedRadar(int id, platform.Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range, double peak_transmission_power)
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)

    {
        this.Pri = pri; // Pulse Repetition Interval
        this.Pwd = pwd;
        this.prf = prf;
        this.peak_transmission_power = peak_transmission_power;

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
        Console.WriteLine("pulse radar is performing OnTick operation");
    }

}
public class ContinousWave : Radar
{
    public double TransmittedFrequency;
    public double ReceivedFrequency;
    public ContinousWave(int id, platform.Platform platform, double transmitted_frequency, double received_frequency, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range)
          : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    {
        this.TransmittedFrequency = transmitted_frequency; // Initialize Transmitted Frequency
        this.ReceivedFrequency = received_frequency; // Initialize Received Frequency
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
        Console.WriteLine("continious wave is performing OnTick operation");
    }

}

public class PulseDoppler : Radar
{

    public PulseDoppler(int id, platform.Platform platform, string operatingMode, string antenna, string modulation, double elevation, double azimuth, double frequency, int pri, double pwd, string antennaScanPattern, double detection_range, double detectability_range, double resolution_cell, double minimum_range, double max_unamb_range) /*List<List<double>> Gain_table*/
        : base(id, platform, operatingMode, antenna, modulation, elevation, azimuth, frequency, pri, pwd, antennaScanPattern, detection_range, detectability_range, resolution_cell, minimum_range, max_unamb_range)
    {

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
        Console.WriteLine("pulse-dopler is performing OnTick operation");
    }

}
