using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.NUI.Samples
{
    public class ControlSample : IExample
    {
        private View root;
        private Control control;
        private TaskCompletionSource<int> taskSource;

        public void Activate()
        {
            Window window = NUIApplication.GetDefaultWindow();

            root = new View()
            {
                Size = window.Size,
                BackgroundColor = new Color(0.8f, 0.8f, 0.8f, 0.6f),
                ParentOrigin = ParentOrigin.Center,
                PivotPoint = PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            window.Add(root);

            control = new Control()
            {
                Size = new Size(100, 100),
                BackgroundColor = Color.Blue,
                ParentOrigin = ParentOrigin.Center,
                PivotPoint = PivotPoint.Center,
                PositionUsesPivotPoint = true,
                BoxShadow = new Shadow(0, new Color(0.2f, 0.2f, 0.2f, 0.3f), new Vector2(5, 5)),
                CornerRadius = 0.5f,
                CornerRadiusPolicy = VisualTransformPolicyType.Relative,
            };

            root.Add(control);

            var animation = new Animation(2000);
            animation.AnimateTo(control, "SizeWidth", 200, 0, 1000);
            animation.AnimateTo(control, "SizeWidth", 100, 1000, 2000);
            animation.Looping = true;
            animation.Play();

            control.TouchEvent += OnTouch;
        }

        private bool OnTouch(object s, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                Tizen.Log.Error("NUI",$"Before ASDF\n");
                ASDF();
                Tizen.Log.Error("NUI",$"End ASDF\n");
                if(control != null)
                {
                    control.Dispose();
                    taskSource?.SetResult(99);
                }
                Tizen.Log.Error("NUI",$"Dispose control\n");
            }
            return true;
        }

        private async void ASDF()
        {
            Tizen.Log.Error("NUI",$"BEfore await\n");
            var task = DummyAsync();
            int ret = await task;
            Tizen.Log.Error("NUI",$"End await {ret}\n");
        }

        private async Task<int> DummyAsync()
        {
            taskSource = new TaskCompletionSource<int>();
            EventHandler handler = (s, e) =>
            {
                Tizen.Log.Error("NUI",$"OnRelayout\n");
                taskSource.SetResult(12);
            };

            control.Relayout += handler;
            control.SizeHeight = control.SizeHeight + 10.0f;

            try
            {
                Tizen.Log.Error("NUI",$"Before await\n");
                return await taskSource.Task;
            }
            finally
            {
                if(control != null)
                {
                    control.Relayout -= handler;
                }
            }
        }

        public void Deactivate()
        {
            if (root != null)
            {
                NUIApplication.GetDefaultWindow().Remove(root);
                root.Dispose();
            }
        }
    }
}
