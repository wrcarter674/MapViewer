using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ksu.Cis300.MapViewer
{
    class QuadTree
    {
        private QuadTree _nwTree = null;
        private QuadTree _neTree = null;
        private QuadTree _swTree = null;
        private QuadTree _seTree = null;   
        private RectangleF _bounds;
        private List<StreetSegment> _streets;
        /// <summary>
        /// Will spit all the streets into visible and non visible streets
        /// </summary>
        /// <param name="streets">All the streets to be analyzed</param>
        /// <param name="height">The height to divide the streets by</param>
        /// <param name="visibleStreets">the streets visible at that height</param>
        /// <param name="invisibleStreets">the invisible streets at the height</param>
        private static void SplitVisbility(List<StreetSegment> streets, int height, List<StreetSegment> visibleStreets, List<StreetSegment> invisibleStreets)
        {
            foreach (StreetSegment str in streets)
            {
                if(str.VisibleLayers > height)
                {
                    visibleStreets.Add(str);
                }
                else
                {
                    invisibleStreets.Add(str);
                }
            }
        }
        /// <summary>
        /// Will split the streets east and west of a given line, and put them in the respective list
        /// </summary>
        /// <param name="streets">All the streets being analyzed</param>
        /// <param name="split">The value of the vertical line</param>
        /// <param name="westSide">in which street segments west of a given vertical line will be placed</param>
        /// <param name="eastSide">where the street segments east of a given vertical line will be plaved.</param>
        private static void SplitEastWest(List<StreetSegment> streets, float split, List<StreetSegment> westSide, List<StreetSegment> eastSide)
        {
            foreach (StreetSegment str in streets)
            {
                if(str.Start.X < split && str.End.X < split)
                {
                    westSide.Add(str);
                }
                else if(str.Start.X > split && str.End.X > split)
                {
                    eastSide.Add(str);
                }
                else
                {
                    StreetSegment temp = str;
                    StreetSegment temp2 = str;
                    float y = (((str.End.Y - str.Start.Y) * (split - str.Start.X))
                        / (str.End.X - str.Start.X))
                        + str.Start.Y;
                    PointF start = new PointF(split, y);
                    PointF end = new PointF(split, y);
                    temp.Start = start;
                    eastSide.Add(temp);
                    temp2.End = end;
                    westSide.Add(temp2);
                }
                    
            }
        }
        /// <summary>
        /// Will split the steets north and south of the given line, and put them in the resepective list
        /// </summary>
        /// <param name="streets">all the streets being analyzed</param>
        /// <param name="split">the y line</param>
        /// <param name="northSide">the list of streets north of the split</param>
        /// <param name="southSide">the list of streets south of the split</param>
        private static void SplitNorthSouth(List<StreetSegment> streets, float split, List<StreetSegment> northSide, List<StreetSegment> southSide)
        {
            foreach (StreetSegment str in streets)
            {
                if (str.Start.X < split && str.End.X < split)
                {
                    southSide.Add(str);
                }
                else if (str.Start.X > split && str.End.X > split)
                {
                    northSide.Add(str);
                }
                else
                {
                    StreetSegment temp = str;
                    StreetSegment temp2 = str;
                    float x = (((str.End.X - str.Start.X) * (split - str.Start.Y))
                        / (str.End.Y - str.Start.Y))
                        + str.Start.X;
                    PointF start = new PointF(split, x);
                    PointF end = new PointF(split, x);
                    temp.Start = start;
                    southSide.Add(temp);
                    temp2.End = end;
                    northSide.Add(temp2);
                }

            }
        }
        /// <summary>
        /// A constructor for the Quad Tree. To be recursively called.
        /// </summary>
        /// <param name="streets">used to intialize the _streets private variable</param>
        /// <param name="area">the area of the quadrant</param>
        /// <param name="height">the height of the tree to be constructed</param>
        public QuadTree(List<StreetSegment> streets, RectangleF area, int height)
        {
            _bounds = area;
      
            if (height == 0)
            {
                _streets = streets;
            }
            else
            {
                _streets = streets;
                List<StreetSegment> northSide = null;
                List<StreetSegment> southSide = null;

                List<StreetSegment> nwSide = null;
                List<StreetSegment> neSide = null;
                List<StreetSegment> swSide = null;
                List<StreetSegment> seSide = null;
                List<StreetSegment> node = _streets;
                float newWidth = _bounds.Width / 2;
                float newHeight = _bounds.Height / 2;                
                float x = (_bounds.Width / 2) + _bounds.Left;
                float y = _bounds.Top - (_bounds.Height / 2);
                SplitNorthSouth(node, y, northSide, southSide);
                SplitEastWest(northSide, x, nwSide, neSide);
                SplitEastWest(southSide, _bounds.Width / 2 + _bounds.Left, swSide, seSide);
                
                _neTree = new QuadTree(neSide, new RectangleF(x, _bounds.Top, newWidth, newHeight), height-1);
                _nwTree = new QuadTree(nwSide, new RectangleF(_bounds.Left, _bounds.Top, newWidth, newHeight), height-1);
                _seTree = new QuadTree(seSide, new RectangleF(x, y, newWidth, newHeight), height-1);
                _swTree = new QuadTree(swSide, new RectangleF(_bounds.Top, y, newWidth, newHeight), height-1);
            }
        }
        /// <summary>
        /// A method to draw the contents of the tree. 
        /// </summary>
        /// <param name="drawing">the graphic being drawn</param>
        /// <param name="scale">the scale factor for translating map coordinates to pixel coordinates</param>
        /// <param name="maxDepth">the maximum depth of tree nodes</param>
        public void Draw(Graphics drawing, int scale, int maxDepth)
        {
            RectangleF rec =drawing.ClipBounds;
            float x = rec.X / scale;
            float y = rec.Y / scale;
            float width = rec.Width / scale;
            float height = rec.Height / scale;
            RectangleF temp = new RectangleF(x, y, width, height);
            if (temp.IntersectsWith(_bounds))
            {
                foreach(StreetSegment str in  _streets)
                {
                    str.Draw(drawing, scale);
                    if (maxDepth > 0)
                    {
                        Draw(drawing, scale, maxDepth - 1);
                    }
                }
            }
        }
    }
}
