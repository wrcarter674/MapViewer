/*UserInterface.cs
 * By: William Carter
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Ksu.Cis300.MapViewer
{
    public partial class UserInterface : Form
    {
        private int _initalScale = 10;
        private Map _map;
        public UserInterface()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Will read in the file using a streamreader. It puts it into a list.
        /// </summary>
        /// <param name="fileName">the name of the file being read</param>
        /// <param name="rectangle">the rectangle out parameter that the streets go with</param>
        /// <returns></returns>
        private static List<StreetSegment> ReadFile(string fileName, out RectangleF rectangle)
        {
            List<StreetSegment> list = new List<StreetSegment>();
            
         
            using (StreamReader input = new StreamReader(fileName))
            {
                string[] conttents = input.ReadLine().Split(',');
                rectangle = new RectangleF(0, 0, Convert.ToSingle(conttents[0]), Convert.ToSingle(conttents[1]));
                while (!input.EndOfStream)
                {
                    string[] contents = input.ReadLine().Split(',');
                                     
                    PointF start = new PointF(Convert.ToSingle(contents[0]), Convert.ToSingle(contents[1]));
                    PointF end = new PointF(Convert.ToSingle(contents[2]), Convert.ToSingle(contents[3]));
                    StreetSegment str = new StreetSegment(start, end, Color.FromArgb(Convert.ToInt32(contents[4])), Convert.ToSingle(contents[5]), Convert.ToInt32(contents[6]));
                    list.Add(str);
                    
                }
            }
            
            return list;
        }
        /// <summary>
        /// Button operator for the open map button. it will open a file dialog and read the contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxOpenMap_Click(object sender, EventArgs e)
        {
            List<StreetSegment> strs = null;
            RectangleF bounds;
            if (uxOpenDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    strs = ReadFile(uxOpenDialog.FileName, out bounds);
                    _map = new Map(strs, bounds, _initalScale);
                    uxMapContainer.Controls.Clear();
                    uxMapContainer.Controls.Add(_map);
                    uxZoomIn.Enabled = true;
                    uxZoomOut.Enabled = false;
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        /// <summary>
        /// zoomin button click. Will update the cursor when zooming out and call the map class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxZoomIn_Click(object sender, EventArgs e)
        {
            Point scrollPosition = uxMapContainer.AutoScrollPosition;
            scrollPosition.X = scrollPosition.X * -1;
            scrollPosition.Y = scrollPosition.Y * -1;
            _map.ZoomIn();
            uxZoomIn.Enabled = _map.CanZoomIn();
            uxZoomOut.Enabled = _map.CanZoomOut();
            uxMapContainer.AutoScrollPosition = new Point(scrollPosition.X * 2 + uxMapContainer.ClientSize.Width / 2, scrollPosition.Y * 2 + uxMapContainer.ClientSize.Height / 2);
        }

        
        /// <summary>
        /// zoomout button click. Will update the cursor when zooming out and call the map class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxZoomOut_Click_1(object sender, EventArgs e)
        {
            Point scrollPosition = uxMapContainer.AutoScrollPosition;
            scrollPosition.X = scrollPosition.X * -1;
            scrollPosition.Y = scrollPosition.Y * -1;
            _map.ZoomOut();
            uxZoomIn.Enabled = _map.CanZoomIn();
            uxZoomOut.Enabled = _map.CanZoomOut();
            uxMapContainer.AutoScrollPosition = new Point(scrollPosition.X / 2 - uxMapContainer.ClientSize.Width / 4, scrollPosition.Y / 2 - uxMapContainer.ClientSize.Height / 4);
        }
    }
}
