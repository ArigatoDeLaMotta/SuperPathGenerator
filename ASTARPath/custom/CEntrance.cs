using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CEntrance
{
    public Pos From;
    public Pos To;

    public CEntrance(Pos from, Pos to)
    {
        From = from;
        To = to;
    }
    public Pos getNextPos()
    {
        Pos pos = new Pos();
        Random r = new Random(Guid.NewGuid().GetHashCode());
        int x = r.Next(From.x, To.x);
        pos.x = x;
        pos.y = From.y;
        return pos;
    }

}

