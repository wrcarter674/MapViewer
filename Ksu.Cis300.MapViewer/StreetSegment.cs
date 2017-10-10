using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ksu.Cis300.MapViewer
{
    public struct StreetSegment
    {
        private PointF _end;
        private PointF _start;
        private Pen _pen;
        private int _visibleLevels;
        /// <summary>
        /// A constructor for the StreetSegment
        /// </summary>
        /// <param name="start">The starting point of the street</param>
        /// <param name="end">The ending points of the street</param>
        /// <param name="color">The color of the street</param>
        /// <param name="width">The width of the pen</param>
        /// <param name="zoomLevel">the visible zoom level of the street</param>
        public StreetSegment(PointF start, PointF end, Color color, float width, int zoomLevel)
        {
            _end = end;
            _start = start;
            _pen = new Pen(color, width);
            _visibleLevels = zoomLevel;
        }
        /// <summary>
        /// Will draw the street segment using the graphics drawline method using the private vaules.
        /// </summary>
        /// <param name="graphics">a graphics object to draw</param>
        /// <param name="scale">The scale of the floats to pixels, which depends on the zoom level</param>
        public void Draw(Graphics graphics, int scale)
        {
            graphics.DrawLine(_pen, _start.X*scale, _start.Y*scale, _end.X*scale, _end.Y*scale);
        }
        /// <summary>
        /// A getter setter method for the start value of the StreetSegment. Will return a PointF
        /// </summary>
        public PointF Start 
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }
        /// <summary>
        /// A getter setter method for the end value of the StreetSegment. Will return a PointF
        /// </summary>
        public PointF End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }
        /// <summary>
        /// Will return the int for the Visible layers of the StreetSegment.
        /// </summary>
        public int VisibleLayers
        {
            get
            {
                return _visibleLevels;
            }
            
        }
    }
}
