namespace MapStudy
{
    public class Range
    {
        public int Start, End;

        public int getLength()
        {
            return End - Start;
        }

        public void shiftLeft()
        {
            if (Start > 0)
            {
                Start--;
                End--;
            }
        }

        public void shiftRight()
        {
            Start++;
            End++;
        }

        public void expandRight()
        {
            End++;
        }
    }
}
