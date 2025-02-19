using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class Geolocation:ValueObject
    {
        public Geolocation() { }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        public Geolocation(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}
