using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public class ContentException : Exception
    {
        public ContentException()
        {
        }

        public ContentException(string message) : base(message)
        {
        }

        public ContentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
