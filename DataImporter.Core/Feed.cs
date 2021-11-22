namespace DataImporter.Core
{
    public class Feed
    {
        public int Feed_ID { get; set; }
        public string Feed_Name { get; set; }
        public override string ToString()
        {
            return string.Format("Feed with ID: {0}, Name: {1}",
                Feed_ID, Feed_Name);
        }
    }
}
