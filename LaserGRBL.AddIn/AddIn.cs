using System;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public abstract class AddIn
    {

        public delegate void EventEnqueueCommand(string command);
        public event EventEnqueueCommand OnEnqueueCommand;

        public delegate void EventSendImmediate(byte b, bool mute = false);
        public event EventSendImmediate OnSendImmediate;

        public abstract string Title { get; }

        public AddIn(ToolStripMenuItem menuItem) {
            menuItem.Text = Title;
        }

        public virtual void OnFileLoaded(long elapsed, string filename) { }
        public virtual void OnFileLoading(long elapsed, string filename) { }

        public void EnqueueCommand(string command) => OnEnqueueCommand?.Invoke(command);

        public void SendImmediate(byte b, bool mute = false) => OnSendImmediate?.Invoke(b, mute);

    }
}
