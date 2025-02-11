/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tizen.NUI
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class AsyncImageLoader : BaseHandle
    {
        private EventHandler<ImageLoadedEventArgs> imageLoadedEventHandler;
        private ImageLoadedSignalType imageLoadedCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImageLoadedSignalType(uint loadingTaskId, IntPtr pixelData);

        internal AsyncImageLoader(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.AsyncImageLoader.DeleteAsyncImageLoader(swigCPtr);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AsyncImageLoader() : this(Interop.AsyncImageLoader.New(), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint Load(string url)
        {
            uint ret = Interop.AsyncImageLoader.Load(SwigCPtr, url);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint Load(string url, Uint16Pair dimensions)
        {
            uint ret = Interop.AsyncImageLoader.Load(SwigCPtr, url, Uint16Pair.getCPtr(dimensions));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint Load(string url, Uint16Pair dimensions, FittingModeType fittingMode, SamplingModeType samplingMode, bool orientationCorrection)
        {
            uint ret = Interop.AsyncImageLoader.Load(SwigCPtr, url, Uint16Pair.getCPtr(dimensions), (int)fittingMode, (int)samplingMode, orientationCorrection);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Cancel(uint loadingTaskId)
        {
            bool ret = Interop.AsyncImageLoader.Cancel(SwigCPtr, loadingTaskId);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CancelAll()
        {
            Interop.AsyncImageLoader.CancelAll(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// You can override it to clean-up your own resources
        /// </summary>
        /// <param name="type">DisposeTypes</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                //Called by User
                //Release your own managed resources here.
                //You should release all of your own disposable objects here.

                if (imageLoadedCallback != null)
                {
                    // TODO : Implement here!
                    NDalicPINVOKE.ThrowExceptionIfExists();
                    imageLoadedCallback = null;
                }
            }

            //Release your own unmanaged resources here.
            //You should not access any managed member here except static instance.
            //because the execution order of Finalizes is non-deterministic.
            CancelAll();

            base.Dispose(type);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<ImageLoadedEventArgs> ImageLoaded
        {
            add
            {
                if (imageLoadedEventHandler == null)
                {
                    imageLoadedCallback = OnImageLoaded;
                    // TODO : Implement here!
                    NDalicPINVOKE.ThrowExceptionIfExists();
                }
                imageLoadedEventHandler += value;
            }

            remove
            {
                imageLoadedEventHandler -= value;
                if (imageLoadedEventHandler == null && imageLoadedCallback != null)
                {
                    // TODO : Implement here!
                    NDalicPINVOKE.ThrowExceptionIfExists();
                    imageLoadedCallback = null;
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class ImageLoadedEventArgs : EventArgs
        {
            public uint LoadingTaskId { get; internal set; }
            public PixelData PixelData { get; internal set; }
        }

        private void OnImageLoaded(uint loadingTaskId, IntPtr pixelData)
        {
            ImageLoadedEventArgs e = new ImageLoadedEventArgs();
            e.LoadingTaskId = loadingTaskId;
            e.PixelData = pixelData != IntPtr.Zero ? new PixelData(pixelData, false) : null;

            imageLoadedEventHandler?.Invoke(this, e);
        }
    }
}
