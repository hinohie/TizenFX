using System;
using System.Runtime.InteropServices;
using Tizen.NUI.BaseComponents;
using System.Collections.Generic;

namespace Tizen.NUI.Samples
{
    public class ConstraintWithCustomShaderTest : IExample
    {
        static readonly string VERTEX_SHADER =
        "//@name ConstraintWithCustomShaderTest.vert\n" +
        "\n" +
        "//@version 100\n" +
        "\n" +
        "precision highp float;\n" +
        "INPUT highp vec2 aPosition;\n" +
        "OUTPUT highp vec2 vPosition;\n" +
        "\n" +
        "UNIFORM_BLOCK VertBlock\n" +
        "{\n" +
        "  UNIFORM highp mat4 uMvpMatrix;\n" +
        "  UNIFORM highp vec3 uSize;\n" +
        "};\n" +
        "void main()\n" +
        "{\n" +
        "    vec4 pos = vec4(aPosition, 0.0, 1.0) * vec4(uSize.xy, 0.0, 1.0);\n" +
        "    vPosition = (aPosition + vec2(0.5)) * uSize.xy;\n" +
        "    gl_Position = uMvpMatrix * pos;\n" +
        "}\n";

        static readonly string FRAGMENT_SHADER =
        "//@name ConstraintWithCustomShaderTest.frag\n" +
        "\n" +
        "//@version 100\n" +
        "\n" +
        "precision highp float;\n" +
        "INPUT highp vec2 vPosition;\n" +
        "\n" +
        "UNIFORM_BLOCK FragBlock\n" +
        "{\n" +
        "  UNIFORM lowp vec4 uColor;\n" +
        "  UNIFORM highp vec2 uCustomCursorPosition;\n" +
        "  UNIFORM highp float uCustomCursorRadius;\n" +
        "};\n" +
        "\n" +
        "void main()\n" +
        "{\n" +
        "    highp float dist = length(vPosition - uCustomCursorPosition);\n" +
        "    dist /= uCustomCursorRadius;\n" +
        "    gl_FragColor = uColor * vec4(dist, 0.0, 1.0, 1.0);\n" +
        "}\n";


        struct SimpleQuadVertex
        {
            public UIVector2 position;
        }

        private PropertyBuffer CreateQuadPropertyBuffer()
        {
            /* Create Property buffer */
            PropertyMap vertexFormat = new PropertyMap();
            vertexFormat.Add("aPosition", (int)PropertyType.Vector2);

            PropertyBuffer vertexBuffer = new PropertyBuffer(vertexFormat);
            
            SimpleQuadVertex vertex1 = new SimpleQuadVertex();
            SimpleQuadVertex vertex2 = new SimpleQuadVertex();
            SimpleQuadVertex vertex3 = new SimpleQuadVertex();
            SimpleQuadVertex vertex4 = new SimpleQuadVertex();
            vertex1.position = new UIVector2(-0.5f, -0.5f);
            vertex2.position = new UIVector2(-0.5f, 0.5f);
            vertex3.position = new UIVector2(0.5f, -0.5f);
            vertex4.position = new UIVector2(0.5f, 0.5f);

            SimpleQuadVertex[] texturedQuadVertexData = new SimpleQuadVertex[4] { vertex1, vertex2, vertex3, vertex4 };

            int size = Marshal.SizeOf(vertex1);
            IntPtr pA = Marshal.AllocHGlobal(checked(size * texturedQuadVertexData.Length));

            try
            {
                for (int i = 0; i < texturedQuadVertexData.Length; i++)
                {
                    Marshal.StructureToPtr(texturedQuadVertexData[i], pA + i * size, true);
                }

                vertexBuffer.SetData(pA, (uint)texturedQuadVertexData.Length);
            }
            catch(Exception e)
            {
                Tizen.Log.Error("NUI", "Exception in PropertyBuffer : " + e.Message);
            }
            finally
            {
                // Free AllocHGlobal memory after call PropertyBuffer.SetData()
                Marshal.FreeHGlobal(pA);
            }

            vertexFormat.Dispose();

            return vertexBuffer;
        }

        private enum ConstraintBehaviorType
        {
            None = 0,
            Noise,
            Max,
        };
        private static ConstraintBehaviorType constraintType;
        private static Random rand = new Random();

        private static object lockObject = new object();

        private const bool _printLogs = false; // Make it true if you want to see the logs

        private static UIVector2 OnConstraintVector2(UIVector2 current, uint id, in PropertyInputContainer container)
        {
            float viewWidth = container.GetFloat(0u);
            float viewHeight = container.GetFloat(1u);
            UIVector3 viewPosition = container.GetVector3(2u);
            UIColor viewColor = container.GetColor(3u);
            using Rotation viewRotation = container.GetRotation(4u);
            using Matrix viewModelMatrix = container.GetMatrix(5u);

            float hoveViewX = container.GetFloat(6u);
            float hoveViewY = container.GetFloat(7u);

            using Vector3 axis = new Vector3();
            using Radian angle = new Radian();
            bool converted = viewRotation.GetAxisAngle(axis, angle);

            ConstraintBehaviorType type;
            lock (lockObject)
            {
                type = constraintType;
            }

            UIVector2 result = new UIVector2(hoveViewX - viewPosition.X + viewWidth * 0.5f, hoveViewY - viewPosition.Y + viewHeight * 0.5f);
            switch(type)
            {
                case ConstraintBehaviorType.Noise:
                {
                    result = new UIVector2(result.X + rand.Next(-1000, 1000) * 0.01f, result.Y + rand.Next(-1000, 1000) * 0.01f);
                    break;
                }
                default:
                {
                    // Do nothing.
                    break;
                }
            }

            if (_printLogs)
            {
                Tizen.Log.Error("NUI", $"ID : {id}, container size : {container.Count}, current : {current.X}, {current.Y}\n");
                Tizen.Log.Error("NUI", $"w:{viewWidth} h:{viewHeight}, x:{viewPosition.X}, y:{viewPosition.Y}, z:{viewPosition.Z}\n");
                Tizen.Log.Error("NUI", $"r:{viewColor.R} g:{viewColor.G}, b:{viewColor.B}, a:{viewColor.A}\n");
                Tizen.Log.Error("NUI", $"{converted} / angle:{angle.Value} axis:{axis.X}, {axis.Y}, {axis.Z}\n");
                Tizen.Log.Error("NUI", $"hover : {hoveViewX}, {hoveViewY}\n");

                Tizen.Log.Error("NUI", $"  Model Matrix :\n");
                for (uint i = 0; i < 4; i++)
                {
                    Tizen.Log.Error("NUI", $"  {viewModelMatrix.ValueOfIndex(i, 0)}, {viewModelMatrix.ValueOfIndex(i, 1)}, {viewModelMatrix.ValueOfIndex(i, 2)}, {viewModelMatrix.ValueOfIndex(i, 3)}\n");
                }
                Tizen.Log.Error("NUI", $"ID : {id}, result : {result.X}, {result.Y}\n");
            }

            return result;
        }

        private Window win;
        private View root;
        private Layer overlayLayer;

        private View view; // Custom renderable holder
        private View hoverView;

        private static Geometry geometry;
        private static Shader shader;
        private Renderable renderable;

        private Constraint constraint;
        private bool constraintApplied;

        private Animation radiusAnimation;

        private Animation cursorMoveAnimation;
        private int cursorPositionIndex;

        public void Activate()
        {
            win = NUIApplication.GetDefaultWindow();
            win.KeyEvent += WindowKeyEvent;
            root = new View()
            {
                Name = "root",
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
            };
            win.Add(root);

            CreateCustomRenderableView();
            GenerateConstraint();

            var infoLabel = new TextLabel()
            {
                Text = "Constraint the mouse cursor position, and use this value as custom shader uniform.\n" +
                       "If touch down and release, curosr position animate.\n" +
                       "\n" +
                       "  Press 0 to give noise at constraint result\n" +
                       "  Press 1 to test re-generate new constraint result\n" +
                       "  Press 2 to test Apply / Remove constraint\n" +
                       "  Press 3 to test re-generate source view\n" +
                       "  Press 4 to test re-generate target renderer\n",
                MultiLine = true,
            };
            overlayLayer = new Layer();
            win.AddLayer(overlayLayer);
            overlayLayer.Add(infoLabel);
        }

        public void Deactivate()
        {
            root?.Unparent();
            root?.DisposeRecursively();

            if (constraint != null)
            {
                Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} will be Disposed(), but callback could be comes after now\n");
                constraint?.Dispose();
            }

            if (overlayLayer != null)
            {
                win?.RemoveLayer(overlayLayer);
            }

            renderable?.Dispose();
            overlayLayer?.Dispose();

            if (win != null)
            {
                win.KeyEvent -= WindowKeyEvent;
            }
        }

        private void WindowKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "0")
                {
                    lock (lockObject)
                    {
                        constraintType = (ConstraintBehaviorType)(((int)constraintType + 1) % ((int)ConstraintBehaviorType.Max));

                        Tizen.Log.Error("NUI", $"Constraint : {constraint.ID} will act by {constraintType}\n");
                    }
                }
                if (e.Key.KeyPressedName == "1")
                {
                    GenerateConstraint();
                }
                if (e.Key.KeyPressedName == "2")
                {
                    ToggleConstraintActivation();
                }
                if (e.Key.KeyPressedName == "3")
                {
                    RegenerateView();
                }
                if (e.Key.KeyPressedName == "4")
                {
                    RegenerateRenderable();
                }
            }
        }

        private void CreateCustomRenderableView()
        {
            view = new View()
            {
                SizeWidth = 300,
                SizeHeight = 500,
                PositionX = 50,
                PositionY = 100,
            };

            hoverView = new View()
            {
                BackgroundColor = Color.Black,
                SizeWidth = 4,
                SizeHeight = 4,
                PositionX = 200,
                PositionY = 350,
            };

            cursorMoveAnimation = new Animation(500);

            root.HoverEvent += (o, e) =>
            {
                //if (cursorMoveAnimation.State == Animation.States.Stopped)
                {
                    using var localPosition = e.Hover.GetLocalPosition(0u);
                    hoverView.PositionX = localPosition.X;
                    hoverView.PositionY = localPosition.Y;
                }
                return true;
            };

            root.TouchEvent += (o, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Down)
                {
                    cursorMoveAnimation.Stop();
                    cursorMoveAnimation.Clear();
                }
                else if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    cursorMoveAnimation.Stop();
                    cursorMoveAnimation.Clear();

                    // Reverse the animation localPosition -> currentPosition.
                    // If than, it will works like currentPositoin -> latest position what updated by Hover
                    using var localPosition = e.Touch.GetLocalPosition(0u);
                    cursorMoveAnimation.AnimateTo(hoverView, "PositionX", hoverView.PositionX, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutBack));
                    cursorMoveAnimation.AnimateTo(hoverView, "PositionY", hoverView.PositionY, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutBack));
                    cursorMoveAnimation.SpeedFactor = -1;
                    cursorMoveAnimation.Play();

                    hoverView.PositionX = localPosition.X;
                    hoverView.PositionY = localPosition.Y;
                }
                return true;
            };

            renderable = GenerateRenderable();
            view.AddRenderable(renderable);

            root.Add(view);
            root.Add(hoverView);
        }

        private void GenerateConstraint()
        {
            if (constraint != null)
            {
                Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} will be Disposed(), but callback could be comes after now\n");
                constraint?.Dispose();
            }

            // Create constraint for renderable
            constraint = Constraint.GenerateConstraint(renderable, cursorPositionIndex, new Constraint.ConstraintVector2FunctionCallbackType(OnConstraintVector2));
            constraint.AddSource(view, "sizeWidth");
            constraint.AddSource(view, "sizeHeight");
            constraint.AddSource(view, view.GetPropertyIndex("worldPosition")); // Test for Vector3
            constraint.AddSource(view, view.GetPropertyIndex("color")); // Test for Vector4
            constraint.AddSource(view, view.GetPropertyIndex("orientation")); // Test for rotation
            constraint.AddSource(view, view.GetPropertyIndex("worldMatrix")); // Test for matrix
            constraint.AddSource(hoverView, "worldPositionX");
            constraint.AddSource(hoverView, hoverView.GetPropertyIndex("worldPositionY"));
            constraint.Apply();

            constraintApplied = true;

            Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} Created and applied\n");
        }

        private void ToggleConstraintActivation()
        {
            if (constraint != null)
            {
                if (constraintApplied)
                {
                    constraint.Remove();
                    Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} removed\n");
                }
                else
                {
                    constraint.Apply();
                    Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} applied\n");
                }
                constraintApplied ^= true;
            }
        }

        private void RegenerateView()
        {
            view.Dispose();
            hoverView.Dispose();

            view = new View()
            {
                SizeWidth = 300,
                SizeHeight = 500,
                PositionX = 50,
                PositionY = 100,
            };

            hoverView = new View()
            {
                BackgroundColor = Color.Black,
                SizeWidth = 4,
                SizeHeight = 4,
                PositionX = 200,
                PositionY = 350,
            };

            view.AddRenderable(renderable);
            root.Add(view);
            root.Add(hoverView);

            if (constraint != null)
            {
                Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} invalidate after RenderThread object destroyed\n");
            }
        }

        private void RegenerateRenderable()
        {
            view.RemoveRenderable(renderable);
            renderable.Dispose();

            renderable = GenerateRenderable();
            view.AddRenderable(renderable);

            if (constraint != null)
            {
                Tizen.Log.Error("NUI", $"Constraint with ID : {constraint.ID} invalidate after RenderThread object destroyed\n");
            }
        }

        private Geometry GenerateGeometry()
        {
            if (geometry == null)
            {
                using PropertyBuffer vertexBuffer = CreateQuadPropertyBuffer();
                geometry = new Geometry();
                geometry.AddVertexBuffer(vertexBuffer);
                geometry.SetType(Geometry.Type.TRIANGLE_STRIP);
            }
            return geometry;
        }

        private Shader GenerateShader()
        {
            if (shader == null)
            {
                shader = new Shader(VERTEX_SHADER, FRAGMENT_SHADER, "ConstraintWithCustomShaderTest");
            }
            return shader;
        }

        private Renderable GenerateRenderable()
        {
            if (renderable == null)
            {
                renderable = new Renderable()
                {
                    Geometry = GenerateGeometry(),
                    Shader = GenerateShader(),
                };

                cursorPositionIndex = renderable.RegisterProperty("uCustomCursorPosition", new PropertyValue(100.0f, 200.0f));

                renderable.RegisterProperty("uCustomCursorRadius", new PropertyValue(300.0f));

                radiusAnimation = new Animation(10000);
                radiusAnimation.LoopingMode = Animation.LoopingModes.AutoReverse;
                radiusAnimation.Looping = true;
                radiusAnimation.AnimateTo(renderable, "uCustomCursorRadius", 50.0f, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseInOutSine));
                radiusAnimation.Play();
            }

            return renderable;
        }
    }
}
