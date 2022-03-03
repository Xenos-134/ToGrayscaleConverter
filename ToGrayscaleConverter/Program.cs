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

        public static async void ProcessImage(int wdt, int hgt, string imagePath, StreamWriter file, string label)
        {
            Console.WriteLine($"Largura {GetSize(wdt, 500)}| Altura {GetSize(hgt, 700)}");
            string nameOfImage = imagePath.Split('/').Last();
            Console.WriteLine($"Processing image: {nameOfImage}");
            
            
            int bmWidth = GetSize(wdt, 500);
            int bmHeight = GetSize(hgt, 700);
            int [] bitArray = new int[wdt*hgt];
            Console.WriteLine($"Size of Array{bmHeight * bmHeight}");

            Bitmap imageCoppy = null;

            Bitmap l = new Bitmap(imagePath);

            using (Bitmap image = l.Clone(new Rectangle(0, 0, l.Width, l.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                int x, y;
                int[,] bitMapArr = new int[hgt, wdt];
                int topLX = Int32.MaxValue, botRX = 0;
                int topLY = Int32.MaxValue, botRY = 0;




                for (y = 0; y < image.Height; y++)
                {
                    for (x = 0; x < image.Width; x++)
                    {
                        Color newColor;
                        Color pixelColor = image.GetPixel(x, y);
                        int nc = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                        if (nc < (255 * 0.7))
                        {
                            //Gets coordinates of the upper corner
                            if (x < topLX) topLX = x;
                            if (y < topLY) topLY = y;

                            if (x > botRX) botRX = x;
                            if (y > botRY) botRY = y;
                        }


                        if (nc < (255 * 0.7)) newColor = Color.FromArgb(0, 0, 0);
                        else newColor = Color.FromArgb(255, 255, 255);
                        image.SetPixel(x, y, newColor); // Now greyscale
                    }

                }

                Bitmap nImage = image.Clone(new System.Drawing.Rectangle(topLX, topLY, (botRX - topLX), (botRY - topLY)), image.PixelFormat);
                imageCoppy = nImage;   // imageCoppy is grayscale version of image


                Image rImageCoppy = (Image)(new Bitmap(imageCoppy, new Size(500, 700)));

                Console.WriteLine($"Height:{rImageCoppy.Height}/ Width:{rImageCoppy.Width}");
                //rImageCoppy.Save($"{imagePath}BA.png", System.Drawing.Imaging.ImageFormat.Png);


                //Preenche 2d bitArray
                Bitmap bmi = new Bitmap(rImageCoppy);
                for (int h = 0; h < 700; h++)
                {
                    for (int w = 0; w < 500; w++)
                    {
                        Color pc = bmi.GetPixel(w, h);
                        int mp = (pc.R + pc.G + pc.B) / 3;

                        if (mp < (255 * 0.7))
                        {
                            //Caso area tem algma parte da letra
                            bitMapArr[h / bmHeight, w / bmWidth] = 1;
                        }
                    }
                }


                file.Write($"{label},");

                int c = 0;
                for (int h = 0; h < hgt; h++)
                {
                    for (int w = 0; w < wdt; w++)
                    {

                       /* if (bitMapArr[h, w] == 1) Console.Write($"[X] ");
                        else Console.Write($"[ ] ");*/


                        if (c < (hgt * wdt-1))
                        {
                            file.Write($"{bitMapArr[h, w]},");
                            c++;
                        }
                        else {
                            file.Write($"{bitMapArr[h, w]}");

                            continue;
                        }
                    }
                    //Console.WriteLine();
                }
                file.Write(Environment.NewLine);
                //Doesnt Allow Window to close
            }
        }

        static void Main(string[] args)
        {
            int numData = 100; //numero de amostras de cada dataset
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
                    Console.WriteLine(file);
                    ProcessImage(45, 45, itemsPath+"/"+file.Name, outputFile, label);
                }

            }
            Console.WriteLine("Finished Processing all Images");
            Console.ReadLine();

        }
    }
}
