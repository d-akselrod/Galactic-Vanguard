using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class MusicController
    {
        private Hashtable musicLibrary;

        public MusicController()
        {
            musicLibrary = new Hashtable();
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
        }

        public MusicController(Hashtable musicLibrary)
        {
            this.musicLibrary = musicLibrary;
        }

        public void AddSong(string name, Song song)
        {
            musicLibrary.Add(name, song);
        }

        public void Update(GameState gameState)
        {
            switch (gameState.GetState())
            {
                case (GameState.LAUNCH):
                    MediaPlayer.Play((Song)musicLibrary["LAUNCH"]);
                    break;
                case (GameState.MENU):
                    MediaPlayer.Play((Song)musicLibrary["MENU"]);
                    Console.WriteLine("YES");
                    break;
                case (GameState.INGAME):
                    MediaPlayer.Play((Song)musicLibrary["INGAME"]);
                    break;
            }
        }
    }
}
