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
                    PointF start = new PointF(split, y);
                    PointF end = new PointF(split, y);
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
            _streets = streets;
            if (height == 0)
            {
                _streets = streets;
            }
            else
            {
                List<StreetSegment> northSide = null;
                List<StreetSegment> southSide = null;
                List<StreetSegment> eastSide = null;
                List<StreetSegment> westSide = null;
                List<StreetSegment> node = _streets;
                SplitNorthSouth(node, _bounds.Height / 2 + _bounds.Top, northSide, southSide);
                SplitEastWest(northSide, _bounds.Width / +_bounds.Left, nw, ne);
            }
        }
    }
}
