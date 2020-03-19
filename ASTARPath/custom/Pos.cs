using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Pos
{
    public int x = 0;
    public int y = 0;
    public int ms = 0; //milliseconds
 
    public Point getPoint(int Size)
    {
        return new Point(x * Size, y * Size);
    }
    public static bool operator ==(Pos c1, Pos c2)
    {
        if (c1.x == c2.x && c1.y == c2.y)
            return true;
        else
            return false;
    }
    public static bool operator !=(Pos c1, Pos c2)
    {
        
        if (c1.x == c2.x && c1.y == c2.y)
            return false;
        else
            return true;

    }
}
