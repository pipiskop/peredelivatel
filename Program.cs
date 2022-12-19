using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace LoveKorovokkushat
{
    internal class gnom
    {
        static void Main(string[] args)
        {
            Cursor ukaz = new Cursor();
            ukaz.offset = 3;
            Console.WriteLine("Vvedite put do faila");
            ChooseFile openedFile = new ChooseFile();
            openedFile.way = Console.ReadLine();
            Console.Clear();
            openedFile.text = Editor.Edit(openedFile.way);
            Converters.Convert(openedFile.way);
        }
    }
}
