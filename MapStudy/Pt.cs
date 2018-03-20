using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MapStudy
{
    [DebuggerDisplay("{X}, {Y}")]
    public class Pt : IComparable<Pt>
    {
        private int x, y;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Pt)
            {
                Pt oPt = (Pt)obj;
                return oPt.x == x && oPt.y == y;
            }

            return false;
        }

        int IComparable<Pt>.CompareTo(Pt y)
        {
            Pt x = this;
            if (x.X < y.X)
            {
                return -1;
            }
            else if (y.X < x.X)
            {
                return 1;
            }
            else
            {
                if (x.Y < y.Y)
                {
                    return -1;
                }
                else if (y.Y < x.Y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Pt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Pt()
        {
            this.x = 0;
            this.y = 0;
        }
    }
}
