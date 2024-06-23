using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{

    public abstract class CommonGrblCore
    {
        public enum MacStatus
        { Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog, Queue, Cooling, AutoHold, Tool } // "Tool" added in GrblHal

        public enum JogDirection
        { None, Abort, Home, N, S, W, E, NW, NE, SW, SE, Zup, Zdown }

        public enum StreamingMode
        { Buffered, Synchronous, RepeatOnError }


        [Serializable]
        public class GrblVersionInfo : IComparable, ICloneable
        {
            int mMajor;
            int mMinor;
            char mBuild;
            bool mOrtur;
            bool mGrblHal;

            string mVendorInfo;
            string mVendorVersion;

            public GrblVersionInfo(int major, int minor, char build, string VendorInfo, string VendorVersion, bool IsHAL)
            {
                mMajor = major; mMinor = minor; mBuild = build;
                mVendorInfo = VendorInfo;
                mVendorVersion = VendorVersion;
                mOrtur = VendorInfo != null && (VendorInfo.Contains("Ortur") || VendorInfo.Contains("Aufero"));
                mGrblHal = IsHAL;
            }

            public GrblVersionInfo(int major, int minor, char build)
            { mMajor = major; mMinor = minor; mBuild = build; }

            public GrblVersionInfo(int major, int minor)
            { mMajor = major; mMinor = minor; mBuild = (char)0; }

            public static bool operator !=(GrblVersionInfo a, GrblVersionInfo b)
            { return !(a == b); }

            public static bool operator ==(GrblVersionInfo a, GrblVersionInfo b)
            {
                if (Object.ReferenceEquals(a, null))
                    return Object.ReferenceEquals(b, null);
                else
                    return a.Equals(b);
            }

            public static bool operator <(GrblVersionInfo a, GrblVersionInfo b)
            {
                if ((Object)a == null)
                    throw new ArgumentNullException("a");
                return (a.CompareTo(b) < 0);
            }

            public static bool operator <=(GrblVersionInfo a, GrblVersionInfo b)
            {
                if ((Object)a == null)
                    throw new ArgumentNullException("a");
                return (a.CompareTo(b) <= 0);
            }

            public static bool operator >(GrblVersionInfo a, GrblVersionInfo b)
            { return (b < a); }

            public static bool operator >=(GrblVersionInfo a, GrblVersionInfo b)
            { return (b <= a); }

            public override string ToString()
            {
                if (mBuild == (char)0)
                    return string.Format("{0}.{1}", mMajor, mMinor);
                else
                    return string.Format("{0}.{1}{2}", mMajor, mMinor, mBuild);
            }

            public override bool Equals(object obj)
            {
                GrblVersionInfo v = obj as GrblVersionInfo;
                return v != null && this.mMajor == v.mMajor && this.mMinor == v.mMinor && this.mBuild == v.mBuild && this.mOrtur == v.mOrtur && this.mGrblHal == v.mGrblHal;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    // Maybe nullity checks, if these are objects not primitives!
                    hash = hash * 23 + mMajor.GetHashCode();
                    hash = hash * 23 + mMinor.GetHashCode();
                    hash = hash * 23 + mBuild.GetHashCode();
                    hash = hash * 23 + mOrtur.GetHashCode();
                    hash = hash * 23 + mGrblHal.GetHashCode();
                    return hash;
                }
            }

            public int CompareTo(Object version)
            {
                if (version == null)
                    return 1;

                GrblVersionInfo v = version as GrblVersionInfo;
                if (v == null)
                    throw new ArgumentException("Argument must be GrblVersionInfo");

                if (this.mMajor != v.mMajor)
                    if (this.mMajor > v.mMajor)
                        return 1;
                    else
                        return -1;

                if (this.mMinor != v.mMinor)
                    if (this.mMinor > v.mMinor)
                        return 1;
                    else
                        return -1;

                if (this.mBuild != v.mBuild)
                    if (this.mBuild > v.mBuild)
                        return 1;
                    else
                        return -1;

                return 0;
            }

            public object Clone()
            { return this.MemberwiseClone(); }

            public int Major => mMajor;
            public int Minor => mMinor;
            public bool IsOrtur => mOrtur;
            public bool IsHAL => mGrblHal;
            public bool IsLuckyOrturWiFi => IsOrtur && mVendorInfo == "Ortur Laser Master 3";
            public string MachineName => mVendorInfo;

            public string VendorName
            {
                get
                {
                    if (mVendorInfo != null && mVendorInfo.ToLower().Contains("ortur"))
                        return "Ortur";
                    else if (mVendorInfo != null && mVendorInfo.ToLower().Contains("Vigotec"))
                        return "Vigotec";
                    else if (mBuild == '#')
                        return "Emulator";
                    else
                        return "Unknown";
                }
            }

            public int OrturFWVersionNumber
            {
                get
                {
                    try { return int.Parse(mVendorVersion); }
                    catch { return -1; }
                }
            }
        }

        public virtual GrblVersionInfo GrblVersion { get; protected set; }

        public delegate void EventOnMachineStatus();
        public delegate void EventOnFileLoaded(long elapsed, string filename);
        public delegate void EventOnJogStateChange(bool jog);
        public delegate void EventOnProgramEnded(bool completed);

        public event EventOnMachineStatus MachineStatusChanged;
        public event EventOnFileLoaded OnFileLoading;
        public event EventOnFileLoaded OnFileLoaded;
        public event EventOnJogStateChange JogStateChange;
        public event EventOnProgramEnded OnProgramEnded;

        protected Control syncro;

        protected virtual void RiseMachineStatusChanged()
        {
            if (MachineStatusChanged != null)
            {
                if (syncro.InvokeRequired)
                    syncro.BeginInvoke(new EventOnMachineStatus(RiseMachineStatusChanged));
                else
                    MachineStatusChanged();
            }
        }

        protected virtual void RiseOnFileLoading(long elapsed, string filename)
        {
            OnFileLoading?.Invoke(elapsed, filename);
        }

        protected virtual void RiseOnFileLoaded(long elapsed, string filename)
        {
            OnFileLoaded?.Invoke(elapsed, filename);
        }

        protected virtual void RiseJogStateChange(bool jog)
        {
            if (JogStateChange != null)
            {
                if (syncro.InvokeRequired)
                    syncro.BeginInvoke(new EventOnJogStateChange(RiseJogStateChange), jog);
                else
                    JogStateChange(jog);
            }
        }

        protected virtual void RiseOnProgramEnded(bool completed)
        {
            OnProgramEnded?.Invoke(completed);
        }

        public abstract void OpenFile(Form parent, string filename = null, bool append = false);

        public abstract int ProgramTarget { get; }
        public abstract int ProgramSent { get; }
        public abstract int ProgramExecuted { get; }
        public abstract TimeSpan ProgramTime { get; }
        public abstract TimeSpan ProgramGlobalTime { get; }
        public abstract TimeSpan ProjectedTime { get; }
        public abstract MacStatus MachineStatus { get; }
        public abstract bool InProgram { get; }
        public abstract GPoint MachinePosition { get; }
        public abstract GPoint WorkPosition { get; }
        public abstract GPoint WorkingOffset { get; }
        public abstract int Executed { get; }
        public abstract bool IsConnected { get; }

        #region Grbl Version Support

        public bool SupportRTO
        { get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

        public virtual bool SupportTrueJogging
        { get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

        public bool SupportCSV
        { get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

        public bool SupportOverride
        { get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

        public bool SupportLaserMode
        { get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

        #endregion

        public virtual bool JogEnabled
        {
            get
            {
                if (SupportTrueJogging)
                    return IsConnected && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Jog);
                else
                    return IsConnected && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Run) && !InProgram;
            }
        }

        public abstract void EnqueueZJog(JogDirection dir, decimal step, bool fast);

        public abstract void BeginJog(PointF target, bool fast);

        public abstract void BeginJog(JogDirection dir, bool fast);

        public abstract void EndJogV11();

        public abstract bool CanReOpenFile { get; }

        public abstract bool QueueEmpty { get; }

        public abstract bool HasProgram { get; }

        public abstract bool CanLoadNewFile { get; }

        public abstract bool CanSendFile { get; }

        public abstract bool CanAbortProgram { get; }

        public abstract bool CanImportExport { get; }

        public abstract bool CanResetGrbl { get; }

        public abstract bool CanSendManualCommand { get; }

        public abstract bool CanDoHoming { get; }

        public abstract bool CanDoZeroing { get; }

        public abstract bool CanUnlock { get; }
        public abstract bool CanFeedHold { get; }

        public abstract bool CanResumeHold { get; }

        public abstract bool CanReadWriteConfig { get; }

        public abstract decimal LoopCount { get; set; }

        public abstract StreamingMode CurrentStreamingMode { get; }

        public abstract bool AutoCooling { get; }
        public abstract bool SupportAutoCooling { get; }

        public abstract TimeSpan AutoCoolingOn { get; }

        public abstract TimeSpan AutoCoolingOff { get; }

        public abstract void GrblHoming();

        public abstract void GrblUnlock();

        public abstract void SetNewZero();

        public int JogSpeed { get; set; }
        public decimal JogStep { get; set; }

        public abstract bool ContinuosJogEnabled { get; }

        public abstract void ExecuteCustombutton(string buttoncode);

        public abstract float CurrentF { get; }

        public abstract float CurrentS { get; }

    }


    public struct GPoint
    {
        public float X, Y, Z;

        public GPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static GPoint Zero { get { return new GPoint(); } }

        public static bool operator ==(GPoint a, GPoint b)
        { return a.X == b.X && a.Y == b.Y && a.Z == b.Z; }

        public static bool operator !=(GPoint a, GPoint b)
        { return !(a == b); }

        public static GPoint operator -(GPoint a, GPoint b)
        { return new GPoint(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }

        public static GPoint operator +(GPoint a, GPoint b)
        { return new GPoint(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }

        public override bool Equals(object obj)
        {
            return obj is GPoint && ((GPoint)obj) == this;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }

        public PointF ToPointF()
        {
            return new PointF(X, Y);
        }
    }

}
