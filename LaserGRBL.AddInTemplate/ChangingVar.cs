using SharpDX.XInput;
using System.Collections.Generic;

namespace LaserGRBL.AddInTemplate
{
    public class ChangingVar<T>
    {
        private T mCurrentValue;
        private T mPreviousValue;

        public T Value
        {
            get => mCurrentValue;
            set
            {
                mPreviousValue = mCurrentValue;
                mCurrentValue = value;
            }
        }

        public bool IsChanged => !EqualityComparer<T>.Default.Equals(mPreviousValue, mCurrentValue);

        public T PreviousValue => mPreviousValue;

    }

    public class ButtonVar : ChangingVar<bool>
    {

        private readonly string mOnCommand;
        private readonly string mOffCommand;
        private readonly GamepadButtonFlags mButtonCode;

        public ButtonVar(GamepadButtonFlags buttonCode, string onCommand, string offCommand) : base()
        {
            mButtonCode = buttonCode;
            mOnCommand = onCommand;
            mOffCommand = offCommand;
        }

        public string CurrentCommand
        {
            get
            {
                if (!PreviousValue && Value)
                    return mOnCommand;
                if (PreviousValue && !Value)
                    return mOffCommand;
                else
                    return null;
            }
        }

        public GamepadButtonFlags Buttons
        {
            set
            {
                Value = (value & mButtonCode) != 0;
            }
        }

    }

}
