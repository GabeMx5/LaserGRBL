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
        { Abort, Home, N, S, W, E, NW, NE, SW, SE, Zup, Zdown, Position }

        public enum StreamingMode
        { Buffered, Synchronous, RepeatOnError }

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

        public abstract CommonImageButton AddButton(string icon, string tooltip);

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
        public abstract bool SupportRTO { get; }
        public abstract bool SupportTrueJogging { get; }
        public abstract bool SupportCSV { get; }
        public abstract bool SupportOverride { get; }
        public abstract bool SupportLaserMode { get; }
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
        
        public abstract void JogToPosition(PointF target, bool fast);

        public abstract void JogToPosition(PointF target, float speed);

        public abstract void JogToDirection(JogDirection dir, bool fast);

        public abstract void JogToDirection(JogDirection dir, bool fast, decimal step);

        public abstract void JogToDirection(JogDirection dir, float speed, decimal step);

        public abstract void ContinuousJogToPosition(PointF target, float speed);

        public abstract void ContinuousJogToDirection(JogDirection dir, float speed);

        public abstract void JogAbort();

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

        public abstract void ExecuteCustomCode(string buttoncode);

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
