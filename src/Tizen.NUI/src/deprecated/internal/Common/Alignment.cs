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
using Tizen.NUI.BaseComponents;

namespace Tizen.NUI
{
    [Obsolete("This class has been deprecated in API14.")]
    internal class Alignment : View
    {
        internal Alignment(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
        }

        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
        }

        public new class Padding : Disposable
        {
            internal Padding(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
            {
            }

            protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
            {
            }

            public Padding() : this(global::System.IntPtr.Zero, false)
            {
            }

            public Padding(float l, float r, float t, float b) : this(global::System.IntPtr.Zero, false)
            {
            }

            public float left { set; get; }

            public float right { set; get; }

            public float top { set; get; }

            public float bottom { set; get; }
        }

        public Alignment(Alignment.Type horizontal, Alignment.Type vertical) : this(global::System.IntPtr.Zero, false)
        {
        }

        public Alignment(Alignment.Type horizontal) : this(global::System.IntPtr.Zero, false)
        {
        }

        public Alignment() : this(global::System.IntPtr.Zero, false)
        {
        }

        public Alignment(Alignment alignment) : this(global::System.IntPtr.Zero, false)
        {
        }

        public static Alignment DownCast(BaseHandle handle)
        {
            Alignment ret = Registry.GetManagedBaseHandleFromNativePtr(handle) as Alignment;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        public void SetAlignmentType(Alignment.Type type)
        {
        }

        public Alignment.Type GetAlignmentType()
        {
            return Alignment.Type.HorizontalLeft;
        }

        public void SetScaling(Alignment.Scaling scaling)
        {
        }

        public Alignment.Scaling GetScaling()
        {
            return Alignment.Scaling.ScaleNone;
        }

        public void SetAlignmentPadding(Alignment.Padding padding)
        {
        }

        public Alignment.Padding GetAlignmentPadding()
        {
            return new Alignment.Padding();
        }

        public Alignment Assign(Alignment alignment)
        {
            return null;
        }

        public enum Type
        {
            HorizontalLeft = 1,
            HorizontalCenter = 2,
            HorizontalRight = 4,
            VerticalTop = 8,
            VerticalCenter = 16,
            VerticalBottom = 32
        }

        public enum Scaling
        {
            ScaleNone,
            ScaleToFill,
            ScaleToFitKeepAspect,
            ScaleToFillKeepAspect,
            ShrinkToFit,
            ShrinkToFitKeepAspect
        }
    }
}
