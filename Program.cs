namespace Bebronimo6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Открыть файл с танчиками");
            string path1 = Console.ReadLine();
            List<Tank> tanks = Converters.OpenFile(path1);
            Console.WriteLine("Путь для сохранения танчиков");
            string path2 = Console.ReadLine();
            Converters.SaveFile(path2, tanks);
        }
    }
}