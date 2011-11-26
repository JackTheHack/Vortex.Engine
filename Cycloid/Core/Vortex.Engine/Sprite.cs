using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vortexWin.Engine
{
    public class Sprite
    {
        protected Texture2D sprite;
        protected Vector2 position = new Vector2(0, 0);
        //protected Game game;
        protected Vector2 spriteSize;

        public bool Visible=true;

        public Vector2 Position
        {
            get { return position; }
            set { 
                position = value;
                OnPositionUpdate();
            }
        }

        protected float alpha = 0;
        public float Alpha
        {
            get { return alpha; }
            set { 
                alpha = value;
                OnPositionUpdate();
            }
        }

        public Vector2 Size
        {
            get { return spriteSize; }
            set { spriteSize = value; }
        }

        public virtual void OnPositionUpdate(){}

        public Sprite()
        {
            sprite=null;
        }

        public virtual void Move(Vector2 newpos)
        {
            position += newpos;
            OnPositionUpdate();
        }

        public virtual void Rotate(float newrotation)
        {
            alpha += newrotation;
            OnPositionUpdate();
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if(Visible)
            batch.Draw(sprite, 
                new Rectangle((int)position.X,(int)position.Y,(int)spriteSize.X,(int)spriteSize.Y), 
                new Rectangle(0,0,(int)spriteSize.X,(int)spriteSize.Y),
                Color.White,alpha,new Vector2(0,0),SpriteEffects.None,0);
        }

        public virtual void Update(GameTime time){}

        public virtual void Load(GameScreen view) { }

        public void Load(GameScreen view, string name)
        {
            sprite = view.Content.Load<Texture2D>(name);
            spriteSize = new Vector2(sprite.Width, sprite.Height);
        }
    }
}
