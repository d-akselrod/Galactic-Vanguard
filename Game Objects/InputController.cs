using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class InputController
    {
        public static KeyboardState currKeyboard;
        public static KeyboardState prevKeyboard;
        public static MouseState currMouse;
        public static MouseState prevMouse;

        public void Update()
        {
            prevKeyboard = currKeyboard;
            currKeyboard = Keyboard.GetState();

            prevMouse = currMouse;
            currMouse = Mouse.GetState();
        }

        public static InputController GetInput()
        {
            InputController currInput =  new InputController();
            currInput.Update();
            return currInput;
        }

        public static bool Click(Rectangle rec)
        {
            return currMouse.LeftButton == ButtonState.Pressed && rec.Contains(currMouse.Position);         
        }

        public static bool Misclick(Rectangle rec)
        {
            return currMouse.LeftButton == ButtonState.Pressed && !rec.Contains(currMouse.Position);
        }
    }
}
