﻿// ReSharper disable MemberCanBePrivate.Global

using System;
using RGB.NET.Core;

namespace RGB.NET.Brushes.Helper
{
    /// <summary>
    /// Offers some extensions and helper-methods for gradient related things.
    /// </summary>
    public static class GradientHelper
    {
        #region Methods

        // Based on https://web.archive.org/web/20170125201230/https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Calculates the offset of an given <see cref="Point"/> on an gradient.
        /// </summary>
        /// <param name="startPoint">The start <see cref="Point"/> of the gradient.</param>
        /// <param name="endPoint">The end <see cref="Point"/> of the gradient.</param>
        /// <param name="point">The <see cref="Point"/> on the gradient to which the offset is calculated.</param>
        /// <returns>The offset of the <see cref="Point"/> on the gradient.</returns>
        public static double CalculateLinearGradientOffset(Point startPoint, Point endPoint, Point point)
        {
            Point intersectingPoint;
            if (startPoint.Y.Equals(endPoint.Y)) // Horizontal case
                intersectingPoint = new Point(point.X, startPoint.Y);

            else if (startPoint.X.Equals(endPoint.X)) // Vertical case
                intersectingPoint = new Point(startPoint.X, point.Y);

            else // Diagonal case
            {
                double slope = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                double orthogonalSlope = -1 / slope;

                double startYIntercept = startPoint.Y - (slope * startPoint.X);
                double pointYIntercept = point.Y - (orthogonalSlope * point.X);

                double intersectingPointX = (pointYIntercept - startYIntercept) / (slope - orthogonalSlope);
                double intersectingPointY = (slope * intersectingPointX) + startYIntercept;
                intersectingPoint = new Point(intersectingPointX, intersectingPointY);
            }

            // Calculate distances relative to the vector start
            double intersectDistance = CalculateDistance(intersectingPoint, startPoint, endPoint);
            double gradientLength = CalculateDistance(endPoint, startPoint, endPoint);

            return intersectDistance / gradientLength;
        }

        // Based on https://web.archive.org/web/20170125201230/https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Returns the signed magnitude of a <see cref="Point"/> on a vector.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> on the vector of which the magnitude should be calculated.</param>
        /// <param name="origin">The origin of the vector.</param>
        /// <param name="direction">The direction of the vector.</param>
        /// <returns>The signed magnitude of a <see cref="Point"/> on a vector.</returns>
        public static double CalculateDistance(Point point, Point origin, Point direction)
        {
            double distance = CalculateDistance(point, origin);

            return (((point.Y < origin.Y) && (direction.Y > origin.Y)) ||
                ((point.Y > origin.Y) && (direction.Y < origin.Y)) ||
                ((point.Y.Equals(origin.Y)) && (point.X < origin.X) && (direction.X > origin.X)) ||
                ((point.Y.Equals(origin.Y)) && (point.X > origin.X) && (direction.X < origin.X)))
                ? -distance : distance;
        }

        /// <summary>
        /// Calculated the distance between two <see cref="Point"/>.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        /// <returns>The distance between the two <see cref="Point"/>.</returns>
        public static double CalculateDistance(Point point1, Point point2) => Math.Sqrt(((point1.Y - point2.Y) * (point1.Y - point2.Y)) + ((point1.X - point2.X) * (point1.X - point2.X)));

        #endregion
    }
}
