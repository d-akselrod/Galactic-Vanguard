namespace Galactic_Vanguard
{
    public class GameState
    {
        public const int LAUNCH = 0;
        public const int MENU = 1;
        public const int PREGAME = 2;
        public const int INGAME = 3;
        public const int PAUSE = 4;
        public const int ENDGAME = 5;

        private int gameState = 0;

        public GameState()
        {
            gameState = LAUNCH;
        }

        public void SetState(int gameState)
        {
            this.gameState = gameState;
        }

        public int GetState()
        {
            return gameState;
        }
    }
}