using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;

namespace Kontur.ImageTransformer
{
    internal class AsyncHttpServer : IDisposable
    {
        public AsyncHttpServer()
        {
            listener = new HttpListener();
        }
        
        public void Start(string prefix)
        {
            lock (listener)
            {
                if (!isRunning)
                {
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(prefix);
                    listener.Start();

                    listenerThread = new Thread(Listen)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.Highest
                    };
                    listenerThread.Start();
                    
                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();
                
                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();
        }
        
        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.IsListening)
                    {
                        var context = listener.GetContext();
                        Task.Run(() => HandleContextAsync(context));
                    }
                    else Thread.Sleep(0);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
            }
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                if (declineRequests)
                    throw new LatencyException("overload!");
                if (listenerContext.Request.ContentLength64 > 100 * 1024)
                    throw new Exception("big image!");
                var requestParams = UrlParser.ParseUrl(listenerContext.Request.RawUrl);
                var filter = FilterBuilder.BuildFilterFromParams(requestParams);
                using (var bitMap = new Bitmap(listenerContext.Request.InputStream))
                {

                    var transformedImage = await ApplyFilterAsync(bitMap, filter);
                    listenerContext.Response.StatusCode = (int) HttpStatusCode.OK;
                    listenerContext.Response.ContentType = "image/png";
                    transformedImage.Save(listenerContext.Response.OutputStream, ImageFormat.Png);
                }
            }
            catch (LatencyException e)
            {
                listenerContext.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
            }
            catch (ContentException e)
            {
                listenerContext.Response.StatusCode = (int) HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                listenerContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            finally
            {
                listenerContext.Response.OutputStream.Close();
                RecalcRequestDeclining(watch.ElapsedMilliseconds);
            }
        }

        private static Task<Bitmap> ApplyFilterAsync(Bitmap bitmap, Func<Bitmap, Bitmap> filter)
        {
            return Task.Run(() => filter(bitmap));
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var percentile95 = Get95Percentile(latencies);
            Console.WriteLine("percentile : {0}, latencies.count : {1}", percentile95, latencies.Count);
            declineRequests = percentile95 > 1000;
        }

        private long Get95Percentile(List<long> latencyData)
        {
                if (latencyData.Count == 0)
                    return 0;
                int idx = latencyData.Count * 95 / 100;
                return latencyData.OrderBy(x => x).ElementAt(idx);
        }

        private void RecalcLatencies(long miliseconds)
        {
            if (latencies.Count > 300)
                latencies = latencies.GetRange(100, 200); //чтобы со временем память не заканчивалась
            latencies.Add(miliseconds);
        }

        private void RecalcRequestDeclining(long miliseconds)
        {
            lock (latencies)
            {
                RecalcLatencies(miliseconds);
                var percentile95 = Get95Percentile(latencies);
                Console.WriteLine(percentile95);
                declineRequests = percentile95 > 700;
            }
        }

        private readonly HttpListener listener;
        private bool declineRequests = false;
        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;
        private List<long> latencies = new List<long>();
    }
}