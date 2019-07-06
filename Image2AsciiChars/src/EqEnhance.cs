Bitmap picequalization(Bitmap basemap, int width, int height)
        {
            Bitmap retmap = new Bitmap(basemap, width, height);
            int size = width * height;
            int[] gray = new int[256];
            double[] graydense = new double[256];
            for(int i=0;i<width;++i)
                for (int j = 0; j < height; ++j)
                {
                    Color pixel = basemap.GetPixel(i,j);
                    gray[Convert.ToInt16(pixel.R)] += 1;
                }
            for (int i = 0; i < 256; i++)
            {
                graydense[i] = (gray[i]*1.0) / size;
            }

            for (int i = 1; i < 256; i++)
            {
                graydense[i] += graydense[i-1];
            }

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    Color pixel = basemap.GetPixel(i, j);
                    int gg = Convert.ToInt16(pixel.R);
                    int g = 0;
                    if(gg==0)
                       g=0;
                    else
                    g =Convert.ToInt16( graydense[Convert.ToInt16(pixel.R)] * Convert.ToInt16(pixel.R));
                    
                    pixel = Color.FromArgb(g,g,g);
                    retmap.SetPixel(i, j, pixel);
                    //gray[Convert.ToInt16(pixel.R)] += 1;
                }
                return retmap;
        }