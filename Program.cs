
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json;

var db = new MongoClient("mongodb://localhost:27017/").GetDatabase("test");

var productCollection = db.GetCollection<Product>("product").AsQueryable();
var variantCollection = db.GetCollection<Variant>("variant").AsQueryable();

var methodSyntax = productCollection
    .GroupJoin(variantCollection,
        p => p.Id,
        v => v.Product_Id,
        (p, variants) =>
            new { Product = p, Variants = variants }
    );

var querySyntax = from p in productCollection
                  join v in variantCollection on p.Id equals v.Product_Id into variants
                  select new { Product = p, Variants = variants };

Console.WriteLine(JsonSerializer.Serialize(methodSyntax, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine(JsonSerializer.Serialize(querySyntax, new JsonSerializerOptions { WriteIndented = true }));

Console.ReadKey();


public class Product
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
}

public class Variant
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string Product_Id { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
}