using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    class LatencyException : Exception
    {
        public LatencyException()
        {
        }

        public LatencyException(string message) : base(message)
        {
        }

        public LatencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LatencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
