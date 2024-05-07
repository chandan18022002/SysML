using System.Collections.Generic;
using System;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV.Dnn;
using platform;
using radars;
using missile;
using aircraft;
namespace pulse;

public class Pulse
{
    public int Id;
    public Vector position;
    public Vector velocity;
    public double energy = 0;
    public Radar source;
    public double pwd;
    public double frequency;
    public double beam_width;
    public double beam_width_vel;
    public double distance_travelled;
    public double elevation;
    public double azimuth;



    public Pulse(int id, Vector position, Vector velocity, double energy, Radar source, double pwd, double frequency, double beam_width, double beam_width_vel, double Distance_travelled, double elevation, double azimuth)
    {
        this.Id = id;
        this.position = position;
        this.velocity = velocity;
        this.energy = energy;
        this.source = source;
        this.pwd = pwd;
        this.frequency = frequency;
        this.beam_width = beam_width;
        this.beam_width_vel = beam_width_vel;
        this.elevation = elevation;
        this.azimuth = azimuth;

    }

    public void Move()
    {
        double time_diff = 1.0;
        this.position.X += this.velocity.X;
        this.position.Y += this.velocity.Y;
        this.distance_travelled += Math.Sqrt(Math.Pow(this.velocity.X, 2) + Math.Pow(this.velocity.Y, 2)) * time_diff;
        this.beam_width += this.beam_width_vel;

    }
    public void reverse()
    {
        velocity.X = -velocity.X;
        velocity.Y = -velocity.Y;
    }

    public void Collided_Target(Aircraft target)
    {
        this.velocity.X = -this.velocity.X;
        this.velocity.Y = -this.velocity.Y;
        this.position.X = target.position.X;
        this.position.Y = target.position.Y;
        this.beam_width = 0;
        int temp_azimuth = (int)(target.heading - this.azimuth);
        while (temp_azimuth < 0)
        {
            temp_azimuth += 360;
        }
        if (temp_azimuth > 360)
        {
            temp_azimuth = temp_azimuth % 360;
        }
        double temp_val = target.RadarCrossSection[(int)this.frequency][temp_azimuth];
        this.energy = this.energy * (temp_val) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));
        this.azimuth += 180;
        this.distance_travelled = 0;
        for (int j = 0; j < 11; j++)
        {
            this.Move();
        }


    }
    static double DegreesToRadians(double degrees)// converting degree into radians (azimuth input)
    {
        return degrees * (Math.PI / 180);
    }
    public double CalculateDistance(RadarBase radarbase)
    {
        return Math.Sqrt(Math.Pow(radarbase.position.X - this.position.X, 2) + Math.Pow(radarbase.position.Y - this.position.Y, 2));
    }



    public void collide_radar(int tick, int latest_radar_transmit_tick, RadarBase rb, Pulsed_radar pradar)
    {
        double time_diff = tick + 20 - latest_radar_transmit_tick;
        double target_distance = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) * time_diff / 2;
        //Target x coordinate = radar_base’s x coordinate + (target_distance * cosine (radar.azimuth)
        double target_x_coordinate = rb.position.X + (target_distance * Math.Cos(DegreesToRadians(pradar.azimuth)));
        double target_y_coordinate = rb.position.Y + (target_distance * Math.Sin(DegreesToRadians(pradar.azimuth)));

        double lambda = Math.Sqrt(Math.Pow(this.velocity.X, 2) + Math.Pow(this.velocity.Y, 2)) / pradar.frequency;
        this.energy = this.energy * (pradar.Gain_table[(int)frequency][(int)(this.azimuth - pradar.azimuth)]) * Math.Pow(lambda, 2) / (4 * Math.PI * Math.Pow(this.distance_travelled, 2));

        /* Console.WriteLine("Target's x coordinate: " + target_x_coordinate);
         Console.WriteLine("Target's y coordinate: " + target_y_coordinate);*/
        pradar.UpdateLatestTargetCoordinates(position.X, position.Y, latest_radar_transmit_tick);
        // Console.Write(pradar.latest_five_target_coordinates);
        //Console.WriteLine("Latest Five Target Coordinates: " + string.Join(", ", pradar.latest_five_target_coordinates));


        double temp_distance = CalculateDistance(rb);
        if (temp_distance > 0)
        {
            double temp_x1 = position.X + (temp_distance * Math.Cos(DegreesToRadians(pradar.azimuth + 90)));
            double temp_y1 = position.Y + (temp_distance * Math.Sin(DegreesToRadians(pradar.azimuth + 90)));
            double temp_x2 = position.X + (temp_distance * Math.Cos(DegreesToRadians(pradar.azimuth - 90)));
            double temp_y2 = position.Y + (temp_distance * Math.Sin(DegreesToRadians(pradar.azimuth - 90)));

            double distanceToTemp1 = Math.Sqrt(Math.Pow(temp_x1 - rb.position.X, 2) + Math.Pow(temp_y1 - rb.position.Y, 2));
            double distanceToTemp2 = Math.Sqrt(Math.Pow(temp_x2 - rb.position.X, 2) + Math.Pow(temp_y2 - rb.position.Y, 2));
            //Console.Write(", delta : " + Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI));
            if (distanceToTemp1 < distanceToTemp2)
            {
                //pradar.azimuth -= Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI);
                pradar.azimuth += temp_distance * .1;
            }
            else
            {
                //pradar.azimuth += Math.Atan(temp_distance / this.distance_travelled) * (180 / Math.PI);
                pradar.azimuth -= temp_distance * .1;

            }
            
            //return target_x_coordinate,target_y_coordinate;
            //Console.WriteLine(", after : " + pradar.azimuth);
            Console.WriteLine("------------------------------------------------------------");

            Console.WriteLine("target_x_coordinate" + target_x_coordinate);
            Console.WriteLine("target_y_coordinate" + target_y_coordinate);

        }

    }
    

}
