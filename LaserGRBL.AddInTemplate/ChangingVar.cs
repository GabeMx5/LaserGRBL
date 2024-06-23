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
}
