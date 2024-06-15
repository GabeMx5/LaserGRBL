using System.Windows.Forms;

namespace LaserGRBL.AddInTemplate
{
    public class Template: AddIn.AddIn
    {
        public override string Title => "Test LaserGRBL add-in";

        public Template(ToolStripMenuItem menuItem): base(menuItem)
        {
            ToolStripMenuItem privateItem = new ToolStripMenuItem
            {
                Text = "Private item"
            };
            privateItem.Click += PrivateItem_Click;
            menuItem.DropDownItems.Add(privateItem);
        }

        private void PrivateItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        public override void OnFileLoaded(long elapsed, string filename)
        {
            //MessageBox.Show($"Loaded {filename}");
        }
    }
}
