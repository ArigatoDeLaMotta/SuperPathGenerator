using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

public class CLine
{
    public Pos From;
    public Pos To;
    public Pos Normal;
}

public class CSector
{
    public Pos From = new Pos { x = 0, y = 0 };
    public Pos To = new Pos { x = 0, y = 0 };
    public string Name = "";
    public double Peso = 0;

}

public class CPoly
{
    public List<CLine> Lines = new List<CLine>();
}



public class CProdCategory
{
    public string Name;
}

public class CProduct
{
    public Pos pos = new Pos { x = 0, y = 0 };
    public string Name;
    public double Peso;
    public Orientation Or;

}
public enum Orientation { North =1,East =2,South =3,West=4 }
public static class GraphicsExtensions
{
    public static void DrawCircle(this Graphics g, Pen pen,
                                  float centerX, float centerY, float radius)
    {
        g.DrawEllipse(pen, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
    }

    public static void FillCircle(this Graphics g, Brush brush,
                                  float centerX, float centerY, float radius)
    {
        g.FillEllipse(brush, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
    }
}
public class CShelf
{
    public Pos P1;
    public Pos P2;

    private List<CLine> Lines;
    private List<CProduct> NList = new List<CProduct>();
    private List<CProduct> SList = new List<CProduct>();
    private List<CProduct> EList = new List<CProduct>();
    private List<CProduct> WList = new List<CProduct>();
    public List<CProduct> FullList = new List<CProduct>();

    private List<CSector> NSList = new List<CSector>();
    private List<CSector> SSList = new List<CSector>();
    private List<CSector> ESList = new List<CSector>();
    private List<CSector> WSList = new List<CSector>();
    public List<CSector> FullListSector = new List<CSector>();

    public CProduct GetNextProduct()
    {
        Random r = new Random(Guid.NewGuid().GetHashCode());
        int pid = r.Next(0, FullList.Count);
        return FullList[pid];
    }



    public void Draw(PaintEventArgs e,int PixelSize)
    {
        Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
        Pen pen2 = new Pen(Color.FromArgb(0, 255, 0, 0));
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        SolidBrush Brush = new SolidBrush(Color.Red);
        SolidBrush Brush2 = new SolidBrush(Color.Green);


        //Products
        foreach (CProduct p in FullList)
        {
            Rectangle rect = new Rectangle(p.pos.x * PixelSize, p.pos.y * PixelSize, 1 * PixelSize, 1 * PixelSize);
            //GraphicsExtensions.FillCircle(e.Graphics, Brush, p.pos.x * PixelSize, p.pos.y * PixelSize, 5);
            //e.Graphics.FillRectangle(Brush, rect);
        }

        //Sectors
        foreach (CSector sector in FullListSector)
        {
            //e.Graphics.DrawLine(pen, sector.From.getPoint(PixelSize), sector.To.getPoint(PixelSize));
            GraphicsExtensions.FillCircle(e.Graphics, Brush2, sector.From.x * PixelSize, sector.From.y * PixelSize, 3);
            GraphicsExtensions.FillCircle(e.Graphics, Brush2, sector.To.x * PixelSize, sector.To.y * PixelSize, 3);
        }


        /*
        foreach (CProduct p in NList)
        {
            Rectangle rect = new Rectangle(p.pos.x* PixelSize, p.pos.y* PixelSize, 1 * PixelSize, 1 * PixelSize);
            e.Graphics.FillRectangle(Brush, rect);
        }
        foreach (CProduct p in EList)
        {
            Rectangle rect = new Rectangle(p.pos.x* PixelSize, p.pos.y* PixelSize, 1 * PixelSize, 1 * PixelSize);
            e.Graphics.FillRectangle(Brush, rect);
        }
        foreach (CProduct p in SList)
        {
            Rectangle rect = new Rectangle(p.pos.x * PixelSize, p.pos.y * PixelSize, 1  * PixelSize, 1 * PixelSize);
            e.Graphics.FillRectangle(Brush, rect);
        }
        foreach (CProduct p in WList)
        {
            Rectangle rect = new Rectangle(p.pos.x * PixelSize, p.pos.y * PixelSize, 1 * PixelSize, 1 * PixelSize);
            e.Graphics.FillRectangle(Brush, rect);
        }
        */



    }

    public void AddProduct(CProduct p)
    {
        if (p.Or == Orientation.North)
        {
            NList.Add(p);
        }
        if (p.Or == Orientation.East)
        {
            EList.Add(p);
        }
        if (p.Or== Orientation.South)
        {
            SList.Add(p);
        }
        if (p.Or== Orientation.West)
        {
            WList.Add(p);
        }
        InitProduct();
        FullList.Add(p);
    }
    public void AddSector(CSector s, Orientation o)
    {
        if (o == Orientation.North)
        {
            NSList.Add(s);
        }
        if (o == Orientation.East)
        {
            ESList.Add(s);
        }
        if (o == Orientation.South)
        {
            SSList.Add(s);
        }
        if (o == Orientation.West)
        {
            WSList.Add(s);
        }
        
        FullListSector.Add(s);
        InitSector();

    }
    public CShelf(Pos p1, Pos p2)
    {
        P1 = p1;
        P2 = p2;

        Lines = new List<CLine>();
        Init();

    }
    private void Init()
    {
        Pos p1;
        Pos p2;
        Pos Normal = new Pos { x = 0, y = 0 };



        p1 = new Pos { x = P1.x, y = P1.y };
        p2 = new Pos { x = P2.x, y = P2.y };
        p2.y = p1.y;
        Normal =  new Pos { x = 0, y = 0 };  Normal.x = 0; Normal.y = 1;

        CLine l1 = new CLine { From = p1, To = p2, Normal = Normal };
        Lines.Add(l1);


        p1 = new Pos { x = p2.x, y = p2.y };
        p2 = new Pos { x = P2.x, y = P2.y };
        Normal = new Pos { x = 0, y = 0 }; Normal.x = 1; Normal.y = 0;

        CLine l2 = new CLine { From = p1, To = p2, Normal = Normal };
        Lines.Add(l2);


        p1 = new Pos { x = p2.x, y = p2.y };
        p2 = new Pos { x = P2.x, y = P2.y };
        p2.x = P1.x;
        Normal = new Pos { x = 0, y = 0 }; Normal.x = 0; Normal.y = -1;

        CLine l3 = new CLine { From = p1, To = p2, Normal = Normal };
        Lines.Add(l3);

        p1 = new Pos { x = p2.x, y = p2.y };
        p2 = new Pos { x = P1.x, y = P1.y };
        Normal = new Pos { x = 0, y = 0 }; Normal.x = -1; Normal.y = 0;

        CLine l4 = new CLine { From = p1, To = p2, Normal = Normal };
        Lines.Add(l4);



    }
    private void InitProduct()
    {
        int np = NList.Count();
        np += 1;

        if (np > 1)
        {
            double width = (double)P2.x - (double)P1.x;
            double seg = width / np;
            double s = seg;
            foreach (CProduct p in NList)
            {

                p.pos.x = P1.x + (int)s;
                p.pos.y = P1.y;
                s += seg;
            }
        }

        np = SList.Count();
        np += 1;

        if (np > 1)
        {
            double width = (double)P2.x - (double)P1.x;
            double seg = width / np;
            double s = seg;
            foreach (CProduct p in SList)
            {
                p.pos.x = P1.x + (int)s;
                p.pos.y = P2.y;
                s += seg;
            }
        }


        np = EList.Count();
        np += 1;

        if (np > 1)
        {
            double width = (double)P2.y - (double)P1.y;
            double seg = width / np;
            double s = seg;
            foreach (CProduct p in EList)
            {
                p.pos.x = P2.x;
                p.pos.y = P1.y + (int)s;
                s += seg;
            }
        }

        np = WList.Count();
        np += 1;

        if (np > 1)
        {
            double width = (double)P2.y - (double)P1.y;
            double seg = width / np;
            double s = seg;
            foreach (CProduct p in WList)
            {
                p.pos.x = P1.x;
                p.pos.y = P1.y + (int)s;
                s += seg;
            }
        }


    }
    private void InitSector()
    {
        int np = NSList.Count();
        if (np > 0)
        {
            double width = (double)P2.x - (double)P1.x;
            double seg = width / np;
            int s = 0;
            foreach (CSector sector in NSList)
            {
                sector.From.x = P1.x + (int)(s * seg);
                sector.From.y = P1.y;

                sector.To.x = P1.x + (int)((s + 1) * seg);
                sector.To.y = P1.y;
                s++;
            }
        }

        np = SSList.Count();
        if (np > 0)
        {
            double width = (double)P2.x - (double)P1.x;
            double seg = width / np;
            int s = 0;
            foreach (CSector sector in SSList)
            {
                sector.From.x = P1.x + (int)(s * seg);
                sector.From.y = P2.y;

                sector.To.x = P1.x + (int)((s + 1) * seg);
                sector.To.y = P2.y;
                s++;
            }
        }

        np = ESList.Count();
        if (np > 0)
        {
            double width = (double)P2.y - (double)P1.y;
            double seg = width / np;
            int s = 0;
            foreach (CSector sector in ESList)
            {
                sector.From.x = P2.x;
                sector.From.y = P1.y + (int)(s * seg);

                sector.To.x = P2.x;
                sector.To.y = P1.y+ (int)((s+1) * seg);
                s++;
            }
        }


        np = WSList.Count();
        if (np > 0)
        {
            double width = (double)P2.y - (double)P1.y;
            double seg = width / np;
            int s = 0;
            foreach (CSector sector in WSList)
            {
                sector.From.x = P1.x;
                sector.From.y = P1.y + (int)(s * seg);

                sector.To.x = P1.x;
                sector.To.y = P1.y + (int)((s + 1) * seg);
                s++;
            }
        }


    }
}

public class CMatrix
{
    //List<CPoly> Polys = new List<CPoly>();
 
    int MatrixSizeX = 0;
    int MatrixSizeY = 0;
    int PixelSize = 0;

 

    public CMatrix(int sizeX,int sizeY, int pixelSize)
    {
         MatrixSizeX = sizeX;
         MatrixSizeY = sizeY;
         PixelSize = pixelSize;
       // Matrix = new  CNode[MatrixSizeX, MatrixSizeY];
     
       
    }
   
   

   
  /*
    public CNode get(int id)
    {
        for (int x = 0; x < MatrixSizeX; x++)
        {
            for (int y = 0; y < MatrixSizeY; y++)
            {
                if (Matrix[x, y].id == id)
                {
                    return Matrix[x, y];
                }

            }
        }
        return null;

    }
   */

    public void DrawString(Graphics formGraphics,string text, int x, int y)
    {
        
        
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 6);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        formGraphics.DrawString(text, drawFont, drawBrush, x, y, drawFormat);
        drawFont.Dispose();
        drawBrush.Dispose();

    }
    
    public void Draw(PaintEventArgs e)
    {
        /*
        Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();




        SolidBrush Brush = new SolidBrush(Color.Black);

        Rectangle rect = new Rectangle(0, 0, MatrixSizeX * PixelSize, MatrixSizeY * PixelSize);
        e.Graphics.DrawRectangle(pen, rect);

       
        #region draw F
     
        for (int x = 0; x < MatrixSizeX; x++)
        {
            for (int y = 0; y < MatrixSizeY; y++)
            {
                rect = new Rectangle(x * PixelSize,
                             y * PixelSize,
                             PixelSize,
                              PixelSize);
                
                e.Graphics.FillRectangle(new SolidBrush(Matrix[x, y].Color), rect);
                
                if (Matrix[x, y].f > 0| true)
                {
                   // DrawString(e.Graphics, Matrix[x, y].F.ToString(), x * PixelSize,                             y * PixelSize);
                }
            }
        }
        
     
        #endregion
        foreach (CShelf s in LShelf)
        {

            rect = new Rectangle(s.P1.x * PixelSize,
                s.P1.y * PixelSize,
                (s.P2.x - s.P1.x) * PixelSize,
                (s.P2.y - s.P1.y) * PixelSize);

            e.Graphics.DrawRectangle(pen, rect);
            //s.Draw(e, PixelSize);


        }



        try

        {
            SolidBrush point = new SolidBrush(Color.Red);
            Image newImage = Image.FromFile("C:\\Users\\mfcortes.ENTEL\\Desktop\\ariel\\apath\\ASTARPath\\ASTARPath\\cart.png");

            int px = CartPath.End.x * PixelSize;
            int py = CartPath.End.y * PixelSize;
            rect = new Rectangle(px, py, PixelSize, PixelSize);
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), rect);

            Pos p = CartPath.CurrentPos;

            px = p.x * PixelSize;
            py = p.y * PixelSize;
            rect = new Rectangle(px, py, PixelSize, PixelSize);
            //e.Graphics.FillRectangle(point, rect);
            e.Graphics.DrawImage(newImage, rect);

            for (int pp = CartPath.Path.Count() - 1; pp > CartPath.CurrentStep; pp--)
            {
                px = CartPath.Path[pp].x * PixelSize;
                py = CartPath.Path[pp].y * PixelSize;

                rect = new Rectangle(px, py, PixelSize, PixelSize);
                //e.Graphics.DrawEllipse(pen, rect);
                e.Graphics.FillEllipse(new SolidBrush(Color.Violet), rect);
            }

        

            CartPath.CurrentStep--;

            if (CartPath.CurrentStep == 0)
            {
            
                CartPath = getPath(CartPath.CurrentPos, GetNextPosFromSectors());
            }

        }
        catch (Exception exx)
        {
            Console.WriteLine(exx.ToString());
        }
        */
    }
   
  
}

