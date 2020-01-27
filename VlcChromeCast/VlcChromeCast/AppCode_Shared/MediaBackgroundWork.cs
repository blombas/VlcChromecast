using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace VlcChromeCast.AppCode_Shared
{
    public class MediaBackgroundWork
    {
        public async Task LogMediaState(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await StartListening();
                }
                catch
                {
                }
                finally
                {
                    if (token.IsCancellationRequested)
                        await StopListening();
                }
            }, token);
        }
        //start listening location
        private async Task StartListening()
        {
            
        }
        private async Task StopListening()
        { }
    }
}
