        /// <summary>

        /// Histogram equalization process.

        /// </summary>

        /// <param name="src">The source image.</param>

        /// <returns></returns>

        public static WriteableBitmap HistogramEqualProcess(WriteableBitmap src)////30图像直方图均衡化

        {

            if (src != null)

            {

                int w = src.PixelWidth;

                int h = src.PixelHeight;

                WriteableBitmap histogramEqualImage = new WriteableBitmap(w, h);

                byte[] temp = src.PixelBuffer.ToArray();

                byte gray;

                int[] tempArray = new int[256];

                int[] countPixel = new int[256];

                byte[] pixelMap = new byte[256];

                for (int i = 0; i < temp.Length; i += 4)

                {

                    gray = (byte)(temp[i] * 0.114 + temp[i + 1] * 0.587 + temp[i + 2] * 0.299);

                    countPixel[gray]++;

                }

                for (int i = 0; i < 256; i++)

                {

                    if (i != 0)

                    {

                        tempArray[i] = tempArray[i - 1] + countPixel[i];

                    }

                    else

                    {

                        tempArray[0] = countPixel[0];

                    }

                    pixelMap[i] = (byte)(255 * tempArray[i] * 4 / temp.Length + 0.5);

                }

                for (int i = 0; i < temp.Length; i+=4)

                {

                    gray = temp[i];

                    temp[i] = pixelMap[gray];

                    gray = temp[i+1];

                    temp[i+1] = pixelMap[gray];

                    gray = temp[i+2];

                    temp[i+2] = pixelMap[gray];

                }

                Stream sTemp = histogramEqualImage.PixelBuffer.AsStream();

                sTemp.Seek(0, SeekOrigin.Begin);

                sTemp.Write(temp, 0, w * 4 * h);

                return histogramEqualImage;

            }

            else

            {

                return null;

            }         

        }