namespace DataImporter.Core
{
    public class Product
    {
        public Product() { }

        public string UniqueId { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("Product with ID: {0}, Name: {1}, by: {2}, is: {3}",
                UniqueId, Name, Brand, Description);
        }
    }
}
