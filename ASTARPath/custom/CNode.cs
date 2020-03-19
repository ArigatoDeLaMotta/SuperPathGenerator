using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CNode
{
    
    public int id;
    public int g;
    public int h;
    public int F
    {
        get
        {

            return g + h + f;
        }
    }
    public int f;
    public CNode Parent;
    public int x;
    public int y;
    public bool block = false;

    public Color Color = Color.FromArgb(255, 255, 255, 255);
    public List<CNode> Neibor = new List<CNode>();


}

