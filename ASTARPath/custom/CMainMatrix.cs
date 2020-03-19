using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;






public class CMainMatrix
{
    int contador = 0;
    DateTime LastRender;
    DateTime StartRender;
    List<CPath> Paths = new List<CPath>();
    public const int months = 12;

    public const int MatrixSizeX = 38;
    public const int MatrixSizeY = 70;
    public const int PixelSize = 10;
    List<CShelf> LShelf = new List<CShelf>();
    CNode[,] Matrix = new CNode[MatrixSizeX, MatrixSizeY];
    List<CShop> lShop = new List<CShop>();
    
    CEntrance Entrance;
    List<CCashier> LCachiser = new List<CCashier>();

    public CMainMatrix()
    {
        CMatrix m = new CMatrix(MatrixSizeX, MatrixSizeY, PixelSize);

        Entrance = new CEntrance(new Pos { x = 30, y = 65 }, new Pos { x = 37, y = 65 });

        Init();



        for ( int i=1;i<50;i++)
        { 
        //CPath path = new CPath(Matrix, PixelSize);
        //Paths.Add(path);
        //path.BuildPath(path.GetNextPos(), path.GetNextPos());
        }





    }
    public void Init()
    {
        StartRender = System.DateTime.Now;
        int count = 1;
        for (int x = 0; x < MatrixSizeX; x++)
        {
            for (int y = 0; y < MatrixSizeY; y++)
            {
                Matrix[x, y] = new CNode { id = count, g = 0, h = 0, f = 0, Parent = null, x = x, y = y };
                Matrix[x, y].Color = Color.White;
                count++;
            }
        }

        LShelf.Add(new CShelf(new Pos { x = 4, y = 4 }, new Pos { x = 8, y = 28 }));
        LShelf.Add(new CShelf(new Pos { x = 12, y = 4 }, new Pos { x = 16, y = 28 }));
        LShelf.Add(new CShelf(new Pos { x = 20, y = 4 }, new Pos { x = 24, y = 28 }));

        LShelf.Add(new CShelf(new Pos { x = 4, y = 32 }, new Pos { x = 8, y = 56 }));
        LShelf.Add(new CShelf(new Pos { x = 12, y = 32 }, new Pos { x = 16, y = 56 }));
        LShelf.Add(new CShelf(new Pos { x = 20, y = 32 }, new Pos { x = 24, y = 56 }));

        LCachiser.Add(new CCashier(new Pos { x = 4, y = 60 }, 4, 4));
        LCachiser.Add(new CCashier(new Pos { x = 12, y = 60 }, 4, 4));
        LCachiser.Add(new CCashier(new Pos { x = 20, y = 60 }, 4, 4));


        #region sector
        /*
        LShelf[0].AddProduct(new CProduct { Name = "p1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p2" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);
        LShelf[0].AddProduct(new CProduct { Name = "p3.1" }, Orientation.North);

        LShelf[0].AddProduct(new CProduct { Name = "p4" }, Orientation.East);
        LShelf[0].AddProduct(new CProduct { Name = "p5" }, Orientation.South);
        LShelf[0].AddProduct(new CProduct { Name = "p6" }, Orientation.West);

        LShelf[0].AddSector(new CSector { Name = "SEvtgor" }, Orientation.North);
        LShelf[0].AddSector(new CSector { Name = "SEvtgor" }, Orientation.North);
        */
        #endregion

        // create products
        Random r = new Random();
        for (int i = 0; i < 100; i++)
        {

            int rs = r.Next(0, LShelf.Count());
            double o = r.NextDouble() * 4;

            Orientation orie = Orientation.East;

            if (o < 0.2) { orie = Orientation.North; }

            if (o > 3.8) { orie = Orientation.South; }

            if (o > 0.2 && o < 2) { orie = Orientation.East; }

            if (o > 2 && o < 3.8) { orie = Orientation.West; }


            LShelf[rs].AddProduct(new CProduct { Name = "P[" + i.ToString() + "]", Or = orie });

            //LShelf[rs].AddSector(new CSector { Name = "SEvtgor" }, orie);
        }

        // block all pixels inside the shelf
        foreach (CShelf s in LShelf)
        {
            for (int x = s.P1.x; x < s.P2.x; x++)
            {
                for (int y = s.P1.y; y < s.P2.y; y++)
                {
                    Matrix[x, y].block = true;
                }
            }
        }
        DumpImage();
        
        // set color main matrix not used
        for (int i = 0; i < MatrixSizeX; i++)
        {
            for (int j = 0; j < MatrixSizeY; j++)
            {
                int c = r.Next(150, 250);
               // Matrix[i, j].Color = Color.FromArgb(c, c, c);
            }
        }

        for (int i = 0; i < 150; i++)
        {
            CShop shop = new CShop(Matrix, PixelSize,DateTime.Now,100);

            shop.AddPos(Entrance.getNextPos());
            for (int numprod = 0; numprod < r.Next(1,5); numprod++)
            {
                shop.AddProduct(GetNextProduct());
            }


            int idc = r.Next(0, LCachiser.Count );
            shop.AddPos(LCachiser[idc].getPos());

            idc = r.Next(0, LCachiser.Count - 1);
            shop.BuildPath();
            lShop.Add(shop);
        }

        //shop.AddPos(LCachiser[idc].getPos());


        //LoadImage();

        //CartPath = getPath(GetNextPos(), GetNextPos());

    }
    public CProduct GetNextProduct()
    {
       

        CProduct pro = new CProduct();

        
        Random r = new Random(Guid.NewGuid().GetHashCode());

        
        int sid = 0; // shelf id
        int pid = 0; // sector id
        
         sid = r.Next(0, LShelf.Count);
         

        pid = r.Next(0, LShelf[sid].FullList.Count);

        pro = LShelf[sid].FullList[pid];

        return pro;
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
    
    public void CreateBlock(int x0, int y0, int x1, int y1)
    {
        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y < y1; y++)
            {
                Matrix[x, y].block = true;
                Matrix[x, y].Color = Color.Black;
            }
        }

    }

    public void DumpImage()
    {
        Bitmap bmp = new Bitmap(MatrixSizeX * PixelSize, MatrixSizeY * PixelSize);
        Graphics graph = Graphics.FromImage(bmp);

        foreach (CShelf s in LShelf)
        {

            Rectangle rect = new Rectangle(s.P1.x * PixelSize,
                s.P1.y * PixelSize,
                (s.P2.x - s.P1.x) * PixelSize,
                (s.P2.y - s.P1.y) * PixelSize);
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));

            graph.FillRectangle(new SolidBrush(Color.Black), rect);

        }
        bmp.Save(@"C:\Users\mfcortes.ENTEL\Desktop\ariel\img.bmp");



    }

    public void LoadImage()
    {
        Bitmap MyImage = (Bitmap)Image.FromFile(@"C:\Users\mfcortes.ENTEL\Desktop\ariel\imgIn.bmp", false);

        for (int i = 0; i < MatrixSizeX; i++)
        {
            for (int j = 0; j < MatrixSizeY; j++)
            {
                int r = 0;
                int g = 0;
                int b = 0;

                int total = 0;
                for (int x = 0; x < PixelSize; x++)
                {
                    for (int y = 0; y < PixelSize; y++)
                    {
                        Color clr = MyImage.GetPixel(i * PixelSize + x, j * PixelSize + y);
                        r += clr.R;
                        g += clr.G;
                        b += clr.B;

                        total++;
                    }

                }
                r /= total;
                g /= total;
                b /= total;

                Color avg = Color.FromArgb(r, g, b);
                Matrix[i, j].Color = avg;
                //Matrix[i, j].f = (int)(255 - r / 255);

            }
        }
    }
    public void Run()
    {
        foreach (CPath CartPath in Paths)
        {
            if (CartPath.Terminado)
            {
                CartPath.BuildPath(CartPath.End, CartPath.GetNextPos());
            }
        }
    
    }
    public Pos GetNextPosFromSectors()
    {
        Pos p = new Pos();
        bool blocked = true;
        Random r = new Random(Guid.NewGuid().GetHashCode());

        int scounts = 0; // sector counts
        int scid = 0; // shelf id
        int sid = 0; // sector id
        while (blocked)
        {
            sid = r.Next(0, LShelf.Count);
            scounts = LShelf[sid].FullListSector.Count;
            scid = r.Next(0, LShelf[sid].FullListSector.Count);
            p = LShelf[sid].FullListSector[scid].From;
            blocked = Matrix[p.x, p.y].block;
        }




        return p;
    }
    public void Draw(PaintEventArgs e)
    {
        
        
        Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();


        contador++;
        DrawString(e.Graphics, contador.ToString(), 1, 1);


        SolidBrush Brush = new SolidBrush(Color.Black);


        // DRAW BORDER
        Rectangle rect = new Rectangle(0, 0, MatrixSizeX * PixelSize, MatrixSizeY * PixelSize);
        e.Graphics.DrawRectangle(pen, rect);


        #region draw Matrix

        for (int x = 0; x < MatrixSizeX; x++)
        {
            for (int y = 0; y < MatrixSizeY; y++)
            {
                rect = new Rectangle(x * PixelSize,
                             y * PixelSize,
                             PixelSize,
                              PixelSize);

                e.Graphics.FillRectangle(new SolidBrush(Matrix[x, y].Color), rect);
                /*
                if (Matrix[x, y].f > 0| true)
                {
                    DrawString(e.Graphics, Matrix[x, y].F.ToString(), x * PixelSize,                             y * PixelSize);
                }*/
            }
        }

        #endregion

        // DRAW SHOP PATH

        // draw curve path
        
        TimeSpan ts = (System.DateTime.Now - StartRender);
        foreach (CShop s in lShop)
        {
            //s.Draw(e);
            s.Draw(e, (int)ts.TotalMilliseconds);
            
        }
        
   



        #region Draw Shelf
        foreach (CShelf s in LShelf)
        {
            rect = new Rectangle(s.P1.x * PixelSize,
                s.P1.y * PixelSize,
                (s.P2.x - s.P1.x) * PixelSize,
                (s.P2.y - s.P1.y) * PixelSize);

            e.Graphics.DrawRectangle(pen, rect);
            s.Draw(e, PixelSize);
        }
        #endregion

        foreach (CCashier c  in LCachiser)
        {
            rect = new Rectangle(c.Pos.x * PixelSize,
                c.Pos.y * PixelSize,
                (c.With) * PixelSize,
                (c.Height) * PixelSize);

            e.Graphics.DrawRectangle(pen, rect);
            //s.Draw(e, PixelSize);
        }



        // DRAW ENTRANCE
        e.Graphics.DrawLine(pen, Entrance.From.getPoint(PixelSize), Entrance.To.getPoint(PixelSize));

        try
        {
            SolidBrush point = new SolidBrush(Color.Red);
            Image newImage = Image.FromFile("C:\\Users\\mfcortes.ENTEL\\Desktop\\ariel\\apath\\ASTARPath\\ASTARPath\\cart.png");

            int px = 0;
            int py = 0;
            foreach (CPath CartPath in Paths)
            {

                // GRAFICA EL END
                /*
                 px = CartPath.End.x * PixelSize;
                 py = CartPath.End.y * PixelSize;
                rect = new Rectangle(px, py, PixelSize, PixelSize);
                e.Graphics.FillRectangle(new SolidBrush(Color.Blue), rect);
                */

                //e.Graphics.FillRectangle(point, rect);

                /*
                for (int pp = CartPath.Path.Count() - 1; pp > CartPath.CurrentStep; pp--)
                {
                    px = CartPath.Path[pp].x * PixelSize;
                    py = CartPath.Path[pp].y * PixelSize;

                    rect = new Rectangle(px, py, PixelSize, PixelSize);
                    //e.Graphics.DrawEllipse(pen, rect);
                    //e.Graphics.FillEllipse(new SolidBrush(Color.Violet), rect);
                }
                */
                double  numelementos = CartPath.Estela.Count();
                for (int pp = CartPath.Estela.Count() - 1; pp >= 1; pp--)
                {
                    px = CartPath.Estela[pp].x;
                    py = CartPath.Estela[pp].y;
                    
                    rect = new Rectangle(px, py, PixelSize, PixelSize);
                    int alpha = (int)(255 /  (numelementos - pp));
                    Color c = Color.FromArgb(alpha, Color.Violet);
                    //e.Graphics.DrawEllipse(pen, rect);
                    e.Graphics.FillEllipse(new SolidBrush(c), rect);
                }

                Pos p = CartPath.getPos(System.DateTime.Now);
                px = p.x;
                py = p.y;
                rect = new Rectangle(px, py, PixelSize, PixelSize);
                e.Graphics.FillEllipse(new SolidBrush(Color.Goldenrod), rect);
                //e.Graphics.DrawImage(newImage, rect);

            }
        }
        catch (Exception exx)
        {
            Console.WriteLine(exx.ToString());
        }

    }
    public void DrawString(Graphics formGraphics, string text, int x, int y)
    {


        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 6);
        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        formGraphics.DrawString(text, drawFont, drawBrush, x, y, drawFormat);
        drawFont.Dispose();
        drawBrush.Dispose();

    }

}


