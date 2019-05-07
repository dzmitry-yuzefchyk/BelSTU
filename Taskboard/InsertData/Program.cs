namespace InsertData
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator generator = new DataGenerator(10000000);
            generator.Generate();
        }
    }
}
