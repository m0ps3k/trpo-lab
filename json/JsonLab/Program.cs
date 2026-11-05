using System;
using System.Runtime.Serialization;
using System.Text.Json;
namespace json2
{
    [DataContract]
    class Company
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string INN { get; set; }
        [DataMember]
        public string Address { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            company.INN = "1";
            company.Name = "Romashka";
            company.Address = "Barnaul";
            string json = JsonSerializer.Serialize(company);
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}