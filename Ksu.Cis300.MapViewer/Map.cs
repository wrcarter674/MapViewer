using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.MapViewer
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
        }
        private const int _maxZoom = 6;
        private int _scale;
        private int _zoom = 0;
        private QuadTree _tree;
        
    }
}
