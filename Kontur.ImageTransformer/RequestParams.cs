using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public enum FilterType
    {
        Sepia,
        Grayscale,
        Threshold
    }

    public class RequestParams
    {
        public RequestParams(FilterInfo filter, ImagePieceCoords imagePieceCoords)
        {
            Filter = filter;
            ImagePiece = imagePieceCoords;
        }

        public FilterInfo Filter { get; private set; }

        public ImagePieceCoords ImagePiece {get; private set; }
    }

    public class FilterInfo
    {
        public FilterInfo(FilterType type)
        {
            FilterType = type;
        }

        public FilterInfo(FilterType type, int filterParam) :this(type)
        {
            FilterParam = filterParam;
        }

        public FilterType FilterType { get; private set; }

        public int FilterParam { get; set; } //нужен для threshold-фильтра

    }

    public class ImagePieceCoords
    {
        public ImagePieceCoords(Point upperLeft, int width, int height)
        {
            StartingPoint = upperLeft;
            Width = width;
            Height = height;
        }

        public Point StartingPoint { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }

    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
