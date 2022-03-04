using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//Probelmas
/*
    Quando estamos a trabalhar com imagens de fundo preto a imagem nao vai ser recortada
    >Tentar fazer treino com sem dar resize da imagem
 */

namespace ToGrayscaleConverter
{
    public class ExtractorProgram
    {
        public class Command
        { 
            public string Type { get; set; }
            public string Folder { get; set; }
            public string InputName { get; set; }
            public string OutputName { get; set; }
            public int NumOfElm { get; set; }
        }


        //Devolve dimensao de matriz binaria 
        public static int GetSize(int size, int resolution)
        {
            if ((float)resolution% size != 0) return (resolution/size) + 1;
            return ( resolution/ size);
        }

        public static void ConvertSingle(Command command)
        {
            Console.WriteLine($"Convert Single {command.Type}");
            using (StreamWriter outputFileTest = new StreamWriter($"C:/Users/artem/Desktop/AI/PreProcess/{command.InputName}.txt"))
            {
                ExtractProperties($"C:/Users/artem/Desktop/AI/PreProcess/{command.InputName}.jpg", outputFileTest, null);
            }
        }

        public static void ConvertAll(Command command)
        {
            int numData = 2000; //numero de amostras de cada dataset
            Console.WriteLine($"Convert All {command.Folder} / {command.OutputName}");
            if (File.Exists($"C:/Users/artem/Desktop/AI/PreProcess/{command.OutputName}.txt"))
            {
                File.Delete($"C:/Users/artem/Desktop/AI/PreProcess/{command.OutputName}.txt");
            }
            StreamWriter outputFile = new StreamWriter($"C:/Users/artem/Desktop/AI/PreProcess/{command.OutputName}.txt");

            //Test with some random params
            string path = command.Folder;
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] directories = info.GetDirectories();


            int ic;
            foreach (var directory in directories)
            {
                ic = numData;
                string label = directory.ToString(); // Label do ficheiro a ser classificado 
                string itemsPath = System.IO.Path.Combine(path, directory.ToString());

                Console.WriteLine($"Folder: {itemsPath}");

                DirectoryInfo dataDir = new DirectoryInfo(itemsPath);
                FileInfo[] files = dataDir.GetFiles();
                foreach (var file in files)
                {
                    if (ic-- < 0) continue;
                    ExtractProperties(itemsPath + "/" + file.Name, outputFile, label);
                }
            }
            Console.WriteLine("Finished Processing all Images");

        }


        public static void Main(string[] args)
        {

            while (true)
            {
                Console.Write("Introduza Commando: ");
                string input = Console.ReadLine();
                Command command = new Command();
                command.Type = input.Split().First();
                switch (command.Type)
                {
                    case "c": //Convets sigle element
                        
                        if (input.Split().Length == 2)
                        {
                            command.InputName = command.Type = input.Split()[1];
                            ConvertSingle(command);
                            continue;
                        }
                        Console.WriteLine("Numero de argumentos incorreto");
                        continue;
                    case "ca": //Converts all items inside of hte folder
                        if (input.Split().Length == 3)
                        {
                            command.Folder = command.Type = input.Split()[1];
                            command.OutputName = command.Type = input.Split()[2];
                            ConvertAll(command);
                            continue;
                        }
                        Console.WriteLine("Numero de argumentos incorreto");
                        continue;
                    case "exit": 
                        break;
                    default:
                        Console.WriteLine("Comando desconhecido");
                        continue;
                }
            }
        }

        //Estamos a procesar imagens de 28 por 28
        public static string ExtractProperties(string imagePath, StreamWriter file, string label)
        {
            string vector = "";

            Console.WriteLine($"Label:{label}-Processind image:{imagePath.Split('/').Last()}");
            using (Bitmap image = new Bitmap(imagePath))
            {
                if (label != null)
                {
                    file.Write($"{label},");
                }
                int pixelValue;
                int counter = 0;
                for (int h = 0; h < image.Height; h++)
                {
                    for (int w = 0; w < image.Width; w++)
                    {
                        //Uma vez que estamos a trabahar com imagens a preto e branco isso nem seria preciso
                        pixelValue = (image.GetPixel(h,w).R+image.GetPixel(h, w).G + image.GetPixel(h, w).B)/3;
                        if (counter < (image.Width*image.Height - 1))
                        {
                            file.Write($"{pixelValue},");
                            vector += $"{pixelValue},";
                            counter++;
                        }
                        else
                        { //Last pixel
                            file.Write($"{pixelValue}");
                            vector += $"{pixelValue}";
                        }
                    }
              
                }
            }
            file.Write(Environment.NewLine);
            return vector;
        }

    }
}
