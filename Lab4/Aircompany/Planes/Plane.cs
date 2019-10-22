using System.Text.Json;

namespace Aircompany.Planes
{
    public abstract class Plane
    {
        public string Model { get; set; }
        public int MaxSpeed { get; set; }
        public int MaxFlightDistance { get; set; }
        public int MaxLoadCapacity { get; set; }

        public Plane(string model, int maxSpeed, int maxFlightDistance, int maxLoadCapacity)
        {
            Model = model;
            MaxSpeed = maxSpeed;
            MaxFlightDistance = maxFlightDistance;
            MaxLoadCapacity = maxLoadCapacity;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public bool Equals(Plane plane)
        {
            bool isEqual = Model == plane.Model &&
                           MaxSpeed == plane.MaxSpeed &&
                           MaxFlightDistance == plane.MaxFlightDistance &&
                           MaxLoadCapacity == plane.MaxLoadCapacity;
            return isEqual;
        }
    }
}
