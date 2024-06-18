using System;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public abstract class AddIn
    {

        public delegate void EventEnqueueCommand(string command);
        public event EventEnqueueCommand OnEnqueueCommand;

        public abstract string Title { get; }

        public AddIn(ToolStripMenuItem menuItem) {
            menuItem.Text = Title;
        }

        public virtual void OnFileLoaded(long elapsed, string filename) { }
        public virtual void OnFileLoading(long elapsed, string filename) { }

        public void EnqueueCommand(string command) {
            OnEnqueueCommand?.Invoke(command);
        }

    }
}
