using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;               //Bitmap
using System.Windows.Forms;         //Application.StartupPath


namespace ShootBasketball.BaseClass
{
    class LoadBitmap
    {
        public static Bitmap LoadBmp(string bmpFileName)
        {
            return new Bitmap(Application.StartupPath + "\\GamePictures\\" + bmpFileName + ".bmp");
        }

    }
}
