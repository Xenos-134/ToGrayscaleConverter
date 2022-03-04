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

        //Devolve dimensao de matriz binaria 
        public static int GetSize(int size, int resolution)
        {
            if ((float)resolution% size != 0) return (resolution/size) + 1;
            return ( resolution/ size);
        }


        public static void Main(string[] args)
        {
            int numData = 1000; //numero de amostras de cada dataset
/*            if (File.Exists("C:/Users/artem/Desktop/AI/PreProcess/Output.txt"))
            {
                File.Delete("C:/Users/artem/Desktop/AI/PreProcess/Output.txt");
            }
            StreamWriter outputFile = new StreamWriter("C:/Users/artem/Desktop/AI/PreProcess/Output.txt");*/

            //Test with some random params
            string path = "C:/Users/artem/Desktop/AI/PreProcess/trainingSet";
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] directories = info.GetDirectories();

            using (StreamWriter outputFileTest = new StreamWriter("C:/Users/artem/Desktop/AI/PreProcess/testOutput.txt"))
            {
                ExtractProperties("C:/Users/artem/Desktop/AI/PreProcess/test.jpg", outputFileTest, "4");
            }


            int ic;
/*          foreach (var directory in directories)
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
            }*/



            Console.WriteLine("Finished Processing all Images");
            Console.ReadLine();

        }



        //Estamos a procesar iamgens de 28 por 28
        public static string ExtractProperties(string imagePath, StreamWriter file, string label)
        {
            string vector = "";

            Console.WriteLine($"Label:{label}-Processind image:{imagePath.Split('/').Last()}");
            using (Bitmap image = new Bitmap(imagePath))
            {
                file.Write($"{label},");
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
