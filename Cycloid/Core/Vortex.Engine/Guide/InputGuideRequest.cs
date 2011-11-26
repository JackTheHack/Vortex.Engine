using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using vortexWin.Engine.Input;

namespace vortexWin.Engine.Xbox
{
    public class InputGuideRequest : GuideRequest
    {
        public string Description { get; set; }

        public string DefaultText { get; set; }

        public AsyncCallback Callback { get; set; }

        public object State { get; set; }



        public override void Run()
        {
            if (GamePadController.Index.HasValue)
            {
                Guide.BeginShowKeyboardInput(
                                    GamePadController.Index.Value,
                                    Title, Description, DefaultText,
                                    Callback, State);
            }
        }
    }
}
