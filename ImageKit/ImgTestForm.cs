using ImgPocess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageKit
{
    public partial class ImgTestForm : Form
    {
        public ImgTestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = (Bitmap)picB1.Image;
            Bitmap bmp2 = new Bitmap(bmp1.Width, bmp1.Height, bmp1.PixelFormat);

            for (int i = 0; i < bmp1.Height; i++)
            {
                for (int j = 0; j < bmp1.Width; j++)
                {
                    Color clr;
                    clr = ImgeTool.myGetPixel(bmp1, j, i);
                    //clr = bmp1.GetPixel(j, i);
                    ImgeTool.mySetPixel(bmp2, clr, j, i);
                    //bmp2.SetPixel(j, i, clr);
                }
                picB2.Image = bmp2;
                picB2.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Font font = new Font("黑体", 48);
            picB2.Image = ImgeTool.WriteEmbossed((Bitmap)(picB1.Image), "情报中心", font, 10, picB1.Image.Height/4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            picB2.Image = ImgeTool.GetRotateImage(picB1.Image, 45);
     

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //picB2.Image = ImgeTool.BlindWindow((Bitmap)picB1.Image, 10, "H", 10);
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayBlindWindow((Bitmap)picB1.Image, delDiplay, "V", 10);
            //Bitmap bmp = (Bitmap)(picB1.Image);
            ////picB2.Image = 
            //Graphics g = picB2.CreateGraphics();
            //g.DrawImage(ImgeTool.getSubImage(bmp, 0, 0, bmp.Width, bmp.Height),0,0);
        }
        private void displayPicBox(Bitmap bmp)
        {
            picB2.Image = bmp;
            picB2.Refresh();

        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayFade((Bitmap)(picB1.Image), delDiplay, !true,50,-2);

            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayZoom((Bitmap)(picB1.Image), delDiplay, !true, 50, 50);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)picB1.Image;
            bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            picB2.Image = bmp;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            picB2.Image = ImgeTool.deform((Bitmap)(picB1.Image), 4, 50f, picB1.Image.Height);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //picB2.Image = ImgeTool.circle((Bitmap)(picB1.Image), 100);
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayCircle((Bitmap)(picB1.Image), delDiplay, true, 50, 50);
            //picB2.Image = ImgeTool.displayCircle((Bitmap)(picB1.Image), 100);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displaySquare((Bitmap)(picB1.Image), delDiplay, true, 50, 60);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //picB2.Image = ImgeTool.triangle((Bitmap)(picB1.Image), 50);
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayTriangle((Bitmap)(picB1.Image), delDiplay, true, 50, 50);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //picB2.Image = ImgeTool.sector((Bitmap)(picB1.Image), 99,15,45);
            ImgeTool.delDisplayImg delDiplay = new ImgeTool.delDisplayImg(displayPicBox);
            ImgeTool.displayRadar((Bitmap)(picB1.Image), delDiplay, true, 10, 20);
        }
    }
}
