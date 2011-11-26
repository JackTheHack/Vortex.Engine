using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;

namespace vortexWin.Engine.Xbox
{
    public class MessageBoxGuideRequest : GuideRequest
    {
        public MessageBoxGuideRequest()
        {
            Type = GuideRequestType.MessageBox;
        }

        public IEnumerable<string> Buttons { get; set; }
        public IAsyncResult Result { get; set; }
        public string Text { get; set; }
        public int FocusedButton { get; set; }
        public AsyncCallback Callback { get; set; }
        public object State { get; set; }
        public MessageBoxIcon MessageBoxIcon { get; set; }

        public override void Run()
        {
            Guide.BeginShowMessageBox(
                Title,
                Text,
                Buttons,
                FocusedButton,
                MessageBoxIcon,
                Callback,
                State);
        }
    }
}
