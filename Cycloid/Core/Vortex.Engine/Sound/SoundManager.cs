using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using vortexWin.Other;

namespace vortexWin.Engine.Sound
{
    public class SoundManager
    {
        static SoundEffectInstance background = null;
        public static float BackgroundVolume=0.5f;
        public static float EffectVolume=1.0f;

        static SoundManager()
        {
            //UpdateVolume();
        }

        //static public void UpdateVolume()
        //{
        //    SoundEffect.MasterVolume = GameSettings.Settings.Volume / 100.0f;
        //    EffectVolume = GameSettings.Settings.EffectVolume / 100.0f;
        //}
        
        static public void Play(SoundEffect effect){
            SoundEffectInstance inst=effect.CreateInstance();
            inst.Volume=EffectVolume;
            inst.Play();
        }

        static public SoundEffect Load(ContentManager content, string filename)
        {
            return content.Load<SoundEffect>(filename);
        }

        static public Song LoadSong(ContentManager content, string filename)
        {
            return content.Load<Song>(filename);
        }

        static public void PlayBG(SoundEffect effect)
        {
            if (background != null)
            {
                background.Pause();
                background.Dispose();
            }
            SoundEffectInstance inst = effect.CreateInstance();
            background = inst;

            background.IsLooped = true;
            background.Volume = BackgroundVolume;
            background.Play();
        }

        static public void PauseBG()
        {
            if (background != null)
                background.Pause();
        }

        static public void ResumeBG()
        {
            if(background!=null)
                background.Resume();
        }

        
    }
}
