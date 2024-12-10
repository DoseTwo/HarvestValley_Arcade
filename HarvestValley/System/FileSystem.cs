using System.IO;
using System.Runtime.InteropServices;
using IniParser;
using IniParser.Model;

namespace HarvestValley.System
{
    internal class FileSystem
    {
        private readonly string rootPath = "lol";
        //there really isn't much to put here ¯\_(ツ)_/¯
        public FileSystem() 
        { 
            
        }
        //this is gonna use .ini

        /// <summary>
        /// Writes/Overwrites a string of your choice to the specific ini file that you want
        /// </summary>
        /// <param name="_file">name for the file</param>
        /// <param name="ID">the id tag for the ini file</param>
        /// <param name="_toWrite">string to write into the ini file</param>
        public void WriteToIni(string _file, string ID, string _toWrite)
        {
            //TODO proper error checking
            if (File.Exists(_file + ".ini"))
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(_file + ".ini");

                data["Save"][ID.ToLower()] = _toWrite;
                parser.WriteFile(_file + ".ini", data);
            }
        }

        /// <summary>
        /// Reads a string of your choice to the specific ini file that you want
        /// </summary>
        /// <param name="_file">name for the file</param>
        /// <param name="ID">the id tag for the ini file</param>
        public string ReadFromIni(string _file, string ID)
        {
            //TODO add reading, and proper error checking
            if (File.Exists(_file + ".ini"))
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(_file + ".ini");

                string _tmp = data["Save"][ID.ToLower()];

                return _tmp;
            }
            else
            {
                return string.Empty;
            }
        }

            /// <summary>
            /// Creates a simple ini file for easy reading/writing
            /// </summary>
            /// <param name="_name">file name</param>
            /// <param name="_path">path for where you want it to be located</param>
            public void CreateFile(string _name, string _path = "")
        {
            if (!File.Exists(_name + ".ini"))
            {
                File.Create(_name + ".ini").Close();
            }
        }
    }
}
