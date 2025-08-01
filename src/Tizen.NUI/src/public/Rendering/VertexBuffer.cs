/*
 * Copyright(c) 2025 Samsung Electronics Co., Ltd.
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
    /// <summary>
    /// VertexBuffer is a handle to an object that contains a buffer of structured data.<br />
    /// VertexBuffers can be used to provide data to Geometry objects.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    public partial class VertexBuffer : BaseHandle
    {
        private static int aliveCount;

        /// <summary>
        /// The constructor to creates a VertexBuffer.
        /// </summary>
        /// <param name="bufferFormat">The map of names and types that describes the components of the buffer.</param>
        /// <since_tizen> 8 </since_tizen>
        public VertexBuffer(PropertyMap bufferFormat) : this(Interop.VertexBuffer.New(PropertyMap.getCPtr(bufferFormat)), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending)
                throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal VertexBuffer(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
            ++aliveCount;
        }

        /// <summary>
        /// Gets the number of elements in the buffer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint Size
        {
            get => RetrieveSize();
        }

        /// <summary>
        /// Gets the number of currently alived VertexBuffer object.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int AliveCount => aliveCount;

        /// <summary>
        /// Updates the whole buffer information.<br />
        /// This function expects an array of structures with the same format that was given in the construction.
        /// </summary>
        /// <param name="vertices">The vertex data that will be copied to the buffer.</param>
        /// <exception cref="ArgumentNullException"> Thrown when vertices is null or length of the vertices is 0. </exception>
        /// <exception cref="OverflowException"> Thrown when length of the vertices is overflow. </exception>
        /// <since_tizen> 8 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715: Identifiers should have correct prefix")]
        public void SetData<VertexType>(VertexType[] vertices) where VertexType : struct
        {
            if (null == vertices || vertices.Length == 0)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            int structSize = Marshal.SizeOf<VertexType>();
            global::System.IntPtr buffer = Marshal.AllocHGlobal(checked(structSize * vertices.Length));

            try
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Marshal.StructureToPtr(vertices[i], buffer + i * structSize, true);
                }

                Interop.VertexBuffer.SetData(SwigCPtr, buffer, (uint)vertices.Length);
            }
            finally
            {
                // Free AllocHGlobal memory after call Interop.VertexBuffer.SetData()
                Marshal.FreeHGlobal(buffer);
            }

            if (NDalicPINVOKE.SWIGPendingException.Pending)
                throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Gets the number of elements in the buffer.
        /// </summary>
        /// <returns>Number of elements in the buffer.</returns>
        private uint RetrieveSize()
        {
            uint ret = Interop.VertexBuffer.GetSize(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending)
                throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            --aliveCount;

            base.Dispose(type);
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.VertexBuffer.DeleteVertexBuffer(swigCPtr);
        }
    }
}
