using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Bebronimo6
{
    public class Tank
    {
        public Tank()
        {

        }
        public Tank(string name, double weight, string country)
        {
            this.name = name;
            this.weight = weight;
            this.country = country;
        }
        public string name;
        public double weight;
        public string country;
    }

    public class Editor
    {
        public List<Tank> tanks;
        public Editor(List<Tank> tanks)
        {
            string text = Converters.ToText(tanks);
            string[] lines = text.Trim().Split('\n');
            int pos = 0;
            int max = lines.Count()-1;
            int edit = -1;
            while (edit!=-2)
            {
                Console.Clear();
                foreach (string line in lines)
                {
                    Console.WriteLine("  "+line);
                }
                
                if (edit != -1)
                {
                    Console.SetCursorPosition(0, max+3);
                    Console.WriteLine("Изменение строки");
                    lines[edit] = Console.ReadLine();
                    Console.SetCursorPosition(0, pos);
                    edit = -1;
                    continue;
                }
                Console.SetCursorPosition(0, pos);
                Console.Write('>');
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (pos == 0)
                            pos = max;
                        else
                            pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos == max)
                            pos = 0;
                        else
                            pos++;
                        break;
                    case ConsoleKey.Enter:
                        lines[pos] = "";
                        edit = pos;
                        break;
                    case ConsoleKey.F1:
                        edit = -2;
                        this.tanks = Converters.FromTxt(String.Join('\n', lines));
                        break;
                }
            }
            Console.Clear();
        }
    }
    public class Converters
    {
        public static string ToText(List<Tank> tanks)
        {
            string text = "";
            for (int i = 0; i < tanks.Count; i++)
            {
                text = text + $"{tanks[i].name}\n{tanks[i].weight}\n{tanks[i].country}\n";
            }
            return text;
        }
        public static List<Tank> OpenFile(string path)
        {

            if (path[^3..^1] == "xml")
            {
                return FromXml(path);
            }
            else if (path[^4..^1] == "json")
            {
                return FromJson(path);
            }
            else
            {
                return FromTxt(File.ReadAllText(path));
            }
        }
        public static void SaveFile(string path, List<Tank> tanks)
        {
            if (path.Contains(".xml"))
            {
                SaveXml(path, tanks);
            }
            if (path.Contains(".json"))
            {
                SaveJson(path, tanks);
            }
            if (path.Contains(".txt"))
            {
                SaveTxt(path, tanks);
            }
        }
        private static void SaveTxt(string path, List<Tank> tanks)
        {
            string text = ToText(tanks);
            File.WriteAllText(path, text);
        }
        private static void SaveJson(string path, List<Tank> tanks)
        {
            string json = JsonConvert.SerializeObject(tanks);
            File.WriteAllText(path, json);
        }
        private static void SaveXml(string path, List<Tank> tanks)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Tank>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, tanks);
            }
        }
        private static List<Tank> FromJson(string path)
        {
            string text = File.ReadAllText(path);
            List<Tank> tanks = JsonConvert.DeserializeObject<List<Tank>>(text);
            return tanks;
        }
        private static List<Tank> FromXml(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Tank>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Tank> tanks = (List<Tank>)xml.Deserialize(fs);
                return tanks;
            }
            
        }
        public static List<Tank> FromTxt(string text)
        {
            string[] lines = text.Split('\n');
            int count = lines.Count();
            count = count - (count % 3);
            List<Tank> tanks = new List<Tank>();
            for (int i = 0; i < count; i+=3)
            {
                Console.WriteLine(lines[i + 1]);
                string name = lines[i];
                double weight = Convert.ToDouble(lines[i + 1].Replace('.',','));
                string country = lines[i + 2];
                Tank tank = new Tank(name, weight, country);
                tanks.Add(tank);
            }
            return tanks;
        }
    }
}
