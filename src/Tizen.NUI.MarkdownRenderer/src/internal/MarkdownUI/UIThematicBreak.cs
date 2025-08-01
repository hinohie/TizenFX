﻿/*
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

using Tizen.NUI.BaseComponents;

namespace Tizen.NUI.MarkdownRenderer
{
    /// <summary>
    /// Represents a visual horizontal rule (thematic break) in a Markdown UI.
    /// Renders a line with specified color, thickness, and margin.
    /// </summary>
    internal class UIThematicBreak : View
    {
        public UIThematicBreak(ThematicBreakStyle thematicBreakStyle) : base()
        {
            ushort margin = (ushort)thematicBreakStyle.Margin;

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = thematicBreakStyle.Thickness;
            BackgroundColor = new Color(thematicBreakStyle.Color);
            Margin = new Extents(0, 0, margin, margin);
        }
    }
}
