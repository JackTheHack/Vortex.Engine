using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace vortexWin
{
    public class FontHelper
    {
        public static SpriteFont Font;
        public static SpriteFont Arial40;
        public static SpriteFont GungsugChe,Micradi13,Micradi19;
        public static SpriteFont ButtonImgs = null;

        static FontHelper()
        {           
        }

        public static string EncodeString(string str, SpriteFont font)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
                if (font.Characters.Contains(c))
                    sb.Append(c);
            return sb.ToString();
        }

        public static void LoadFonts(ContentManager content)
        {
            Font=content.Load<SpriteFont>(@"fonts\Arial");
            Arial40 = content.Load<SpriteFont>(@"fonts\Arial40");
            GungsugChe = content.Load<SpriteFont>(@"fonts\Serif");
            Micradi13 = content.Load<SpriteFont>(@"fonts\Micradi13");
            Micradi19 = content.Load<SpriteFont>(@"fonts\Micradi19");
            ButtonImgs = content.Load<SpriteFont>(@"controls\xboxControllerSpriteFont");

        }

        public static void Unload()
        {
            Font = null;
            Arial40 = null;
            GungsugChe = null;
            Micradi13 = null;
            Micradi19 = null;
            ButtonImgs = null;
        }
    }
}
