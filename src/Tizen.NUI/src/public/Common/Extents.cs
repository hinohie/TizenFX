/*
* Copyright (c) 2019 Samsung Electronics Co., Ltd.
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
using Tizen.NUI.Binding;

namespace Tizen.NUI
{
    /// <summary>
    /// Extents class describing the a collection of uint16_t.
    /// </summary>
    /// <since_tizen> 4 </since_tizen>
    [Binding.TypeConverter(typeof(ExtentsTypeConverter))]
    public class Extents : Disposable, ICloneable
    {


        /// <summary>
        /// Default constructor of Extents class.
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        public Extents() : this(Interop.Extents.NewExtents(), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="copy">A reference to the copied Extents.</param>
        /// <since_tizen> 4 </since_tizen>
        public Extents(Extents copy) : this(Interop.Extents.NewExtents(Extents.getCPtr(copy)), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// The type cast operator, ushort to Extents.
        /// </summary>
        /// <param name="value">A value of ushort type.</param>
        /// <returns>return a Extents instance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static implicit operator Extents(ushort value)
        {
            return new Extents(value, value, value, value);
        }

        /// <summary>
        /// Constructor.
        /// <param name="start">The start extent value horizontally.</param>
        /// <param name="end">The end extent value horizontally.</param>
        /// <param name="top">The top extent value vertically.</param>
        /// <param name="bottom">The bottom extent value vertically.</param>
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        public Extents(ushort start, ushort end, ushort top, ushort bottom) : this(Interop.Extents.NewExtents(start, end, top, bottom), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal Extents(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="start">The start extent value horizontally.</param>
        /// <param name="end">The end extent value horizontally.</param>
        /// <param name="top">The top extent value vertically.</param>
        /// <param name="bottom">The bottom extent value vertically.</param>
        /// <since_tizen> Only used by Tizen.NUI.Components, will not be opened </since_tizen>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Extents(ExtentsChangedCallback cb, ushort start, ushort end, ushort top, ushort bottom) : this(Interop.Extents.NewExtents(start, end, top, bottom), true)
        {
            callback = cb;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Copy other extents
        /// </summary>
        /// <param name="that"></param>
        /// <exception cref="ArgumentNullException"> Thrown when that is null. </exception>
        /// <since_tizen> Only used by Tizen.NUI.Components, will not be opened </since_tizen>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void CopyFrom(Extents that)
        {
            if (null == that)
            {
                throw new ArgumentNullException(nameof(that));
            }
            Interop.Extents.StartSet(SwigCPtr, that.Start);
            Interop.Extents.EndSet(SwigCPtr, that.End);
            Interop.Extents.TopSet(SwigCPtr, that.Top);
            Interop.Extents.BottomSet(SwigCPtr, that.Bottom);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">The start extent value horizontally.</param>
        /// <param name="end">The end extent value horizontally.</param>
        /// <param name="top">The top extent value vertically.</param>
        /// <param name="bottom">The bottom extent value vertically.</param>
        /// <since_tizen> Only used by Tizen.NUI.Components, will not be opened </since_tizen>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public delegate void ExtentsChangedCallback(ushort start, ushort end, ushort top, ushort bottom);
        private ExtentsChangedCallback callback;

        /// <summary>
        /// The Start extent.
        /// </summary>
        /// <remarks>
        /// The setter is deprecated in API8 and will be removed in API10. Ue new Extents(...) constructor.
        /// </remarks>
        /// <code>
        /// // DO NOT use like the followings!
        /// Extents extents = new Extents();
        /// extents.Start = 1; 
        /// // USE like this
        /// ushort start = 1, end = 2, top = 3, bottom = 4;
        /// Extents extents = new Extents(start, end, top, bottom);
        /// </code>
        /// <since_tizen> 4 </since_tizen>
        public ushort Start
        {
            [Obsolete("Do not use this setter, that is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor")]
            set
            {
                Interop.Extents.StartSet(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                callback?.Invoke(value, End, Top, Bottom);
            }
            get
            {
                ushort ret = Interop.Extents.StartGet(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw new InvalidOperationException("FATAL: get Exception", NDalicPINVOKE.SWIGPendingException.Retrieve());
                return ret;
            }
        }

        /// <summary>
        /// The End extend.
        /// </summary>
        /// <remarks>
        /// The setter is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor.
        /// </remarks>
        /// <code>
        /// // DO NOT use like the followings!
        /// Extents extents = new Extents();
        /// extents.End = 2; 
        /// // USE like this
        /// ushort start = 1, end = 2, top = 3, bottom = 4;
        /// Extents extents = new Extents(start, end, top, bottom);
        /// </code>
        /// <since_tizen> 4 </since_tizen>
        public ushort End
        {
            [Obsolete("Do not use this setter, that is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor")]
            set
            {
                Interop.Extents.EndSet(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                callback?.Invoke(Start, value, Top, Bottom);
            }
            get
            {
                ushort ret = Interop.Extents.EndGet(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw new InvalidOperationException("FATAL: get Exception", NDalicPINVOKE.SWIGPendingException.Retrieve());
                return ret;
            }
        }

        /// <summary>
        /// The Top extend.
        /// </summary>
        /// <remarks>
        /// The setter is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor.
        /// </remarks>
        /// <code>
        /// // DO NOT use like the followings!
        /// Extents extents = new Extents();
        /// extents.Top = 3; 
        /// // USE like this
        /// ushort start = 1, end = 2, top = 3, bottom = 4;
        /// Extents extents = new Extents(start, end, top, bottom);
        /// </code>
        /// <since_tizen> 4 </since_tizen>
        public ushort Top
        {
            [Obsolete("Do not use this setter, that is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor")]
            set
            {
                Interop.Extents.TopSet(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                callback?.Invoke(Start, End, value, Bottom);
            }
            get
            {
                ushort ret = Interop.Extents.TopGet(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw new InvalidOperationException("FATAL: get Exception", NDalicPINVOKE.SWIGPendingException.Retrieve());
                return ret;
            }
        }

        /// <summary>
        /// The Bottom Extend.
        /// </summary>
        /// <remarks>
        /// The setter is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor.
        /// </remarks>
        /// <code>
        /// // DO NOT use like the followings!
        /// Extents extents = new Extents();
        /// extents.Bottom = 4; 
        /// // USE like this
        /// ushort start = 1, end = 2, top = 3, bottom = 4;
        /// Extents extents = new Extents(start, end, top, bottom);
        /// </code>
        /// <since_tizen> 4 </since_tizen>
        public ushort Bottom
        {
            [Obsolete("Do not use this setter, that is deprecated in API8 and will be removed in API10. Use new Extents(...) constructor")]
            set
            {
                Interop.Extents.BottomSet(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                callback?.Invoke(Start, End, Top, value);
            }
            get
            {
                ushort ret = Interop.Extents.BottomGet(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw new InvalidOperationException("FATAL: get Exception", NDalicPINVOKE.SWIGPendingException.Retrieve());
                return ret;
            }
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="rhs">The Extents to test against.</param>
        /// <returns>True if the extents are not equal.</returns>
        /// <since_tizen> 4 </since_tizen>
        public bool EqualTo(Extents rhs)
        {
            bool ret = Interop.Extents.EqualTo(SwigCPtr, Extents.getCPtr(rhs));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="rhs">The Extents to test against.</param>
        /// <returns>True if the extents are not equal.</returns>
        /// <since_tizen> 4 </since_tizen>
        public bool NotEqualTo(Extents rhs)
        {
            bool ret = Interop.Extents.NotEqualTo(SwigCPtr, Extents.getCPtr(rhs));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Clone() => new Extents(this);

        internal Extents Assign(Extents copy)
        {
            Extents ret = new Extents(Interop.Extents.Assign(SwigCPtr, Extents.getCPtr(copy)), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.Extents.DeleteExtents(swigCPtr);
        }
    }
}
