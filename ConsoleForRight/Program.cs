using System;

namespace ConsoleForRight
{
    class Program
    {
        static void Main(string[] args)
        {
            //var regedit = new EditReg();
            //regedit.AddNewRightMenu(@"D:\TestProject\TestApp\ConsoleForRight\bin\Debug\net5.0\ConsoleForRight.exe");
            //Console.WriteLine("Path:{0}", args[0]);
            //var file = new FileStream(@"C:\Users\DATIA-SOFT.DEV02\Desktop\tete", FileMode.CreateNew);
            //file.Write(Encoding.Default.GetBytes(args[0]));
            //file.Flush();
            //file.Close();
            var fileServer = new FileServer();
            Console.WriteLine("Please Enter your output path:");
            var path = Console.ReadLine();
            Console.WriteLine("Is Overwrite?(Y/N , default is NO)");
            var ow = Console.ReadLine();
            var overwrite = ow.ToUpper() == "Y";
            int success;


            int sum;
            if (args[1] != "1")
            {
                fileServer.MoveFile(args[0], path, out success, out sum, overwrite);
            }
            else
            {
                fileServer.CopyFile(args[0], path, out success, out sum, overwrite);
            }
            Console.WriteLine("File Task Finished , sucess {0} , total {1}.\r\nPress any key to exit.", success, sum);
            Console.ReadKey();
        }
    }
}
