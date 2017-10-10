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

        private const int _maxZoom = 6;
        private int _scale;
        private int _zoom = 0;
        private QuadTree _tree;


        /// <summary>
        /// A public constructor for the map class. It will send the information to the quadtree as well as call the IsWithinBounds
        /// to check to the streets.
        /// </summary>
        /// <param name="streets">the list of streets being put into the map</param>
        /// <param name="bounds">the bounds of the map</param>
        /// <param name="scale">the scale factor of the map</param>
        public Map(List<StreetSegment> streets, RectangleF bounds, int scale)
        {
            
            int count = 0;
            try
            {
                foreach (StreetSegment str in streets)
                {
                    if (IsWithinBounds(str.Start, bounds) && IsWithinBounds(str.Start, bounds))
                    {
                        count++;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Street " + count + " is not within the given bounds");
            }
            InitializeComponent();
            _tree = new QuadTree(streets, bounds, _maxZoom);
            _scale = scale;
            bounds.Size= bounds.Size*scale;
        }
       
        /// <summary>
        /// Will check if the map can zoom out at the current zoom level.
        /// </summary>
        /// <returns></returns>
        public bool CanZoomOut()
        {
            if (_zoom > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// will check to see if the map can zoom in at the current level.
        /// </summary>
        /// <returns></returns>
        public bool CanZoomIn()
        {
            if(_zoom < _maxZoom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Will check to see if the point is in the rectangle including the sides of the rectangle
        /// </summary>
        /// <param name="point">the point being checked</param>
        /// <param name="rectangle">the rectangle check if the point is in there</param>
        /// <returns></returns>
        private static bool IsWithinBounds(PointF point, RectangleF rectangle)
        {
            if (rectangle.Contains(point))
            {
                return true;
            }
            else if(point.Y == rectangle.Top && point.X <= rectangle.Left && point.X >= rectangle.Right)
            {
                return true;
            }
            else if(point.Y == rectangle.Bottom && point.X <= rectangle.Left && point.X >= rectangle.Right)
            {
                return true;
            }
            else if(point.X == rectangle.Left && point.Y <= rectangle.Top && point.Y >= rectangle.Bottom)
            {
                return true;
            }
            else if(point.X == rectangle.Right && point.Y <= rectangle.Top && point.Y >= rectangle.Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
 
    }
}
