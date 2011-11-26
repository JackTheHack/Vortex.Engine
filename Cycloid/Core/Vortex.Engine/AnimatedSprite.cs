using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vortexWin.Engine
{
    public class AnimatedSprite:Sprite
    {
        protected int currentPos;
        public int CurrentPosition { get { return currentPos; } set { currentPos = value; } }

        protected int currentRow;
        protected int currentSprite;
        protected Vector2 origin;

        public AnimatedSprite():base()
        {
            origin = new Vector2(0,0);
        }

        public int CurrentSprite
        {
            get { return GetCurrentSprite(); }
            set {
                SetCurrentSprite(value);
            }
        }

        protected virtual void SetCurrentSprite(int value)
        {
            currentSprite = value;
            if (sprite != null)
            {
                currentRow = (int)((currentSprite * spriteSize.X) / sprite.Width);
                currentPos = (int)(((currentSprite * spriteSize.X) % sprite.Width) / spriteSize.X);
            }
        }

        protected virtual int GetCurrentSprite()
        {
            return (int)(currentRow * (sprite.Width / spriteSize.X) + currentPos);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void  Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (Visible)
            Render(batch, 1.0f,currentPos);
        }

        public virtual void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, float opacity, int frame)
        {
            Vector2 _origin = new Vector2(0, 0);
            if (origin != null) _origin = this.origin;

            Color color = Color.White * opacity;

            batch.Draw(
                       sprite,
                       new Rectangle((int)position.X, (int)position.Y, (int)spriteSize.X, (int)spriteSize.Y),
                       new Rectangle(frame* (int)spriteSize.X, currentRow * (int)spriteSize.Y, (int)spriteSize.X, (int)spriteSize.Y),
                        color, alpha, _origin, SpriteEffects.None, 0);
        }

        

    }
}
