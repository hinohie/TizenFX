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

extern alias TizenSystemInformation;
using TizenSystemInformation.Tizen.System;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Tizen.NUI.BaseComponents;
using Tizen.Common;

namespace Tizen.NUI
{
    /// <summary>
    /// The window class is used internally for drawing.<br />
    /// The window has an orientation and indicator properties.<br />
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public partial class Window : BaseHandle, IWindowProvider
    {
        private HandleRef stageCPtr;
        private Layer rootLayer;
        private Layer overlayLayer;
        private Layer borderLayer;
        private string windowTitle;
        private List<Layer> childLayers = new List<Layer>();
        private LayoutController localController;
        private Key internalLastKeyEvent;
        private Touch internalLastTouchEvent;
        private Hover internalLastHoverEvent;
        private Timer internalHoverTimer;

        private static int aliveCount;

        static internal bool IsSupportedMultiWindow()
        {
            bool isSupported = false;
            try
            {
                Information.TryGetValue("http://tizen.org/feature/opengles.surfaceless_context", out isSupported);
            }
            catch (DllNotFoundException e)
            {
                Tizen.Log.Fatal("NUI", $"{e}\n");
            }
            return isSupported;
        }

        internal Window(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
            if (Interop.Stage.IsInstalled())
            {
                stageCPtr = new global::System.Runtime.InteropServices.HandleRef(this, Interop.Stage.GetCurrent());

                localController = new LayoutController(this);
                NUILog.Debug("layoutController id:" + localController.GetId());
            }

            ++aliveCount;
        }

        /// <summary>
        /// A helper method to get the current window where the view is added
        /// </summary>
        /// <param name="view">The View added to the window</param>
        /// <returns>A Window.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        static public Window Get(View view)
        {
            if (view == null)
            {
                NUILog.Error("if there is no view, it can not get a window");
                return null;
            }

            //to fix memory leak issue, match the handle count with native side.
            Window ret = view.GetInstanceSafely<Window>(Interop.Window.Get(View.getCPtr(view)));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Creates a new Window.<br />
        /// This creates an extra window in addition to the default main window<br />
        /// </summary>
        /// <param name="windowPosition">The position and size of the Window.</param>
        /// <param name="isTranslucent">Whether Window is translucent.</param>
        /// <returns>A new Window.</returns>
        /// <since_tizen> 6 </since_tizen>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        public Window(Rectangle windowPosition = null, bool isTranslucent = false) : this(Interop.Window.New(Rectangle.getCPtr(windowPosition), "", isTranslucent), true)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Creates a new Window with a specific name.<br />
        /// This creates an extra window in addition to the default main window<br />
        /// </summary>
        /// <param name="name">The name for extra window. </param>
        /// <param name="windowPosition">The position and size of the Window.</param>
        /// <param name="isTranslucent">Whether Window is translucent.</param>
        /// <returns>A new Window.</returns>
        /// <since_tizen> 6 </since_tizen>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        public Window(string name, Rectangle windowPosition = null, bool isTranslucent = false) : this(Interop.Window.New(Rectangle.getCPtr(windowPosition), name, isTranslucent), true)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            this.windowTitle = name;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Creates a new Window with a specific name.<br />
        /// This creates an extra window in addition to the default main window<br />
        /// </summary>
        /// <param name="name">The name for extra window. </param>
        /// <param name="borderInterface"><see cref="Tizen.NUI.IBorderInterface"/>If borderInterface is null, defaultBorder is enabled.</param>
        /// <param name="windowPosition">The position and size of the Window.</param>
        /// <param name="isTranslucent">Whether Window is translucent.</param>
        /// <returns>A new Window.</returns>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Window(string name, IBorderInterface borderInterface, Rectangle windowPosition = null, bool isTranslucent = false) : this(Interop.Window.New(Rectangle.getCPtr(windowPosition), name, isTranslucent), true)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            this.windowTitle = name;
            this.EnableBorder(borderInterface);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Creates a new Window with a specific name using WindowData.<br />
        /// This creates an extra window in addition to the default main window<br />
        /// </summary>
        /// <param name="name">The name for extra window. </param>
        /// <param name="windowData">The window data</param>
        /// <returns>A new Window.</returns>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Window(string name, WindowData windowData) : this(Interop.Window.New(name, "", WindowData.getCPtr(windowData)), true)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            this.windowTitle = name;
            this.EnableBorder(windowData.BorderInterface);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }



        /// <summary>
        /// Enumeration for orientation of the window is the way in which a rectangular page is oriented for normal viewing.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public enum WindowOrientation
        {
            /// <summary>
            /// Portrait orientation. The height of the display area is greater than the width.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            Portrait = 0,
            /// <summary>
            /// Landscape orientation. A wide view area is needed.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            Landscape = 90,
            /// <summary>
            /// Portrait inverse orientation.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            PortraitInverse = 180,
            /// <summary>
            /// Landscape inverse orientation.
            /// </summary>
            /// <since_tizen> 3 </since_tizen>
            LandscapeInverse = 270,
            /// <summary>
            /// No orientation. It is for the preferred orientation
            /// Especially, NoOrientationPreference only has the effect for the preferred orientation.
            /// It is used to unset the preferred orientation with SetPreferredOrientation.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            NoOrientationPreference = -1
        }

        /// <summary>
        /// Enumeration for the key grab mode for platform-level APIs.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public enum KeyGrabMode
        {
            /// <summary>
            /// Grabs a key only when on the top of the grabbing-window stack mode.
            /// </summary>
            Topmost = 0,
            /// <summary>
            /// Grabs a key together with the other client window(s) mode.
            /// </summary>
            Shared,
            /// <summary>
            /// Grabs a key exclusively regardless of the grabbing-window's position on the window stack with the possibility of overriding the grab by the other client window mode.
            /// </summary>
            OverrideExclusive,
            /// <summary>
            /// Grabs a key exclusively regardless of the grabbing-window's position on the window stack mode.
            /// </summary>
            Exclusive
        };

        /// <summary>
        /// Enumeration for transition effect's state.
        /// </summary>
        [Obsolete("Do not use this, that will be removed. Use Window.EffectState instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        //  This is already deprecated, so suppress warning here.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1717:Only FlagsAttribute enums should have plural names", Justification = "<Pending>")]
        public enum EffectStates
        {
            /// <summary>
            /// None state.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectState.None instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,
            /// <summary>
            /// Transition effect is started.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectState.Start instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            Start,
            /// <summary>
            /// Transition effect is ended.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectState.End instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            End,
        }

        /// <summary>
        /// Enumeration for transition effect's state.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum EffectState
        {
            /// <summary>
            /// None state.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,
            /// <summary>
            /// Transition effect is started.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Start,
            /// <summary>
            /// Transition effect is ended.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            End,
        }

        /// <summary>
        /// Enumeration for transition effect's type.
        /// </summary>
        [Obsolete("Do not use this, that will be removed. Use Window.EffectType instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        //  This is already deprecated, so suppress warning here.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1717:Only FlagsAttribute enums should have plural names", Justification = "<Pending>")]
        public enum EffectTypes
        {
            /// <summary>
            /// None type.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectType.None instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,
            /// <summary>
            /// Window show effect.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectType.Show instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            Show,
            /// <summary>
            /// Window hide effect.
            /// </summary>
            [Obsolete("Do not use this, that will be removed. Use Window.EffectType.Hide instead.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            Hide,
        }

        /// <summary>
        /// Enumeration for transition effect's type.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum EffectType
        {
            /// <summary>
            /// None type.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,
            /// <summary>
            /// Window show effect.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Show,
            /// <summary>
            /// Window hide effect.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Hide,
        }

        /// <summary>
        /// Enumeration for result of window operation.
        /// </summary>
        internal enum OperationResult
        {
            /// <summary>
            /// Failed for unknown reason
            /// </summary>
            UnknownError = 0,
            /// <summary>
            /// Succeed
            /// </summary>
            Succeed,
            /// <summary>
            /// Permission denied
            /// </summary>
            PermissionDenied,
            /// <summary>
            /// The operation is not supported.
            /// </summary>
            NotSupported,
        }

        /// <summary>
        /// Enumeration for window resized mode by display server.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum ResizeDirection
        {
            /// <summary>
            /// None type.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,
            /// <summary>
            /// Start resizing window to the top-left edge.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            TopLeft = 1,
            /// <summary>
            /// Start resizing window to the top side.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Top = 2,
            /// <summary>
            /// Start resizing window to the top-right edge.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            TopRight = 3,
            /// <summary>
            /// Start resizing window to the left side.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Left = 4,
            /// <summary>
            /// Start resizing window to the right side.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Right = 5,
            /// <summary>
            /// Start resizing window to the bottom-left edge.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            BottomLeft = 6,
            /// <summary>
            /// Start resizing window to the bottom side.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Bottom = 7,
            /// <summary>
            /// Start resizing window to the bottom-right edge.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            BottomRight = 8,
        }

        /// <summary>
        /// The Pointer edge boundary for grab.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum PointerBoundary
        {
            /// <summary>
            /// Default value
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            None = 0,

            /// <summary>
            /// Top
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Top = 1,

            /// <summary>
            /// Right
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Right = 2,

            /// <summary>
            /// Bottom
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Bottom = 3,

            /// <summary>
            /// Left
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            Left = 4,

        }

        /// <summary>
        /// The stage instance property (read-only).<br />
        /// Gets the current window.<br />
        /// </summary>
        [Obsolete("This has been deprecated in API12, please use Default instead")]
        public static Window Instance { get; internal set; }

        /// <summary>
        /// Gets the default window.
        /// The main window or default window is automatically created when the application is launched,
        /// and it remains constant and unchanged throughout the application's operation.
        /// </summary>
        /// <since_tizen> 12 </since_tizen>
        public static Window Default { get; internal set; }

        /// <summary>
        /// Gets or sets a window type.
        /// Most of window type can be set to use WindowType, except for IME type.
        /// IME type can be set to use one of NUIApplication's constrcutors.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public WindowType Type
        {
            get
            {
                WindowType ret = (WindowType)Interop.Window.GetType(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw new InvalidOperationException("FATAL: get Exception", NDalicPINVOKE.SWIGPendingException.Retrieve());
                return ret;
            }
            set
            {
                Interop.Window.SetType(SwigCPtr, (int)value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }
        }

        /// <summary>
        /// Gets/Sets a window title.
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        public string Title
        {
            get
            {
                return windowTitle;
            }
            set
            {
                windowTitle = value;
                SetClass(windowTitle, "");
            }
        }

        /// <summary>
        /// The rendering behavior of a Window.
        /// </summary>
        /// <since_tizen> 5 </since_tizen>
        public RenderingBehaviorType RenderingBehavior
        {
            get
            {
                return GetRenderingBehavior();
            }
            set
            {
                SetRenderingBehavior(value);
            }
        }

        /// <summary>
        /// The window size property (read-only).
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public Size2D Size
        {
            get
            {
                Size2D ret = GetSize();
                return ret;
            }
        }

        /// <summary>
        /// The background color property.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public Color BackgroundColor
        {
            set
            {
                SetBackgroundColor(value);
            }
            get
            {
                Color ret = GetBackgroundColor();
                return ret;
            }
        }

        /// <summary>
        /// The DPI property (read-only).<br />
        /// Retrieves the DPI of the display device to which the Window is connected.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public Vector2 Dpi
        {
            get
            {
                return GetDpi();
            }
        }

        /// <summary>
        /// The layer count property (read-only).<br />
        /// Queries the number of on-Window layers.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public uint LayerCount
        {
            get
            {
                return GetLayerCount();
            }
        }

        /// <summary>
        /// Gets or sets a size of the window.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when value is null. </exception>
        /// <since_tizen> 4 </since_tizen>
        public Size2D WindowSize
        {
            get
            {
                return GetWindowSize();
            }
            set
            {
                SetWindowSize(value);
            }
        }

        /// <summary>
        /// Gets or sets a position of the window.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when value is null. </exception>
        /// <since_tizen> 4 </since_tizen>
        public Position2D WindowPosition
        {
            get
            {
                return GetPosition();
            }
            set
            {
                SetPosition(value);
            }
        }

        /// <summary>
        /// Sets position and size of the window. This API guarantees that
        /// both moving and resizing of window will appear on the screen at once.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle WindowPositionSize
        {
            get
            {
                Position2D position = GetPosition();
                Size2D size = GetSize();
                Rectangle ret = new Rectangle(position?.X ?? 0, position?.Y ?? 0, size?.Width ?? 0, size?.Height ?? 0);
                position.Dispose();
                return ret;
            }
            set
            {
                SetPositionSize(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the window will update partial area or full area.
        /// If this value is true, window will update and render partial area.
        /// If false, full area updated.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool PartialUpdate
        {
            get
            {
                return IsPartialUpdate();
            }
            set
            {
                SetPartialUpdate(value);
            }
        }

        /// <summary>
        /// Gets the number of currently alived Window object.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int AliveCount => aliveCount;

        internal List<Layer> LayersChildren
        {
            get
            {
                return childLayers;
            }
        }

        /// <summary>
        ///  Get the LayoutController for this Window.
        /// </summary>
        internal LayoutController LayoutController
        {
            get
            {
                return localController;
            }
        }

        /// <summary>
        /// Feed a key-event into the window.
        /// </summary>
        /// <param name="keyEvent">The key event to feed.</param>
        /// <since_tizen> 4 </since_tizen>
        [Obsolete("Do not use this, that will be deprecated. Use FeedKey(Key keyEvent) instead.")]
        public static void FeedKeyEvent(Key keyEvent)
        {
            Interop.Window.FeedKeyEvent(Key.getCPtr(keyEvent));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets whether the window accepts a focus or not.
        /// </summary>
        /// <param name="accept">If a focus is accepted or not. The default is true.</param>
        /// <since_tizen> 3 </since_tizen>
        public void SetAcceptFocus(bool accept)
        {
            Interop.Window.SetAcceptFocus(SwigCPtr, accept);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether the window accepts a focus or not.
        /// </summary>
        /// <returns>True if the window accepts a focus, false otherwise.</returns>
        /// <since_tizen> 3 </since_tizen>
        public bool IsFocusAcceptable()
        {
            bool ret = Interop.Window.IsFocusAcceptable(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

            return ret;
        }

        /// <summary>
        /// Shows the window if it is hidden.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public void Show()
        {
            Interop.Window.Show(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Hides the window if it is showing.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public void Hide()
        {
            Interop.Window.Hide(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Retrieves whether the window is visible or not.
        /// </summary>
        /// <returns>True if the window is visible.</returns>
        /// <since_tizen> 3 </since_tizen>
        public bool IsVisible()
        {
            bool temp = Interop.Window.IsVisible(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return temp;
        }

        /// <summary>
        /// Gets the count of supported auxiliary hints of the window.
        /// </summary>
        /// <returns>The number of supported auxiliary hints.</returns>
        /// <since_tizen> 3 </since_tizen>
        public uint GetSupportedAuxiliaryHintCount()
        {
            uint ret = Interop.Window.GetSupportedAuxiliaryHintCount(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets the supported auxiliary hint string of the window.
        /// </summary>
        /// <param name="index">The index of the supported auxiliary hint lists.</param>
        /// <returns>The auxiliary hint string of the index.</returns>
        /// <since_tizen> 3 </since_tizen>
        public string GetSupportedAuxiliaryHint(uint index)
        {
            string ret = Interop.Window.GetSupportedAuxiliaryHint(SwigCPtr, index);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Creates an auxiliary hint of the window.
        /// </summary>
        /// <param name="hint">The auxiliary hint string.</param>
        /// <param name="value">The value string.</param>
        /// <returns>The ID of created auxiliary hint, or 0 on failure.</returns>
        /// <since_tizen> 3 </since_tizen>
        public uint AddAuxiliaryHint(string hint, string value)
        {
            uint ret = Interop.Window.AddAuxiliaryHint(SwigCPtr, hint, value);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Removes an auxiliary hint of the window.
        /// </summary>
        /// <param name="id">The ID of the auxiliary hint.</param>
        /// <returns>True if no error occurred, false otherwise.</returns>
        /// <since_tizen> 3 </since_tizen>
        public bool RemoveAuxiliaryHint(uint id)
        {
            bool ret = Interop.Window.RemoveAuxiliaryHint(SwigCPtr, id);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Changes a value of the auxiliary hint.
        /// </summary>
        /// <param name="id">The auxiliary hint ID.</param>
        /// <param name="value">The value string to be set.</param>
        /// <returns>True if no error occurred, false otherwise.</returns>
        /// <since_tizen> 3 </since_tizen>
        public bool SetAuxiliaryHintValue(uint id, string value)
        {
            bool ret = Interop.Window.SetAuxiliaryHintValue(SwigCPtr, id, value);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets a value of the auxiliary hint.
        /// </summary>
        /// <param name="id">The auxiliary hint ID.</param>
        /// <returns>The string value of the auxiliary hint ID, or an empty string if none exists.</returns>
        /// <since_tizen> 3 </since_tizen>
        public string GetAuxiliaryHintValue(uint id)
        {
            string ret = Interop.Window.GetAuxiliaryHintValue(SwigCPtr, id);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets an ID of the auxiliary hint string.
        /// </summary>
        /// <param name="hint">The auxiliary hint string.</param>
        /// <returns>The ID of auxiliary hint string, or 0 on failure.</returns>
        /// <since_tizen> 3 </since_tizen>
        public uint GetAuxiliaryHintId(string hint)
        {
            uint ret = Interop.Window.GetAuxiliaryHintId(SwigCPtr, hint);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets a region to accept input events.
        /// </summary>
        /// <param name="inputRegion">The region to accept input events.</param>
        /// <since_tizen> 3 </since_tizen>
        public void SetInputRegion(Rectangle inputRegion)
        {
            Interop.Window.SetInputRegion(SwigCPtr, Rectangle.getCPtr(inputRegion));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets a priority level for the specified notification window.
        /// </summary>
        /// <param name="level">The notification window level.</param>
        /// <returns>True if no error occurred, false otherwise.</returns>
        /// <privilege>http://tizen.org/privilege/window.priority.set</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool SetNotificationLevel(NotificationLevel level)
        {
            var ret = (OperationResult)Interop.Window.SetNotificationLevel(SwigCPtr, (int)level);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret == OperationResult.Succeed;
        }

        /// <summary>
        /// Gets a priority level for the specified notification window.
        /// </summary>
        /// <returns>The notification window level.</returns>
        /// <since_tizen> 3 </since_tizen>
        public NotificationLevel GetNotificationLevel()
        {
            NotificationLevel ret = (NotificationLevel)Interop.Window.GetNotificationLevel(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets a transparent window's visual state to opaque. <br />
        /// If a visual state of a transparent window is opaque, <br />
        /// then the window manager could handle it as an opaque window when calculating visibility.
        /// </summary>
        /// <param name="opaque">Whether the window's visual state is opaque.</param>
        /// <remarks>This will have no effect on an opaque window. <br />
        /// It doesn't change transparent window to opaque window but lets the window manager know the visual state of the window.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public void SetOpaqueState(bool opaque)
        {
            Interop.Window.SetOpaqueState(SwigCPtr, opaque);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether a transparent window's visual state is opaque or not.
        /// </summary>
        /// <returns>True if the window's visual state is opaque, false otherwise.</returns>
        /// <remarks> The return value has no meaning on an opaque window. </remarks>
        /// <since_tizen> 3 </since_tizen>
        public bool IsOpaqueState()
        {
            bool ret = Interop.Window.IsOpaqueState(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets a window's screen off mode.
        /// </summary>
        /// <param name="screenOffMode">The screen mode.</param>
        /// <returns>True if no error occurred, false otherwise.</returns>
        /// <privilege>http://tizen.org/privilege/display</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 4 </since_tizen>
        public bool SetScreenOffMode(ScreenOffMode screenOffMode)
        {
            var ret = (OperationResult)Interop.Window.SetScreenOffMode(SwigCPtr, (int)screenOffMode);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret == OperationResult.Succeed;
        }

        /// <summary>
        /// Gets the screen mode of the window.
        /// </summary>
        /// <returns>The screen off mode.</returns>
        /// <since_tizen> 4 </since_tizen>
        public ScreenOffMode GetScreenOffMode()
        {
            ScreenOffMode ret = (ScreenOffMode)Interop.Window.GetScreenOffMode(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets preferred brightness of the window.
        /// </summary>
        /// <param name="brightness">The preferred brightness (0 to 100).</param>
        /// <returns>True if no error occurred, false otherwise.</returns>
        /// <privilege>http://tizen.org/privilege/display</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool SetBrightness(int brightness)
        {
            var ret = (OperationResult)Interop.Window.SetBrightness(SwigCPtr, brightness);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret == OperationResult.Succeed;
        }

        /// <summary>
        /// Gets the preferred brightness of the window.
        /// </summary>
        /// <returns>The preferred brightness.</returns>
        /// <since_tizen> 3 </since_tizen>
        public int GetBrightness()
        {
            int ret = Interop.Window.GetBrightness(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the window name and the class string.
        /// </summary>
        /// <param name="name">The name of the window.</param>
        /// <param name="klass">The class of the window.</param>
        /// <since_tizen> 4 </since_tizen>
        public void SetClass(string name, string klass)
        {
            Interop.Window.SetClass(SwigCPtr, name, klass);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Raises the window to the top of the window stack.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Method used to raise the object, not event")]
        public void Raise()
        {
            Interop.Window.Raise(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Lowers the window to the bottom of the window stack.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public void Lower()
        {
            Interop.Window.Lower(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Activates the window to the top of the window stack even it is iconified.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public void Activate()
        {
            Interop.Window.Activate(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Gets the default ( root ) layer.
        /// </summary>
        /// <returns>The root layer.</returns>
        /// <since_tizen> 3 </since_tizen>
        public Layer GetDefaultLayer()
        {
            return this.GetRootLayer();
        }

        /// <summary>
        /// Gets the overlay layer.
        /// </summary>
        /// <returns>The overlay layer.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Layer GetOverlayLayer()
        {
            // Window.IsInstalled() is actually true only when called from event thread and
            // Core has been initialized, not when Stage is ready.
            if (overlayLayer == null && Window.IsInstalled())
            {
                overlayLayer = new Layer(Interop.Window.GetOverlayLayer(SwigCPtr), true);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                LayersChildren?.Add(overlayLayer);
                overlayLayer.SetWindow(this);
            }
            return overlayLayer;
        }

        /// <summary>
        /// Add a child view to window.
        /// </summary>
        /// <param name="view">the child should be added to the window.</param>
        /// <since_tizen> 3 </since_tizen>
        public void Add(View view)
        {
            this.GetRootLayer().Add(view);
        }

        /// <summary>
        /// Remove a child view from window.
        /// </summary>
        /// <param name="view">the child to be removed.</param>
        /// <since_tizen> 3 </since_tizen>
        public void Remove(View view)
        {
            this.GetRootLayer().Remove(view);
        }

        /// <summary>
        /// Retrieves the layer at a specified depth.
        /// </summary>
        /// <param name="depth">The layer's depth index.</param>
        /// <returns>The layer found at the given depth.</returns>
        /// <since_tizen> 3 </since_tizen>
        public Layer GetLayer(uint depth)
        {
            if (depth < LayersChildren?.Count)
            {
                Layer ret = LayersChildren?[Convert.ToInt32(depth)];
                return ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Destroy the window immediately.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Destroy()
        {
            this.Dispose();
        }

        /// <summary>
        /// Keep rendering for at least the given amount of time.
        /// </summary>
        /// <param name="durationSeconds">Time to keep rendering, 0 means render at least one more frame.</param>
        /// <since_tizen> 3 </since_tizen>
        public void KeepRendering(float durationSeconds)
        {
            Interop.Window.KeepRendering(SwigCPtr, durationSeconds);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Grabs the key specified by a key for a window only when a window is the topmost window.<br />
        /// This function can be used for following example scenarios: <br />
        /// - Mobile - Using volume up or down as zoom up or down in camera apps.<br />
        /// </summary>
        /// <param name="DaliKey">The key code to grab.</param>
        /// <returns>True if the grab succeeds.</returns>
        /// <privilege>http://tizen.org/privilege/keygrab</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool GrabKeyTopmost(int DaliKey)
        {
            bool ret = Interop.Window.GrabKeyTopmost(HandleRef.ToIntPtr(this.SwigCPtr), DaliKey);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Ungrabs the key specified by a key for the window.<br />
        /// Note: If this function is called between key down and up events of a grabbed key, an application doesn't receive the key up event.<br />
        /// </summary>
        /// <param name="DaliKey">The key code to ungrab.</param>
        /// <returns>True if the ungrab succeeds.</returns>
        /// <privilege>http://tizen.org/privilege/keygrab</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool UngrabKeyTopmost(int DaliKey)
        {
            bool ret = Interop.Window.UngrabKeyTopmost(HandleRef.ToIntPtr(this.SwigCPtr), DaliKey);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        ///  Grabs the key specified by a key for a window in a GrabMode. <br />
        ///  Details: This function can be used for following example scenarios: <br />
        ///  - TV - A user might want to change the volume or channel of the background TV contents while focusing on the foregrund app. <br />
        ///  - Mobile - When a user presses the Home key, the homescreen appears regardless of the current foreground app. <br />
        ///  - Mobile - Using the volume up or down as zoom up or down in camera apps. <br />
        /// </summary>
        /// <param name="DaliKey">The key code to grab.</param>
        /// <param name="GrabMode">The grab mode for the key.</param>
        /// <returns>True if the grab succeeds.</returns>
        /// <privilege>http://tizen.org/privilege/keygrab</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool GrabKey(int DaliKey, KeyGrabMode GrabMode)
        {
            bool ret = Interop.Window.GrabKey(HandleRef.ToIntPtr(this.SwigCPtr), DaliKey, (int)GrabMode);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Ungrabs the key specified by a key for a window.<br />
        /// Note: If this function is called between key down and up events of a grabbed key, an application doesn't receive the key up event. <br />
        /// </summary>
        /// <param name="DaliKey">The key code to ungrab.</param>
        /// <returns>True if the ungrab succeeds.</returns>
        /// <privilege>http://tizen.org/privilege/keygrab</privilege>
        /// <exception cref="UnauthorizedAccessException">This exception can be thrown due to permission denied.</exception>
        /// <since_tizen> 3 </since_tizen>
        public bool UngrabKey(int DaliKey)
        {
            bool ret = Interop.Window.UngrabKey(HandleRef.ToIntPtr(this.SwigCPtr), DaliKey);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the keyboard repeat information.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        /// <since_tizen> 5 </since_tizen>
        public bool SetKeyboardRepeatInfo(float rate, float delay)
        {
            bool ret = Interop.Window.SetKeyboardRepeatInfo(rate, delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets the keyboard repeat information.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        /// <since_tizen> 5 </since_tizen>
        public bool GetKeyboardRepeatInfo(out float rate, out float delay)
        {
            bool ret = Interop.Window.GetKeyboardRepeatInfo(out rate, out delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the keyboard repeat information of horizontal way.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SetKeyboardHorizentalRepeatInfo(float rate, float delay)
        {
            bool ret = Interop.Window.SetKeyboardHorizentalRepeatInfo(rate, delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets the keyboard repeat information of horizontal way.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool GetKeyboardHorizentalRepeatInfo(out float rate, out float delay)
        {
            bool ret = Interop.Window.GetKeyboardHorizentalRepeatInfo(out rate, out delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the keyboard repeat information of vertical way.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SetKeyboardVerticalRepeatInfo(float rate, float delay)
        {
            bool ret = Interop.Window.SetKeyboardVerticalRepeatInfo(rate, delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets the keyboard repeat information of vertical way.
        /// </summary>
        /// <param name="rate">The key repeat rate value in seconds.</param>
        /// <param name="delay">The key repeat delay value in seconds.</param>
        /// <returns>True if setting the keyboard repeat succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool GetKeyboardVerticalRepeatInfo(out float rate, out float delay)
        {
            bool ret = Interop.Window.GetKeyboardVerticalRepeatInfo(out rate, out delay);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Adds a layer to the stage.
        /// </summary>
        /// <param name="layer">Layer to add.</param>
        /// <exception cref="ArgumentNullException"> Thrown when layer is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public void AddLayer(Layer layer)
        {
            Add(layer);
        }

        /// <summary>
        /// Removes a layer from the stage.
        /// </summary>
        /// <param name="layer">Layer to remove.</param>
        /// <exception cref="ArgumentNullException"> Thrown when layer is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public void RemoveLayer(Layer layer)
        {
            Remove(layer);
        }

        /// <summary>
        /// Feeds a key event into the window.
        /// </summary>
        /// <param name="keyEvent">The key event to feed.</param>
        /// <since_tizen> 5 </since_tizen>
        public void FeedKey(Key keyEvent)
        {
            Interop.Window.FeedKeyEvent(SwigCPtr, Key.getCPtr(keyEvent));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Feeds a hover event into the window. <br />
        /// This is feed after a default time of 48 ms. You can also set this time.
        /// </summary>
        /// <param name="time">The time of how much later it will be feed (default is 48ms)</param>
        /// <remarks>If you want to do FeedHover after the UI is updated, it is recommended to set the time to at least 16ms. This will be a good time waiting for the UI to update.<br />
        /// and LazyFeedHover called within the set time are ignored. Only the last request becomes a FeedHover.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void LazyFeedHover(uint time = 48)
        {
            if (internalHoverTimer == null)
            {
                internalHoverTimer = new Timer(time);
                internalHoverTimer.Tick += (s, e) =>
                {
                    FeedHover();
                    internalHoverTimer?.Stop();
                    internalHoverTimer?.Dispose();
                    internalHoverTimer = null;
                    return false;
                };
                internalHoverTimer.Start();
            }
            else
            {
                internalHoverTimer.Start();
            }
        }

        /// <summary>
        /// Feeds a touch point into the window.
        /// </summary>
        /// <param name="touchPoint">The touch point to feed.</param>
        /// <param name="timeStamp">The timeStamp.</param>
        internal void FeedTouch(TouchPoint touchPoint, int timeStamp)
        {
            Interop.Window.FeedTouchPoint(SwigCPtr, TouchPoint.getCPtr(touchPoint), timeStamp);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Feeds a wheel event into the window.
        /// </summary>
        /// <param name="wheelEvent">The wheel event to feed.</param>
        internal void FeedWheel(Wheel wheelEvent)
        {
            Interop.Window.FeedWheelEvent(SwigCPtr, Wheel.getCPtr(wheelEvent));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Feeds a hover event into the window.
        /// </summary>
        /// <param name="touchPoint">The touch point to feed hover event. If null is entered, the feed hover event is generated with the last inputed touch point.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void FeedHover(TouchPoint touchPoint = null)
        {
            if (touchPoint == null)
            {
                Hover hover = GetLastHoverEvent();
                if (hover == null || hover.GetPointCount() < 1)
                {
                    return;
                }
                using Vector2 screenPosition = hover.GetScreenPosition(0);
                touchPoint = new TouchPoint(hover.GetDeviceId(0), TouchPoint.StateType.Motion, screenPosition.X, screenPosition.Y);
            }
            Interop.Window.FeedHoverEvent(SwigCPtr, TouchPoint.getCPtr(touchPoint));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            touchPoint.Dispose();
        }

        /// <summary>
        /// Allows at least one more render, even when paused.
        /// The window should be shown, not minimised.
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        public void RenderOnce()
        {
            Interop.Window.RenderOnce(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets whether the window is transparent or not.
        /// </summary>
        /// <param name="transparent">Whether the window is transparent or not.</param>
        /// <since_tizen> 5 </since_tizen>
        public void SetTransparency(bool transparent)
        {
            Interop.Window.SetTransparency(SwigCPtr, transparent);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

            // Setting transparency of the window should request a relayout of the tree in the case the window changes from fully transparent.
        }

        /// <summary>
        /// Sets parent window of the window.
        /// After setting that, these windows do together when raise-up, lower and iconified/deiconified.
        /// Initially, the window is located on top of the parent. The window can go below parent by calling Lower().
        /// If parent's window stack is changed by calling Raise() or Lower(), child windows are located on top of the parent again.
        /// </summary>
        /// <param name="parent">The parent window.</param>
        /// <since_tizen> 6 </since_tizen>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        public void SetParent(Window parent)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            Interop.Window.SetParent(SwigCPtr, Window.getCPtr(parent));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets parent window of the window.
        /// After setting that, these windows do together when raise-up, lower and iconified/deiconified.
        /// This function has the additional flag whether the child is located above or below of the parent.
        /// </summary>
        /// <param name="parent">The parent window.</param>
        /// <param name="belowParent">The flag is whether the child is located above or below of the parent.</param>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetParent(Window parent, bool belowParent)
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            Interop.Window.SetParentWithStack(SwigCPtr, Window.getCPtr(parent), belowParent);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Unsets parent window of the window.
        /// After unsetting, the window is disconnected his parent window.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        public void Unparent()
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("Fail to create window. because this device does not support opengles.surfaceless_context.");
            }
            Interop.Window.Unparent(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Gets parent window of the window.
        /// </summary>
        /// <returns>The parent window of the window.</returns>
        /// <since_tizen> 6 </since_tizen>
        /// <feature> http://tizen.org/feature/opengles.surfaceless_context </feature>
        /// <exception cref="NotSupportedException">The required feature is not supported.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1721: Property names should not match get methods")]
        public Window GetParent()
        {
            if (IsSupportedMultiWindow() == false)
            {
                NUILog.Error("This device does not support surfaceless_context. So Window cannot be created. ");
            }
            Window ret = this.GetInstanceSafely<Window>(Interop.Window.GetParent(SwigCPtr));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ObjectDump()
        {
            Layer rootLayer = GetRootLayer();
            foreach (View view in rootLayer.Children)
            {
                view.ObjectDump();
            }
        }

        internal static bool IsInstalled()
        {
            bool ret = Interop.Stage.IsInstalled();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Adds an orientation to the list of available orientations.
        /// </summary>
        /// <param name="orientation">The available orientation to add</param>
        /// <since_tizen> 6 </since_tizen>
        public void AddAvailableOrientation(Window.WindowOrientation orientation)
        {
            Interop.Window.AddAvailableOrientation(SwigCPtr, (int)orientation);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Removes an orientation from the list of available orientations.
        /// </summary>
        /// <param name="orientation">The available orientation to remove.</param>
        /// <since_tizen> 6 </since_tizen>
        public void RemoveAvailableOrientation(Window.WindowOrientation orientation)
        {
            Interop.Window.RemoveAvailableOrientation(SwigCPtr, (int)orientation);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets a preferred orientation.
        /// </summary>
        /// <param name="orientation">The preferred orientation.</param>
        /// <since_tizen> 6 </since_tizen>
        public void SetPreferredOrientation(Window.WindowOrientation orientation)
        {
            Interop.Window.SetPreferredOrientation(SwigCPtr, (int)orientation);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Gets the preferred orientation.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// <returns>The preferred orientation if previously set, or none.</returns>
        public Window.WindowOrientation GetPreferredOrientation()
        {
            Window.WindowOrientation ret = (Window.WindowOrientation)Interop.Window.GetPreferredOrientation(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets current orientation of the window.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// <returns>The current window orientation if previously set, or none.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Window.WindowOrientation GetCurrentOrientation()
        {
            Window.WindowOrientation ret = (Window.WindowOrientation)Interop.Window.GetCurrentOrientation(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets available orientations of the window.
        /// This API is for setting several orientations one time.
        /// </summary>
        /// <param name="orientations">The list of orientations.</param>
        /// <since_tizen> 6 </since_tizen>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetAvailableOrientations(List<Window.WindowOrientation> orientations)
        {
            if (null == orientations)
            {
                throw new ArgumentNullException(nameof(orientations));
            }

            PropertyArray orientationArray = new PropertyArray();
            for (int i = 0; i < orientations.Count; i++)
            {
                PropertyValue value = new PropertyValue((int)orientations[i]);
                orientationArray.PushBack(value);
                value.Dispose();
            }

            Interop.Window.SetAvailableOrientations(SwigCPtr, PropertyArray.getCPtr(orientationArray), orientations.Count);
            orientationArray.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Get native window ID
        /// </summary>
        /// <returns>native window ID</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetNativeId()
        {
            int ret = Interop.Window.GetNativeId(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal Any GetNativeHandle()
        {
            Any ret = new Any(Interop.WindowInternal.WindowGetNativeHandle(SwigCPtr), true);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal void Add(Layer layer)
        {
            if (null == layer)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            if (isBorderWindow)
            {
                Interop.Actor.Add(GetRootLayer().SwigCPtr, layer.SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) { throw NDalicPINVOKE.SWIGPendingException.Retrieve(); }
            }
            else
            {
                Interop.Window.Add(SwigCPtr, Layer.getCPtr(layer));
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }

            LayersChildren?.Add(layer);
            layer.SetWindow(this);
        }

        internal void Remove(Layer layer)
        {
            if (null == layer)
            {
                throw new ArgumentNullException(nameof(layer));
            }
            Interop.Window.Remove(SwigCPtr, Layer.getCPtr(layer));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

            LayersChildren?.Remove(layer);
            layer.SetWindow(null);
        }

        internal Vector2 GetSize()
        {
            var val = new Uint16Pair(Interop.Window.GetSize(SwigCPtr), true);

            convertRealWindowSizeToBorderWindowSize(val);

            Vector2 ret = new Vector2(val.GetWidth(), val.GetHeight());
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            val.Dispose();
            return ret;
        }

        /// This will be public opened in next tizen after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderTaskList GetRenderTaskList()
        {
            global::System.IntPtr cPtr = Interop.Stage.GetRenderTaskList(stageCPtr);

            RenderTaskList ret = Registry.GetManagedBaseHandleFromNativePtr(cPtr) as RenderTaskList;
            if (ret != null)
            {
                HandleRef CPtr = new HandleRef(this, cPtr);
                Interop.BaseHandle.DeleteBaseHandle(CPtr);
                CPtr = new HandleRef(null, global::System.IntPtr.Zero);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                return ret;
            }
            else
            {
                ret = new RenderTaskList(cPtr, true);
                return ret;
            }
        }

        /// <summary>
        /// Queries the number of on-window layers.
        /// </summary>
        /// <returns>The number of layers.</returns>
        /// <remarks>Note that a default layer is always provided (count >= 1).</remarks>
        internal uint GetLayerCount()
        {
            if (LayersChildren == null || LayersChildren.Count < 0)
                return 0;

            return (uint)LayersChildren.Count;
        }

        internal Layer GetRootLayer()
        {
            if (isBorderWindow)
            {
                if (borderLayer == null)
                {
                    borderLayer = GetBorderWindowRootLayer();
                    LayersChildren?.Add(borderLayer);
                    borderLayer.SetWindow(this);
                }
                return borderLayer;
            }
            else
            {
                // Window.IsInstalled() is actually true only when called from event thread and
                // Core has been initialized, not when Stage is ready.
                if (rootLayer == null && Window.IsInstalled())
                {
                    rootLayer = new Layer(Interop.Window.GetRootLayer(SwigCPtr), true);
                    if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                    LayersChildren?.Add(rootLayer);
                    rootLayer.SetWindow(this);
                }
                return rootLayer;
            }
        }

        internal void SetBackgroundColor(Vector4 color)
        {
            Interop.Window.SetBackgroundColor(SwigCPtr, Vector4.getCPtr(color));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal Vector4 GetBackgroundColor()
        {
            Vector4 ret = new Vector4(Interop.Window.GetBackgroundColor(SwigCPtr), true);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal Vector2 GetDpi()
        {
            Vector2 ret = new Vector2(Interop.Stage.GetDpi(stageCPtr), true);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal void SetRenderingBehavior(RenderingBehaviorType renderingBehavior)
        {
            Interop.Stage.SetRenderingBehavior(stageCPtr, (int)renderingBehavior);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal RenderingBehaviorType GetRenderingBehavior()
        {
            RenderingBehaviorType ret = (RenderingBehaviorType)Interop.Stage.GetRenderingBehavior(stageCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending)
                throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal void SetWindowSize(Size2D size)
        {
            if (null == size)
            {
                throw new ArgumentNullException(nameof(size));
            }
            var val = new Uint16Pair((uint)size.Width, (uint)size.Height);

            convertBorderWindowSizeToRealWindowSize(val);

            Interop.Window.SetSize(SwigCPtr, Uint16Pair.getCPtr(val));
            val.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            // Resetting Window size should request a relayout of the tree.
        }

        internal Size2D GetWindowSize()
        {
            var val = new Uint16Pair(Interop.Window.GetSize(SwigCPtr), true);

            convertRealWindowSizeToBorderWindowSize(val);

            Size2D ret = new Size2D(val.GetWidth(), val.GetHeight());
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            val.Dispose();
            return ret;
        }

        internal void SetPosition(Position2D position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }
            var val = new Int32Pair(position.X, position.Y);
            Interop.Window.SetPosition(SwigCPtr, Int32Pair.getCPtr(val));
            val.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            // Setting Position of the window should request a relayout of the tree.
        }

        internal Position2D GetPosition()
        {
            var val = new Int32Pair(Interop.Window.GetPosition(SwigCPtr), true);
            Position2D ret = new Position2D((int)val.GetX(), (int)val.GetY());
            val.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        internal void SetPositionSize(Rectangle positionSize)
        {
            if (positionSize == null)
            {
                throw new ArgumentNullException(nameof(positionSize));
            }
            var val = new Uint16Pair((uint)positionSize.Width, (uint)positionSize.Height);

            convertBorderWindowSizeToRealWindowSize(val);

            positionSize.Width = val.GetX();
            positionSize.Height = val.GetY();

            Interop.Window.SetPositionSize(SwigCPtr, Rectangle.getCPtr(positionSize));
            val.Dispose();

            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

            // Setting Position of the window should request a relayout of the tree.
        }

        /// <summary>
        /// Set the window use partial update or not.
        /// </summary>
        /// <param name="enabled">If window enable partial update or disable.</param>
        internal void SetPartialUpdate(bool enabled)
        {
            Interop.Window.SetPartialUpdateEnabled(SwigCPtr, enabled);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether the window is enabled partial update or not.
        /// </summary>
        /// <returns>True if the window is enabled partial update, false otherwise.</returns>
        internal bool IsPartialUpdate()
        {
            bool ret = Interop.Window.IsPartialUpdateEnabled(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Enables the floating mode of window.
        /// The floating mode is to support window is moved or resized by display server.
        /// For example, if the video-player window sets the floating mode,
        /// then display server changes its geometry and handles it like a popup.
        /// The way of handling floating mode window is decided by display server.
        /// A special display server(as a Tizen display server) supports this mode.
        /// </summary>
        /// <param name="enable">Enable floating mode or not.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void EnableFloatingMode(bool enable)
        {
            Interop.Window.EnableFloatingMode(SwigCPtr, enable);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether the window is floating mode or not.
        /// </summary>
        /// <returns>True if the window is enabled floating mode, false otherwise.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsFloatingModeEnabled()
        {
            bool ret = Interop.Window.IsFloatingModeEnabled(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Requests to display server for the window is moved by display server.
        /// It can be work with setting window floating mode.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RequestMoveToServer()
        {
            Interop.Window.RequestMoveToServer(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        ///  Requests to display server for the window is resized by display server.
        /// It can be work with setting window floating mode.
        /// </summary>
        /// <param name="direction">It is indicated the window's side or edge for starting point.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RequestResizeToServer(ResizeDirection direction)
        {
            Interop.Window.RequestResizeToServer(SwigCPtr, (int)direction);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Includes input region.
        /// This function inlcudes input regions.
        /// It can be used multiple times and supports multiple regions.
        /// It means input region will be extended.
        /// This input is related to mouse and touch event.
        /// If device has touch screen, this function is useful.
        /// Otherwise device does not have that, we can use it after connecting mouse to the device.
        /// </summary>
        /// <param name="inputRegion">The included region to accept input events.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void IncludeInputRegion(Rectangle inputRegion)
        {
            Interop.Window.IncludeInputRegion(SwigCPtr, Rectangle.getCPtr(inputRegion));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// This function excludes input regions.
        /// It can be used multiple times and supports multiple regions.
        /// It means input region will be reduced.
        /// Nofice, should be set input area by IncludeInputRegion() before this function is used.
        /// This input is related to mouse and touch event.
        /// If device has touch screen, this function is useful.
        /// Otherwise device does not have that, we can use it after connecting mouse to the device.
        /// </summary>
        /// <param name="inputRegion">The excluded region to except input events.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ExcludeInputRegion(Rectangle inputRegion)
        {
            Interop.Window.ExcludeInputRegion(SwigCPtr, Rectangle.getCPtr(inputRegion));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets the pointer constraints lock.
        /// </summary>
        /// <returns>True if PointerConstraintsLock succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool PointerConstraintsLock()
        {
            bool ret = Interop.Window.PointerConstraintsLock(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the pointer constraints unlock.
        /// </summary>
        /// <returns>True if PointerConstraintsUnlock succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool PointerConstraintsUnlock()
        {
            bool ret = Interop.Window.PointerConstraintsUnlock(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the locked pointer region.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void LockedPointerRegionSet(int x, int y, int width, int height)
        {
            Interop.Window.LockedPointerRegionSet(SwigCPtr, x, y, width, height);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets the locked pointer cursor position hintset
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void LockedPointerCursorPositionHintSet(int x, int y)
        {
            Interop.Window.LockedPointerCursorPositionHintSet(SwigCPtr, x, y);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets the pointer warp. The pointer moves to the set coordinates.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>True if PointerWarp succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool PointerWarp(int x, int y)
        {
            bool ret = Interop.Window.PointerWarp(SwigCPtr, x, y);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets visibility on/off of cursor
        /// </summary>
        /// <param name="visible">The visibility of cursor.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CursorVisibleSet(bool visible)
        {
            Interop.Window.CursorVisibleSet(SwigCPtr, visible);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Requests grab key events according to the requested device subtype
        /// </summary>
        /// <param name="deviceSubclass">The deviceSubclass type.</param>
        /// <returns>True if KeyboardGrab succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool KeyboardGrab(DeviceSubClassType deviceSubclass)
        {
            bool ret = Interop.Window.KeyboardGrab(SwigCPtr, (uint)deviceSubclass);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Requests ungrab key events
        /// </summary>
        /// <returns>True if KeyboardUnGrab succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool KeyboardUnGrab()
        {
            bool ret = Interop.Window.KeyboardUnGrab(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }


        /// <summary>
        /// Maximizes window's size.
        /// If this function is called with true, window will be resized with screen size.
        /// Otherwise window will be resized with previous size.
        /// It is for the window's MAX button in window's border.
        /// If window border is supported by display server, it is not necessary.
        /// </summary>
        /// <param name="max">If window is maximized or unmaximized.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Maximize(bool max)
        {
            Interop.Window.Maximize(SwigCPtr, max);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether the window is maximized or not.
        /// </summary>
        /// <returns>True if the window is maximized, false otherwise.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsMaximized()
        {
            bool ret = Interop.Window.IsMaximized(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets window's maximum size.
        ///
        /// It is to set the maximized size when window is maximized or the window's size is increased by RequestResizeToServer().
        /// Although the size is set by this function, window's size can be increased over the limitation by SetPositionSize() or SetSize().
        ///
        /// After setting, if Maximize() is called, window is resized with the setting size and move the center.
        ///
        /// </summary>
        /// <param name="size">the maximum size.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetMaximumSize(Size2D size)
        {
            if (null == size)
            {
                throw new ArgumentNullException(nameof(size));
            }
            var val = new Uint16Pair((uint)size.Width, (uint)size.Height);

            Interop.Window.SetMaximumSize(SwigCPtr, Uint16Pair.getCPtr(val));
            val.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Minimizes window's size.
        /// If this function is called with true, window will be iconified.
        /// Otherwise window will be activated.
        /// It is for the window's MIN button in window border.
        /// If window border is supported by display server, it is not necessary.
        /// </summary>
        /// <param name="min">If window is minimized or unminimized.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Minimize(bool min)
        {
            Interop.Window.Minimize(SwigCPtr, min);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Returns whether the window is minimized or not.
        /// </summary>
        /// <returns>True if the window is minimized, false otherwise.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsMinimized()
        {
            bool ret = Interop.Window.IsMinimized(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets window's minimum size.
        /// It is to set the minimum size when window's size is decreased by RequestResizeToServer().
        /// Although the size is set by this function, window's size can be decreased over the limitation by SetPositionSize() or SetSize().
        /// </summary>
        /// <param name="size">the minimum size.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetMimimumSize(Size2D size)
        {
            if (null == size)
            {
                throw new ArgumentNullException(nameof(size));
            }
            var val = new Uint16Pair((uint)size.Width, (uint)size.Height);

            Interop.Window.SetMimimumSize(SwigCPtr, Uint16Pair.getCPtr(val));
            val.Dispose();
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Sets the layout of the window.
        /// </summary>
        /// <param name="numCols">The number of columns in the layout.</param>
        /// <param name="numRows">The number of rows in the layout.</param>
        /// <param name="column">The column number of the window within the layout.</param>
        /// <param name="row">The row number of the window within the layout.</param>
        /// <param name="colSpan">The number of columns the window should span within the layout.</param>
        /// <param name="rowSpan">The number of rows the window should span within the layout.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetLayout(uint numCols, uint numRows, uint column, uint row, uint colSpan, uint rowSpan)
        {
            Interop.Window.SetLayout(SwigCPtr, numCols, numRows, column, row, colSpan, rowSpan);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

        }

        /// <summary>
        /// Sets the layout of the window.
        /// </summary>
        /// <param name="layoutType">The type of layout to set for the window.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetLayout(WindowLayoutType layoutType)
        {
            switch (layoutType)
            {
                case WindowLayoutType.LeftHalf:
                    Interop.Window.SetLayout(SwigCPtr, 2, 1, 0, 0, 1, 1);
                    break;
                case WindowLayoutType.RightHalf:
                    Interop.Window.SetLayout(SwigCPtr, 2, 1, 1, 0, 1, 1);
                    break;

                case WindowLayoutType.TopHalf:
                    Interop.Window.SetLayout(SwigCPtr, 1, 2, 0, 0, 1, 1);
                    break;
                case WindowLayoutType.BottomHalf:
                    Interop.Window.SetLayout(SwigCPtr, 1, 2, 0, 1, 1, 1);
                    break;

                case WindowLayoutType.UpperLeftQuarter:
                    Interop.Window.SetLayout(SwigCPtr, 2, 2, 0, 0, 1, 1);
                    break;
                case WindowLayoutType.UpperRightQuarter:
                    Interop.Window.SetLayout(SwigCPtr, 2, 2, 1, 0, 1, 1);
                    break;
                case WindowLayoutType.LowerLeftQuarter:
                    Interop.Window.SetLayout(SwigCPtr, 2, 2, 0, 1, 1, 1);
                    break;
                case WindowLayoutType.LowerRightQuarter:
                    Interop.Window.SetLayout(SwigCPtr, 2, 2, 1, 1, 1, 1);
                    break;

                case WindowLayoutType.LeftThird:
                    Interop.Window.SetLayout(SwigCPtr, 3, 1, 0, 0, 1, 1);
                    break;
                case WindowLayoutType.CenterThird:
                    Interop.Window.SetLayout(SwigCPtr, 3, 1, 1, 0, 1, 1);
                    break;
                case WindowLayoutType.RightThird:
                    Interop.Window.SetLayout(SwigCPtr, 3, 1, 2, 0, 1, 1);
                    break;

                case WindowLayoutType.TopThird:
                    Interop.Window.SetLayout(SwigCPtr, 1, 3, 0, 0, 1, 1);
                    break;
                case WindowLayoutType.MiddleThird:
                    Interop.Window.SetLayout(SwigCPtr, 1, 3, 0, 1, 1, 1);
                    break;
                case WindowLayoutType.BottomThird:
                    Interop.Window.SetLayout(SwigCPtr, 1, 3, 0, 2, 1, 1);
                    break;
            }
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

        }

        /// <summary>
        /// Query whether window is rotating or not.
        /// </summary>
        /// <returns>True if window is rotating, false otherwise.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsWindowRotating()
        {
            bool ret = Interop.Window.IsWindowRotating(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Gets the last key event the window gets.
        /// </summary>
        /// <remarks>
        /// We will use weak reference of last key events.
        /// Return value will be invalidated if last key event changed internally.
        /// </remarks>
        /// <returns>The last key event the window gets.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Key GetLastKeyEvent()
        {
            if (internalLastKeyEvent == null)
            {
                // Create empty event handle without register.
                internalLastKeyEvent = new Key(Interop.Key.New(), true, false);
            }
            Interop.Window.InternalRetrievingLastKeyEvent(SwigCPtr, internalLastKeyEvent.SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return internalLastKeyEvent;
        }

        /// <summary>
        /// Gets the last touch event the window gets.
        /// </summary>
        /// <remarks>
        /// We will use weak reference of last touch events.
        /// Return value will be invalidated if last touch event changed internally.
        /// </remarks>
        /// <returns>The last touch event the window gets.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Touch GetLastTouchEvent()
        {
            if (internalLastTouchEvent == null)
            {
                // Create empty event handle without register.
                internalLastTouchEvent = new Touch(Interop.Touch.NewTouch(), true, false);
            }
            Interop.Window.InternalRetrievingLastTouchEvent(SwigCPtr, internalLastTouchEvent.SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return internalLastTouchEvent;
        }

        /// <summary>
        /// Gets the last hover event the window gets.
        /// </summary>
        /// <remarks>
        /// We will use weak reference of last hover events.
        /// Return value will be invalidated if last hover event changed internally.
        /// </remarks>
        /// <returns>The last hover event the window gets.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Hover GetLastHoverEvent()
        {
            if (internalLastHoverEvent == null)
            {
                // Create empty event handle without register.
                internalLastHoverEvent = new Hover(Interop.Hover.New(0u), true, false);
            }
            Interop.Window.InternalRetrievingLastHoverEvent(SwigCPtr, internalLastHoverEvent.SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return internalLastHoverEvent;
        }

        /// <summary>
        /// Gets the last pan gesture state the window gets.
        /// </summary>
        /// <returns>The last pan gesture state the window gets.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Gesture.StateType GetLastPanGestureState()
        {
            Gesture.StateType ret = (Gesture.StateType)Interop.Window.InternalRetrievingLastPanGestureState(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets the necessary for window rotation Acknowledgement.
        /// After this function called, SendRotationCompletedAcknowledgement() should be called to complete window rotation.
        ///
        /// This function is supprot that application has the window rotation acknowledgement's control.
        /// It means display server waits when application's rotation work is finished.
        /// It is useful application has the other rendering engine which works asynchronous.
        /// For instance, GlView.
        /// </summary>
        /// <param name="needAcknowledgement">the flag is true if window rotation acknowledge is sent.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetNeedsRotationCompletedAcknowledgement(bool needAcknowledgement)
        {
            Interop.Window.SetNeedsRotationCompletedAcknowledgement(SwigCPtr, needAcknowledgement);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// send the Acknowledgement to complete window rotation.
        /// For this function, SetNeedsRotationCompletedAcknowledgement should be already called with true.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendRotationCompletedAcknowledgement()
        {
            Interop.Window.SendRotationCompletedAcknowledgement(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Add FrameUpdateCallback
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddFrameUpdateCallback(FrameUpdateCallbackInterface frameUpdateCallback)
        {
            frameUpdateCallback?.AddFrameUpdateCallback(stageCPtr, new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero));
        }

        /// <summary>
        /// Add FrameUpdateCallback with root view.
        /// FrameUpdateCallbackInterface can only detach Views under given view.
        /// </summary>
        /// <remarks>
        /// We can give rootView as null if we want to make frameUpdateCallback don't have any dependency with some view.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddFrameUpdateCallback(FrameUpdateCallbackInterface frameUpdateCallback, View rootView)
        {
            frameUpdateCallback?.AddFrameUpdateCallback(stageCPtr, View.getCPtr(rootView));
        }

        /// <summary>
        /// Remove FrameUpdateCallback
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveFrameUpdateCallback(FrameUpdateCallbackInterface frameUpdateCallback)
        {
            frameUpdateCallback?.RemoveFrameUpdateCallback(stageCPtr);
        }

        /// <summary>
        /// Dispose for Window
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            this.DisconnectNativeSignals();

            if (type == DisposeTypes.Explicit)
            {
                //Called by User
                //Release your own managed resources here.
                //You should release all of your own disposable objects here.

                if (IsBorderEnabled)
                {
                    DisposeBorder();
                }

                foreach (var layer in childLayers)
                {
                    if (layer != null)
                    {
                        layer.Dispose();
                    }
                }

                childLayers.Clear();

                localController?.Dispose();

                internalLastKeyEvent?.Dispose();
                internalLastKeyEvent = null;
                internalLastTouchEvent?.Dispose();
                internalLastTouchEvent = null;
                internalLastHoverEvent?.Dispose();
                internalLastHoverEvent = null;

                internalHoverTimer?.Stop();
                internalHoverTimer?.Dispose();
                internalHoverTimer = null;
            }

            --aliveCount;

            base.Dispose(type);
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.Window.DeleteWindow(swigCPtr);
        }

        private static Dictionary<int, internalHookCallbackType> frameCallbackList = new Dictionary<int, internalHookCallbackType>();

        private static readonly object locker = new object();

        private static int key;

        private static FrameCallbackType internalHookFrameCallback = OnInternalHookFrameCallback;

        private struct internalHookCallbackType
        {
            public FrameCallbackType userCallback;
            public int frameId;
        }

        private static void OnInternalHookFrameCallback(int id)
        {
            lock (locker)
            {
                if (frameCallbackList.ContainsKey(id))
                {
                    if (frameCallbackList[id].userCallback != null)
                    {
                        frameCallbackList[id].userCallback.Invoke(frameCallbackList[id].frameId);
                        frameCallbackList.Remove(id);
                    }
                    else
                    {
                        NUILog.Error($"found userCallback is NULL");
                        frameCallbackList.Remove(id);
                    }
                }
            }
        }

        private int AddInterHookCallback(FrameCallbackType callback, int frameId)
        {
            if (null == callback)
            {
                throw new ArgumentNullException(nameof(callback), "FrameCallbackType should not be null");
            }
            var assignedKey = 0;
            lock (locker)
            {
                key++;
                assignedKey = key;
                frameCallbackList.Add(assignedKey, new internalHookCallbackType()
                {
                    userCallback = callback,
                    frameId = frameId,
                });
            }
            return assignedKey;
        }

        /// <summary>
        /// Type of callback which is called when the frame rendering is done by graphics driver or when the frame is displayed on display.
        /// </summary>
        /// <param name="frameId">The Id to specify the frame. It will be passed when the callback is called.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void FrameCallbackType(int frameId);

        /// <summary>
        /// Adds a callback that is called when the frame rendering is done by the graphics driver.
        /// A callback of the following type may be used:
        /// <code>
        /// void MyFunction( int frameId )
        /// </code>
        /// This callback will be deleted once it is called.
        /// <remarks>
        /// Ownership of the callback is passed onto this class
        /// </remarks>
        /// </summary>
        /// <param name="callback">The function to call</param>
        /// <param name="frameId">The Id to specify the frame. It will be passed when the callback is called.</param>
        /// <exception cref="ArgumentNullException">This exception can occur by the callback is null.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddFrameRenderedCallback(FrameCallbackType callback, int frameId)
        {
            var assignedKey = AddInterHookCallback(callback, frameId);
            Interop.WindowInternal.AddFrameRenderedCallback(SwigCPtr, new HandleRef(this, Marshal.GetFunctionPointerForDelegate<Delegate>(internalHookFrameCallback)), assignedKey);

            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Adds a callback that is called when the frame is displayed on the display.
        /// A callback of the following type may be used:
        /// <code>
        /// void MyFunction( int frameId )
        /// </code>
        /// This callback will be deleted once it is called.
        /// <remarks>
        /// Ownership of the callback is passed onto this class
        /// </remarks>
        /// </summary>
        /// <param name="callback">The function to call</param>
        /// <param name="frameId">The Id to specify the frame. It will be passed when the callback is called.</param>
        /// <exception cref="ArgumentNullException">This exception can occur by the callback is null.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddFramePresentedCallback(FrameCallbackType callback, int frameId)
        {
            var assignedKey = AddInterHookCallback(callback, frameId);
            Interop.WindowInternal.AddFramePresentedCallback(SwigCPtr, new HandleRef(this, Marshal.GetFunctionPointerForDelegate<Delegate>(internalHookFrameCallback)), assignedKey);

            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Search through this Window for a Layer with the given unique ID.
        /// </summary>
        /// <param name="id">The ID of the Layer to find.</param>
        /// <remarks>Hidden-API</remarks>
        /// <returns>A handle to the Layer if found, or an empty handle if not.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Layer FindLayerByID(uint id)
        {
            Layer defaultLayer = this.GetDefaultLayer();
            IntPtr cPtr = Interop.Actor.FindChildById(defaultLayer.SwigCPtr, id);
            Layer ret = this.GetInstanceSafely<Layer>(cPtr);

            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets to resize window with full screen.
        /// If full screen size is set for the window,
        /// window will be resized with full screen.
        /// In addition, the full screen sized window's z-order is the highest.
        /// </summary>
        /// <param name="fullscreen"> If fullscreen is true, set fullscreen or unset.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetFullScreen(bool fullscreen)
        {
            Interop.Window.SetFullScreen(SwigCPtr, fullscreen);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Gets whether the full screen sized window or not.
        /// </summary>
        /// <returns>Returns true if the full screen sized window is.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool GetFullScreen()
        {
            bool ret = Interop.Window.GetFullScreen(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Get Native Window handle.
        /// <example>
        /// How to get Native Window handle
        /// <code>
        /// Window window = NUIApplication.GetDefaultWindow();
        /// var handle = window.NativeHandle;
        /// if(handle.IsInvalid == false)
        /// {
        ///     IntPtr nativeHandle = handle.DangerousGetHandle();
        ///     // do something with nativeHandle
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public SafeHandle NativeHandle
        {
            get
            {
                return new NUI.SafeNativeWindowHandle(this);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window is modal or not.
        /// The modal property of a window requires that it be set to a parent window.
        /// The window modal function operates on the specified parent window.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsModal
        {
            get
            {
                bool ret = Interop.Window.IsModal(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                return ret;
            }
            set
            {
                Interop.Window.SetModal(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the window is always on top of other windows or not.
        /// This is valid between windows that have no notification level or a notification level of 'none'.
        /// If it has a notification level, this will not do anything.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsAlwaysOnTop
        {
            get
            {
                bool ret = Interop.Window.IsAlwaysOnTop(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                return ret;
            }
            set
            {
                Interop.Window.SetAlwaysOnTop(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window is the bottom of other windows or not.
        /// If the enable flag is true, this window will be placed below other windows.
        /// Otherwise, if it's called with a false value, it will be located above other windows..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsBottom
        {
            get
            {
                bool ret = Interop.Window.IsBottom(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                return ret;
            }
            set
            {
                Interop.Window.SetBottom(SwigCPtr, value);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }
        }

        /// <summary>
        /// Requests relative motion grab
        /// </summary>
        /// <returns>True if RelativeMotionGrab succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool RelativeMotionGrab(PointerBoundary boundary)
        {
            bool ret = Interop.Window.RelativeMotionGrab(SwigCPtr, (uint)boundary);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Requests relative motion ungrab
        /// </summary>
        /// <returns>True if RelativeMotionGrab succeeds.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool RelativeMotionUnGrab()
        {
            bool ret = Interop.Window.RelativeMotionUnGrab(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Sets or gets the window blur using window blur information.
        /// 
        /// It is designed to apply a blur effect to a window based on specified parameters. 
        /// This supports different types of blurring effects, including blurring the window's background only.
        /// Or blurring the area surrounding the window while keeping the window itself clear.
        /// The more information is written WindowBlurInfo struct.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WindowBlurInfo BlurInfo
        {
            get
            {
                IntPtr internalBlurInfo = Interop.Window.GetBlur(SwigCPtr);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                WindowBlurInfo blurInfo = new WindowBlurInfo();
                blurInfo.BlurType = (WindowBlurType)Interop.WindowBlurInfo.GetBlurType(internalBlurInfo);
                blurInfo.BlurRadius = Interop.WindowBlurInfo.GetBlurRadius(internalBlurInfo);
                blurInfo.BackgroundCornerRadius = Interop.WindowBlurInfo.GetBackgroundCornerRadius(internalBlurInfo);

                Interop.WindowBlurInfo.DeleteWindowBlurInfo(internalBlurInfo);

                return blurInfo;
            }
            set
            {
                IntPtr internalBlurInfo = Interop.WindowBlurInfo.New((int)value.BlurType, value.BlurRadius, value.BackgroundCornerRadius);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

                try {
                    Interop.Window.SetBlur(SwigCPtr, internalBlurInfo);
                    if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
                }
                finally {
                    Interop.WindowBlurInfo.DeleteWindowBlurInfo(internalBlurInfo);
                }
            }            
        }

        /// <summary>
        /// Calculates sizes and positions of views in all layers if the view's layout is required to be updated.
        /// e.g. RequestLayout() is called for the view.
        /// Since UpdateLayout calculates sizes and positions of views manually right now, do not abuse calling it.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateLayout()
        {
            using var size = GetSize();

            LayersChildren?.ForEach(layer =>
            {
                if (layer?.LayoutCount > 0)
                {
                    layer?.Children?.ForEach(view =>
                    {
                        if (view != null)
                        {
                            LayoutController.FindRootLayouts(view, size.Width, size.Height);
                        }
                    });
                }
            });
        }

        IntPtr IWindowProvider.WindowHandle => GetNativeWindowHandler();
        float IWindowProvider.X => WindowPosition.X;
        float IWindowProvider.Y => WindowPosition.Y;
        float IWindowProvider.Width => WindowSize.Width;
        float IWindowProvider.Height => WindowSize.Height;
        int IWindowProvider.Rotation => (int)GetCurrentOrientation();
    }
}
