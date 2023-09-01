using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens
{
    public enum menuOptions
    {
        StartGame,
        Controls,
        Exit
    }
   
    public class TitleMenu
    {
        private KeyboardState curKeyboardState;
        private KeyboardState prevKeyboardState;

        public menuOptions curMenuOption = menuOptions.StartGame;

        public string[] menuButtons =  { "Start Game", "Controls", "Exit" };

        public void Update()
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            if(curKeyboardState.IsKeyDown(Keys.W) && prevKeyboardState.IsKeyUp(Keys.W))
            {
                switch (curMenuOption)
                {
                    case menuOptions.StartGame:
                        break;
                    case menuOptions.Controls: 
                    case menuOptions.Exit:
                        curMenuOption--;
                        break;
                }
            }
            if (curKeyboardState.IsKeyDown(Keys.S) && prevKeyboardState.IsKeyUp(Keys.S))
            {
                switch (curMenuOption)
                {
                    case menuOptions.StartGame:
                    case menuOptions.Controls:
                        curMenuOption++;
                        break;
                    case menuOptions.Exit:
                        break;
                }
            }
        }
    }
}