using _Scripts.Interfaces.Input;

namespace _Scripts.Services.Input
{
    public sealed class InputModeProvider : IInputModeProvider
    {
        public InputMode CurrentMode { get; private set; }

        public InputModeProvider(InputMode initMode)
        {
            CurrentMode = initMode;
        }

        public void Set(InputMode mode)
        {
            if (mode == CurrentMode) return;
            CurrentMode = mode;
        }
    }
}