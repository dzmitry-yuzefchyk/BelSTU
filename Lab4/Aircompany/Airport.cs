using Aircompany.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using Aircompany.Models;

namespace Aircompany
{
    public class Airport
    {
        public IEnumerable<Plane> Planes { get; set; }
        public IEnumerable<PassengerPlane> PassengerPlanes => Planes.Where(p => p is PassengerPlane).Cast<PassengerPlane>().ToList();
        public IEnumerable<MilitaryPlane> MilitaryPlanes => Planes.Where(p => p is MilitaryPlane).Cast<MilitaryPlane>().ToList();

        public IEnumerable<MilitaryPlane> MilitaryTransportPlanes =>
            MilitaryPlanes.Where(p => p.PlaneType == MilitaryType.TRANSPORT).ToList();

        public Airport(IEnumerable<Plane> planes)
        {
            Planes = planes.ToList();
        }

        public PassengerPlane GetPassengerPlaneWithMaxPassengersCapacity()
        {
            return PassengerPlanes.OrderByDescending(p => p.PassengersCapacity).First();
        }

        public Airport SortByMaxDistance()
        {
            var planes = Planes.OrderBy(p => p.MaxFlightDistance);
            return new Airport(planes);
        }

        public Airport SortByMaxSpeed()
        {
            var planes = Planes.OrderBy(p => p.MaxSpeed);
            return new Airport(planes);
        }
        
        public Airport SortByMaxLoadCapacity()
        {
            var planes = Planes.OrderBy(p => p.MaxLoadCapacity);
            return new Airport(planes);
        }

        public override string ToString()
        {
            return "Airport: {" +
                    $"planes={string.Join(", ", Planes.Select(p => p.Model))}" +
                    '}';
        }
    }
}
