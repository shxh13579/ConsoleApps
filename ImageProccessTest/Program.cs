using System;
using System.IO;

namespace ImageProccessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var path = args[0];
            var path = "C:\\Users\\DATIA-SOFT.DEV02\\Desktop\\images.png";
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("No Params Error");
            }
            if (!File.Exists(path))
            {
                Console.WriteLine("Path NOT Validated");
            }
            var proccessor = new ImageProccessor(path);
            var result = proccessor.Convolution();
            var savePath = "C:\\Users\\DATIA-SOFT.DEV02\\Desktop\\imges-" + Guid.NewGuid().ToString() + ".png";
            result.Save(savePath);
            Console.WriteLine("ssuccess");
        }

    }
}
