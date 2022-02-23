using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        //Esta funcao depois tem que devolver uma 2d array
        /*
            Funcao que faz pre-procesamento de imagem:
                >Converte tudo para duas cores.
                >Faz recorte de retangulo onde a imagem esta.
                >Faz resize para resolucao 500*700
         */
        public static void ProcessImage(int wdt, int hgt, string imagePath)
        {
            Console.WriteLine($"Largura {GetSize(wdt, 500)}| Altura {GetSize(hgt, 700)}");

            int bmWidth = GetSize(wdt, 500);
            int bmHeight = GetSize(hgt, 700);

            Bitmap imageCoppy = null;

            using (Bitmap image = new Bitmap(imagePath))
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
                        else
                        {
                            newColor = Color.FromArgb(255, 255, 255);
                        }

                        image.SetPixel(x, y, newColor); // Now greyscale
                    }

                }

                Bitmap nImage = image.Clone(new System.Drawing.Rectangle(topLX, topLY, (botRX - topLX), (botRY - topLY)), image.PixelFormat);
                imageCoppy = nImage;   // imageCoppy is grayscale version of image


                Image rImageCoppy = (Image)(new Bitmap(imageCoppy, new Size(500, 700)));

                Console.WriteLine($"Height:{rImageCoppy.Height}/ Width:{rImageCoppy.Width}");
                rImageCoppy.Save($"{imagePath}BA.png", System.Drawing.Imaging.ImageFormat.Png);


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

                for (int h = 0; h < hgt; h++)
                {
                    for (int w = 0; w < wdt; w++)
                    {
                        if (bitMapArr[h, w] == 1) Console.Write($"[X] ");
                        else Console.Write($"[ ] ");
                    }
                    Console.WriteLine();
                }
                //Doesnt Allow Window to close
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            //Test with some random params
            ProcessImage(30,36, "C:/Users/artem/Desktop/AL.jpg");
        }
    }
}
