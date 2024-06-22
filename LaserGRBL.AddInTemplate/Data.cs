namespace LaserGRBL.AddInTemplate
{
    public class Data
    {
        private double mX = 0;
        private bool mIsChanged = false;
        public double X {
            get => mX;
            set
            {
                if (value != mX)
                {
                    mX = value;
                    mIsChanged = true;
                }
            }
        }

        private double mY = 0;
        public double Y {
            get => mY;
            set
            {
                if (value != mY)
                {
                    mY = value;
                    mIsChanged = true;
                }
            }
        }

        private bool mPowerChanged = false;
        private double mPower = 0;
        public double Power {
            get => mPower;
            set
            {
                if (value != mPower)
                {
                    mPower = value;
                    mPowerChanged = true;
                }
            }
        }

        public bool IsChanged => mIsChanged || mPowerChanged;

        public bool IsPowerChanged => mPowerChanged;

        public bool IsZeroPosition => mX == 0 && mY == 0;

        public void BeginUpdate()
        {
            mIsChanged = false;
            mPowerChanged = false;
        }

    }
}
