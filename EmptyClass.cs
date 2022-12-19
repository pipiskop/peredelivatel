using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Formatting = Newtonsoft.Json.Formatting;

namespace LoveKorovokkushat
{
    public class pups
    {
        public pups()
        {

        }
        public string name;
        public string dog;
        public string numb;
        public pups(string Name, string MyDogs, string MyNumb)
        {
            name = Name;
            dog = MyDogs;
            numb = MyNumb;
        }
    }

    public class Cursor
    {
        public int position = 1;
        public int offset = 1;
        public int max = 1;
    }
    public class ChooseFile
    {
        public string way;
        public string text;
    }

    public class Converters
    {
        internal static string ToText(string way)
        {
            string text = File.ReadAllText(way);

            List<pups> result;
            string ext = way.Split(".")[^1];
            if (ext == "xml")
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<pups>));
                using (FileStream fs = new FileStream(way, FileMode.Open))
                {
                    result = (List<pups>)xml.Deserialize(fs);
                }
            }
            else if (ext == "json")
            {
                result = JsonConvert.DeserializeObject<List<pups>>(File.ReadAllText(way));
            }
            else
            {
                result = ToList(text);
            }
            string response = "";
            foreach (pups rifle in result)
            {
                response += $"{rifle.name}\n{rifle.dog}\n{rifle.numb}\n";
            }
            return response;
        }
        internal static List<pups> ToList(string text)
        {
            List<string> lines = text.Split("\n").ToList();
            List<pups> rifles = new List<pups>();
            lines.RemoveAll(x => x == "");
            for (int i = 0; i < lines.Count(); i += 3)
            {
                try
                {
                    string name = lines[i];
                    string years = lines[i + 1];
                    string song = lines[i + 2];
                    pups rifle = new pups(name, years, song);
                    rifles.Add(rifle);
                }
                catch
                {
                    break;
                }
            }
            return rifles;
        }
    internal static string ToJson(string text)
     {
        List<pups> rifles = ToList(text);
        return JsonContent.SerializeObject(rifles, Formatting.Indented);
    }
        internal static string ToXml(string text)
        {
            List<pups> rifles = ToList(text);
            XmlSerializer xml = new XmlSerializer(typeof(List<pups>));
            using (FileStream fs = new FileStream("cache.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, rifles);
            }
            string response = File.ReadAllText("cache.xml");
            File.Delete("cache.xml");
            return response;
        }
        public static void Convert(string way)
        {
            Console.Clear();
            string converted = Converters.ToText(way);

            Console.WriteLine("Vvedite put for saving texta");
            string exp = Console.ReadLine();
            string ext = exp.Split(".")[^1];
            Console.Clear();
            if (ext == "XML")
            {
                converted = Converters.ToXml(converted);
                File.WriteAllText(exp, converted);
            }
            else if (ext == "Json")
            {
                converted = Converters.ToJson(converted);
                File.WriteAllText(exp, converted);
            }
            else
            {
                File.WriteAllText(exp, converted);
            }
            Console.Clear();
            Console.WriteLine("text sohranen");
        }

    }

    public class Editor
    {
        public static string Edit(string way)
        {
            string readFile = Converters.ToText(way);
            string file = EditFile(readFile.Split("\n").ToList(), way);
            File.WriteAllText(way, file);
            return file;
        }
        private static string EditFile(List<string> text, string way)
        {
            Console.Clear();
            static string ArrayToString(List<char> line)
            {
                string response = "";
                foreach (char c in line)
                {
                    response += c;
                }
                return response;
            }
            static List<char> RemoveChar(List<char> line, int position)
            {
                List<char> new_line = new List<char>();
                int y = 0;
                for (int i = 0; i < line.Count(); i++)
                {
                    if (i != position)
                    {
                        new_line.Add(line[i]);
                        y++;
                    }
                }
                return new_line;
            }

            int pos1 = 0;
            int MaxPos1 = 0;
            int pos2 = 0;
            int MaxPos2 = 0;
            ConsoleKeyInfo vvod;
            bool exit = false;
            while (!exit)
            {
                if (pos1 < 0)
                    pos1 = 0;
                if (pos2 < 0)
                    pos2 = 0;
                if (pos1 > MaxPos1)
                    pos1 = MaxPos1;
                Console.Clear();
                MaxPos2 = 0;
                Console.SetCursorPosition(0, 0);
                foreach (string line in text)
                {
                    MaxPos2++;
                    Console.WriteLine(line);
                }
                List<char> currentLine = new List<char>();
                currentLine.AddRange(text[pos2].ToArray());

                MaxPos1 = currentLine.Count();
                Console.SetCursorPosition(pos1, pos2);
                vvod = Console.ReadKey();

                switch (vvod.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (pos2 != 0)
                            pos2--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos2 != MaxPos2 - 1)
                            pos2++;
                        break;
                    case ConsoleKey.RightArrow:
                        if (pos1 != MaxPos1)
                            pos1++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (pos1 != 0)
                            pos1--;
                        break;
                    case ConsoleKey.Backspace:
                        if (pos1 != 0)
                        {
                            pos1 -= 1;
                            currentLine = RemoveChar(currentLine, pos1);
                            text[pos2] = ArrayToString(currentLine);
                        }
                        else
                        {
                            if (pos2 != 0)
                            {
                                text[pos2 - 1] += ArrayToString(currentLine);
                                text.RemoveAt(pos2);
                                pos2 -= 1;
                                pos1 = 0;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    case ConsoleKey.Enter:
                        string partOfLine = ArrayToString(currentLine.ToArray()[pos1..^0].ToList());
                        text.Insert(pos2 + 1,
partOfLine);
                        currentLine = currentLine.ToArray()[0..pos1].ToList();
                        text[pos2] = ArrayToString(currentLine);
                        pos1 = 0;
                        pos2++;
                        break;
                    case ConsoleKey.Delete:
                        break;

                }
            }
            string response = "";
            foreach (string line in text)
            {
                response += (line + "\n");
            }
            if (way.Contains(".xml"))
            {
                response = Converters.ToXml(response).ReplaceLineEndings("\n");
            }
            else if (way.Contains(".json"))
            {
                response = Converters.ToJson(response);
            }
            return response;
        }
    }
}
