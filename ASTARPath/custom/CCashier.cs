using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class CCashier
    {
        public Pos Pos;
        public int With;
        public int Height;

        public CCashier(Pos pos,int with, int height)
        {
            Pos = pos;
            With = with;
            Height = height;
        }
        public Pos getPos()
        {
            Pos pos = new Pos();
            pos.x = Pos.x + (With / 2);
            pos.y = Pos.y;
            return pos;
        }

    }

