using System.Text.Json;
using Aircompany.Models;

namespace Aircompany.Planes
{
    public class MilitaryPlane : Plane
    {
        public MilitaryType PlaneType { get; set; }

        public MilitaryPlane(string model, int maxSpeed, int maxFlightDistance, int maxLoadCapacity, MilitaryType militaryType)
            : base(model, maxSpeed, maxFlightDistance, maxLoadCapacity)
        {
            PlaneType = militaryType;
        }

        public bool Equals(MilitaryPlane plane)
        {
            bool isEqual = base.Equals(plane) && PlaneType == plane.PlaneType;
            return isEqual;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }        
    }
}
