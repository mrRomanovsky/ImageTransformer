using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public static class UrlParser
    {
        private static Dictionary<string, FilterType> filterTypes = new Dictionary<string, FilterType>()
        {
            {"grayscale", FilterType.Grayscale},
            {"sepia", FilterType.Sepia },
            {"threshold", FilterType.Threshold }
        };

        public static RequestParams ParseUrl(string url)
        {
            var parts = url.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3 || parts[0] != "process")
                throw new InvalidOperationException("bad url address!");
            var filter = ParseFilter(parts[1]);
            var imagePieceCoords = ParseCoords(parts[2]);
            return new RequestParams(filter, imagePieceCoords);
        }

        private static ImagePieceCoords ParseCoords(string coords)
        {
            var intsParsed = new int[4];
            var numbersSplitted = coords.Split(',');
            for (int i = 0; i < numbersSplitted.Length; ++i)
                if (!int.TryParse(numbersSplitted[i], out intsParsed[i]))
                    throw new InvalidOperationException("bad coordinates!");
            var upperLeft = new Point(intsParsed[0], intsParsed[1]);
            return new ImagePieceCoords(upperLeft, intsParsed[2], intsParsed[3]);
        }

        private static FilterInfo ParseFilter(string filterStr)
        {
            var filterParam = filterStr.TrimEnd(')').Split('(');
            int filterParameter = 0;
            if (filterParam.Length > 0)
            {
                FilterType filterType;
                if (filterTypes.TryGetValue(filterParam[0], out filterType))
                {
                    if (filterType == FilterType.Threshold)
                        filterParameter = ParseThresholdParam(filterParam[1]);

                    return new FilterInfo(filterType, filterParameter);
                }
            }
            throw new InvalidOperationException("bad url!");
        }

        private static int ParseThresholdParam(string param)
        {
            int thresholdParam;
            if (!int.TryParse(param, out thresholdParam)
                || 0 > thresholdParam || thresholdParam > 100)
                throw new InvalidOperationException("bad threshold parameter!");
            return thresholdParam;
        }
    }
}
