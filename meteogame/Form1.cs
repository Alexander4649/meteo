using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace meteogame
{
    public partial class Form1 : Form
    {
        static Bitmap canvas = new Bitmap(480, 320);
        Graphics gg = Graphics.FromImage(canvas);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //隠し
            pMeteor.Hide();
            pPlayer.Hide();
            pBG.Hide();
            pEXP.Hide();
            pGameover.Hide();
            pMsg.Hide();
            pTitle.Hide();

            gg.DrawImage(pBG.Image, 0, 0, 480, 320);
            gg.DrawImage(pMeteor.Image, 50, 50, 70, 70);

            pBase.Image = canvas;
        }
    }
}
