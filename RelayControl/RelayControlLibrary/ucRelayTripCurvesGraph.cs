using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using RelayControlLibrary;

namespace PhasorDisplayGraph
{
    public partial class ucRelayTripCurvesGraph : UserControl
    {
        public ucRelayTripCurvesGraph()
        {
            InitializeComponent();
        }

        public ArrayList DrawObjects = new ArrayList();
        private float[] intersectionAngles = new float[2];

        /// <summary>
        /// Returns the intersection angles between a line and a circle that is cocentric with the origin
        /// </summary>
        /// <param name="angle">Accute angle of the line from measure from the Y-axis</param>
        /// <param name="offset">Offset of the line</param>
        /// <param name="radius">Radius o7f the circle</param>
        /// <returns>2 float angles</returns>
        private float[] intersectionAnglesOf(float angle, float offset, float radius)
        {
            float[] returnFloats = new float[2];
            double angleOffset = Math.Abs((double)angle);                     //angle opposite of specified side
            double angleY;
            double angleResult;
            double radiansOffset = this.radiansFrom(angleOffset);   //radian equivalent of specified side
            double radiansY;

            double sideY;                                           //y interest of line
            double sideRadius = (double)radius;                              

            if (angleOffset != 0)
            {
                if (offset != 0)
                {
                    //determine Y intersect to get side Y
                    sideY = (double)offset * Math.Sin(this.radiansFrom(90d - angleOffset)) / Math.Sin(radiansOffset);

                    //determine radians/angle opposite side Y when the radius is used
                    radiansY = Math.Asin(sideY * Math.Sin(radiansOffset) / sideRadius);
                    angleY = this.angleFrom(radiansY);
                    if (angleY >= 180)
                        angleY = angleY - 180;
                    else if (angleY < 0)
                        angleY += 180;

                    //determine the resulting angle
                    angleResult = 180d - angleOffset - angleY;
                    returnFloats[0] = (float)angleResult;

                    //determine the other angleY, resulting angle;
                    angleY = 180d - angleY;
                    angleResult = 180d - angleOffset - angleY;
                    returnFloats[1] = (float)angleResult;

                    if (angle > 0)
                    {
                        if (offset < 0)
                        {
                            returnFloats[0] += 90f;
                            returnFloats[1] += 90f;
                        }
                        else
                        {
                            returnFloats[0] += 0f;
                            returnFloats[1] += 0f;
                        }
                    }
                    else
                    {
                        if (offset < 0)
                        {
                            returnFloats[0] += 180f;
                            returnFloats[1] += 180f;
                        }
                        else
                        {
                            returnFloats[0] += 270f;
                            returnFloats[1] += 270f;
                        }
                    }
                    
                }
                else
                {
                    returnFloats[0] = 90 - angle;
                    returnFloats[1] = 180 - angle;
                }
            }
            else
            {
                sideY = Math.Sqrt((double)(radius * radius - offset * offset));
                returnFloats[0] = this.angleFrom(Math.Asin(sideY / (double)radius));
                returnFloats[1] = Offset180From(returnFloats[0]);
            }
            
            return returnFloats;
        }

        private float Offset180From(float angle)
        {
            angle = angle - 180;
            if (angle < 0)
                angle += 360;
            return angle;
        }

        private float angleFrom(double radians)
        {
            float returnAngle;

            returnAngle = (float)(radians * 180d / Math.PI);

            return returnAngle;
        }

        private double radiansFrom(double angle)
        {
            double returnRadians;

            returnRadians = (double)angle * Math.PI / 180d;

            return returnRadians;
        }

        private PointF endPoint(float angle, float radius)
        {
            PointF returnPoint = new PointF();

            double dAngle = (double)angle;
            double radiansRadius = this.radiansFrom(90d);

            while (dAngle > 90)
            {
                dAngle -= 90;
            }

            double dRadians = this.radiansFrom(dAngle);

            returnPoint.X = radius * (float)Math.Sin(dRadians);
            returnPoint.Y = radius * (float)Math.Cos(dRadians);
            if (angle > 270)
            {
                returnPoint.Y = -returnPoint.Y;
            }
            else if (angle > 180)
            {
                returnPoint.Y = -returnPoint.Y;
                returnPoint.X = -returnPoint.X;
            }
            else if (angle > 90)
            {
                returnPoint.X = -returnPoint.X;
            }

            return returnPoint;
        }

        private ArrayList DrawSegments(ref ArrayList curveDefs)
        {
            return curveDefs;
        }
    }
}
