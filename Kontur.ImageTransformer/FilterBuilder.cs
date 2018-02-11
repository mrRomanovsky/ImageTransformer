using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public static class FilterBuilder
    {

        public static Func<Bitmap, Bitmap> BuildFilterFromParams(RequestParams requestParams)
        {
            var imagePiece = requestParams.ImagePiece;
            var sPoint = imagePiece.StartingPoint;
            Func<Bitmap, Bitmap> truncate = b => TruncateBitmap(b, sPoint.X, sPoint.Y,
                imagePiece.Width, imagePiece.Height);
            switch (requestParams.Filter.FilterType)
            {
                case FilterType.Grayscale:
                    return bitMap => ImageFilters.Grayscale(truncate(bitMap));
                case FilterType.Sepia:
                    return bitMap => ImageFilters.Sepia(truncate(bitMap));
                default:
                    return bitMap => ImageFilters.Threshold(truncate(bitMap), requestParams.Filter.FilterParam);
            }
        }

        public static Bitmap TruncateBitmap(Bitmap bitmap, int x, int y, int width, int height)
        {
            int xNew = CalcCoord(x + width, bitmap.Width);
            int yNew = CalcCoord(y + height, bitmap.Height);
            x = CalcCoord(x, bitmap.Width);
            y = CalcCoord(y, bitmap.Height);
            if (x == 0 && xNew == 0 && yNew == 0 && y == 0)
                throw new ContentException("no content!");
            var upperLeft = GetUpperLeft(x, y, xNew, yNew);
            return bitmap.Clone(new Rectangle(upperLeft.X, upperLeft.Y,
                    Math.Abs(x - xNew), Math.Abs(y - yNew)), bitmap.PixelFormat);
        }

        private static int CalcCoord(int coord, int dimensionSize)
        {
            if (coord < 0)
                return 0;
            return Math.Min(coord, dimensionSize);
        }

        private static Point GetUpperLeft(int x0, int y0, int x1, int y1)
        {
            if (x0 <= x1 && y0 <= y1)
                return new Point(x0, y0);
            return new Point(Math.Min(x0, x1), Math.Min(y0, y1));
        }
    }
}
