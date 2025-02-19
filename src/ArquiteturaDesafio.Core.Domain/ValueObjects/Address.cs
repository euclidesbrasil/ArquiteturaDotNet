using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string City { get; private set; }
        public string Street { get; private set; }
        public int Number { get; private set; }
        public string ZipCode { get; private set; }
        public Geolocation Geolocation { get; private set; }
        public Address() { }
        public Address(string city, string street, int number, string zipCode, Geolocation geolocation)
        {
            City = city;
            Street = street;
            Number = number;
            ZipCode = zipCode;
            Geolocation = geolocation;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Street;
            yield return Number;
            yield return ZipCode;
            yield return Geolocation;
        }
    }
}
