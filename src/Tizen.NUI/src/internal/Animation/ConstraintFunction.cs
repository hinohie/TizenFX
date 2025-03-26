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

using global::System;
using global::System.ComponentModel;
using global::System.Runtime.InteropServices;
namespace Tizen.NUI
{
    /// <summary>
    /// An abstract base class for Constraints.
    /// This class only use for inhouse currently.
    ///
    /// This can be used to constrain a property of an object, after animations have been applied.
    /// Constraints are applied in the following order:
    ///  - Constraints are applied to on-stage views in a depth-first traversal.
    ///  - For each view, the constraints are applied in the same order as the calls to Apply().
    ///
    /// Constraints are not applied to off-stage views.
    ///
    /// Create a constraint using one of the New method depending on the type of callback functions used.
    ///
    /// Note : This function will called every frame. Maybe reduce performance if you applyed too many constraints.
    ///
    /// TODO : AddSource(ConstraintSource); API need to be implemented.
    ///   To implement this, we have to bind ConstraintSource.
    /// TODO : Currently We don't support custom functions.
    ///   To implement this, we have to bind PropertyInputContainer
    /// </summary>
    internal class ConstraintFunction : Disposable
    {
        public ConstraintFunction(System.Delegate callback) : this(, true)
        {
        }

        internal ConstraintFunction(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="type"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if(disposed)
            {
                return;
            }
            if(type == DisposeTypes.Explicit)
            {
                //Called by User
                //Release your own managed resources here.
                //You should release all of your own disposable objects here.
            }
            base.Dispose(type);
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(HandleRef swigCPtr)
        {
            Interop.Constraint.DeleteConstraint(swigCPtr);
            NDalicPINVOKE.ThrowExceptionIfExists();
        }
    }
}
