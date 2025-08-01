﻿/*
 * Copyright(c) 2023 Samsung Electronics Co., Ltd.
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
using System.Runtime.InteropServices;

namespace Tizen.NUI
{
    internal static partial class Interop
    {
        internal static partial class TextLabel
        {
            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_TEXT_get")]
            public static extern int TextGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_FONT_FAMILY_get")]
            public static extern int FontFamilyGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_FONT_STYLE_get")]
            public static extern int FontStyleGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_POINT_SIZE_get")]
            public static extern int PointSizeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_MULTI_LINE_get")]
            public static extern int MultiLineGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_HORIZONTAL_ALIGNMENT_get")]
            public static extern int HorizontalAlignmentGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_VERTICAL_ALIGNMENT_get")]
            public static extern int VerticalAlignmentGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_TEXT_COLOR_get")]
            public static extern int TextColorGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ENABLE_MARKUP_get")]
            public static extern int EnableMarkupGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ENABLE_AUTO_SCROLL_get")]
            public static extern int EnableAutoScrollGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_AUTO_SCROLL_SPEED_get")]
            public static extern int AutoScrollSpeedGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_AUTO_SCROLL_LOOP_COUNT_get")]
            public static extern int AutoScrollLoopCountGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_AUTO_SCROLL_GAP_get")]
            public static extern int AutoScrollGapGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_LINE_SPACING_get")]
            public static extern int LineSpacingGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_RELATIVE_LINE_SIZE_get")]
            public static extern int RelativeLineHeightGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_UNDERLINE_get")]
            public static extern int UnderlineGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_SHADOW_get")]
            public static extern int ShadowGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_EMBOSS_get")]
            public static extern int EmbossGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_OUTLINE_get")]
            public static extern int OutlineGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_New__SWIG_0")]
            public static extern global::System.IntPtr New();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_New_With_Style")]
            public static extern global::System.IntPtr New(bool hasStyle);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_New_With_String_Style")]
            public static extern global::System.IntPtr New(string text, bool hasStyle);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_delete_TextLabel")]
            public static extern void DeleteTextLabel(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_PIXEL_SIZE_get")]
            public static extern int PixelSizeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_ELLIPSIS_get")]
            public static extern int EllipsisGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_ELLIPSIS_POSITION_get")]
            public static extern int EllipsisPositionGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_LINE_COUNT_get")]
            public static extern int LineCountGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_LINE_WRAP_MODE_get")]
            public static extern int LineWrapModeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_TEXT_DIRECTION_get")]
            public static extern int TextDirectionGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_VERTICAL_LINE_ALIGNMENT_get")]
            public static extern int VerticalLineAlignmentGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_AUTO_SCROLL_STOP_MODE_get")]
            public static extern int AutoScrollStopModeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_AUTO_SCROLL_LOOP_DELAY_get")]
            public static extern int AutoScrollLoopDelayGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_MATCH_SYSTEM_LANGUAGE_DIRECTION_get")]
            public static extern int MatchSystemLanguageDirectionGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_TEXT_FIT_get")]
            public static extern int TextFitGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_TextFitChangedSignal")]
            public static extern global::System.IntPtr TextFitChangedSignal(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_TextLabel_Property_MIN_LINE_SIZE_get")]
            public static extern int MinLineSizeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_FONT_SIZE_SCALE_get")]
            public static extern int FontSizeScaleGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ENABLE_FONT_SIZE_SCALE_get")]
            public static extern int EnableFontSizeScaleGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_GetTextSize")]
            public static extern global::System.IntPtr GetTextSize(HandleRef textLabelRef, uint start, uint end);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_GetTextPosition")]
            public static extern global::System.IntPtr GetTextPosition(HandleRef textLabelRef, uint start, uint end);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_SetTextFitArray")]
            public static extern void SetTextFitArray(HandleRef textLabel, bool enable, uint arraySize, float[] pointSizeArray, float[] minLineSizeArray);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_GetTextFitArray")]
            public static extern global::System.IntPtr GetTextFitArray(HandleRef textLabel);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AnchorClickedSignal")]
            public static extern global::System.IntPtr AnchorClickedSignal(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabelSignal_Empty")]
            [return: MarshalAs(UnmanagedType.U1)]
            public static extern bool TextLabelSignalEmpty(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabelSignal_GetConnectionCount")]
            public static extern uint TextLabelSignalGetConnectionCount(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabelSignal_Connect")]
            public static extern void TextLabelSignalConnect(HandleRef jarg1, HandleRef jarg2);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabelSignal_Disconnect")]
            public static extern void TextLabelSignalDisconnect(HandleRef jarg1, HandleRef jarg2);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabelSignal_Emit")]
            public static extern void TextLabelSignalEmit(HandleRef jarg1, HandleRef jarg2);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_new_TextLabelSignal")]
            public static extern global::System.IntPtr NewTextLabelSignal();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_delete_TextLabelSignal")]
            public static extern void DeleteTextLabelSignal(HandleRef jarg1);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_STRIKETHROUGH_get")]
            public static extern int StrikethroughGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_CHARACTER_SPACING_get")]
            public static extern int CharacterSpacingGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ANCHOR_COLOR_get")]
            public static extern int AnchorColorGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ANCHOR_CLICKED_COLOR_get")]
            public static extern int AnchorClickedColorGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_REMOVE_FRONT_INSET_get")]
            public static extern int RemoveFrontInsetGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_REMOVE_BACK_INSET_get")]
            public static extern int RemoveBackInsetGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_CUTOUT_get")]
            public static extern int CutoutGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_RENDER_MODE_get")]
            public static extern int RenderModeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_LAYOUT_DIRECTION_POLICY_get")]
            public static extern int LayoutDirectionPolicyGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ELLIPSIS_MODE_get")]
            public static extern int EllipsisModeGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_IS_SCROLLING_get")]
            public static extern int IsScrollingGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_MANUAL_RENDERED_get")]
            public static extern int ManualRenderedGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_NEED_REQUEST_ASYNC_RENDER_get")]
            public static extern int NeedRequestAsyncRenderGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_ASYNC_LINE_COUNT_get")]
            public static extern int AsyncLineCountGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_RENDER_SCALE_get")]
            public static extern int RenderScaleGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_Property_PIXEL_SNAP_FACTOR_get")]
            public static extern int PixelSnapFactorGet();

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncRenderWithFixedSize")]
            public static extern void RequestAsyncRenderWithFixedSize(HandleRef textLabelRef, float width, float height);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncRenderWithFixedWidth")]
            public static extern void RequestAsyncRenderWithFixedWidth(HandleRef textLabelRef, float width, float heightConstraint);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncRenderWithFixedHeight")]
            public static extern void RequestAsyncRenderWithFixedHeight(HandleRef textLabelRef, float widthConstraint, float height);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncRenderWithConstraint")]
            public static extern void RequestAsyncRenderWithConstraint(HandleRef textLabelRef, float widthConstraint, float heightConstraint);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncNaturalSize")]
            public static extern void RequestAsyncNaturalSize(HandleRef textLabelRef);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RequestAsyncHeightForWidth")]
            public static extern void RequestAsyncHeightForWidth(HandleRef textLabelRef, float width);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncTextRenderedSignal_Connect")]
            public static extern void AsyncTextRenderedConnect(HandleRef textLabelRef, HandleRef handler);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncTextRenderedSignal_Disconnect")]
            public static extern void AsyncTextRenderedDisconnect(HandleRef textLabelRef, HandleRef handler);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncNaturalSizeComputedSignal_Connect")]
            public static extern void AsyncNaturalSizeComputedConnect(HandleRef textLabelRef, HandleRef handler);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncNaturalSizeComputedSignal_Disconnect")]
            public static extern void AsyncNaturalSizeComputedDisconnect(HandleRef textLabelRef, HandleRef handler);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncHeightForWidthComputedSignal_Connect")]
            public static extern void AsyncHeightForWidthComputedConnect(HandleRef textLabelRef, HandleRef handler);

            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_AsyncHeightForWidthComputedSignal_Disconnect")]
            public static extern void AsyncHeightForWidthComputedDisconnect(HandleRef textLabelRef, HandleRef handler);
            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_RegisterFontVariationProperty")]
            public static extern int RegisterFontVariationProperty(HandleRef textLabelRef, string pTag);
            [DllImport(NDalicPINVOKE.Lib, EntryPoint = "CSharp_Dali_TextLabel_SetMaskEffect")]
            public static extern void SetMaskEffect(HandleRef textLabelRef, HandleRef control);
        }
    }
}
