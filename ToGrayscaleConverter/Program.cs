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
    internal class Program
    {

        //Devolve dimensao de matriz binaria 
        public static int GetSize(int size, int resolution)
        {
            if ((float)resolution% size != 0) return (resolution/size) + 1;
            return ( resolution/ size);
        }


        /*
            Funcao que faz pre-procesamento de imagem:
                >Converte tudo para duas cores.
                >Faz recorte de retangulo onde a imagem esta.
                >Faz resize para resolucao 500*700

                NOTA: Por enquanto isto nao funciona para imagens de fundo preto devida a pobre resolucao
                Isso requere calibrar o limiar  a partir da qual sera considerad oque o pixel esta 'Ativado'
         */



        static void Main(string[] args)
        {
            int numData = 1000; //numero de amostras de cada dataset
            if (File.Exists("C:/Users/artem/Desktop/AI/PreProcess/Output.txt"))
            {
                File.Delete("C:/Users/artem/Desktop/AI/PreProcess/Output.txt");
            }

            StreamWriter outputFile = new StreamWriter("C:/Users/artem/Desktop/AI/PreProcess/Output.txt");


            //Test with some random params
            string path = "C:/Users/artem/Desktop/AI/PreProcess/trainingSet";
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
                    //ProcessImage(45, 45, itemsPath+"/"+file.Name, outputFile, label);
                    ExtractProperties(itemsPath + "/" + file.Name, outputFile, label);
                }

            }


            Console.WriteLine("Finished Processing all Images");
            Console.ReadLine();

        }



        //Estamos a procesar iamgens de 28 por 28
        public static void ExtractProperties(string imagePath, StreamWriter file, string label)
        {
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
                            counter++;
                        }
                        else
                        { //Last pixel
                            file.Write($"{pixelValue}");
                        }

                       // Console.Write($"[{pixelValue}] ");
                    }
                    //Console.WriteLine();
                }
            }
            file.Write(Environment.NewLine);
        }

    }
}
