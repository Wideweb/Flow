﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DirectXGame
{
    public class InputManager
    {
        KeyboardState currentKeyState, prevKeyState;
        ButtonState currentMouseState, prevMouseState; 

        private static InputManager instance;

        private InputManager() { }

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();

                return instance;
            }
        }

        public void Update()
        {
            prevKeyState = currentKeyState;
            if(!ScreenManager.Instance.isTransitioning)
                currentKeyState = Keyboard.GetState();
        }

        public bool keyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }

            return false;
        }

        public bool keyReleased(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }

            return false;
        }

        public bool keyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }

            return false;
        }

        public bool MousePressed()
        {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    return true;
            return false;
        }

        public Vector2 MousePosition()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
