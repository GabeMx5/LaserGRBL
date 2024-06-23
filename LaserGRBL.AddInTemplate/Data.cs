namespace LaserGRBL.AddInTemplate
{
    public class Data
    {
        public ChangingVar<double> X = new ChangingVar<double>();
        public ChangingVar<double> Y = new ChangingVar<double>();
        public ChangingVar<double> Power = new ChangingVar<double>();

        public bool IsChanged => X.IsChanged || Y.IsChanged || Power.IsChanged;

        public bool IsPowerChanged => Power.IsChanged;

        public bool IsZeroPosition => X.Value == 0 && Y.Value == 0;

    }
}
