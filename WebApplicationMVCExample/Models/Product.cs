namespace WebApplicationMVCExample.Models
{
    public class Product
    { 
        public int Id { get; set; }
        public string Name { get; set;}
        public string Type {  get; set;}
        public bool isDeleted { get; set; }

        public Product() { 
        }
        public Product(int id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = type;
        }
        public Product(int id, string name, string type,bool isDeleted)
        {
            Id = id;
            Name = name;
            Type = type;
            isDeleted = isDeleted;
        }
    }
}
