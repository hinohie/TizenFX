/*
 * Copyright (c) 2024 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Constants;
using Tizen.NUI.Scene3D;
using System.Collections.Generic;

class Scene3DSample : NUIApplication
{
    Window mWindow;
    Vector2 mWindowSize;

    Timer timer;

    void OnKeyEvent(object source, Window.KeyEventArgs e)
    {
        Tizen.Log.Error("NUI", $"KEY[0x{e.Key.SwigCPtr.Handle.ToInt64():X}][{e.Key.State}][{e.Key.KeyPressedName}]");
        if (e.Key.State == Key.StateType.Down)
        {
            switch (e.Key.KeyPressedName)
            {
                case "Escape":
                case "Back":
                {
                    Deactivate();
                    Exit();
                    break;
                }

                case "Return":
                case "Select":
                {
                    break;
                }

                case "1":
                {
                    timer.Start();
                    break;
                }

                case "2":
                {
                    key.Dispose();
                    break;
                }

                case "Wahaha":
                {
                    OnTick(null, null);
                    OnTick(null, null);
                    key.Dispose();
                    break;
                }
            }

            FullGC();
        }
        else if (e.Key.State == Key.StateType.Up)
        {
        }
    }

    Key key;

    public void Activate()
    {
        mWindow = Window.Instance;

        View view = new View()
        {
            BackgroundColor = Color.Red,
            SizeWidth = 10.0f,
            SizeHeight = 10.0f,
        };
        mWindow.GetDefaultLayer().Add(view);

        mWindow.KeyEvent += OnKeyEvent;

        timer = new Timer(1000);
        timer.Tick += OnTick;
        timer.Start();
    }

    bool OnTick(object o, EventArgs e)
    {
        if(key == null)
        {
            key = new Key();
        }
        key.State = Key.StateType.Down;
        key.KeyPressedName = "Wahaha";
        Tizen.Log.Error("NUI",$"0x{key.SwigCPtr.Handle.ToInt64():X}");
        Window.Instance.FeedKey(key);
        return false;
    }
    public void FullGC()
    {
        global::System.GC.Collect();
        global::System.GC.WaitForPendingFinalizers();
        global::System.GC.Collect();
    }

    public void Deactivate()
    {
    }
    protected override void OnCreate()
    {
        // Up call to the Base class first
        base.OnCreate();
        Activate();
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread] // Forces app to use one thread to access NUI
    static void Main(string[] args)
    {
        Scene3DSample example = new Scene3DSample();
        example.Run(args);
    }
}
