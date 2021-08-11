using Microsoft.Win32;

namespace ConsoleForRight
{
    public class EditReg
    {
        /// <summary>
        /// add system registry.
        /// It just be used in my windows system.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public void AddNewRightMenu(string path)
        {
            var parentReg = Registry.ClassesRoot;
            //father level,MUIVerb's value is display words.
            var dirReg = parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles");
            dirReg.SetValue("MUIVerb", "Output Child Files");

            //ExtendedSubCommandsKey is a cascaded menu setting,the value must be this registry subkey.
            dirReg.SetValue("ExtendedSubCommandsKey", @"Directory\shell\OutputChildFiles");
            parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles\\Shell");

            //move file menu reg.
            var moveReg = parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles\\Shell\\move");
            moveReg.SetValue("MUIVerb", "Move All Files");
            var moveComReg = parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles\\Shell\\move\\command");
            moveComReg.SetValue("", path + " %V 0");

            //copy file menu reg.
            var copyReg = parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles\\Shell\\copy");
            copyReg.SetValue("MUIVerb", "Copy All Files");
            var copyComReg = parentReg.CreateSubKey(@"Directory\\shell\\OutputChildFiles\\Shell\\copy\\command");
            copyComReg.SetValue("", path + " %V 1");
            dirReg.Close();
            moveReg.Close();
            moveComReg.Close();
            copyReg.Close();
            copyComReg.Close();
            parentReg.Close();
        }
    }
}
