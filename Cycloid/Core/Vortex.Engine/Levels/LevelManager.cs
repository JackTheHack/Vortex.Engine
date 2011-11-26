using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace vortexWin.Other
{
    public class LevelManager
    {
        public static LevelData Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            LevelData ld = new LevelData();
            ld.FromStream(stream);
            stream.Close();
            return ld;
        }
    }
}
