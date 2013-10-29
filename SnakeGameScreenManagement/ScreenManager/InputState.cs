using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SnakeGameScreenManagement
{
    public class InputState
    {
        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyBoardStates;
        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public InputState()
        {
            CurrentKeyBoardStates = new KeyboardState[MaxInputs];
            LastKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];
            GamePadWasConnected = new bool[MaxInputs];
        }

        public void Update()
        {
            for (int argi = 0; argi < MaxInputs; ++argi)
            {
                LastKeyboardStates[argi] = CurrentKeyBoardStates[argi];
                LastGamePadStates[argi] = CurrentGamePadStates[argi];

                CurrentKeyBoardStates[argi] = Keyboard.GetState((PlayerIndex)argi);
                CurrentGamePadStates[argi] = GamePad.GetState((PlayerIndex)argi);

                if (CurrentGamePadStates[argi].IsConnected) GamePadWasConnected[argi] = true;
            }
        }

        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int argi = (int)playerIndex;
                return (CurrentKeyBoardStates[argi].IsKeyDown(key) && LastKeyboardStates[argi].IsKeyUp(key));
            }
            else return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
        }

        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int argi = (int)playerIndex;
                return (CurrentGamePadStates[argi].IsButtonDown(button) && LastGamePadStates[argi].IsButtonUp(button));
            }
            else return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
        }

        public bool IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }

        public bool IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }

        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }

        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }

        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                IsNewKeyPress(Keys.P, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }
    }
}
