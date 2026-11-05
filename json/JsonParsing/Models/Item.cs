using System.Runtime.Serialization;

namespace JsonParsing.Models;


[DataContract]
public class Item
{
    [DataMember]
    public int Code { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public double Price { get; set; }

    public Item(int Code, string Name, double Price)
    {
        this.Code = Code;
        this.Name = Name;
        this.Price = Price;
    }
}

