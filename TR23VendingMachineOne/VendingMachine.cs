using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR23VendingMachineOne
{
    public class VendingMachine
    {
        public DateTime LastUpdated { get; set; }
        public int SpiralCapacity { get; set; }
        public string ProductOneName { get; set; }
        public int ProductOneQty { get; set; }
        public double ProductOnePrice { get; set; }
        public string ProductTwoName { get; set; }
        public int ProductTwoQty { get; set; }
        public double ProductTwoPrice { get; set; }
        public double Cash { get; set; }
        public bool ExactChangeAlarm { get; set; }
    }
}
