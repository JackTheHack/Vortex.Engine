using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace vortexWin.Other
{

    public class LevelData
    {
        public virtual void FromStream(Stream input) { }
        public virtual void ToStream(Stream output){}
    }
}
