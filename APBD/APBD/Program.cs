using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace APBD
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Container.fillDic();
            // Create containers
            Container container1 = new Container(5, 10, 100, 3, 10);
            Console.WriteLine(container1.ToString());
            Container container2 = new Container(5, 1000, 100, 3, 10);
            Console.WriteLine(container2.ToString());
            LiquidContainer liquidContainer1 = new LiquidContainer(6, 12, 150, 4, false, 10);
            RefrigeratedContainer refrigeratedContainer1 = new RefrigeratedContainer(7, 15, 200, 5, "Apples", 4.4, 10);

            // Load cargo into containers
            container1.LoadMass(50);
            liquidContainer1.LoadMass(75);
            refrigeratedContainer1.LoadMass(100);

            // Create a ship
            Ship ship1 = new Ship(10, 20, 5000);

            // Load containers onto the ship
            ship1.LoadContainer(container1);
            ship1.LoadContainer(liquidContainer1);
            ship1.LoadContainer(refrigeratedContainer1);

            // Print information about the ship and its cargo
            Console.WriteLine("Ship Information:");
            Console.WriteLine(ship1);

            // Unload a container from the ship
            ship1.RemoveContainer(container1);

            // Print updated information about the ship and its cargo
            Console.WriteLine("Ship Information After Unloading a Container:");
            Console.WriteLine(ship1);

            // Replace a container on the ship with another container
            Container newContainer = new Container(6, 11, 120, 3, 10);
            Console.WriteLine(newContainer.ToString());
            ship1.Switch(liquidContainer1.sNumber, newContainer);

            // Print updated information about the ship and its cargo
            Console.WriteLine("Ship Information After Switching Containers:");
            Console.WriteLine(ship1);

            // Create another ship
            Ship ship2 = new Ship(15, 25, 750);

            // Move a container from one ship to another
            ship1.Switch(liquidContainer1.sNumber, newContainer);

            // Print updated information about both ships and their cargo
            Console.WriteLine("Information About Both Ships:");
            Console.WriteLine("Ship 1:");
            Console.WriteLine(ship1);
            Console.WriteLine("Ship 2:");
            Console.WriteLine(ship2);

            // Print information about a specific container
            Console.WriteLine("Information About a Specific Container:");
            Console.WriteLine(container1);
        }
    }

    public class Container
    {
        protected double Mass;
        protected double Weight;
        protected double Height;
        private double Depth;
        protected static Dictionary<string, int> Names = new Dictionary<string, int>();
        public string sNumber { get; set; }
        protected double MaxMass;
        private static List<Container> cons = new List<Container>();
        
        public Container(double height, double weight, double maxMass, double depth, double mass)
        {
            Mass = mass;
            Height = height;
            sNumber = $"KON-K-{Names["Container"]}";
            Names["Container"]++;
            MaxMass = maxMass;
            Depth = depth;
            Weight = weight;

            cons.Add(this);
        }

        public static void fillDic()
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
                throw new OverFillException("Mass has been exceeded");
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

    public class LiquidContainer : Container, IHazardNotifier
    {
        private bool _DangerousContent;
        public LiquidContainer(double height, double weight, double maxMass, double depth, bool dangerousContent, double mass) : base(height, weight, maxMass, depth, mass)
        {
            _DangerousContent = dangerousContent;
            sNumber = $"KON-L-{Names["Liquid"]}";
            Names["Liquid"]++;
        }
        
        
        public override void LoadMass(double massLoad)
        {
            if (_DangerousContent)
            {
                MaxMass = 0.5 * massLoad;
            }
                
            if (massLoad >= 0 && massLoad <= MaxMass && !_DangerousContent)
            {
                MaxMass = 0.9 * massLoad;
            }
            else
            {
                throw new Exception("Tried to do a dangerous operation");
            }
        }
        public void Notify()
        {
            Console.WriteLine($"HAZARD for container {sNumber}");
        }
    }

    public class GasContainer : Container, IHazardNotifier
    {
        
        public GasContainer(double height, double weight, double maxMass, double depth, double mass) : base(height, weight, maxMass, depth, mass)
        {
            sNumber = $"KON-G-{Names["Gas"]}";
            Names["Gas"]++;
            
        }

        public override void Empty()
        {
            Mass = 0.05 * Mass;
        }

        public void Notify()
        {
            Console.WriteLine($"HAZARD for container {sNumber}");
        }
    }

    public class RefrigeratedContainer : Container, IHazardNotifier
    {
        private string _Type;
        private double _Temp;
        private Dictionary<String, double> _Types = new Dictionary<string, double>();
        public RefrigeratedContainer(double height, double weight, double maxMass, double depth, string type, double temp, double mass) : base(height, weight, maxMass, depth, mass)
        {
            AddTypes();
            sNumber = $"KON-R-{Names["Refrigerated"]}";
            Names["Refrigerated"]++;
            _Temp = temp;
            _Type = type;
            if (_Temp > _Types[type])
            {
                Notify();
            }
        }
        public void AddTypes()
        {
            _Types.Add("Bananas", 13.3);
            _Types.Add("Apples", 4.4);
            _Types.Add("Oranges", 7.2);
            _Types.Add("Grapes", 1.7);
            _Types.Add("Strawberries", 0.0);
            _Types.Add("Carrots", 0.0);
            _Types.Add("Lettuce", 1.7);
            _Types.Add("Broccoli", 0.0);
            _Types.Add("Tomatoes", 12.8);
            _Types.Add("Milk", 4.4);
            _Types.Add("Cheese", 2.2);
            _Types.Add("Yogurt", 2.2);
            _Types.Add("Eggs", 1.7);
            _Types.Add("Chicken", -17.8);
            _Types.Add("Fish", -17.8);
            _Types.Add("Shrimp", -17.8);
            _Types.Add("Ice Cream", -23.3);
            _Types.Add("Frozen Vegetables", -17.8);
            _Types.Add("Frozen Fruits", -23.3);
            _Types.Add("Frozen Pizza", -17.8);
        }
        public void Notify()
        {
            Console.WriteLine($"HAZARD for container {sNumber}, temperature is too high!");
        }
    }
    public interface IHazardNotifier
    {
        void Notify();
    }
    public class Ship
    {
        private double MaxSpeed;
        private int MaxAmountOfContainers;
        private double MaxMassOfContainers;
        private List<Container> LoadedContainers = new List<Container>();

        public Ship(int maxAmountOfContainers,double maxSpeed, double maxMassOfContainers)
        {
            MaxMassOfContainers = maxMassOfContainers;
            MaxAmountOfContainers =maxAmountOfContainers;
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
                Console.WriteLine(MaxAmountOfContainers);
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

    public class OverFillException : Exception
    {
        public OverFillException(string msg): base (msg)
        {
            
        }
    }
    
}