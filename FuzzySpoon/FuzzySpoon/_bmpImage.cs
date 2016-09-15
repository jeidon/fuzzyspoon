using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzySpoon
{
    public struct RGBTRIPLE
    {
        public byte rgbtBlue;
        public byte rgbtGreen;
        public byte rgbtRed;
    }

    class _bmpImage
    {
        enum bitOptions { bit8 = 8, bit16 = 16, bit18 = 18, bit24 = 24, bit32 = 32 };

        //Private Properties
        //File components
        //private char drive[_MAX_DRIVE];
        //private char dir[_MAX_DIR];
        //private char fname[_MAX_FNAME];
        //private char ext[_MAX_EXT];
        //private string workingDir;

        //Other stuff
        //private byte alpha;
        //private RGBTRIPLE image;
        //private string sourceFileName;
        public System.Drawing.Bitmap sourceImage;

        //Public Properties
        //        public BITMAPFILEHEADER bfh;
        //        public BITMAPINFOHEADER bih;

        public int sendImageArray(int bpp)
        {
            PACKET packet = new PACKET();
            int arrayProgress = 0;
            int arraySize = sourceImage.Height * sourceImage.Width;
            int bytesRemaining = arraySize;
            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    byte newValue = 0;
                    //byte msb, lsb;
                    System.Drawing.Color pixel = sourceImage.GetPixel(x, y);

                    //if (bpp == 32)
                    //{
                    //    32Bit Easy... done
                    //    fprintf(fDest, "0x%02x, ", alpha);
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtRed);
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtGreen);
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtBlue);
                    //}
                    //else if (bpp == 24)
                    //{
                    //    24Bit
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtRed);
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtGreen);
                    //    fprintf(fDest, "0x%02x, ", pixel.rgbtBlue);
                    //}
                    //else if (bpp == 18)
                    //{
                    //    18Bit
                    //    downConvert18(&pixel, &newPixel);
                    //    fprintf(fDest, "0x%02x, ", newPixel.rgbtRed);
                    //    fprintf(fDest, "0x%02x, ", newPixel.rgbtGreen);
                    //    fprintf(fDest, "0x%02x, ", newPixel.rgbtBlue);
                    //}
                    //else if (bpp == 16)
                    //{
                    //    16Bit
                    //    downConvert16(&pixel, &msb, &lsb);
                    //    fprintf(fDest, "0x%02x, ", msb);
                    //    fprintf(fDest, "0x%02x, ", lsb);
                    //}
                    //else
                    if (bpp == 8)
                    {
                        //8 bit
                        downConvert8(pixel, newValue);
                        packet.data[arrayProgress++] = newValue;
                    }

                    if (arrayProgress == 256)
                    {
                        packet.CD = 0;
                        packet.dataLength = 255;
                        frmMain.Transmit(packet);
                        arrayProgress = 0;
                        bytesRemaining -= 255;
                    }

                }
            }
            if(bytesRemaining > 0)
            {
                packet.CD = 0;
                packet.dataLength = Convert.ToByte(bytesRemaining);
                frmMain.Transmit(packet);
            }
            return 0;
        }

        ////RGBTRIPLE getPixel(long x, long y)
        ////{
        ////    // Image define from earlier
        ////    return image[(sourceImage.Height - 1 - x) * sourceImage.Width + y];
        ////}
        //public void initBlank(int x, int y);
        //public void openSourceFile();
        //public void setPixel(int x, int y, RGBTRIPLE color);
        //public void setSourceFile(string path);

        //Private Methods
        private void downConvert8(System.Drawing.Color pixel, byte newValue)
        {
            // Assuming the desired color config is RRRGGGBB
            System.Drawing.Color newPixel1 = System.Drawing.Color.FromArgb(
                linearConvert(pixel.R, 0, 255, 0, 7),
                linearConvert(pixel.G, 0, 255, 0, 7),
                linearConvert(pixel.B, 0, 255, 0, 3)
                );

            System.Drawing.Color newPixel2 = System.Drawing.Color.FromArgb(
                (byte)((Convert.ToInt32(newPixel1.R) << 5) & 0xE0),
                (byte)((Convert.ToInt32(newPixel1.G) << 2) & 0x1C),
                (byte)((Convert.ToInt32(newPixel1.B) << 0) & 0x03)
                );

            newValue = (byte)(newPixel2.R + newPixel2.G + newPixel2.B);
        }
        //    void _bmpImage::downConvert16(const RGBTRIPLE* pixel, BYTE* newValue1, BYTE* newValue2)
        //        {
        //             Assuming the desired color config is RRRRRGGG GGGBBBBB
        //            RGBTRIPLE newPixel = *pixel;
        //    The ranges for R are[0 - 255] and[0 - 31]
        //     newPixel.rgbtRed = linearConvert(pixel->rgbtRed, 0, 255, 0, 31);
        //    The ranges for G are[0 - 255] and[0 - 63]
        //     newPixel.rgbtGreen = linearConvert(pixel->rgbtGreen, 0, 255, 0, 63);
        //    The ranges for B are[0 - 3] and[0 - 31]
        //     newPixel.rgbtBlue = linearConvert(pixel->rgbtBlue, 0, 255, 0, 31);

        //            *newValue1 = ((newPixel.rgbtRed << 3) & 0xF8) | (newPixel.rgbtGreen & 0x07);
        //            *newValue2 = ((newPixel.rgbtGreen << 2) & 0xE0) | (newPixel.rgbtBlue & 0x1F);
        //        }
        //void _bmpImage::downConvert18(const RGBTRIPLE* pixel, RGBTRIPLE* newPixel)
        //{
        //    Assuming the desired color config is RRRRRGGG GGGBBBBB
        //   The ranges for R are[0 - 255] and[0 - 63]

        //   newPixel->rgbtRed = linearConvert(pixel->rgbtRed, 0, 255, 0, 63);
        //    The ranges for G are[0 - 255] and[0 - 63]

        //    newPixel->rgbtGreen = linearConvert(pixel->rgbtGreen, 0, 255, 0, 63);
        //    The ranges for B are[0 - 255] and[0 - 63]

        //    newPixel->rgbtBlue = linearConvert(pixel->rgbtBlue, 0, 255, 0, 63);
        //}
        byte linearConvert(byte x, float A, float B, float C, float D)
        {
            byte returnValue;

            // This equation came from a stack exchange post
            //http://math.stackexchange.com/questions/43698/range-scaling-problem
            returnValue = (byte)(C + (D - C) * ((x - A) / (B - A)));
            return returnValue;
        }
};
}
