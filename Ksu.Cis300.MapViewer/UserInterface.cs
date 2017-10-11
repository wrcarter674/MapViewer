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

        private static List<StreetSegment> ReadFile(string fileName, out RectangleF rectangle)
        {
            List<StreetSegment> list = null;
            int count = 0;
            float width;
            float height;
            using (StreamReader input = new StreamReader(fileName))
            {
                string[] conttents = input.ReadLine().Split(',');
                width = Convert.ToSingle(conttents[0]);
                height = Convert.ToSingle(conttents[1]);
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();
                    string[] contents = line.Split(',');
                   
                    PointF start = new PointF(Convert.ToSingle(contents[0]), Convert.ToSingle(contents[1]));
                    PointF end = new PointF(Convert.ToSingle(contents[2]), Convert.ToSingle(contents[3]));
                    StreetSegment str = new StreetSegment(start, end, Color.FromArgb(Convert.ToInt32(contents[4])), Convert.ToSingle(contents[5]), Convert.ToInt32(contents[6]));
                    list.Add(str);
                    
                }

            }
            rectangle = new RectangleF(0, 0, width, height);
            return list;
        }

        private void uxOpenMap_Click(object sender, EventArgs e)
        {
            if(uxOpenDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = uxOpenDialog.FileName;
                RectangleF rectangle = new RectangleF();
                List<StreetSegment> strs = ReadFile(filename, out rectangle);
                _map = new Map(strs, rectangle, _initalScale);
                _map.Clear();
            }
        }
    }
}
