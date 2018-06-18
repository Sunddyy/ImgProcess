// Source:The followings are copied from http://download.csdn.net/download/u012723301/8586329
// Date:2018-01-30        Author:Sund

//C#通过指针二值化、细化图像

//关于算法，主要用lockbits来读取位图，大概20ms左右完成二值化5000x5000pixels的bmp。（E5200 @ 3GHz），相比之下最烂的getpixel要10s以上

//二值化算法使用的是HSL色彩空间，通过H或L的阈值来完成
//复制内容到剪贴板

//代码:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;

namespace ImgPocess
{
    
    // various kinds tools to manipulate the image
    // dataArray is a 2 dimension array which stores  a binary image
    /// <summary>
    /// x,y the location of the first point
    ///  usage: the invoker should provide the first point of the path 
    ///  mark: the mark value set by the invoker , it should be 0 or 255
    /// </summary>
    public class ImageToolKit
    {
        public int pathLeft, pathUp, pathRight, pathDown;// the bound of the path area found 

        int m_ttlColLen = 0;
        int m_ttlRowLen = 0;
        int m_ttlRow = 0;
        int m_startRow = 0;
        int m_sqrNumbers = 0;
        int m_iWidth;
        //高
        int m_iHeight;
        //按照屏幕宽高创建位图
        Image m_imgForCapture;
        Graphics m_gcForCapture;
        private ArrayList m_sqrAlist;
        private double cfdThresh=0.5;

        public void setImgSize(Form fm)
        {
            m_iWidth = fm.Width;
            //高
            m_iHeight = fm.Height;
            //按照屏幕宽高创建位图
            m_imgForCapture = new Bitmap(m_iWidth, m_iHeight);
            m_gcForCapture = Graphics.FromImage(m_imgForCapture);
            m_sqrAlist = new ArrayList();

        }

        public void searchPath(byte[,] dataArray)
        {
            int clmLen, rowLen;
            rowLen = dataArray.GetLength(0);
            clmLen = dataArray.GetLength(1);
            pathLeft = clmLen;
            pathUp = rowLen;
            pathRight = 0;
            pathDown = 0;
            byte mark = 1;
            bool bFound = false;


            for (int r = 0; r < rowLen; r++)
            {
                bFound = false;
                if (r == 91)
                    bFound = false;
                for (int c = 0; c < clmLen; c++)
                {
                    if (dataArray[r, c] == 255)// find the first point , exit
                    {
                        bFound = true;
                        searchPath(dataArray, r, c, mark);
                        mark++;
                        //break;
                    }

                }
                //if (!bFound && pathDown > 0)
                //    break;

            }



        }
        /// <summary>
        ///  search the path in a array which is conformed by 255 continuiously. 
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="row"></param>
        /// <param name="clm"></param>
        /// <param name="mark"></param>
        public void searchPath(byte[,] dataArray, int row, int clm, byte mark)
        {
            int clmLen, rowLen;
            rowLen = dataArray.GetLength(0);
            clmLen = dataArray.GetLength(1);
            bool bFound;
            bFound = false;
            // border checking
            if (row < 0 || row >= rowLen || clm < 0 || clm >= clmLen)
            {
                return;
            }

            if (dataArray[row, clm] == 255)
            {
                bFound = true;
                if (clm < pathLeft)
                {
                    pathLeft = clm;
                }
                else
                {
                    if (clm > pathRight) pathRight = clm;
                }
                if (row < pathUp)
                {
                    pathUp = row;
                }
                else
                {
                    if (row > pathDown) pathDown = row;
                }
            }
            //else //1# search the first point of 255


            if (!bFound)
            {
                return;//
            }
            else
            {
                dataArray[row, clm] = mark;//means this point has been check it 
            }
            int startRow, startClm;
            //2# search left
            startRow = row;
            startClm = clm - 1;
            searchPath(dataArray, startRow, startClm, mark);
            //3# search up left
            startRow = row - 1;
            startClm = clm - 1;
            searchPath(dataArray, startRow, startClm, mark);
            //4# search up
            startRow = row - 1;
            startClm = clm;
            searchPath(dataArray, startRow, startClm, mark);
            //5# search up right
            startRow = row - 1;
            startClm = clm + 1;
            searchPath(dataArray, startRow, startClm, mark);
            //6# search right
            startRow = row;
            startClm = clm + 1;
            searchPath(dataArray, startRow, startClm, mark);
            //7# search down right
            startRow = row + 1;
            startClm = clm + 1;
            searchPath(dataArray, startRow, startClm, mark);
            //8# search down
            startRow = row + 1;
            startClm = clm;
            searchPath(dataArray, startRow, startClm, mark);
            //9# search left down
            startRow = row + 1;
            startClm = clm - 1;
            searchPath(dataArray, startRow, startClm, mark);


        }
        #region 从大图中截取一部分图片, from https://www.cnblogs.com/kehaocheng/p/7126690.html
        /// <summary>
        /// 从大图中截取一部分图片
        /// modified by sund 
        /// date:2018-01-31
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static Image getSubImage(Bitmap fromImage, int offsetX, int offsetY, int width, int height)
        {
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            IntPtr bitPtr;
            bitPtr = bitmap.GetHbitmap();
            Image subImage = Image.FromHbitmap(bitPtr);
            //释放资源   
            ImgPocess.ImgeTool.DeleteObject(bitPtr);
            graphic.Dispose();
            bitmap.Dispose();
            return subImage;

        }
        #endregion
        public Image captureSubtitleBand(Form fm)
        {
            //宽

            //Image img = new Bitmap(200, 200);
            //从一个继承自Image类的对象中创建Graphics对象
            //抓屏并拷贝到myimage里
            int left, top, right, bottom;
            left = fm.Location.X;
            top = fm.Location.Y;
            right = fm.Width;
            bottom = fm.Height;

            m_gcForCapture.CopyFromScreen(new Point(left, top), new Point(0, 0), new Size(m_iWidth, m_iHeight));
            //gc.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));
            return m_imgForCapture;

        }

        public Image CopyScreen()
        {
            //屏幕宽
            int iWidth = Screen.PrimaryScreen.Bounds.Width;
            //屏幕高
            int iHeight = Screen.PrimaryScreen.Bounds.Height;
            //按照屏幕宽高创建位图
            Image img = new Bitmap(iWidth, iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics gc = Graphics.FromImage(img);
            //抓屏并拷贝到myimage里
            gc.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));
            return img;

        }
        #region Get a square from a binary picture , that block is separated by 0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binDat"></param>
        /// <param name="colLeft"></param>
        /// <param name="rowTop"></param>
        /// <param name="colLen"></param>
        /// <param name="rowLen"></param>
        /// <param name="bFixedRow">all </param>
        /// <returns></returns>
        public bool getSquareDat(byte[,] binDat, ref int colLeft, ref int rowTop, ref int colLen, ref int rowLen, bool bFixedRow)
        {
            //byte[,] square;
            //1# check the with intial h and w
            int chkPntNum = 1;//if the continious 2 pints is 255, we consider it is a real point rather than noise 
            int pntSum;//the value summery of the tow adjacent point 
            int colPtr;
            int colRight, rowBottom;
            //colPtr = colLen-1;
            int ttlCol;
            ttlCol = binDat.GetLength(1);
            int ttlRow;
            ttlRow = binDat.GetLength(0);
            bool retVal = true;
            //search the left column of the square
            colPtr = colLeft;
            bool bFoundLeft = false;
            //the initial condition check
            if (rowTop + rowLen >= ttlRow || colLeft + colLen >= ttlCol)
            {
                retVal = false;
                goto exit;
            }
            while (true)
            {

                for (int r = rowTop; r < rowLen + rowTop; r++)
                {

                    pntSum = 0;
                    if (r + chkPntNum < rowLen + rowTop)
                    {
                        for (int i = r; i < r + chkPntNum; i++)
                        {
                            pntSum += binDat[i, colPtr];
                        }
                    }

                    if (pntSum / chkPntNum == 255)
                    {
                        bFoundLeft = true;//find the left border 
                        break;
                    }
                }
                if (bFoundLeft)
                {
                    break;
                }
                else
                {

                    if (colPtr < ttlCol - colLen)// search to the right 
                    {
                        colPtr++;
                        continue;
                    }
                    else// the left border not find for the the firt rowLen rows , than move the row the next rowLen rows
                    {
                        colPtr = colLeft;
                        if (rowTop + rowLen * 2 < ttlRow)
                        {
                            rowTop += rowLen;
                            continue;
                        }
                        else
                        {
                            break;// there is no square found 
                        }
                    }
                }

            }// notice if met the border of the array , it meets the requirement, therefore bRowOK = true;

            if (!bFoundLeft)
            {

                retVal = false;
                goto exit;
            }

            //set to the minimam right border
            colLeft = colPtr;
            //search the top  border 


            int rowPtr;
            rowPtr = rowTop;
            bool bFoundTop = true;

            while (rowPtr < ttlRow)
            {
                bFoundTop = false;
                for (int c = colLeft; c < colLeft + colLen; c++)
                {
                    if (binDat[rowPtr, c] != 0)
                    {
                        bFoundTop = true;
                        break;
                    }
                }
                if (bFoundTop)
                {
                    break;
                }
                else
                {

                    rowPtr++;
                    continue;
                }

            }// notice if met the border of the array , it meets the requirement, therefore bRowOK = true;

            if (!bFoundTop)// a possible exit branch with goto command 
            {
                retVal = false;
                goto exit;///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }

            rowTop = rowPtr;

            // seach the right side of the square 
            colPtr = colLeft + colLen;
            colRight = colPtr;

            bool bFoundRight = true;

            while (colPtr < ttlCol)
            {
                bFoundRight = true;
                for (int r = rowTop; r < rowTop + rowLen; r++)
                {

                    pntSum = 0;
                    if (r + chkPntNum < rowLen + rowTop)
                    {
                        for (int i = r; i < r + chkPntNum; i++)
                        {
                            if (i < ttlRow)
                                pntSum += binDat[i, colPtr];
                        }
                    }

                    if (pntSum / chkPntNum == 255)
                    {
                        bFoundRight = false;
                        break;
                    }

                    //if (binDat[r, colPtr] != 0)
                    //{
                    //    bFoundRight = false;
                    //    break;
                    //}
                }
                if (bFoundRight)
                {
                    break;
                }
                else
                {

                    colPtr++;
                    continue;
                }

            }// notice if met the border of the array , it meets the requirement, therefore bRowOK = true;

            if (!bFoundRight)// a possible exit branch with goto command 
            {
                retVal = false;
                goto exit;///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            colLen = colPtr - colLeft;
            colRight = colPtr;

            //search the bottom  border 

            //set to the minimam rifht border

            rowPtr = rowTop + rowLen;
            rowBottom = rowPtr;
            bool bFoundBottom = true;

            while (rowPtr < ttlRow)
            {
                bFoundBottom = true;
                for (int c = colLeft; c < colLeft + colLen; c++)
                {
                    if (binDat[rowPtr, c] != 0)
                    {
                        bFoundBottom = false;
                        break;
                    }
                }
                if (bFoundBottom)
                {
                    break;
                }
                else
                {
                    if (rowPtr + 1 < ttlRow)
                    {
                        rowPtr++;// search to the bottom 
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }


            }// notice if met the border of the array , it meets the requirement, therefore bRowOK = true;

            if (!bFoundBottom)// a possible exit branch with goto command 
            {
                retVal = false;
                goto exit;///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            rowLen = rowPtr - rowTop;
            rowBottom = rowPtr;

            retVal = true;

        exit:
            return retVal;

        }
        /// <summary>
        ///  a simple version of get area from imgage including subtitle that is it has just the excat subtitle with fixed top and bottom
        /// </summary>
        /// <param name="binDat"></param>
        /// <param name="colLeft"></param>
        /// <param name="rowTop"></param>
        /// <param name="colLen"></param>
        /// <param name="rowLen"></param>
        /// <param name="bFixedRow"></param>
        /// <returns></returns>
        public ArrayList getSimpleSquareDat(byte[,] binDat, ref int colLeft, ref int colLen,double cdf_H,double cdf_L,double des_H,double des_L, bool bFixedRow)
        {
            int colPtr;
            int ttlCol;
            ttlCol = binDat.GetLength(1);
            int ttlRow;
            ttlRow = binDat.GetLength(0);
            bool retVal = true;
            //search the left column of the square
            colPtr = colLeft;
            int rowLen = ttlRow;
            // calculat the vertical points number for each horizontal points 
            double[] hstGramVal = new double[ttlCol];
            int affectLen=6;// used to set the adjacent point value to 1 ,if their neighbours are not zero.
            int sumPtr = 0;
            
            for (int c = 0; c < ttlCol; c++)
            {
                for (int r = 0; r < ttlRow; r++)
                {
                    hstGramVal[c] += binDat[r, c] ;// line density 
                }
                //if (c > affectLen)
                //{
                //    if (hstGramVal[c] != 0 && hstGramVal[c - affectLen] != 0)
                //        for (int subC = c - affectLen + 1; subC < c; subC++)
                //        {
                //            hstGramVal[subC] = hstGramVal[subC] == 0 ? 1 : hstGramVal[subC];
                //        }
                //}
                hstGramVal[c] = hstGramVal[c] / 255.0 / ttlRow;
                if (hstGramVal[c] != 0 && sumPtr ==0)
                {
                    sumPtr = c;


                }
                if (sumPtr != 0)
                {
                    if (c - sumPtr >= colLen)
                    {
                        hstGramVal=ImgeTool.removeNoise(hstGramVal, sumPtr, c, cdf_H,cdf_L,des_H,des_L,true);
                        
                        sumPtr = 0;
                    }
                }
            }


            // search the border from the midle part of the line... 
            // while(true)
            {
                m_sqrAlist.Clear();
                long callTimes = 0;
                int curPos, nextPos;
                int searchStep = 1;// 
                //curPos = ttlCol / 2;
                curPos = 0;
                int searchWidth;
                searchWidth = rowLen;// the default search width is the same avheight 
                int mySchWidth =(int)(0.6 * searchWidth);// seach from the 60% of the default width , for chinese charater is not the exact squre 
                mySchWidth = 2;
                while (true)
                {
                    callTimes++;

                    nextPos = getBorderAtLine(hstGramVal, curPos, searchStep);
                    if (nextPos < 0)
                    {
                        break;
                    }
                    int dist = Math.Abs(curPos - nextPos);
                    if (dist<=1 )//|| dist > searchWidth)
                    {
                        curPos += 1;
                        searchStep=1;
                    }
                    else
                    {
                        
                        if (dist >=1 && dist < mySchWidth)// adjacent column
                        {
                            curPos += 1;
                            searchStep = 1;
                            goto Next;// change the curpos and begin a new search

                        }
                        else if (dist >= mySchWidth)// && dist <= searchWidth)// find allmost an exact  border 
                        {
                            SquareDat squareDat = new SquareDat();// memory ....!!!!!!!!!!!!!!!

                            squareDat.col = searchStep<0 ? nextPos : curPos;

                            squareDat.row = 0;
                            squareDat.rowLen = 0;// NA
                            squareDat.colLen = Math.Abs(curPos - nextPos);
                            m_sqrAlist.Add(squareDat);
                            curPos = nextPos+1;
                            continue;
                        }
                        else// dist > searchWidth, it might be encounter an block of white point or black piont 
                        {
                           //MessageBox.Show("Wrong process！");
                            throw new Exception("Your process is not correct!");
                        }
                    }
                    //if (nextPos == 0 && searchDir == -1)
                    //{
                    //    curPos = curPos = ttlCol / 2;
                    //    searchDir = 1;
                    //    continue;
                    //}
                    Next:
                    if (nextPos + searchStep >=ttlCol- 1)
                    {
                        // end codes....
                        break;
                    }
                }

            }

            // exit:
            return m_sqrAlist;

        }
        /// <summary>
        ///  search the nearest border of the character in the subtitle to pos , dir indicates the search function  
        /// </summary>
        /// <param name="lineDat">an array whose value is the number of points at each column  (histgram) </param>
        /// <param name="pos">the start search point </param>
        /// <param name="step">-1 search to the left , 1 to the right </param>
        /// <returns> the position of the border found, the borer of the array is valid value </returns>
        private int getBorderAtLine(double[] lineDat, int pos, int step)
        {
            int foundPos = -1;
            int datLen;
            datLen = lineDat.GetLength(0);
            //if (step != 1 && step != -1)
            //{
            //    return pos;
            //}
            if (step + pos <= 0)
            {
                foundPos = 0;
                goto exit;
            }
            if (step + pos >= datLen - 1)
            {
                foundPos = datLen - 1;
                goto exit;
            }
            if (pos < 0 || pos >= datLen)
            {
                foundPos = pos;
                goto exit;

            }

            
            if (pos < datLen)
            {
                if (lineDat[pos]  == 0)
                {
                    foundPos = pos;
                    goto exit;

                }
                else
                {
                    pos += step;
                    foundPos = getBorderAtLine(lineDat, pos, step);
                }
            }
            else
            {
                foundPos = datLen - 1;
            }
            


        exit:
            return foundPos;
        }
        #endregion
        /// <summary>
        ///  Get the area that display the subtitle and return the topRow and bottomRow 
        /// </summary>
        /// <param name="binDat"></param>
        /// <param name="row"></param>
        /// <param name="rowBottom"></param>
        public void getSubtitelBand(byte[,] binDat, ref int topRow, ref int bottomRow)
        {
            int ttlCol;
            ttlCol = binDat.GetLength(1);

            int ttlRow;
            ttlRow = binDat.GetLength(0);
            int[] rowPnts = new int[ttlRow];
            int leftCol, rightCol;
            double[] rowPntsAvage = new double[ttlRow];
            leftCol = ttlCol;
            rightCol = 0;
            bool bTopRowHasFound = false;
            for (int r = 0; r < ttlRow; r++)
            {
                rowPnts[r] = 0;

                for (int c = 0; c < ttlCol; c++)
                {
                    if (binDat[r, c] == 255)
                    {
                        rowPnts[r]++;
                        if (c > rightCol) rightCol = c; ;
                        if (c < leftCol) leftCol = c;
                    }
                }


            }
            for (int r = 0; r < ttlRow; r++)
            {
                if (r > 1)
                {
                    if (rowPnts[r - 1] == 0 && rowPnts[r] != 0 && r > topRow) topRow = r;
                }
                if (r > topRow)
                {
                    if (rowPnts[r - 1] != 0 && rowPnts[r] == 0 && bottomRow > topRow + 30)
                    {
                        bottomRow = r;
                        break;
                    }
                }
            }
            //for (int r = 0; r < ttlRow; r++)
            //{
            //    rowPntsAvage[r] = (double)rowPnts[r] / (rightCol - leftCol);
            //}
        }

        #region get a img which might be the image include chinese character

        public ArrayList getLikelySubtitleImg(byte[,] binDat)
        {

            ArrayList sqrAlist = new ArrayList();

            int col, row, colLen, rowLen;
            col = 0;
            row = 0;

            int chHmax = 120, chHmin = 16;
            int chWmax = 70, chWmin = 16;
            colLen = chHmin;
            rowLen = chWmin;

            int ttlCallTime = 2;// the recursive time to call the function
            int callTime = 0;

            for (int i = 0; i < ttlCallTime; i++)
            {
                sqrAlist.Clear();

                while (true)
                {
                    callTime++;



                    if (!getSquareDat(binDat, ref col, ref row, ref colLen, ref rowLen, false))
                    {
                        break;
                    }
                    if (m_startRow != 0 && row != 0 && Math.Abs(m_startRow - row) > rowLen / 3) break;
                    //if (i == ttlCallTime - 1)
                    //{
                    //    if (row - startRow >= rowLen)// to avoid searching the line below 
                    //    {
                    //        col += colLen;
                    //        continue;
                    //    }
                    //}
                    SquareDat squareDat = new SquareDat();
                    //if (colLen > chWmax || colLen < chWmin || rowLen > chHmax || rowLen < chHmin)
                    //{
                    //    col += colLen;
                    //    continue;
                    //}
                    squareDat.col = col;
                    if (m_sqrNumbers > 100)
                    {
                        squareDat.row = m_startRow;
                        squareDat.rowLen = m_ttlRowLen / m_sqrNumbers;

                    }
                    else
                    {

                        squareDat.row = row;
                        squareDat.rowLen = rowLen;
                    }
                    squareDat.colLen = colLen;
                    sqrAlist.Add(squareDat);
                    col += colLen;
                    m_ttlRow += row;
                    m_ttlColLen += colLen;
                    m_ttlRowLen += rowLen;
                    //if (startRow > row) startRow = row;//set the row to the minimam value 
                    m_sqrNumbers++;
                }
                if (sqrAlist.Capacity == 0)
                {
                    break;
                }
                if (m_sqrNumbers > 0 && m_sqrNumbers < 10000)
                {
                    //colLen = m_ttlColLen / m_sqrNumbers;
                    rowLen = m_ttlRowLen / m_sqrNumbers;
                    col = 0;
                    m_startRow = m_ttlRow / m_sqrNumbers;
                    //let the row be the value of the maximum group memebers , ie   1 2 2 3 , the row  is 2
                    row = m_startRow;
                }


            }
            //check if they are allmost the same size

            return sqrAlist;
        }

        public ArrayList getSimpleSubtitle(byte[,] binDat,double cdf_h,double cdf_l,double des_h,double des_l)
        {


            int col, row, colLen, rowLen;
            col = 0;
            row = 0;

            // 
            colLen = binDat.GetLength(0) - 4;
            rowLen = binDat.GetLength(0);

            int ttlCallTime = 2;// the recursive time to call the function
            int callTime = 0;
            getSimpleSquareDat(binDat, ref col, ref colLen, cdf_h,cdf_l,des_h,des_l,true);

            return m_sqrAlist;


        }

        public ArrayList getSimpleSubtitle_old(byte[,] binDat)
        {


            int col, row, colLen, rowLen;
            col = 0;
            row = 0;

            // 
            colLen = binDat.GetLength(0) - 4;
            rowLen = binDat.GetLength(0);

            int ttlCallTime = 2;// the recursive time to call the function
            int callTime = 0;

            for (int i = 0; i < ttlCallTime; i++)
            {
                m_sqrAlist.Clear();

                while (true)
                {
                    callTime++;


                    colLen = binDat.GetLength(0) - 4;
                    //if (!getSimpleSquareDat(binDat, ref col, ref colLen, false))
                    //{
                    //    break;
                    //}


                    SquareDat squareDat = new SquareDat();

                    squareDat.col = col;

                    squareDat.row = row;
                    squareDat.rowLen = rowLen;
                    squareDat.colLen = colLen;
                    m_sqrAlist.Add(squareDat);
                    col += colLen;

                    m_ttlColLen += colLen;

                    //if (startRow > row) startRow = row;//set the row to the minimam value 
                    m_sqrNumbers++;
                }
                if (m_sqrAlist.Capacity == 0)
                {
                    break;
                }
                //if (m_sqrNumbers > 0 && m_sqrNumbers < 10000)
                {
                    //colLen = m_ttlColLen / m_sqrNumbers;

                    col = 0;

                }


            }
            //check if they are allmost the same size

            return m_sqrAlist;
        }

        #endregion
        // the structure is used to store the location and bound of a squre img which Chinese is likely in it.
        public struct SquareDat
        {
            public int row, col;
            public int rowLen, colLen;
        };


        
    }
}
