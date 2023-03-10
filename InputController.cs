﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class InputController
    {
        public KeyboardState currKeyboard;
        public KeyboardState prevKeyboard;
        public MouseState currMouse;
        public MouseState prevMouse;

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
    }
}
