using System;
using UnityEngine;
using System.Collections;

public abstract class ElectroMeter : AbstractElement {
    public class Arrow
    {
        public int angle;
        public Vector2 position;
        public double length;
    }
    public class Scale
    {
        private const int DefaultDivisions = 100;
        private const int BigSmallDivisionsDifference = 10;
        public readonly double minimum;
        public readonly double maximum;
        public readonly double oneUnit;
        public readonly Arrow arrow;

        public Scale(double newMinimum, double newMaximum, Arrow arrow)
        {
            minimum = newMinimum;
            maximum = newMaximum;
            this.arrow = arrow;
            oneUnit = Math.Abs(minimum - maximum) / DefaultDivisions;
        }

        public double GetPercentage(double number)
        {
            var wholeScale = Math.Abs(minimum - maximum);
            return number/wholeScale;
        }
    }
}
