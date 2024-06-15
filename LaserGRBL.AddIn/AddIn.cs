using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public abstract class AddIn
    {
        public abstract string Title { get; }

        public AddIn(ToolStripMenuItem menuItem) {
            menuItem.Text = Title;
        }

        public virtual void OnFileLoaded(long elapsed, string filename) { }
        public virtual void OnFileLoading(long elapsed, string filename) { }

    }
}
