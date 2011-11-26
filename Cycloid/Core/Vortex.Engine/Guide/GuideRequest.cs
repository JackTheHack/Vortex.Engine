using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vortexWin.Engine.Xbox
{
    public abstract class GuideRequest
    {
        public GuideRequestType Type { get; set; }
        public string Title { get; set; }

        public abstract void Run();
    }
}
