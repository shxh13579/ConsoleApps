using System;

namespace RegistRightMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            //add registry for rightclick menu.
            var regedit = new EditReg();
            regedit.AddNewRightMenu(Environment.CurrentDirectory + "\\ConsoleForRight.exe");
        }
    }
}
