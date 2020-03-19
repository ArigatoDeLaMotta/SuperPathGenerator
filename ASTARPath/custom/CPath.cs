using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class CPath
{
    public double speed = 10; // 10 cm por segundo

    public List<Pos> Path = new List<Pos>();
    public Pos Start = new Pos();
    public Pos End = new Pos();
    public int CurrentStep = 0;
    CNode[,] Matrix;// = new CNode[1000, 1000];
    int MatrixSizeX = 0;
    int MatrixSizeY = 0;
    int PixelSize = 0;
    public bool Terminado = false;

    List<CNode> OpenList = new List<CNode>();
    List<CNode> CloseList = new List<CNode>();
    public List<Pos> Estela = new List<Pos>();
    CNode NodeStart = null;
    CNode NodeEnd = null;
    CNode FinalNode = null;

    public CPath(CNode[,] matrix, int pixelSize)
    {
        PixelSize = pixelSize;
        Random r = new Random(Guid.NewGuid().GetHashCode());
        speed = (int)r.Next(1000, 9000) / 100;

        Matrix = matrix;
        MatrixSizeX = Matrix.GetLength(0);
        MatrixSizeY = Matrix.GetLength(1);


    }
    public CNode getClosetNode()
    {
        CNode node = null;
        List<CNode> lnode = new List<CNode>();

        foreach (CNode cnode in OpenList)
        {
            if (node == null)
            {
                node = cnode;
            }
            else if (node.F > cnode.F)
            {
                node = cnode;
            }
            else if (node.F == cnode.F)
            {
                if (node.h > cnode.h)
                {
                    node = cnode;
                }
            }
        }

        return node;

    }
    public Pos CurrentPos
    {
        get
        {
            return Path[CurrentStep];
        }
    }
    public List<CNode> getNeibors(CNode node)
    {
        List<CNode> neibors = new List<CNode>();

        // TOP LINE
        if (node.x > 0 && node.y > 0)
        {
            neibors.Add(Matrix[node.x - 1, node.y - 1]);
        }
        if (node.y > 0)
        {
            neibors.Add(Matrix[node.x, node.y - 1]);
        }
        if (node.x < MatrixSizeX - 1 && node.y > 0)
        {
            neibors.Add(Matrix[node.x + 1, node.y - 1]);
        }


        // MIDDLE
        if (node.x > 0)
        {
            neibors.Add(Matrix[node.x - 1, node.y]);
        }
        // CENTER
        if (node.x < MatrixSizeX - 1)
        {
            neibors.Add(Matrix[node.x + 1, node.y]);
        }

        // BOTTOM
        if (node.x > 0 && node.y < MatrixSizeY - 1)
        {
            neibors.Add(Matrix[node.x - 1, node.y + 1]);
        }
        if (node.y < MatrixSizeY - 1)
        {
            neibors.Add(Matrix[node.x, node.y + 1]);
        }
        if (node.x < MatrixSizeX - 1 && node.y < MatrixSizeY - 1)
        {
            neibors.Add(Matrix[node.x + 1, node.y + 1]);
        }
        foreach (CNode cnode in neibors)
        {
            //  cnode.Parent = node;
            //  cnode.Color = Color.Green;
        }
        return neibors;

    }
    public void Step(CNode node)
    {
        List<CNode> neibors = getNeibors(node);

        OpenList.Remove(node);
        CloseList.Add(node);
        foreach (CNode cnode in neibors)
        {
            if (!cnode.block && !CloseList.Exists((x => x.id == cnode.id)))
            {
                // g COST FROM THE START
                // H COST FROM THE END
                if (cnode.Parent == null)
                {
                    cnode.Parent = node;
                }
                int g = Math.Abs(cnode.x - node.x) + Math.Abs(cnode.y - node.y);
                g = g == 2 ? 14 : 10;

                //cnode.Color = Color.Green;
                int newg = g + node.g;
                if (cnode.g == 0)
                {
                    cnode.g = g;

                }
                else if (cnode.g > newg)
                {
                    cnode.g = newg;
                    cnode.Parent = node;
                }
                if (!OpenList.Exists((x => x.id == cnode.id)))
                {
                    OpenList.Add(cnode);
                }

                foreach (CNode cn in CloseList)
                {
                    //cn.Color = Color.BlueViolet;
                }
            }
        }

    }
    public void BuildPath(Pos start, Pos end)

    {

        Start = start;
        End = end;
        Terminado = false;
        CurrentStep = 0;

        OpenList.Clear();

        CloseList.Clear();
        Estela.Clear();
        while (Start == End)
        {
            return;
        }
        try
        {
            NodeStart = Matrix[Start.x, Start.y];
            NodeEnd = Matrix[End.x, End.y];

            Random r = new Random(System.Guid.NewGuid().GetHashCode());

            for (int x = 0; x < MatrixSizeX; x++)
            {
                for (int y = 0; y < MatrixSizeY; y++)
                {
                    Matrix[x, y].Parent = null;
                    int h = Math.Abs(NodeEnd.x - x) + Math.Abs(NodeEnd.y - y);
                    Matrix[x, y].h = h * 10;
                    Matrix[x, y].g = 0;
                    Matrix[x, y].f = 0;

                    if (Matrix[x, y].Color.R < 255)
                    {
                        Matrix[x, y].f = 255 - Matrix[x, y].Color.R;
                    }
                    else if (y < 58)
                    {
                        Matrix[x, y].f = 55 - r.Next(0, 55);
                    }
                }
            }

            List<Pos> lpath = new List<Pos>();
            CloseList.Add(NodeStart);
            Step(NodeStart);
            int cont = 0;
            CNode next = null;
            do
            {
                cont++;
                next = getClosetNode();
                Step(next);
            }
            while (next.h > 10);
            FinalNode = next;


            while (NodeEnd != NodeStart)
            {
                lpath.Add(new Pos { x = NodeEnd.x, y = NodeEnd.y });
                NodeEnd = NodeEnd.Parent;
            }
            lpath.Add(new Pos { x = NodeStart.x, y = NodeStart.y });
            Path = lpath;


            CurrentStep = lpath.Count() - 1;
        }
        catch (Exception e)
        {
            Console.Write(e.ToString());
        }
    }
    public Pos GetNextPos()
    {
        Pos p = new Pos();
        bool valid = false;
        Random r = new Random(Guid.NewGuid().GetHashCode());
        int x = 0;
        int y = 0;
        while (!valid)
        {
            x = r.Next(1, MatrixSizeX - 1);
            y = r.Next(1, MatrixSizeY - 1);
            valid = !Matrix[x, y].block;
        }

        p.x = x;
        p.y = y;


        return p;
    }
    public Pos getPos(DateTime Now)
    {
        DateTime CreateDate = DateTime.Now;
        Pos p1 = Path.Last();
        Pos p2 = null;
        TimeSpan ts = (Now - CreateDate);
        double miliseconds = (ts.Milliseconds + ts.Seconds * 1000);
        double distanciaRecorrida = miliseconds * speed; // 50 cms x seg 
        distanciaRecorrida = distanciaRecorrida / 1000;
        double distacia = 0;
        double distaciaAnterior = 0;
        bool sigue = true;
        double vx = 0;
        double vy = 0;
        try
        {

            if (miliseconds > 0)
            {
                for (int i = Path.Count - 2; i >= 0 & sigue == true; i--)
                {

                    if (distacia < distanciaRecorrida)
                    {
                        p2 = Path[i];
                        distaciaAnterior = distacia;
                        distacia += Distancia(p2, p1);
                        CurrentStep = i;


                    }
                    if (distacia >= distanciaRecorrida)
                    {
                        double diferencia = distanciaRecorrida - distaciaAnterior;
                        vx = p2.x - p1.x;
                        vy = p2.y - p1.y;
                        double modulo = Math.Sqrt(Math.Pow(vx, 2) + Math.Pow(vy, 2));
                        vx = vx / modulo;
                        vy = vy / modulo;
                        vx = vx * diferencia;
                        vy = vy * diferencia;
                        sigue = false;
                    }
                    else
                    {
                        p1 = p2;
                    }
                }

                if ((object)p1 != null)
                {
                    if (p1.x == Path[0].x && p1.y == Path[0].y)
                    {
                        Terminado = true;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Pos p = new Pos();
        p.x = p1.x * PixelSize + (int)vx;
        p.y = p1.y * PixelSize + (int)vy;
        Estela.Add(p);
        return p;

    }
    private double Distancia(Pos p1, Pos p2)
    {
        return Math.Sqrt(Math.Pow((p2.x - p1.x), 2) + Math.Pow((p2.y - p1.y), 2)) * PixelSize;
    }

}
public class CShop
{
    List<CPath> ShopingRoute = new List<CPath>();
    List<CProduct> LProducts = new List<CProduct>();
    List<Pos> CProductShoped = new List<Pos>();
    CNode[,] Matrix;
    int PixelSize = 0;
    Color color;
    public List<Pos> Route = new List<Pos>();
    DateTime StartDate;
    DateTime EndDate;
    int Speed = 0; // ms per unite 



    public CShop(CNode[,] matrix, int pixelSize, DateTime start, int speed)
    {

        Random rnd = new Random();
        int r = rnd.Next(0, 150);
        int g = rnd.Next(0, 150);
        int b = rnd.Next(0, 150);
        StartDate = start;
        Speed = speed;
        color = Color.FromArgb(r, g, b);

        Matrix = matrix;
        PixelSize = pixelSize;
    }
    public void AddProduct(CProduct p)
    {
        LProducts.Add(p);
        CProductShoped.Add(GetPosFromProduct(p));
    }
    public void Draw(System.Windows.Forms.PaintEventArgs e, int ms)
    {
        int n = 0;
        for (int i = 0; i < Route.Count - 1; i++)
        {
            if (n > ms)
            {
                Rectangle rect = new Rectangle(Route[i].x * PixelSize, Route[i].y * PixelSize, PixelSize, PixelSize);
                e.Graphics.FillEllipse(new SolidBrush(color), rect);
                i = Route.Count;
            }
            else
            {
                n += Route[i].ms;
            }
        }

    }
    public void Draw(System.Windows.Forms.PaintEventArgs e)
    {
        PointF[] points = new PointF[Route.Count];
        if (true) // punto
        {
            for (int i = 0; i < Route.Count; i++)
            {
                Pos pos = Route[i];
                int px = pos.x;
                int py = pos.y;
                Rectangle rect = new Rectangle(px * PixelSize, py * PixelSize, PixelSize / 2, PixelSize / 2);
                if (i == 0)
                {
                    rect = new Rectangle(px * PixelSize, py * PixelSize, PixelSize, PixelSize);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Gold), rect);
                }
                else if (i == (Route.Count - 1))
                {
                    rect = new Rectangle(px * PixelSize, py * PixelSize, PixelSize, PixelSize);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Green), rect);
                }
                else
                {
                    int a = 255 - ((200 * i / Route.Count));
                    Color c = Color.FromArgb(a, 0, 0, 0);
                    //c = Color.FromArgb( 0, 0, 0);
                    e.Graphics.FillEllipse(new SolidBrush(c), rect);
                }

                foreach (CProduct p in LProducts)
                {
                    rect = new Rectangle(p.pos.x * PixelSize, p.pos.y * PixelSize, PixelSize, PixelSize);
                    e.Graphics.FillEllipse(new SolidBrush(Color.BlueViolet), rect);
                }
            }
        }

        if (false) // curva
        {
            Point[] curvePoints = new Point[Route.Count - 1];
            int i = 0;
            for (i = 0; i < Route.Count - 1; i++)
            {
                curvePoints[i] = Route[i].getPoint(PixelSize);
                // Draw curve to screen.
            }
            e.Graphics.DrawCurve(new Pen(new SolidBrush(color), 5), curvePoints);

            Rectangle rect = new Rectangle(Route[i].x * PixelSize, Route[i].y * PixelSize, PixelSize, PixelSize);

            e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect);

        }
        foreach (CProduct p in LProducts)
        {
            Rectangle rect = new Rectangle(p.pos.x * PixelSize, p.pos.y * PixelSize, PixelSize, PixelSize);
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect);
        }
    }

    public void AddPos(Pos p)
    {
        CProductShoped.Add(p);
    }

    public void BuildPath()
    {
        for (int i = 0; i < CProductShoped.Count - 1; i++)
        {
            Pos p1 = CProductShoped[i];
            Pos p2 = CProductShoped[i + 1];
            CPath p = new CPath(Matrix, PixelSize);
            p.BuildPath(p1, p2);
            ShopingRoute.Add(p);
        }

        foreach (CPath path in ShopingRoute)
        {
            for (int i = path.Path.Count - 1; i > 0; i--)
            {
                path.Path[i].ms = Speed;
                Route.Add(path.Path[i]);
            }
            if (path.Path.Count > 0)
            {
                path.Path[0].ms = Speed * 3;
                Route.Add(path.Path[0]);
            }
        }
        int ii = 0;
    }

    public Pos GetPosFromProduct(CProduct pro)
    {
        Pos pos = new Pos();

        pos.x = pro.pos.x;
        pos.y = pro.pos.y;

        if (pro.Or == Orientation.North)
        {
            pos.y--;
        }
        if (pro.Or == Orientation.South)
        {
            pos.y++;
        }
        if (pro.Or == Orientation.West)
        {
            pos.x--;
        }
        if (pro.Or == Orientation.East)
        {
            pos.x++;
        }

        return pos;
    }

}
