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
                    temp.Start = new PointF(split, y);
                    eastSide.Add(temp);
                    temp2.End = new PointF(split, y);
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
                    
                    temp.End = new PointF(x, split); 
                    southSide.Add(temp);
                    temp2.Start = new PointF(x, split);
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
                List<StreetSegment> nonVisible = new List<StreetSegment>();
                List<StreetSegment> northSide = new List<StreetSegment>();
                List<StreetSegment> southSide = new List<StreetSegment>();

                List<StreetSegment> nwSide = new List<StreetSegment>();
                List<StreetSegment> neSide = new List<StreetSegment>();
                List<StreetSegment> swSide = new List<StreetSegment>();
                List<StreetSegment> seSide = new List<StreetSegment>();

                float newWidth = area.Width / 2;
                float newHeight = area.Height / 2;                
                float x = (area.Width / 2) + area.Left;
                float y = area.Top +(area.Height / 2);
                
                SplitVisbility(streets, height, _streets, nonVisible);
                
                SplitNorthSouth(nonVisible, y, northSide, southSide);
                SplitEastWest(northSide, x, nwSide, neSide);
                SplitEastWest(southSide, x, swSide, seSide);
                
                _neTree = new QuadTree(neSide, new RectangleF(x, area.Top, newWidth, newHeight), height-1);
                _nwTree = new QuadTree(nwSide, new RectangleF(area.Left, area.Top, newWidth, newHeight), height-1);
                _seTree = new QuadTree(seSide, new RectangleF(x, y, newWidth, newHeight), height-1);
                _swTree = new QuadTree(swSide, new RectangleF(area.Left, y, newWidth, newHeight), height-1);
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
                    _nwTree.Draw(drawing, scale, maxDepth - 1);
                    _neTree.Draw(drawing, scale, maxDepth - 1);
                    _swTree.Draw(drawing, scale, maxDepth - 1);
                    _seTree.Draw(drawing, scale, maxDepth - 1);
                }
            }
        }
    }
}
