using System;
using System.Collections.Generic;
using System.Text;

namespace APBD
{
    internal class Program
    {
        public static void Main(string[] args)
        {
        }
    }

    public class Container
    {
        protected double Mass;
        protected double Weight;
        protected double Height;
        private double Depth;
        private Dictionary<string, int> Names = new Dictionary<string, int>();
        private HashSet<string> b = new HashSet<string>();
        public string sNumber { get; set; }
        protected double MaxMass;
        private static List<Container> cons = new List<Container>();
        
        public Container(double height, double weight, double maxMass, double depth)
        {
            fillDic();
            Height = height;
            MaxMass = weight;
            sNumber = $"KON-K-{Names["Container"]}";
            Names["Container"]++;
            MaxMass = maxMass;
            Depth = depth;

            cons.Add(this);
        }

        public void fillDic()
        {
            Names.Add("Container", 0);
            Names.Add("Refrigerated", 0);
            Names.Add("Gas", 0);
            Names.Add("Liquid", 0);
        }

        public virtual void Empty()
        {
            Mass = 0;
        }

        public Double getMass()
        {
            return Mass;
        }

        public virtual void LoadMass(double massLoad)
        {
            if (massLoad >= 0 && massLoad <= MaxMass)
            {
                MaxMass = massLoad;
            }
            else
            {
                throw new Exception("Wrong load mass");
            }
        }

        public static Container GetBySerialNumber(string sNum)
        {
            foreach(Container con in cons)
            {
                if (con.sNumber.Equals(sNum))
                {
                    return con;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return
                $"Container Serial Number: {sNumber}, Load Mass: {Mass}, Height: {Height}, Weight: {Weight}, Max mass: {MaxMass}, depth: {Depth}";
        }
    }
    public class Ship
    {
        private double MaxSpeed;
        private int MaxAmountOfContainers;
        private double MaxMassOfContainers;
        private List<Container> LoadedContainers = new List<Container>();

        public Ship(int maxAmountOfContainers,double maxSpeed, double maxMassOfContainers)
        {
            maxMassOfContainers = MaxMassOfContainers;
            maxAmountOfContainers = MaxAmountOfContainers;
            MaxSpeed = maxSpeed;
        }

        public void Load(List<Container> toLoad)
        {
            double totalMass = 0;
            foreach (var con in toLoad)
            {
                totalMass += con.getMass();
            }

            double oldMass = 0;
            foreach (var con in LoadedContainers)
            {
                oldMass += con.getMass();
            }
            if (LoadedContainers.Count + toLoad.Count > MaxAmountOfContainers)
            {
                throw new Exception("The Maximum Amount is exceeded");
            } 
            else if (totalMass + oldMass > MaxMassOfContainers * 1000)
            {
                throw new Exception("The maximum mass has been exceeded");
            }
            LoadedContainers.AddRange(toLoad);    
        }

        public void LoadContainer(Container toLoad)
        {
            Load(new List<Container> { toLoad });
        }

        public void RemoveContainer(Container toRemove)
        {
            LoadedContainers.Remove(toRemove);
        }
        public void Switch(string sNumToSwitch, Container switchWith)
        {
            foreach (Container con in LoadedContainers)
            {
                if (con.sNumber.Equals(sNumToSwitch))
                {
                    LoadedContainers.Remove(con);
                    break;
                }
            }
            LoadedContainers.Add(switchWith);
        }

        public override string ToString()
        {
            StringBuilder build = new StringBuilder();
            build.AppendLine("Loaded Containers: ");
            foreach (Container con in LoadedContainers)
            {
                if (con != null)
                {
                    build.AppendLine(con.ToString());
                }
            }

            build.AppendLine($"Max speed {MaxSpeed}, Max amount: {MaxAmountOfContainers}, Max mass : {MaxMassOfContainers}");
            return build.ToString();
        }
    }
}