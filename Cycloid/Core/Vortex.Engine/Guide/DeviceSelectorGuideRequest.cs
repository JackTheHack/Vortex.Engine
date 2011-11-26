using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;

namespace vortexWin.Engine.Xbox
{
    public class DeviceSelectorGuideRequest : GuideRequest
    {
        public AsyncCallback Callback { get; set; }
        public object State { get; set; }       

        public override void Run()
        {
            StorageDevice.BeginShowSelector(Callback, State);
        }
    }
}
