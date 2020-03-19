using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTARPath.custom
{
    class CCompra
    {

        
        int MatrixSizeX = 0;
        int MatrixSizeY = 0;
        int PixelSize = 0;

        CNode NodeStart = null;
        CNode NodeEnd = null;
        CNode FinalNode = null;

        List<CNode> OpenList = new List<CNode>();
        List<CNode> CloseList = new List<CNode>();

      
    }
}
