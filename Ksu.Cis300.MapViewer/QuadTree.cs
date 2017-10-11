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
        private RectangleF _bounds = new RectangleF();
        private List<StreetSegment> _streets = new List<StreetSegment>();
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
        private static void SplitEastWest(List<StreetSegment> streets, float x, List<StreetSegment> westSide, List<StreetSegment> eastSide)
        {
            foreach (StreetSegment str in streets)
            {
                if(str.Start.X <= x && str.End.X <= x)
                {
                    westSide.Add(str);
                }
                else if(str.Start.X > x && str.End.X > x)
                {
                    eastSide.Add(str);
                }
                else
                {
                    StreetSegment temp = str;
                    StreetSegment temp2 = str;
                    float y = (((str.End.Y - str.Start.Y) * (x - str.Start.X))
                        / (str.End.X - str.Start.X))
                        + str.Start.Y;
                    temp.Start = new PointF(x, y);
                    ///eastSide.Add(temp);
                    temp2.End = new PointF(x, y);
                    ///westSide.Add(temp2);
                    if(temp.End.X > x)
                    {
                        eastSide.Add(temp);
                        westSide.Add(temp2);
                    }
                    else
                    {
                        eastSide.Add(temp2);
                        westSide.Add(temp);
                    }
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
        private static void SplitNorthSouth(List<StreetSegment> streets, float y, List<StreetSegment> northSide, List<StreetSegment> southSide)
        {
            foreach (StreetSegment str in streets)
            {
                if (str.Start.Y <= y && str.End.Y <= y)
                {
                    northSide.Add(str);
                }
                else if (str.Start.Y > y && str.End.Y > y)
                {
                    southSide.Add(str);
                }
                else
                {
                    StreetSegment temp = str;
                    StreetSegment temp2 = str;
                    float x = (((str.End.X - str.Start.X) * (y - str.Start.Y))
                        / (str.End.Y - str.Start.Y))
                        + str.Start.X;
                    
                    temp.End = new PointF(x, y); 
                    ///northSide.Add(temp);
                    temp2.Start = new PointF(x, y);
                    ///southSide.Add(temp2);
                    if(temp.End.Y > y)
                    {
                        southSide.Add(temp);
                        northSide.Add(temp2);
                    }
                    else
                    {
                        southSide.Add(temp2);
                        northSide.Add(temp);
                    }
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
                List<StreetSegment> nonVisible = new List<StreetSegment>();
                List<StreetSegment> visible = new List<StreetSegment>();
                SplitVisbility(streets, height, visible, nonVisible);
                _streets = visible;

                List<StreetSegment> northSide = new List<StreetSegment>();
                List<StreetSegment> southSide = new List<StreetSegment>();
                float x = (area.Width / 2) + area.Left;
                float y = (area.Height / 2) + area.Top;

                SplitNorthSouth(nonVisible, y, northSide, southSide);

                List<StreetSegment> nwSide = new List<StreetSegment>();
                List<StreetSegment> neSide = new List<StreetSegment>();
                List<StreetSegment> swSide = new List<StreetSegment>();
                List<StreetSegment> seSide = new List<StreetSegment>();

                SplitEastWest(northSide, x, nwSide, neSide);
                SplitEastWest(southSide, x, swSide, seSide);

                float newWidth = area.Width / 2;
                float newHeight = area.Height / 2;
                height--;
                _neTree = new QuadTree(neSide, new RectangleF(x, area.Top, newWidth, newHeight), height);
                _nwTree = new QuadTree(nwSide, new RectangleF(area.Left, area.Top, newWidth, newHeight), height);
                _seTree = new QuadTree(seSide, new RectangleF(x, y, newWidth, newHeight), height);
                _swTree = new QuadTree(swSide, new RectangleF(area.Left, y, newWidth, newHeight), height);
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
            RectangleF rec = new RectangleF(drawing.ClipBounds.X / scale, drawing.ClipBounds.Y / scale, drawing.ClipBounds.Width / scale, drawing.ClipBounds.Height / scale); ;
           
            if (rec.IntersectsWith(_bounds))
            {
                foreach(StreetSegment str in  _streets)
                {
                    str.Draw(drawing, scale);
                    
                }
                if (maxDepth > 0)
                {
                    maxDepth--;
                    _nwTree.Draw(drawing, scale, maxDepth);
                    _neTree.Draw(drawing, scale, maxDepth);
                    _swTree.Draw(drawing, scale, maxDepth);
                    _seTree.Draw(drawing, scale, maxDepth);
                }
            }
        }
    }
}
