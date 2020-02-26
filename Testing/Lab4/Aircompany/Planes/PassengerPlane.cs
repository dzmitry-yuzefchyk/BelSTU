using System.Text.Json;

namespace Aircompany.Planes
{
    public class PassengerPlane : Plane
    {
        public int PassengersCapacity { get; set; }

        public PassengerPlane(string model, int maxSpeed, int maxFlightDistance, int maxLoadCapacity, int passengersCapacity)
            : base(model, maxSpeed, maxFlightDistance, maxLoadCapacity)
        {
            PassengersCapacity = passengersCapacity;
        }

        public bool Equals(PassengerPlane plane)
        {
            bool isEqual = base.Equals(plane) && PassengersCapacity == plane.PassengersCapacity;
            return isEqual;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
