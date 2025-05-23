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
extern alias TizenSystemSettings;
using TizenSystemSettings.Tizen.System;

using System;
using System.Globalization;
using System.ComponentModel;
using Tizen.NUI.Binding;
using Tizen.NUI.Text;

namespace Tizen.NUI.BaseComponents
{
    /// <summary>
    /// A control which provides a single line editable text field.
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public partial class TextField : View
    {
        static private string defaultStyleName = "Tizen.NUI.BaseComponents.TextField";
        static private string defaultFontFamily = "BreezeSans";
        private string textFieldTextSid;
        private string textFieldPlaceHolderTextSid;
        private string textFieldPlaceHolderTextFocusedSid;
        private InputMethodContext inputMethodCotext;
        private string fontFamily = defaultFontFamily;
        private float fontSizeScale = 1.0f;
        private bool hasSystemLanguageChanged;
        private bool hasSystemFontSizeChanged;
        private bool hasSystemFontTypeChanged;
        private bool isSettingTextInCSharp;

        private Vector4 internalPlaceholderTextColor;
        private Vector4 internalPrimaryCursorColor;
        private Vector4 internalSecondaryCursorColor;
        private Vector4 internalSelectionHighlightColor;
        private Vector4 internalInputColor;
        private Color internalTextColor;
        private Color internalGrabHandleColor;


        static TextField() 
        { 
            if (NUIApplication.IsUsingXaml)
            {
                TranslatableTextProperty = BindableProperty.Create(nameof(TranslatableText), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalTranslatableTextProperty, defaultValueCreator: GetInternalTranslatableTextProperty);
                    
                TranslatablePlaceholderTextProperty = BindableProperty.Create(nameof(TranslatablePlaceholderText), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalTranslatablePlaceholderTextProperty, defaultValueCreator: GetInternalTranslatablePlaceholderTextProperty);
                    
                TranslatablePlaceholderTextFocusedProperty = BindableProperty.Create(nameof(TranslatablePlaceholderTextFocused), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalTranslatablePlaceholderTextFocusedProperty, defaultValueCreator: GetInternalTranslatablePlaceholderTextFocusedProperty);
                    
                TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalTextProperty, defaultValueCreator: GetInternalTextProperty);
                    
                PlaceholderTextProperty = BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalPlaceholderTextProperty, defaultValueCreator: GetInternalPlaceholderTextProperty);
                    
                PlaceholderTextFocusedProperty = BindableProperty.Create(nameof(PlaceholderTextFocused), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalPlaceholderTextFocusedProperty, defaultValueCreator: GetInternalPlaceholderTextFocusedProperty);
                    
                FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalFontFamilyProperty, defaultValueCreator: GetInternalFontFamilyProperty);
                    
                FontStyleProperty = BindableProperty.Create(nameof(FontStyle), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalFontStyleProperty, defaultValueCreator: GetInternalFontStyleProperty);
                    
                PointSizeProperty = BindableProperty.Create(nameof(PointSize), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalPointSizeProperty, defaultValueCreator: GetInternalPointSizeProperty);
                    
                MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(TextField), default(int), 
                    propertyChanged: SetInternalMaxLengthProperty, defaultValueCreator: GetInternalMaxLengthProperty);
                    
                ExceedPolicyProperty = BindableProperty.Create(nameof(ExceedPolicy), typeof(int), typeof(TextField), default(int), 
                    propertyChanged: SetInternalExceedPolicyProperty, defaultValueCreator: GetInternalExceedPolicyProperty);
                    
                HorizontalAlignmentProperty = BindableProperty.Create(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(TextField), HorizontalAlignment.Begin, 
                    propertyChanged: SetInternalHorizontalAlignmentProperty, defaultValueCreator: GetInternalHorizontalAlignmentProperty);
                    
                VerticalAlignmentProperty = BindableProperty.Create(nameof(TextField.VerticalAlignment), typeof(VerticalAlignment), typeof(TextField), VerticalAlignment.Bottom, 
                    propertyChanged: SetInternalVerticalAlignmentProperty, defaultValueCreator: GetInternalVerticalAlignmentProperty);
                    
                TextColorProperty = BindableProperty.Create(nameof(TextField.TextColor), typeof(Color), typeof(TextField), null, 
                    propertyChanged: SetInternalTextColorProperty, defaultValueCreator: GetInternalTextColorProperty);
                    
                PlaceholderTextColorProperty = BindableProperty.Create(nameof(TextField.PlaceholderTextColor), typeof(Vector4), typeof(TextField), null, 
                    propertyChanged: SetInternalPlaceholderTextColorProperty, defaultValueCreator: GetInternalPlaceholderTextColorProperty);
                    
                EnableGrabHandleProperty = BindableProperty.Create(nameof(TextField.EnableGrabHandle), typeof(bool), typeof(TextField), true, 
                    propertyChanged: SetInternalEnableGrabHandleProperty, defaultValueCreator: GetInternalEnableGrabHandleProperty);
                    
                EnableGrabHandlePopupProperty = BindableProperty.Create(nameof(TextField.EnableGrabHandlePopup), typeof(bool), typeof(TextField), true, 
                    propertyChanged: SetInternalEnableGrabHandlePopupProperty, defaultValueCreator: GetInternalEnableGrabHandlePopupProperty);
                    
                PrimaryCursorColorProperty = BindableProperty.Create(nameof(TextField.PrimaryCursorColor), typeof(Vector4), typeof(TextField), null, 
                    propertyChanged: SetInternalPrimaryCursorColorProperty, defaultValueCreator: GetInternalPrimaryCursorColorProperty);
                    
                SecondaryCursorColorProperty = BindableProperty.Create(nameof(TextField.SecondaryCursorColor), typeof(Vector4), typeof(TextField), null, 
                    propertyChanged: SetInternalSecondaryCursorColorProperty, defaultValueCreator: GetInternalSecondaryCursorColorProperty);
                    
                EnableCursorBlinkProperty = BindableProperty.Create(nameof(TextField.EnableCursorBlink), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalEnableCursorBlinkProperty, defaultValueCreator: GetInternalEnableCursorBlinkProperty);
                    
                CursorBlinkIntervalProperty = BindableProperty.Create(nameof(TextField.CursorBlinkInterval), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalCursorBlinkIntervalProperty, defaultValueCreator: GetInternalCursorBlinkIntervalProperty);
                    
                CursorBlinkDurationProperty = BindableProperty.Create(nameof(TextField.CursorBlinkDuration), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalCursorBlinkDurationProperty, defaultValueCreator: GetInternalCursorBlinkDurationProperty);
                    
                CursorWidthProperty = BindableProperty.Create(nameof(TextField.CursorWidth), typeof(int), typeof(TextField), default(int), 
                    propertyChanged: SetInternalCursorWidthProperty, defaultValueCreator: GetInternalCursorWidthProperty);
                    
                GrabHandleImageProperty = BindableProperty.Create(nameof(TextField.GrabHandleImage), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalGrabHandleImageProperty, defaultValueCreator: GetInternalGrabHandleImageProperty);
                    
                GrabHandlePressedImageProperty = BindableProperty.Create(nameof(TextField.GrabHandlePressedImage), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalGrabHandlePressedImageProperty, defaultValueCreator: GetInternalGrabHandlePressedImageProperty);
                    
                ScrollThresholdProperty = BindableProperty.Create(nameof(TextField.ScrollThreshold), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalScrollThresholdProperty, defaultValueCreator: GetInternalScrollThresholdProperty);
                    
                ScrollSpeedProperty = BindableProperty.Create(nameof(TextField.ScrollSpeed), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalScrollSpeedProperty, defaultValueCreator: GetInternalScrollSpeedProperty);
                    
                SelectionPopupStyleProperty = BindableProperty.Create(nameof(SelectionPopupStyle), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionPopupStyleProperty, defaultValueCreator: GetInternalSelectionPopupStyleProperty);
                    
                SelectionHandleImageLeftProperty = BindableProperty.Create(nameof(TextField.SelectionHandleImageLeft), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandleImageLeftProperty, defaultValueCreator: GetInternalSelectionHandleImageLeftProperty);
                    
                SelectionHandleImageRightProperty = BindableProperty.Create(nameof(TextField.SelectionHandleImageRight), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandleImageRightProperty, defaultValueCreator: GetInternalSelectionHandleImageRightProperty);
                    
                SelectionHandlePressedImageLeftProperty = BindableProperty.Create(nameof(TextField.SelectionHandlePressedImageLeft), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandlePressedImageLeftProperty, defaultValueCreator: GetInternalSelectionHandlePressedImageLeftProperty);
                    
                SelectionHandlePressedImageRightProperty = BindableProperty.Create(nameof(TextField.SelectionHandlePressedImageRight), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandlePressedImageRightProperty, defaultValueCreator: GetInternalSelectionHandlePressedImageRightProperty);
                    
                SelectionHandleMarkerImageLeftProperty = BindableProperty.Create(nameof(TextField.SelectionHandleMarkerImageLeft), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandleMarkerImageLeftProperty, defaultValueCreator: GetInternalSelectionHandleMarkerImageLeftProperty);
                    
                SelectionHandleMarkerImageRightProperty = BindableProperty.Create(nameof(TextField.SelectionHandleMarkerImageRight), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHandleMarkerImageRightProperty, defaultValueCreator: GetInternalSelectionHandleMarkerImageRightProperty);
                    
                SelectionHighlightColorProperty = BindableProperty.Create(nameof(TextField.SelectionHighlightColor), typeof(Vector4), typeof(TextField), null, 
                    propertyChanged: SetInternalSelectionHighlightColorProperty, defaultValueCreator: GetInternalSelectionHighlightColorProperty);
                    
                DecorationBoundingBoxProperty = BindableProperty.Create(nameof(TextField.DecorationBoundingBox), typeof(Rectangle), typeof(TextField), null, 
                    propertyChanged: SetInternalDecorationBoundingBoxProperty, defaultValueCreator: GetInternalDecorationBoundingBoxProperty);
                    
                InputMethodSettingsProperty = BindableProperty.Create(nameof(TextField.InputMethodSettings), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalInputMethodSettingsProperty, defaultValueCreator: GetInternalInputMethodSettingsProperty);
                    
                InputColorProperty = BindableProperty.Create(nameof(TextField.InputColor), typeof(Vector4), typeof(TextField), null, 
                    propertyChanged: SetInternalInputColorProperty, defaultValueCreator: GetInternalInputColorProperty);
                    
                EnableMarkupProperty = BindableProperty.Create(nameof(TextField.EnableMarkup), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalEnableMarkupProperty, defaultValueCreator: GetInternalEnableMarkupProperty);
                    
                InputFontFamilyProperty = BindableProperty.Create(nameof(TextField.InputFontFamily), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalInputFontFamilyProperty, defaultValueCreator: GetInternalInputFontFamilyProperty);
                    
                InputFontStyleProperty = BindableProperty.Create(nameof(TextField.InputFontStyle), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalInputFontStyleProperty, defaultValueCreator: GetInternalInputFontStyleProperty);
                    
                InputPointSizeProperty = BindableProperty.Create(nameof(TextField.InputPointSize), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalInputPointSizeProperty, defaultValueCreator: GetInternalInputPointSizeProperty);
                    
                UnderlineProperty = BindableProperty.Create(nameof(TextField.Underline), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalUnderlineProperty, defaultValueCreator: GetInternalUnderlineProperty);
                    
                InputUnderlineProperty = BindableProperty.Create(nameof(TextField.InputUnderline), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalInputUnderlineProperty, defaultValueCreator: GetInternalInputUnderlineProperty);
                    
                ShadowProperty = BindableProperty.Create(nameof(TextField.Shadow), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalShadowProperty, defaultValueCreator: GetInternalShadowProperty);
                    
                InputShadowProperty = BindableProperty.Create(nameof(TextField.InputShadow), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalInputShadowProperty, defaultValueCreator: GetInternalInputShadowProperty);
                    
                EmbossProperty = BindableProperty.Create(nameof(TextField.Emboss), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalEmbossProperty, defaultValueCreator: GetInternalEmbossProperty);
                    
                InputEmbossProperty = BindableProperty.Create(nameof(TextField.InputEmboss), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalInputEmbossProperty, defaultValueCreator: GetInternalInputEmbossProperty);
                    
                OutlineProperty = BindableProperty.Create(nameof(TextField.Outline), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalOutlineProperty, defaultValueCreator: GetInternalOutlineProperty);
                    
                InputOutlineProperty = BindableProperty.Create(nameof(TextField.InputOutline), typeof(string), typeof(TextField), string.Empty, 
                    propertyChanged: SetInternalInputOutlineProperty, defaultValueCreator: GetInternalInputOutlineProperty);
                    
                HiddenInputSettingsProperty = BindableProperty.Create(nameof(TextField.HiddenInputSettings), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalHiddenInputSettingsProperty, defaultValueCreator: GetInternalHiddenInputSettingsProperty);
                    
                PixelSizeProperty = BindableProperty.Create(nameof(TextField.PixelSize), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalPixelSizeProperty, defaultValueCreator: GetInternalPixelSizeProperty);
                    
                EnableSelectionProperty = BindableProperty.Create(nameof(TextField.EnableSelection), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalEnableSelectionProperty, defaultValueCreator: GetInternalEnableSelectionProperty);
                    
                PlaceholderProperty = BindableProperty.Create(nameof(TextField.Placeholder), typeof(PropertyMap), typeof(TextField), null, 
                    propertyChanged: SetInternalPlaceholderProperty, defaultValueCreator: GetInternalPlaceholderProperty);
                    
                EllipsisProperty = BindableProperty.Create(nameof(TextField.Ellipsis), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalEllipsisProperty, defaultValueCreator: GetInternalEllipsisProperty);
                    
                EllipsisPositionProperty = BindableProperty.Create(nameof(EllipsisPosition), typeof(EllipsisPosition), typeof(TextField), EllipsisPosition.End, 
                    propertyChanged: SetInternalEllipsisPositionProperty, defaultValueCreator: GetInternalEllipsisPositionProperty);
                    
                EnableShiftSelectionProperty = BindableProperty.Create(nameof(TextField.EnableShiftSelection), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalEnableShiftSelectionProperty, defaultValueCreator: GetInternalEnableShiftSelectionProperty);
                    
                MatchSystemLanguageDirectionProperty = BindableProperty.Create(nameof(TextField.MatchSystemLanguageDirection), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalMatchSystemLanguageDirectionProperty, defaultValueCreator: GetInternalMatchSystemLanguageDirectionProperty);
                    
                FontSizeScaleProperty = BindableProperty.Create(nameof(FontSizeScale), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalFontSizeScaleProperty, defaultValueCreator: GetInternalFontSizeScaleProperty);
                    
                EnableFontSizeScaleProperty = BindableProperty.Create(nameof(EnableFontSizeScale), typeof(bool), typeof(TextField), default(bool), 
                    propertyChanged: SetInternalEnableFontSizeScaleProperty, defaultValueCreator: GetInternalEnableFontSizeScaleProperty);
                    
                GrabHandleColorProperty = BindableProperty.Create(nameof(TextField.GrabHandleColor), typeof(Color), typeof(TextField), null, 
                    propertyChanged: SetInternalGrabHandleColorProperty, defaultValueCreator: GetInternalGrabHandleColorProperty);
                    
                ShadowOffsetProperty = BindableProperty.Create(nameof(ShadowOffset), typeof(Tizen.NUI.Vector2), typeof(Tizen.NUI.BaseComponents.TextField), null, 
                    propertyChanged: SetInternalShadowOffsetProperty, defaultValueCreator: GetInternalShadowOffsetProperty);
                    
                ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Tizen.NUI.Vector4), typeof(Tizen.NUI.BaseComponents.TextField), null, 
                    propertyChanged: SetInternalShadowColorProperty, defaultValueCreator: GetInternalShadowColorProperty);
                    
                EnableEditingProperty = BindableProperty.Create(nameof(EnableEditing), typeof(bool), typeof(Tizen.NUI.BaseComponents.TextField), false, 
                    propertyChanged: SetInternalEnableEditingProperty, defaultValueCreator: GetInternalEnableEditingProperty);
                    
                PrimaryCursorPositionProperty = BindableProperty.Create(nameof(PrimaryCursorPosition), typeof(int), typeof(Tizen.NUI.BaseComponents.TextField), 0, 
                    propertyChanged: SetInternalPrimaryCursorPositionProperty, defaultValueCreator: GetInternalPrimaryCursorPositionProperty);
                    
                CharacterSpacingProperty = BindableProperty.Create(nameof(CharacterSpacing), typeof(float), typeof(TextField), default(float), 
                    propertyChanged: SetInternalCharacterSpacingProperty, defaultValueCreator: GetInternalCharacterSpacingProperty);

                RemoveFrontInsetProperty = BindableProperty.Create(nameof(RemoveFrontInset), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalRemoveFrontInsetProperty, defaultValueCreator: GetInternalRemoveFrontInsetProperty);

                RemoveBackInsetProperty = BindableProperty.Create(nameof(RemoveBackInset), typeof(bool), typeof(TextField), false, 
                    propertyChanged: SetInternalRemoveBackInsetProperty, defaultValueCreator: GetInternalRemoveBackInsetProperty);
            }
        }

        static internal new void Preload()
        {
            // Do not call View.Preload(), since we already call it

            Property.Preload();
            // Do nothing. Just call for load static values.
        }

        /// <summary>
        /// Creates the TextField control.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public TextField() : this(Interop.TextField.New(ThemeManager.GetStyle(defaultStyleName) == null ? false : true), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Creates the TextField with setting the status of shown or hidden.
        /// </summary>
        /// <param name="shown">false : Not displayed (hidden), true : displayed (shown)</param>
        /// This will be public opened after ACR done. Before ACR, it is used as HiddenAPI (InhouseAPI).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextField(bool shown) : this(Interop.TextField.New(ThemeManager.GetStyle(defaultStyleName) == null ? false : true), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            SetVisible(shown);
        }

        /// <summary>
        /// Get attributes, it is abstract function and must be override.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ViewStyle CreateViewStyle()
        {
            return new TextFieldStyle();
        }

        internal TextField(global::System.IntPtr cPtr, bool cMemoryOwn, ViewStyle viewStyle, bool shown = true) : base(cPtr, cMemoryOwn, viewStyle)
        {
            if (!shown)
            {
                SetVisible(false);
            }
            Focusable = true;
            TextChanged += TextFieldTextChanged;
        }

        internal TextField(global::System.IntPtr cPtr, bool cMemoryOwn, bool shown = true) : base(cPtr, cMemoryOwn, null)
        {
            if (!shown)
            {
                SetVisible(false);
            }
            Focusable = true;
            TextChanged += TextFieldTextChanged;
        }

        internal enum ExceedPolicyType
        {
            ExceedPolicyOriginal,
            ExceedPolicyClip
        }

        private bool HasStyle()
        {
            return ThemeManager.GetStyle(this.GetType()) == null ? false : true;
        }

        /// <summary>
        /// The TranslatableText property.<br />
        /// The text can set the SID value.<br />
        /// </summary>
        /// <exception cref='ArgumentNullException'>
        /// ResourceManager about multilingual is null.
        /// </exception>
        /// <since_tizen> 4 </since_tizen>
        public string TranslatableText
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(TranslatableTextProperty);
                }
                else
                {
                    return translatableText;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TranslatableTextProperty, value);
                }
                else
                {
                    translatableText = value;
                }
            }
        }

        private string translatableText
        {
            get
            {
                return textFieldTextSid;
            }
            set
            {
                if (NUIApplication.MultilingualResourceManager == null)
                {
                    throw new ArgumentNullException(null, "ResourceManager about multilingual is null");
                }
                textFieldTextSid = value;
                Text = SetTranslatable(textFieldTextSid);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The TranslatablePlaceholderText property.<br />
        /// The text can set the SID value.<br />
        /// </summary>
        /// <exception cref='ArgumentNullException'>
        /// ResourceManager about multilingual is null.
        /// </exception>
        /// <since_tizen> 4 </since_tizen>
        public string TranslatablePlaceholderText
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(TranslatablePlaceholderTextProperty);
                }
                else
                {
                    return translatablePlaceholderText;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TranslatablePlaceholderTextProperty, value);
                }
                else
                {
                    translatablePlaceholderText = value;
                }
            }
        }

        private string translatablePlaceholderText
        {
            get
            {
                return textFieldPlaceHolderTextSid;
            }
            set
            {
                if (NUIApplication.MultilingualResourceManager == null)
                {
                    throw new ArgumentNullException(null, "ResourceManager about multilingual is null");
                }
                textFieldPlaceHolderTextSid = value;
                PlaceholderText = SetTranslatable(textFieldPlaceHolderTextSid);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The TranslatablePlaceholderTextFocused property.<br />
        /// The text can set the SID value.<br />
        /// </summary>
        /// <exception cref='ArgumentNullException'>
        /// ResourceManager about multilingual is null.
        /// </exception>
        /// This will be public opened in tizen_6.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TranslatablePlaceholderTextFocused
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(TranslatablePlaceholderTextFocusedProperty);
                }
                else
                {
                    return translatablePlaceholderTextFocused;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TranslatablePlaceholderTextFocusedProperty, value);
                }
                else
                {
                    translatablePlaceholderTextFocused = value;
                }
            }
        }

        private string translatablePlaceholderTextFocused
        {
            get
            {
                return textFieldPlaceHolderTextFocusedSid;
            }
            set
            {
                if (NUIApplication.MultilingualResourceManager == null)
                {
                    throw new ArgumentNullException(null, "ResourceManager about multilingual is null");
                }
                textFieldPlaceHolderTextFocusedSid = value;
                PlaceholderTextFocused = SetTranslatable(textFieldPlaceHolderTextFocusedSid);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The Text property.<br />
        /// The text to display in the UTF-8 format.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string Text
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(TextProperty);
                }
                else
                {
                    return GetInternalText();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TextProperty, value);
                }
                else
                {
                    SetInternalText(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalText(string text)
        {
            if (text != null)
            {
                isSettingTextInCSharp = true;
                Object.InternalSetPropertyString(SwigCPtr, Property.TEXT, text);
                isSettingTextInCSharp = false;
            }
        }

        private string GetInternalText()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.TEXT);
        }

        /// <summary>
        /// The PlaceholderText property.<br />
        /// The text to display when the TextField is empty and inactive. <br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string PlaceholderText
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(PlaceholderTextProperty);
                }
                else
                {
                    return GetInternalPlaceholderText();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PlaceholderTextProperty, value);
                }
                else
                {
                    SetInternalPlaceholderText(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPlaceholderText(string text)
        {
            if (text != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.PlaceholderText, text);
            }
        }

        private string GetInternalPlaceholderText()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.PlaceholderText);
        }

        /// <summary>
        /// The PlaceholderTextFocused property. <br />
        /// The text to display when the TextField is empty with input focus. <br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string PlaceholderTextFocused
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(PlaceholderTextFocusedProperty);
                }
                else
                {
                    return GetInternalPlaceholderTextFocused();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PlaceholderTextFocusedProperty, value);
                }
                else
                {
                    SetInternalPlaceholderTextFocused(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPlaceholderTextFocused(string text)
        {
            if (text != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.PlaceholderTextFocused, text);
            }
        }

        private string GetInternalPlaceholderTextFocused()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.PlaceholderTextFocused);
        }

        /// <summary>
        /// The FontFamily property.<br />
        /// The requested font family to use.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string FontFamily
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(FontFamilyProperty);
                }
                else
                {
                    return InternalFontFamily;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(FontFamilyProperty, value);
                }
                else
                {
                    InternalFontFamily = value;
                }
                NotifyPropertyChanged();
            }
        }

        private string InternalFontFamily
        {
            get
            {
                if (HasStyle())
                    return fontFamily;
                else
                    return Object.InternalGetPropertyString(this.SwigCPtr, TextField.Property.FontFamily);
            }
            set
            {
                if (string.Equals(fontFamily, value)) return;
                fontFamily = value;

                string newFontFamily;
                if (fontFamily == Tizen.NUI.FontFamily.UseSystemSetting)
                {
                    newFontFamily = SystemFontTypeChangedManager.FontType;
                    AddSystemSettingsFontTypeChanged();
                }
                else
                {
                    newFontFamily = fontFamily;
                    RemoveSystemSettingsFontTypeChanged();
                }

                SetInternalFontFamily(newFontFamily);
            }
        }

        private void SetInternalFontFamily(string fontFamily)
        {
            Object.InternalSetPropertyString(this.SwigCPtr, TextField.Property.FontFamily, (string)fontFamily);
        }

        /// <summary>
        /// The FontStyle property.<br />
        /// The requested font style to use.<br />
        /// The fontStyle map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>width (string)</term><description>The width key defines occupied by each glyph. (values: ultraCondensed, extraCondensed, condensed, semiCondensed, normal, semiExpanded, expanded, extraExpanded, ultraExpanded)</description></item>
        /// <item><term>weight (string)</term><description>The weight key defines the thickness or darkness of the glyphs. (values: thin, ultraLight, extraLight, light, demiLight, semiLight, book, normal, regular, medium, demiBold, semiBold, bold, ultraBold, extraBold, black, heavy, extraBlack)</description></item>
        /// <item><term>slant (string)</term><description>The slant key defines whether to use italics. (values: normal, roman, italic, oblique)</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap FontStyle
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(FontStyleProperty);
                }
                else
                {
                    return GetInternalFontStyle();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(FontStyleProperty, value);
                }
                else
                {
                    SetInternalFontStyle(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalFontStyle(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.FontStyle, pv);
            }
        }

        private PropertyMap GetInternalFontStyle()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.FontStyle))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set FontStyle to TextField. <br />
        /// </summary>
        /// <param name="fontStyle">The FontStyle</param>
        /// <remarks>
        /// SetFontStyle specifies the requested font style through <see cref="Tizen.NUI.Text.FontStyle"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetFontStyle method.
        /// <code>
        /// var fontStyle = new Tizen.NUI.Text.FontStyle();
        /// fontStyle.Width = FontWidthType.Expanded;
        /// fontStyle.Weight = FontWeightType.Bold;
        /// fontStyle.Slant = FontSlantType.Italic;
        /// field.SetFontStyle(fontStyle);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetFontStyle(FontStyle fontStyle)
        {
            using (var fontStyleMap = TextMapHelper.GetFontStyleMap(fontStyle))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(FontStyleProperty, fontStyleMap);
                }
                else
                {
                    SetInternalFontStyle(fontStyleMap);
                }
            }
        }

        /// <summary>
        /// Get FontStyle from TextField. <br />
        /// </summary>
        /// <returns>The FontStyle</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.FontStyle"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FontStyle GetFontStyle()
        {
            FontStyle fontStyle;
            if (NUIApplication.IsUsingXaml)
            {
                var fontStyleMap = (PropertyMap)GetValue(FontStyleProperty);
                fontStyle = TextMapHelper.GetFontStyleStruct(fontStyleMap);
            }
            else
            {
                using var fontStyleMap = GetInternalFontStyle();
                fontStyle = TextMapHelper.GetFontStyleStruct(fontStyleMap);
            }
            return fontStyle;
        }

        /// <summary>
        /// The PointSize property.<br />
        /// The size of font in points.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [Binding.TypeConverter(typeof(PointSizeTypeConverter))]
        public float PointSize
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(PointSizeProperty);
                }
                else
                {
                    return GetInternalPointSize();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PointSizeProperty, value);
                }
                else
                {
                    SetInternalPointSize(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPointSize(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.PointSize, newValue);
        }

        private float GetInternalPointSize()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.PointSize);
        }

        /// <summary>
        /// The MaxLength property.<br />
        /// The maximum number of characters that can be inserted.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public int MaxLength
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (int)GetValue(MaxLengthProperty);
                }
                else
                {
                    return GetInternalMaxLength();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(MaxLengthProperty, value);
                }
                else
                {
                    SetInternalMaxLength(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalMaxLength(int len)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.MaxLength, len);
        }

        private int GetInternalMaxLength()
        {
            return Object.InternalGetPropertyInt(SwigCPtr, Property.MaxLength);
        }

        /// <summary>
        /// The ExceedPolicy property.<br />
        /// Specifies how the text is truncated when it does not fit.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public int ExceedPolicy
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (int)GetValue(ExceedPolicyProperty);
                }
                else
                {
                    return GetInternalExceedPolicy();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ExceedPolicyProperty, value);
                }
                else
                {
                    SetInternalExceedPolicy(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalExceedPolicy(int newValue)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.ExceedPolicy, newValue);
        }

        private int GetInternalExceedPolicy()
        {
            return Object.InternalGetPropertyInt(SwigCPtr, Property.ExceedPolicy);
        }

        /// <summary>
        /// The HorizontalAlignment property.<br />
        /// The line horizontal alignment.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty);
                }
                else
                {
                    return GetInternalHorizontalAlignment();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(HorizontalAlignmentProperty, value);
                }
                else
                {
                    SetInternalHorizontalAlignment(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalHorizontalAlignment(HorizontalAlignment newValue)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.HorizontalAlignment, (int)newValue);
        }

        private HorizontalAlignment GetInternalHorizontalAlignment()
        {
            string temp = Object.InternalGetPropertyString(SwigCPtr, Property.HorizontalAlignment);
            return temp.GetValueByDescription<HorizontalAlignment>();
        }

        /// <summary>
        /// The VerticalAlignment property.<br />
        /// The line vertical alignment.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (VerticalAlignment)GetValue(VerticalAlignmentProperty);
                }
                else
                {
                    return GetInternalVerticalAlignment();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(VerticalAlignmentProperty, value);
                }
                else
                {
                    SetInternalVerticalAlignment(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalVerticalAlignment(VerticalAlignment newValue)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.VerticalAlignment, (int)newValue);
        }

        private VerticalAlignment GetInternalVerticalAlignment()
        {
            string temp = Object.InternalGetPropertyString(SwigCPtr, Property.VerticalAlignment);
            return temp.GetValueByDescription<VerticalAlignment>();
        }

        /// <summary>
        /// The TextColor property.<br />
        /// The color of the text.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.TextColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Color TextColor
        {
            get
            {
                Color temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Color)GetValue(TextColorProperty);
                }
                else
                {
                    temp = GetInternalTextColor();
                }
                return new Color(OnTextColorChanged, temp.R, temp.G, temp.B, temp.A);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TextColorProperty, value);
                }
                else
                {
                    SetInternalTextColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalTextColor(Color color)
        {
            if (color != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.TextColor, color.SwigCPtr);
            }
        }

        private Color GetInternalTextColor()
        {
            if (internalTextColor == null)
            {
                internalTextColor = new Color(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.TextColor, internalTextColor.SwigCPtr);
            return internalTextColor;
        }

        /// <summary>
        /// The PlaceholderTextColor property.
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.PlaceholderTextColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Vector4 PlaceholderTextColor
        {
            get
            {
                Vector4 temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Vector4)GetValue(PlaceholderTextColorProperty);
                }
                else
                {
                    temp = GetInternalPlaceholderTextColor();
                }
                return new Vector4(OnPlaceholderTextColorChanged, temp.X, temp.Y, temp.Z, temp.W);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PlaceholderTextColorProperty, value);
                }
                else
                {
                    SetInternalPlaceholderTextColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPlaceholderTextColor(Vector4 color)
        {
            if (color != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.PlaceholderTextColor, color.SwigCPtr);
            }
        }

        private Vector4 GetInternalPlaceholderTextColor()
        {
            if (internalPlaceholderTextColor == null)
            {
                internalPlaceholderTextColor = new Vector4(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.PlaceholderTextColor, internalPlaceholderTextColor.SwigCPtr);
            return internalPlaceholderTextColor;
        }

        /// <summary>
        /// The ShadowOffset property.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <remarks>
        /// Deprecated.(API Level 6) Use Shadow instead.
        /// The property cascade chaining set is possible. For example, this (textField.ShadowOffset.X = 0.1f;) is possible.
        /// </remarks>
        [Obsolete("Do not use this ShadowOffset(Deprecated). Use Shadow instead.")]
        public Vector2 ShadowOffset
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(ShadowOffsetProperty) as Vector2;
                }
                else
                {
                    return InternalShadowOffset;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ShadowOffsetProperty, value);
                }
                else
                {
                    InternalShadowOffset = value;
                }
            }
        }

        private Vector2 InternalShadowOffset
        {
            get
            {
                float x = 0.0f, y = 0.0f;
                using (var propertyValue = Shadow.Find(TextField.Property.SHADOW, "offset"))
                using (var shadowOffset = new Vector2())
                {
                    if (null != propertyValue)
                    {
                        propertyValue.Get(shadowOffset);
                        x = shadowOffset.X;
                        y = shadowOffset.Y;
                    }
                }
                return new Vector2(OnShadowOffsetChanged, x, y);
            }
            set
            {
                using (var map = new PropertyMap())
                {
                    map.Add("offset", value);
                    var shadowMap = Shadow;
                    shadowMap.Merge(map);
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(ShadowProperty, shadowMap);
                    }
                    else
                    {
                        SetInternalShadow(shadowMap);
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The ShadowColor property.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <remarks>
        /// Deprecated.(API Level 6) Use Shadow instead.
        /// The property cascade chaining set is possible. For example, this (textField.ShadowColor.X = 0.1f;) is possible.
        /// </remarks>
        [Obsolete("Do not use this ShadowColor(Deprecated). Use Shadow instead.")]
        public Vector4 ShadowColor
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(ShadowColorProperty) as Vector4;
                }
                else
                {
                    return InternalShadowColor;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ShadowColorProperty, value);
                }
                else
                {
                    InternalShadowColor = value;
                }
            }
        }

        private Vector4 InternalShadowColor
        {
            get
            {
                float x = 0.0f, y = 0.0f, z = 0.0f, w = 0.0f;
                using (var propertyValue = Shadow.Find(TextField.Property.SHADOW, "color"))
                using (var shadowColor = new Vector4())
                {
                    if (null != propertyValue)
                    {
                        propertyValue.Get(shadowColor);
                        x = shadowColor.X;
                        y = shadowColor.Y;
                        z = shadowColor.Z;
                        w = shadowColor.W;
                    }
                }
                return new Vector4(OnShadowColorChanged, x, y, z, w);
            }
            set
            {
                using (var map = new PropertyMap())
                {
                    map.Add("color", value);
                    var shadowMap = Shadow;
                    shadowMap.Merge(map);
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(ShadowProperty, shadowMap);
                    }
                    else
                    {
                        SetInternalShadow(shadowMap);
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The PrimaryCursorColor property.<br />
        /// The color to apply to the primary cursor.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.PrimaryCursorColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Vector4 PrimaryCursorColor
        {
            get
            {
                Vector4 temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Vector4)GetValue(PrimaryCursorColorProperty);
                }
                else
                {
                    temp = GetInternalPrimaryCursorColor();
                }
                return new Vector4(OnPrimaryCursorColorChanged, temp.X, temp.Y, temp.Z, temp.W);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PrimaryCursorColorProperty, value);
                }
                else
                {
                    SetInternalPrimaryCursorColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPrimaryCursorColor(Vector4 color)
        {
            if (color != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.PrimaryCursorColor, color.SwigCPtr);
            }
        }

        private Vector4 GetInternalPrimaryCursorColor()
        {
            if (internalPrimaryCursorColor == null)
            {
                internalPrimaryCursorColor = new Vector4(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.PrimaryCursorColor, internalPrimaryCursorColor.SwigCPtr);
            return internalPrimaryCursorColor;
        }

        /// <summary>
        /// The SecondaryCursorColor property.<br />
        /// The color to apply to the secondary cursor.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.SecondaryCursorColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Vector4 SecondaryCursorColor
        {
            get
            {
                Vector4 temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Vector4)GetValue(SecondaryCursorColorProperty);
                }
                else
                {
                    temp = GetInternalSecondaryCursorColor();
                }
                return new Vector4(OnSecondaryCursorColorChanged, temp.X, temp.Y, temp.Z, temp.W);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SecondaryCursorColorProperty, value);
                }
                else
                {
                    SetInternalSecondaryCursorColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSecondaryCursorColor(Vector4 newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.SecondaryCursorColor, newValue.SwigCPtr);
            }
        }

        private Vector4 GetInternalSecondaryCursorColor()
        {
            if (internalSecondaryCursorColor == null)
            {
                internalSecondaryCursorColor = new Vector4(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.SecondaryCursorColor, internalSecondaryCursorColor.SwigCPtr);
            return internalSecondaryCursorColor;
        }

        /// <summary>
        /// The EnableCursorBlink property.<br />
        /// Whether the cursor should blink or not.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public bool EnableCursorBlink
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableCursorBlinkProperty);
                }
                else
                {
                    return GetInternalEnableCursorBlink();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableCursorBlinkProperty, value);
                }
                else
                {
                    SetInternalEnableCursorBlink(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableCursorBlink(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableCursorBlink, newValue);
        }

        private bool GetInternalEnableCursorBlink()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableCursorBlink);
        }

        /// <summary>
        /// The CursorBlinkInterval property.<br />
        /// The time interval in seconds between cursor on/off states.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public float CursorBlinkInterval
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(CursorBlinkIntervalProperty);
                }
                else
                {
                    return GetInternalCursorBlinkInterval();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(CursorBlinkIntervalProperty, value);
                }
                else
                {
                    SetInternalCursorBlinkInterval(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalCursorBlinkInterval(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.CursorBlinkInterval, newValue);
        }

        private float GetInternalCursorBlinkInterval()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.CursorBlinkInterval);
        }

        /// <summary>
        /// The CursorBlinkDuration property.<br />
        /// The cursor will stop blinking after this number of seconds (if non-zero).<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public float CursorBlinkDuration
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(CursorBlinkDurationProperty);
                }
                else
                {
                    return GetInternalCursorBlinkDuration();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(CursorBlinkDurationProperty, value);
                }
                else
                {
                    SetInternalCursorBlinkDuration(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalCursorBlinkDuration(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.CursorBlinkDuration, newValue);
        }

        private float GetInternalCursorBlinkDuration()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.CursorBlinkDuration);
        }

        /// <summary>
        /// Gets or sets the width of the cursor.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public int CursorWidth
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (int)GetValue(CursorWidthProperty);
                }
                else
                {
                    return GetInternalCursorWidth();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(CursorWidthProperty, value);
                }
                else
                {
                    SetInternalCursorWidth(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalCursorWidth(int newValue)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.CursorWidth, newValue);
        }

        private int GetInternalCursorWidth()
        {
            return Object.InternalGetPropertyInt(SwigCPtr, Property.CursorWidth);
        }

        /// <summary>
        /// The GrabHandleImage property.<br />
        /// The image to display for the grab handle.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string GrabHandleImage
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(GrabHandleImageProperty);
                }
                else
                {
                    return GetInternalGrabHandleImage();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(GrabHandleImageProperty, value);
                }
                else
                {
                    SetInternalGrabHandleImage(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalGrabHandleImage(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.GrabHandleImage, newValue);
            }
        }

        private string GetInternalGrabHandleImage()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.GrabHandleImage);
        }

        /// <summary>
        /// The GrabHandlePressedImage property.<br />
        /// The image to display when the grab handle is pressed.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string GrabHandlePressedImage
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(GrabHandlePressedImageProperty);
                }
                else
                {
                    return GetInternalGrabHandlePressedImage();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(GrabHandlePressedImageProperty, value);
                }
                else
                {
                    SetInternalGrabHandlePressedImage(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalGrabHandlePressedImage(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.GrabHandlePressedImage, newValue);
            }
        }

        private string GetInternalGrabHandlePressedImage()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.GrabHandlePressedImage);
        }

        /// <summary>
        /// The ScrollThreshold property.<br />
        /// Horizontal scrolling will occur if the cursor is this close to the control border.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public float ScrollThreshold
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(ScrollThresholdProperty);
                }
                else
                {
                    return GetInternalScrollThreshold();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ScrollThresholdProperty, value);
                }
                else
                {
                    SetInternalScrollThreshold(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalScrollThreshold(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.ScrollThreshold, newValue);
        }

        private float GetInternalScrollThreshold()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.ScrollThreshold);
        }

        /// <summary>
        /// The ScrollSpeed property.<br />
        /// The scroll speed in pixels per second.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public float ScrollSpeed
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(ScrollSpeedProperty);
                }
                else
                {
                    return GetInternalScrollSpeed();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ScrollSpeedProperty, value);
                }
                else
                {
                    SetInternalScrollSpeed(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalScrollSpeed(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.ScrollSpeed, newValue);
        }

        private float GetInternalScrollSpeed()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.ScrollSpeed);
        }

        /// <summary>
        /// The SelectionPopupStyle property.<br />
        /// The style of the text selection popup can be set through SelectionPopupStyle property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyMap SelectionPopupStyle
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionPopupStyleProperty);
                }
                else
                {
                    return GetInternalSelectionPopupStyle();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionPopupStyleProperty, value);
                }
                else
                {
                    SetInternalSelectionPopupStyle(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionPopupStyle(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionPopupStyle, pv);
            }
        }

        private PropertyMap GetInternalSelectionPopupStyle()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionPopupStyle))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The SelectionHandleImageLeft property.<br />
        /// The image to display for the left selection handle.<br />
        /// The selectionHandleImageLeft map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandleImageLeft
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandleImageLeftProperty);
                }
                else
                {
                    return GetInternalSelectionHandleImageLeft();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandleImageLeftProperty, value);
                }
                else
                {
                    SetInternalSelectionHandleImageLeft(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandleImageLeft(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandleImageLeft, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandleImageLeft()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandleImageLeft))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The SelectionHandleImageRight property.<br />
        /// The image to display for the right selection handle.<br />
        /// The selectionHandleImageRight map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandleImageRight
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandleImageRightProperty);
                }
                else
                {
                    return GetInternalSelectionHandleImageRight();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandleImageRightProperty, value);
                }
                else
                {
                    SetInternalSelectionHandleImageRight(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandleImageRight(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandleImageRight, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandleImageRight()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandleImageRight))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set SelectionHandleImage to TextField. <br />
        /// </summary>
        /// <param name="selectionHandleImage">The SelectionHandleImage</param>
        /// <remarks>
        /// SetSelectionHandleImage specifies the display image used for the selection handle through <see cref="Tizen.NUI.Text.SelectionHandleImage"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetSelectionHandleImage method.
        /// <code>
        /// var selectionHandleImage = new Tizen.NUI.Text.SelectionHandleImage();
        /// selectionHandleImage.LeftImageUrl = "handle_downleft.png";
        /// selectionHandleImage.RightImageUrl = "handle_downright.png";
        /// field.SetSelectionHandleImage(selectionHandleImage);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetSelectionHandleImage(SelectionHandleImage selectionHandleImage)
        {
            if (!String.IsNullOrEmpty(selectionHandleImage.LeftImageUrl))
            {
                using (var leftImageMap = TextMapHelper.GetFileNameMap(selectionHandleImage.LeftImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandleImageLeftProperty, leftImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandleImageLeft(leftImageMap);
                    }
                }
            }

            if (!String.IsNullOrEmpty(selectionHandleImage.RightImageUrl))
            {
                using (var rightImageMap = TextMapHelper.GetFileNameMap(selectionHandleImage.RightImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandleImageRightProperty, rightImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandleImageRight(rightImageMap);
                    }
                }
            }
        }

        /// <summary>
        /// Get SelectionHandleImage from TextField. <br />
        /// </summary>
        /// <returns>The SelectionHandleImage</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.SelectionHandleImage"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SelectionHandleImage GetSelectionHandleImage()
        {
            SelectionHandleImage selectionHandleImage;
            if (NUIApplication.IsUsingXaml)
            {
                var leftImageMap = (PropertyMap)GetValue(SelectionHandleImageLeftProperty);
                var rightImageMap = (PropertyMap)GetValue(SelectionHandleImageRightProperty);
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            else
            {
                using var leftImageMap = GetInternalSelectionHandleImageLeft() ;
                using var rightImageMap = GetInternalSelectionHandleImageRight() ;
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            return selectionHandleImage;
        }

        /// <summary>
        /// The SelectionHandlePressedImageLeft property.<br />
        /// The image to display when the left selection handle is pressed.<br />
        /// The selectionHandlePressedImageLeft map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandlePressedImageLeft
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandlePressedImageLeftProperty);
                }
                else
                {
                    return GetInternalSelectionHandlePressedImageLeft();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandlePressedImageLeftProperty, value);
                }
                else
                {
                    SetInternalSelectionHandlePressedImageLeft(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandlePressedImageLeft(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandlePressedImageLeft, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandlePressedImageLeft()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandlePressedImageLeft))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The SelectionHandlePressedImageRight property.<br />
        /// The image to display when the right selection handle is pressed.<br />
        /// The selectionHandlePressedImageRight map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandlePressedImageRight
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandlePressedImageRightProperty);
                }
                else
                {
                    return GetInternalSelectionHandlePressedImageRight();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandlePressedImageRightProperty, value);
                }
                else
                {
                    SetInternalSelectionHandlePressedImageRight(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandlePressedImageRight(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandlePressedImageRight, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandlePressedImageRight()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandlePressedImageRight))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set SelectionHandlePressedImage to TextField. <br />
        /// </summary>
        /// <param name="selectionHandlePressedImage">The SelectionHandleImage</param>
        /// <remarks>
        /// SetSelectionHandlePressedImage specifies the display image used for the selection handle through <see cref="Tizen.NUI.Text.SelectionHandleImage"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetSelectionHandlePressedImage method.
        /// <code>
        /// var selectionHandlePressedImage = new Tizen.NUI.Text.SelectionHandleImage();
        /// selectionHandlePressedImage.LeftImageUrl = "handle_pressed_downleft.png";
        /// selectionHandlePressedImage.RightImageUrl = "handle_pressed_downright.png";
        /// field.SetSelectionHandlePressedImage(selectionHandlePressedImage);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetSelectionHandlePressedImage(SelectionHandleImage selectionHandlePressedImage)
        {
            if (!String.IsNullOrEmpty(selectionHandlePressedImage.LeftImageUrl))
            {
                using (var leftImageMap = TextMapHelper.GetFileNameMap(selectionHandlePressedImage.LeftImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandlePressedImageLeftProperty, leftImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandlePressedImageLeft(leftImageMap);
                    }
                }
            }

            if (!String.IsNullOrEmpty(selectionHandlePressedImage.RightImageUrl))
            {
                using (var rightImageMap = TextMapHelper.GetFileNameMap(selectionHandlePressedImage.RightImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandlePressedImageRightProperty, rightImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandlePressedImageRight(rightImageMap);
                    }
                }
            }
        }

        /// <summary>
        /// Get SelectionHandlePressedImage from TextField. <br />
        /// </summary>
        /// <returns>The SelectionHandlePressedImage</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.SelectionHandleImage"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SelectionHandleImage GetSelectionHandlePressedImage()
        {
            SelectionHandleImage selectionHandleImage;
            if (NUIApplication.IsUsingXaml)
            {
                var leftImageMap = (PropertyMap)GetValue(SelectionHandlePressedImageLeftProperty);
                var rightImageMap = (PropertyMap)GetValue(SelectionHandlePressedImageRightProperty);
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            else
            {
                using var leftImageMap = GetInternalSelectionHandlePressedImageLeft();
                using var rightImageMap = GetInternalSelectionHandlePressedImageRight();
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            return selectionHandleImage;
        }

        /// <summary>
        /// The SelectionHandleMarkerImageLeft property.<br />
        /// The image to display for the left selection handle marker.<br />
        /// The selectionHandleMarkerImageLeft map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandleMarkerImageLeft
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandleMarkerImageLeftProperty);
                }
                else
                {
                    return GetInternalSelectionHandleMarkerImageLeft();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandleMarkerImageLeftProperty, value);
                }
                else
                {
                    SetInternalSelectionHandleMarkerImageLeft(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandleMarkerImageLeft(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandleMarkerImageLeft, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandleMarkerImageLeft()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandleMarkerImageLeft))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The SelectionHandleMarkerImageRight property.<br />
        /// The image to display for the right selection handle marker.<br />
        /// The selectionHandleMarkerImageRight map contains the following key :<br />
        /// <list type="table">
        /// <item><term>filename (string)</term><description>The path of image file</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap SelectionHandleMarkerImageRight
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(SelectionHandleMarkerImageRightProperty);
                }
                else
                {
                    return GetInternalSelectionHandleMarkerImageRight();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHandleMarkerImageRightProperty, value);
                }
                else
                {
                    SetInternalSelectionHandleMarkerImageRight(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHandleMarkerImageRight(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.SelectionHandleMarkerImageRight, pv);
            }
        }

        private PropertyMap GetInternalSelectionHandleMarkerImageRight()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SelectionHandleMarkerImageRight))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set SelectionHandleMarkerImage to TextField. <br />
        /// </summary>
        /// <param name="selectionHandleMarkerImage">The SelectionHandleImage</param>
        /// <remarks>
        /// SetSelectionHandleMarkerImage specifies the display image used for the selection handle through <see cref="Tizen.NUI.Text.SelectionHandleImage"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetSelectionHandleMarkerImage method.
        /// <code>
        /// var selectionHandleMarkerImage = new Tizen.NUI.Text.SelectionHandleImage();
        /// selectionHandleMarkerImage.LeftImageUrl = "handle_pressed_downleft.png";
        /// selectionHandleMarkerImage.RightImageUrl = "handle_pressed_downright.png";
        /// field.SetSelectionHandleMarkerImage(selectionHandleMarkerImage);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetSelectionHandleMarkerImage(SelectionHandleImage selectionHandleMarkerImage)
        {
            if (!String.IsNullOrEmpty(selectionHandleMarkerImage.LeftImageUrl))
            {
                using (var leftImageMap = TextMapHelper.GetFileNameMap(selectionHandleMarkerImage.LeftImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandleMarkerImageLeftProperty, leftImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandleMarkerImageLeft(leftImageMap);
                    }
                }
            }

            if (!String.IsNullOrEmpty(selectionHandleMarkerImage.RightImageUrl))
            {
                using (var rightImageMap = TextMapHelper.GetFileNameMap(selectionHandleMarkerImage.RightImageUrl))
                {
                    if (NUIApplication.IsUsingXaml)
                    {
                        SetValue(SelectionHandleMarkerImageRightProperty, rightImageMap);
                    }
                    else
                    {
                        SetInternalSelectionHandleMarkerImageRight(rightImageMap);
                    }
                }
            }
        }

        /// <summary>
        /// Get SelectionHandleMarkerImage from TextField. <br />
        /// </summary>
        /// <returns>The SelectionHandleMarkerImage</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.SelectionHandleImage"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SelectionHandleImage GetSelectionHandleMarkerImage()
        {
            SelectionHandleImage selectionHandleImage;
            if (NUIApplication.IsUsingXaml)
            {
                var leftImageMap = (PropertyMap)GetValue(SelectionHandleMarkerImageLeftProperty);
                var rightImageMap = (PropertyMap)GetValue(SelectionHandleMarkerImageRightProperty);
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            else
            {
                using var leftImageMap = GetInternalSelectionHandleMarkerImageLeft();
                using var rightImageMap = GetInternalSelectionHandleMarkerImageRight();
                selectionHandleImage = TextMapHelper.GetSelectionHandleImageStruct(leftImageMap, rightImageMap);
            }
            return selectionHandleImage;
        }

        /// <summary>
        /// The SelectionHighlightColor property.<br />
        /// The color of the selection highlight.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.SelectionHighlightColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Vector4 SelectionHighlightColor
        {
            get
            {
                Vector4 temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Vector4)GetValue(SelectionHighlightColorProperty);
                }
                else
                {
                    temp = GetInternalSelectionHighlightColor();
                }
                return new Vector4(OnSelectionHighlightColorChanged, temp.X, temp.Y, temp.Z, temp.W);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(SelectionHighlightColorProperty, value);
                }
                else
                {
                    SetInternalSelectionHighlightColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalSelectionHighlightColor(Vector4 newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.SelectionHighlightColor, newValue.SwigCPtr);
            }
        }

        private Vector4 GetInternalSelectionHighlightColor()
        {
            if (internalSelectionHighlightColor == null)
            {
                internalSelectionHighlightColor = new Vector4(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.SelectionHighlightColor, internalSelectionHighlightColor.SwigCPtr);
            return internalSelectionHighlightColor;
        }

        /// <summary>
        /// The DecorationBoundingBox property.<br />
        /// The decorations (handles etc) will positioned within this area on-screen.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.DecorationBoundingBox.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Rectangle DecorationBoundingBox
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    Rectangle temp = (Rectangle)GetValue(DecorationBoundingBoxProperty);
                    return new Rectangle(OnDecorationBoundingBoxChanged, temp.X, temp.Y, temp.Width, temp.Height);
                }
                else
                {
                    using Rectangle temp = GetInternalDecorationBoundingBox();
                    return new Rectangle(OnDecorationBoundingBoxChanged, temp.X, temp.Y, temp.Width, temp.Height);
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(DecorationBoundingBoxProperty, value);
                }
                else
                {
                    SetInternalDecorationBoundingBox(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalDecorationBoundingBox(Rectangle newValue)
        {
            if (newValue != null)
            {
                using var pv = new PropertyValue(newValue);
                Object.SetProperty(SwigCPtr, Property.DecorationBoundingBox, pv);
            }
        }

        private Rectangle GetInternalDecorationBoundingBox()
        {
            Rectangle temp = new Rectangle(0, 0, 0, 0);
            using (var prop = Object.GetProperty(SwigCPtr, Property.DecorationBoundingBox))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The InputMethodSettings property.<br />
        /// The settings to relating to the System's Input Method, Key and Value.<br />
        /// </summary>
        /// <remarks>
        /// <see cref="InputMethod"/> is a class encapsulating the input method map. Use the <see cref="InputMethod"/> class for this property.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the InputMethodSettings property.
        /// <code>
        /// InputMethod method = new InputMethod();
        /// method.PanelLayout = InputMethod.PanelLayoutType.Normal;
        /// method.ActionButton = InputMethod.ActionButtonTitleType.Default;
        /// method.AutoCapital = InputMethod.AutoCapitalType.Word;
        /// method.Variation = 1;
        /// textField.InputMethodSettings = method.OutputMap;
        /// </code>
        /// </example>
        /// <since_tizen> 3 </since_tizen>
        public PropertyMap InputMethodSettings
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(InputMethodSettingsProperty);
                }
                else
                {
                    return GetInternalInputMethodSettings();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputMethodSettingsProperty, value);
                }
                else
                {
                    SetInternalInputMethodSettings(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputMethodSettings(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.InputMethodSettings, pv);
            }
        }

        private PropertyMap GetInternalInputMethodSettings()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.InputMethodSettings))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// The InputColor property.<br />
        /// The color of the new input text.<br />
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.InputColor.X = 0.1f;) is possible.
        /// </remarks>
        /// <since_tizen> 3 </since_tizen>
        public Vector4 InputColor
        {
            get
            {
                Vector4 temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Vector4)GetValue(InputColorProperty);
                }
                else
                {
                    temp = GetInternalInputColor();
                }
                return new Vector4(OnInputColorChanged, temp.X, temp.Y, temp.Z, temp.W);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputColorProperty, value);
                }
                else
                {
                    SetInternalInputColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputColor(Vector4 newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.InputColor, newValue.SwigCPtr);
            }
        }

        private Vector4 GetInternalInputColor()
        {
            if (internalInputColor == null)
            {
                internalInputColor = new Vector4(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.InputColor, internalInputColor.SwigCPtr);
            return internalInputColor;
        }

        /// <summary>
        /// The EnableMarkup property.<br />
        /// Whether the mark-up processing is enabled.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public bool EnableMarkup
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableMarkupProperty);
                }
                else
                {
                    return GetInternalEnableMarkup();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableMarkupProperty, value);
                }
                else
                {
                    SetInternalEnableMarkup(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableMarkup(bool enabled)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableMarkup, enabled);
        }

        private bool GetInternalEnableMarkup()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableMarkup);
        }

        /// <summary>
        /// The InputFontFamily property.<br />
        /// The font's family of the new input text.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string InputFontFamily
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(InputFontFamilyProperty);
                }
                else
                {
                    return GetInternalInputFontFamily();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputFontFamilyProperty, value);
                }
                else
                {
                    SetInternalInputFontFamily(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputFontFamily(string family)
        {
            if (family != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.InputFontFamily, family);
            }
        }

        private string GetInternalInputFontFamily()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.InputFontFamily);
        }

        /// <summary>
        /// The InputFontStyle property.<br />
        /// The font's style of the new input text.<br />
        /// The inputFontStyle map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>width (string)</term><description>The width key defines occupied by each glyph. (values: ultraCondensed, extraCondensed, condensed, semiCondensed, normal, semiExpanded, expanded, extraExpanded, ultraExpanded)</description></item>
        /// <item><term>weight (string)</term><description>The weight key defines the thickness or darkness of the glyphs. (values: thin, ultraLight, extraLight, light, demiLight, semiLight, book, normal, regular, medium, demiBold, semiBold, bold, ultraBold, extraBold, black, heavy, extraBlack)</description></item>
        /// <item><term>slant (string)</term><description>The slant key defines whether to use italics. (values: normal, roman, italic, oblique)</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap InputFontStyle
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(InputFontStyleProperty);
                }
                else
                {
                    return GetInternalInputFontStyle();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputFontStyleProperty, value);
                }
                else
                {
                    SetInternalInputFontStyle(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputFontStyle(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.InputFontStyle, pv);
            }
        }

        private PropertyMap GetInternalInputFontStyle()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.InputFontStyle))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set InputFontStyle to TextField. <br />
        /// </summary>
        /// <param name="fontStyle">The FontStyle</param>
        /// <remarks>
        /// SetInputFontStyle specifies the requested font style for new input text through <see cref="Tizen.NUI.Text.FontStyle"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetInputFontStyle method.
        /// <code>
        /// var fontStyle = new Tizen.NUI.Text.FontStyle();
        /// fontStyle.Width = FontWidthType.Expanded;
        /// fontStyle.Weight = FontWeightType.Bold;
        /// fontStyle.Slant = FontSlantType.Italic;
        /// field.SetInputFontStyle(fontStyle);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetInputFontStyle(FontStyle fontStyle)
        {
            using (var fontStyleMap = TextMapHelper.GetFontStyleMap(fontStyle))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputFontStyleProperty, fontStyleMap);
                }
                else
                {
                    SetInternalInputFontStyle(fontStyleMap);
                }
            }
       }

        /// <summary>
        /// Get InputFontStyle from TextField. <br />
        /// </summary>
        /// <returns>The FontStyle</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.FontStyle"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FontStyle GetInputFontStyle()
        {
            FontStyle fontStyle;
            if (NUIApplication.IsUsingXaml)
            {
                var fontStyleMap = (PropertyMap)GetValue(InputFontStyleProperty);
                fontStyle = TextMapHelper.GetFontStyleStruct(fontStyleMap);
            }
            else
            {
                using var fontStyleMap = GetInternalInputFontStyle();
                fontStyle = TextMapHelper.GetFontStyleStruct(fontStyleMap);
            }
            return fontStyle;
        }

        /// <summary>
        /// The InputPointSize property.<br />
        /// The font's size of the new input text in points.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [Binding.TypeConverter(typeof(PointSizeTypeConverter))]
        public float InputPointSize
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(InputPointSizeProperty);
                }
                else
                {
                    return GetInternalInputPointSize();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputPointSizeProperty, value);
                }
                else
                {
                    SetInternalInputPointSize(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputPointSize(float pointSize)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.InputPointSize, pointSize);
        }

        private float GetInternalInputPointSize()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.InputPointSize);
        }

        /// <summary>
        /// The Underline property.<br />
        /// The default underline parameters.<br />
        /// The underline map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>enable (bool)</term><description>Whether the underline is enabled (the default value is false)</description></item>
        /// <item><term>color (Color)</term><description>The color of the underline (If not provided then the color of the text is used)</description></item>
        /// <item><term>height (float)</term><description>The height in pixels of the underline (the default value is 1.f)</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap Underline
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(UnderlineProperty);
                }
                else
                {
                    return GetInternalUnderline();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(UnderlineProperty, value);
                }
                else
                {
                    SetInternalUnderline(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalUnderline(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.UNDERLINE, pv);
            }
        }

        private PropertyMap GetInternalUnderline()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.UNDERLINE))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set Underline to TextField. <br />
        /// </summary>
        /// <param name="underline">The Underline</param>
        /// <remarks>
        /// SetUnderline specifies the underline of the text through <see cref="Tizen.NUI.Text.Underline"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetUnderline method.
        /// <code>
        /// var underline = new Tizen.NUI.Text.Underline();
        /// underline.Enable = true;
        /// underline.Color = new Color("#3498DB");
        /// underline.Height = 2.0f;
        /// field.SetUnderline(underline);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetUnderline(Underline underline)
        {
            using (var underlineMap = TextMapHelper.GetUnderlineMap(underline))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(UnderlineProperty, underlineMap);
                }
                else
                {
                    SetInternalUnderline(underlineMap);
                }
            }
        }

        /// <summary>
        /// Get Underline from TextField. <br />
        /// </summary>
        /// <returns>The Underline</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.Underline"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Underline GetUnderline()
        {
            Underline underline;
            if (NUIApplication.IsUsingXaml)
            {
                var underlineMap = (PropertyMap)GetValue(UnderlineProperty);
                underline = TextMapHelper.GetUnderlineStruct(underlineMap);
            }
            else
            {
                using var underlineMap = GetInternalUnderline();
                underline = TextMapHelper.GetUnderlineStruct(underlineMap);
            }
            return underline;
        }

        /// <summary>
        /// The InputUnderline property.<br />
        /// The underline parameters of the new input text.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string InputUnderline
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(InputUnderlineProperty);
                }
                else
                {
                    return GetInternalInputUnderline();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputUnderlineProperty, value);
                }
                else
                {
                    SetInternalInputUnderline(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputUnderline(string line)
        {
            if (line != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.InputUnderline, line);
            }
        }

        private string GetInternalInputUnderline()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.InputUnderline);
        }

        /// <summary>
        /// The Shadow property.<br />
        /// The default shadow parameters.<br />
        /// The shadow map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>color (Color)</term><description>The color of the shadow (the default color is Color.Black)</description></item>
        /// <item><term>offset (Vector2)</term><description>The offset in pixels of the shadow (If not provided then the shadow is not enabled)</description></item>
        /// <item><term>blurRadius (float)</term><description>The radius of the Gaussian blur for the soft shadow (If not provided then the soft shadow is not enabled)</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap Shadow
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(ShadowProperty);
                }
                else
                {
                    return GetInternalShadow();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ShadowProperty, value);
                }
                else
                {
                    SetInternalShadow(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalShadow(PropertyMap shadow)
        {
            if (shadow != null)
            {
                using var pv = new PropertyValue(shadow);
                Object.SetProperty(SwigCPtr, Property.SHADOW, pv);
            }
        }

        private PropertyMap GetInternalShadow()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.SHADOW))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set Shadow to TextField. <br />
        /// </summary>
        /// <param name="shadow">The Shadow</param>
        /// <remarks>
        /// SetShadow specifies the shadow of the text through <see cref="Tizen.NUI.Text.Shadow"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetShadow method.
        /// <code>
        /// var shadow = new Tizen.NUI.Text.Shadow();
        /// shadow.Offset = new Vector2(3, 3);
        /// shadow.Color = new Color("#F1C40F");
        /// field.SetShadow(shadow);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetShadow(Tizen.NUI.Text.Shadow shadow)
        {
            using (var shadowMap = TextMapHelper.GetShadowMap(shadow))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ShadowProperty, shadowMap);
                }
                else
                {
                    SetInternalShadow(shadowMap);
                }
            }
        }

        /// <summary>
        /// Get Shadow from TextField. <br />
        /// </summary>
        /// <returns>The Shadow</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.Shadow"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Text.Shadow GetShadow()
        {
            Text.Shadow shadow;
            if (NUIApplication.IsUsingXaml)
            {
                var shadowMap = (PropertyMap)GetValue(ShadowProperty);
                shadow = TextMapHelper.GetShadowStruct(shadowMap);
            }
            else
            {
                using var shadowMap = GetInternalShadow();
                shadow = TextMapHelper.GetShadowStruct(shadowMap);
            }
            return shadow;
        }

        /// <summary>
        /// The InputShadow property.<br />
        /// The shadow parameters of the new input text.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string InputShadow
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(InputShadowProperty);
                }
                else
                {
                    return GetInternalInputShadow();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputShadowProperty, value);
                }
                else
                {
                    SetInternalInputShadow(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputShadow(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.InputShadow, newValue);
            }
        }

        private string GetInternalInputShadow()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.InputShadow);
        }

        /// <summary>
        /// The Emboss property.<br />
        /// The default emboss parameters.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string Emboss
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(EmbossProperty);
                }
                else
                {
                    return GetInternalEmboss();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EmbossProperty, value);
                }
                else
                {
                    SetInternalEmboss(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEmboss(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.EMBOSS, newValue);
            }
        }

        private string GetInternalEmboss()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.EMBOSS);
        }

        /// <summary>
        /// The InputEmboss property.<br />
        /// The emboss parameters of the new input text.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string InputEmboss
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(InputEmbossProperty);
                }
                else
                {
                    return GetInternalInputEmboss();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputEmbossProperty, value);
                }
                else
                {
                    SetInternalInputEmboss(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputEmboss(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.InputEmboss, newValue);
            }
        }

        private string GetInternalInputEmboss()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.InputEmboss);
        }

        /// <summary>
        /// The Outline property.<br />
        /// The default outline parameters.<br />
        /// The outline map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>color (Color)</term><description>The color of the outline (the default color is Color.White)</description></item>
        /// <item><term>width (float)</term><description>The width in pixels of the outline (If not provided then the outline is not enabled)</description></item>
        /// </list>
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap Outline
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(OutlineProperty);
                }
                else
                {
                    return GetInternalOutline();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(OutlineProperty, value);
                }
                else
                {
                    SetInternalOutline(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalOutline(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.OUTLINE, pv);
            }
        }

        private PropertyMap GetInternalOutline()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.OUTLINE))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set Outline to TextField. <br />
        /// </summary>
        /// <param name="outline">The Outline</param>
        /// <remarks>
        /// SetOutline specifies the outline of the text through <see cref="Tizen.NUI.Text.Outline"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetOutline method.
        /// <code>
        /// var outline = new Tizen.NUI.Text.Outline();
        /// outline.Width = 2.0f;
        /// outline.Color = new Color("#45B39D");
        /// field.SetOutline(outline);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetOutline(Outline outline)
        {
            using (var outlineMap = TextMapHelper.GetOutlineMap(outline))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(OutlineProperty, outlineMap);
                }
                else
                {
                    SetInternalOutline(outlineMap);
                }
            }
        }

        /// <summary>
        /// Get Outline from TextField. <br />
        /// </summary>
        /// <returns>The Outline</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.Outline"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Outline GetOutline()
        {
            Outline outline;
            if (NUIApplication.IsUsingXaml)
            {
                var outlineMap = (PropertyMap)GetValue(OutlineProperty);
                outline = TextMapHelper.GetOutlineStruct(outlineMap);
            }
            else
            {
                using var outlineMap = GetInternalOutline();
                outline = TextMapHelper.GetOutlineStruct(outlineMap);
            }
            return outline;
        }

        /// <summary>
        /// The InputOutline property.<br />
        /// The outline parameters of the new input text.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string InputOutline
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (string)GetValue(InputOutlineProperty);
                }
                else
                {
                    return GetInternalInputOutline();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(InputOutlineProperty, value);
                }
                else
                {
                    SetInternalInputOutline(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalInputOutline(string newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyString(SwigCPtr, Property.InputOutline, newValue);
            }
        }

        private string GetInternalInputOutline()
        {
            return Object.InternalGetPropertyString(SwigCPtr, Property.InputOutline);
        }

        /// <summary>
        /// The HiddenInputSettings property.<br />
        /// Hides the input characters and instead shows a default character for password or pin entry.<br />
        /// The hiddenInputSettings map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>HiddenInputProperty.Mode (int)</term><description>The mode for input text display (Use HiddenInputModeType)</description></item>
        /// <item><term>HiddenInputProperty.SubstituteCharacter (int)</term><description>All input characters are substituted by this character</description></item>
        /// <item><term>HiddenInputProperty.SubstituteCount (int)</term><description>Length of text to show or hide, available when HideCount/ShowCount mode is used</description></item>
        /// <item><term>HiddenInputProperty.ShowLastCharacterDuration (int)</term><description>Hide last character after this duration, available when ShowLastCharacter mode</description></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// See <see cref="HiddenInputProperty"/> and <see cref="HiddenInputModeType"/> for a detailed description.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the HiddenInputSettings property.
        /// <code>
        /// PropertyMap map = new PropertyMap();
        /// map.Add(HiddenInputProperty.Mode, new PropertyValue((int)HiddenInputModeType.ShowLastCharacter));
        /// map.Add(HiddenInputProperty.ShowLastCharacterDuration, new PropertyValue(500));
        /// map.Add(HiddenInputProperty.SubstituteCharacter, new PropertyValue(0x2A));
        /// textField.HiddenInputSettings = map;
        /// </code>
        /// </example>
        /// <since_tizen> 3 </since_tizen>
        public Tizen.NUI.PropertyMap HiddenInputSettings
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(HiddenInputSettingsProperty);
                }
                else
                {
                    return GetInternalHiddenInputSettings();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(HiddenInputSettingsProperty, value);
                }
                else
                {
                    SetInternalHiddenInputSettings(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalHiddenInputSettings(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.HiddenInputSettings, pv);
            }
        }

        private PropertyMap GetInternalHiddenInputSettings()
        {
            PropertyMap temp = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.HiddenInputSettings))
            {
                prop.Get(temp);
            }
            return temp;
        }

        /// <summary>
        /// Set HiddenInput to TextField. <br />
        /// </summary>
        /// <param name="hiddenInput">The HiddenInput</param>
        /// <remarks>
        /// SetHiddenInput specifies the requested font style through <see cref="Tizen.NUI.Text.HiddenInput"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetHiddenInput method.
        /// <code>
        /// var hiddenInput = new Tizen.NUI.Text.HiddenInput();
        /// hiddenInput.Mode = HiddenInputModeType.ShowLastCharacter;
        /// hiddenInput.SubstituteCharacter = '★';
        /// hiddenInput.SubstituteCount = 0;
        /// hiddenInput.ShowLastCharacterDuration = 1000;
        /// field.SetHiddenInput(hiddenInput);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetHiddenInput(HiddenInput hiddenInput)
        {
            using (var hiddenInputMap = TextMapHelper.GetHiddenInputMap(hiddenInput))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(HiddenInputSettingsProperty, hiddenInputMap);
                }
                else
                {
                    SetInternalHiddenInputSettings(hiddenInputMap);
                }
            }
        }

        /// <summary>
        /// Get HiddenInput from TextField. <br />
        /// </summary>
        /// <returns>The HiddenInput</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.HiddenInput"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HiddenInput GetHiddenInput()
        {
            HiddenInput hiddenInput;
            if (NUIApplication.IsUsingXaml)
            {
                var hiddenInputMap = (PropertyMap)GetValue(HiddenInputSettingsProperty);
                hiddenInput = TextMapHelper.GetHiddenInputStruct(hiddenInputMap);
            }
            else
            {
                using var hiddenInputMap = GetInternalHiddenInputSettings();
                hiddenInput = TextMapHelper.GetHiddenInputStruct(hiddenInputMap);
            }
            return hiddenInput;
        }

        /// <summary>
        /// The PixelSize property.<br />
        /// The size of font in pixels.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        [Binding.TypeConverter(typeof(FloatGraphicsTypeConverter))]
        public float PixelSize
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(PixelSizeProperty);
                }
                else
                {
                    return GetInternalPixelSize();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PixelSizeProperty, value);
                }
                else
                {
                    SetInternalPixelSize(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPixelSize(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.PixelSize, newValue);
        }

        private float GetInternalPixelSize()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.PixelSize);
        }

        /// <summary>
        /// The Enable selection property.<br />
        /// Enables Text selection, such as the cursor, handle, clipboard, and highlight color.<br />
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public bool EnableSelection
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableSelectionProperty);
                }
                else
                {
                    return GetInternalEnableSelection();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableSelectionProperty, value);
                }
                else
                {
                    SetInternalEnableSelection(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableSelection(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableSelection, newValue);
        }

        private bool GetInternalEnableSelection()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableSelection);
        }

        /// <summary>
        /// The Enable grab handle property.<br />
        /// Enables the grab handles for text selection.<br />
        /// The default value is true, which means the grab handles are enabled by default.<br />
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, it is used as HiddenAPI (InhouseAPI).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableGrabHandle
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableGrabHandleProperty);
                }
                else
                {
                    return GetInternalEnableGrabHandle();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableGrabHandleProperty, value);
                }
                else
                {
                    SetInternalEnableGrabHandle(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableGrabHandle(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableGrabHandle, newValue);
        }

        private bool GetInternalEnableGrabHandle()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableGrabHandle);
        }

        /// <summary>
        /// The Enable grab handle popup property.<br />
        /// Enables the grab handle popup for text selection.<br />
        /// The default value is true, which means the grab handle popup is enabled by default.<br />
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, it is used as HiddenAPI (InhouseAPI).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableGrabHandlePopup
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableGrabHandlePopupProperty);
                }
                else
                {
                    return GetInternalEnableGrabHandlePopup();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableGrabHandlePopupProperty, value);
                }
                else
                {
                    SetInternalEnableGrabHandlePopup(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableGrabHandlePopup(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableGrabHandlePopup, newValue);
        }

        private bool GetInternalEnableGrabHandlePopup()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableGrabHandlePopup);
        }

        /// <summary>
        /// The portion of the text that has been selected by the user.
        /// </summary>
        /// <remarks>
        /// Empty string when nothing is selected.
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        public string SelectedText
        {
            get
            {
                string selectedText;
                using (var propertyValue = GetProperty(TextField.Property.SelectedText))
                {
                    propertyValue.Get(out selectedText);
                }
                return selectedText;
            }
        }

        /// <summary>
        /// The start index for selection.
        /// </summary>
        /// <remarks>
        /// When there is no selection, the index is current cursor position.
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        public int SelectedTextStart
        {
            get
            {
                int selectedTextStart;
                using (var propertyValue = GetProperty(TextField.Property.SelectedTextStart))
                {
                    propertyValue.Get(out selectedTextStart);
                }
                return selectedTextStart;
            }
        }

        /// <summary>
        /// The end index for selection.
        /// </summary>
        /// <remarks>
        /// When there is no selection, the index is current cursor position.
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        public int SelectedTextEnd
        {
            get
            {
                int selectedTextEnd;
                using (var propertyValue = GetProperty(TextField.Property.SelectedTextEnd))
                {
                    propertyValue.Get(out selectedTextEnd);
                }
                return selectedTextEnd;
            }
        }

        /// <summary>
        /// Enable editing in text control.
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, it is used as HiddenAPI (InhouseAPI).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableEditing
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableEditingProperty);
                }
                else
                {
                    return InternalEnableEditing;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableEditingProperty, value);
                }
                else
                {
                    InternalEnableEditing = value;
                }
            }
        }

        private bool InternalEnableEditing
        {
            get
            {
                bool enableEditing;
                using (var propertyValue = GetProperty(TextField.Property.EnableEditing))
                {
                    propertyValue.Get(out enableEditing);
                }
                return enableEditing;
            }
            set
            {
                using (var propertyValue = new PropertyValue(value))
                {
                    SetProperty(TextField.Property.EnableEditing, propertyValue);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// PrimaryCursorPosition property.<br />
        /// Specify the position of the primary cursor (caret) in text control.
        /// </summary>
        /// <remarks>
        /// If the value set is out of range (negative or greater than or equal the number of characters in Text) then the PrimaryCursorPosition is moved to the end of Text (the number of characters in Text).
        /// </remarks>
        /// <since_tizen> 10 </since_tizen>
        public int PrimaryCursorPosition
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (int)GetValue(PrimaryCursorPositionProperty);
                }
                else
                {
                    return InternalPrimaryCursorPosition;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PrimaryCursorPositionProperty, value);
                }
                else
                {
                    InternalPrimaryCursorPosition = value;
                }
            }
        }

        private int InternalPrimaryCursorPosition
        {
            get
            {
                int primaryCursorPosition;
                using (var propertyValue = GetProperty(TextField.Property.PrimaryCursorPosition))
                {
                    propertyValue.Get(out primaryCursorPosition);
                }
                return primaryCursorPosition;
            }
            set
            {
                using (PropertyValue propertyValue = new PropertyValue(value))
                {
                    SetProperty(TextField.Property.PrimaryCursorPosition, propertyValue);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The GrabHandleColor property.
        /// </summary>
        /// <remarks>
        /// The property cascade chaining set is possible. For example, this (textField.GrabHandleColor.X = 0.1f;) is possible.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color GrabHandleColor
        {
            get
            {
                Color temp;
                if (NUIApplication.IsUsingXaml)
                {
                    temp = (Color)GetValue(GrabHandleColorProperty);
                }
                else
                {
                    temp = GetInternalGrabHandleColor();
                }
                return new Color(OnGrabHandleColorChanged, temp.R, temp.G, temp.B, temp.A);
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(GrabHandleColorProperty, value);
                }
                else
                {
                    SetInternalGrabHandleColor(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalGrabHandleColor(Color newValue)
        {
            if (newValue != null)
            {
                Object.InternalSetPropertyVector4(SwigCPtr, Property.GrabHandleColor, newValue.SwigCPtr);
            }
        }

        private Color GetInternalGrabHandleColor()
        {
            if (internalGrabHandleColor == null)
            {
                internalGrabHandleColor = new Color(0, 0, 0, 0);
            }
            Object.InternalRetrievingPropertyVector4(SwigCPtr, Property.GrabHandleColor, internalGrabHandleColor.SwigCPtr);
            return internalGrabHandleColor;
        }

        /// <summary>
        /// The ellipsis position of the text.
        /// Specifies which portion of the text should be replaced with an ellipsis when the text size exceeds the layout size.<br />
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public EllipsisPosition EllipsisPosition
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (EllipsisPosition)GetValue(EllipsisPositionProperty);
                }
                else
                {
                    return GetInternalEllipsisPosition();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EllipsisPositionProperty, value);
                }
                else
                {
                    SetInternalEllipsisPosition(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEllipsisPosition(EllipsisPosition newValue)
        {
            Object.InternalSetPropertyInt(SwigCPtr, Property.EllipsisPosition, (int)newValue);
        }

        private EllipsisPosition GetInternalEllipsisPosition()
        {
            return (EllipsisPosition)Object.InternalGetPropertyInt(SwigCPtr, Property.EllipsisPosition);
        }

        /// <summary>
        /// The spaces between characters in Pixels.
        /// <remarks>
        /// A positive value will make the characters far apart (expanded) and a negative value will bring them closer (condensed).<br />
        /// The default value is 0.f which does nothing.
        ///</remarks>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public float CharacterSpacing
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(CharacterSpacingProperty);
                }
                else
                {
                    return GetInternalCharacterSpacing();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(CharacterSpacingProperty, value);
                }
                else
                {
                    SetInternalCharacterSpacing(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalCharacterSpacing(float newValue)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, Property.CharacterSpacing, newValue);
        }

        private float GetInternalCharacterSpacing()
        {
            return Object.InternalGetPropertyFloat(SwigCPtr, Property.CharacterSpacing);
        }

        /// <summary>
        /// Set InputFilter to TextField.
        /// </summary>
        /// <param name="inputFilter">The InputFilter</param>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.InputFilter"/> filters input based on regular expressions. <br />
        /// InputFiltered signal is emitted when the input is filtered by InputFilter <br />
        /// See <see cref="InputFiltered"/>, <see cref="InputFilterType"/> and <see cref="InputFilteredEventArgs"/> for a detailed description.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetInputFilter method.
        /// <code>
        /// var inputFilter = new Tizen.NUI.Text.InputFilter();
        /// inputFilter.Accepted = @"[\d]"; // accept whole digits
        /// inputFilter.Rejected = "[0-3]"; // reject 0, 1, 2, 3
        /// field.SetInputFilter(inputFilter); // acceptable inputs are 4, 5, 6, 7, 8, 9
        /// </code>
        /// </example>
        /// <since_tizen> 9 </since_tizen>
        public void SetInputFilter(InputFilter inputFilter)
        {
            using (var map = TextMapHelper.GetInputFilterMap(inputFilter))
            using (var propertyValue = new PropertyValue(map))
            {
                SetProperty(TextField.Property.InputFilter, propertyValue);
            }
        }

        /// <summary>
        /// Get InputFilter from TextField.
        /// </summary>
        /// <returns>The InputFilter</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.InputFilter"/>
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        public InputFilter GetInputFilter()
        {
            InputFilter inputFilter;
            using (var propertyValue = GetProperty(TextField.Property.InputFilter))
            using (var map = new PropertyMap())
            {
                propertyValue.Get(map);
                inputFilter = TextMapHelper.GetInputFilterStruct(map);
            }
            return inputFilter;
        }

        /// <summary>
        /// Set Strikethrough to TextField. <br />
        /// </summary>
        /// <param name="strikethrough">The Strikethrough</param>
        /// <remarks>
        /// SetStrikethrough specifies the strikethrough of the text through <see cref="Tizen.NUI.Text.Strikethrough"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetStrikethrough method.
        /// <code>
        /// var strikethrough = new Tizen.NUI.Text.Strikethrough();
        /// strikethrough.Enable = true;
        /// strikethrough.Color = new Color("#3498DB");
        /// strikethrough.Height = 2.0f;
        /// field.SetStrikethrough(strikethrough);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetStrikethrough(Strikethrough strikethrough)
        {
            using (var map = TextMapHelper.GetStrikethroughMap(strikethrough))
            using (var propertyValue = new PropertyValue(map))
            {
                SetProperty(TextField.Property.Strikethrough, propertyValue);
            }
        }

        /// <summary>
        /// Get Strikethrough from TextField. <br />
        /// </summary>
        /// <returns>The Strikethrough</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.Strikethrough"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Strikethrough GetStrikethrough()
        {
            Strikethrough strikethrough;
            using (var propertyValue = GetProperty(TextField.Property.Strikethrough))
            using (var map = new PropertyMap())
            {
                propertyValue.Get(map);
                strikethrough = TextMapHelper.GetStrikethroughStruct(map);
            }
            return strikethrough;
        }

        /// <summary>
        /// The Placeholder property.
        /// The placeholder map contains the following keys :<br />
        /// <list type="table">
        /// <item><term>text (string)</term><description>The text to display when the TextField is empty and inactive</description></item>
        /// <item><term>textFocused (string)</term><description>The text to display when the placeholder has focus</description></item>
        /// <item><term>color (Color)</term><description>The color of the placeholder text</description></item>
        /// <item><term>fontFamily (string)</term><description>The fontFamily of the placeholder text</description></item>
        /// <item><term>fontStyle (PropertyMap)</term><description>The fontStyle of the placeholder text</description></item>
        /// <item><term>pointSize (float)</term><description>The pointSize of the placeholder text</description></item>
        /// <item><term>pixelSize (float)</term><description>The pixelSize of the placeholder text</description></item>
        /// <item><term>ellipsis (bool)</term><description>The ellipsis of the placeholder text</description></item>
        /// </list>
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the Placeholder property.
        /// <code>
        /// PropertyMap propertyMap = new PropertyMap();
        /// propertyMap.Add("text", new PropertyValue("Setting Placeholder Text"));
        /// propertyMap.Add("textFocused", new PropertyValue("Setting Placeholder Text Focused"));
        /// propertyMap.Add("color", new PropertyValue(Color.Red));
        /// propertyMap.Add("fontFamily", new PropertyValue("Arial"));
        /// propertyMap.Add("pointSize", new PropertyValue(12.0f));
        ///
        /// PropertyMap fontStyleMap = new PropertyMap();
        /// fontStyleMap.Add("weight", new PropertyValue("bold"));
        /// fontStyleMap.Add("width", new PropertyValue("condensed"));
        /// fontStyleMap.Add("slant", new PropertyValue("italic"));
        /// propertyMap.Add("fontStyle", new PropertyValue(fontStyleMap));
        ///
        /// TextField field = new TextField();
        /// field.Placeholder = propertyMap;
        /// </code>
        /// </example>
        /// <since_tizen> 3 </since_tizen>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721: Property names should not match get methods")]
        public PropertyMap Placeholder
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (PropertyMap)GetValue(PlaceholderProperty);
                }
                else
                {
                    return GetInternalPlaceholder();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PlaceholderProperty, value);
                }
                else
                {
                    SetInternalPlaceholder(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalPlaceholder(PropertyMap map)
        {
            if (map != null)
            {
                using var pv = new PropertyValue(map);
                Object.SetProperty(SwigCPtr, Property.PLACEHOLDER, pv);
            }
        }

        private PropertyMap GetInternalPlaceholder()
        {
            PropertyMap map = new PropertyMap();
            using (var prop = Object.GetProperty(SwigCPtr, Property.PLACEHOLDER))
            {
                prop.Get(map);
            }

            string defalutText = "";

            if (TextMapHelper.IsValue(map, 0))
                map.Add("text", TextMapHelper.GetStringFromMap(map, 0, defalutText));

            if (TextMapHelper.IsValue(map, 1))
                map.Add("textFocused", TextMapHelper.GetStringFromMap(map, 1, defalutText));

            if (TextMapHelper.IsValue(map, 2))
            {
                using (var color = TextMapHelper.GetColorFromMap(map, 2))
                {
                    map.Add("color", color);
                }
            }

            if (TextMapHelper.IsValue(map, 3))
                map.Add("fontFamily", TextMapHelper.GetStringFromMap(map, 3, defalutText));

            if (TextMapHelper.IsValue(map, 4))
            {
                using (var properyValue = map.Find(4))
                using (var fontStyle = new PropertyMap())
                {
                    properyValue.Get(fontStyle);
                    map.Add("fontStyle", fontStyle);
                }
            }

            if (TextMapHelper.IsValue(map, 5))
                map.Add("pointSize", TextMapHelper.GetNullableFloatFromMap(map, 5));

            if (TextMapHelper.IsValue(map, 6))
                map.Add("pixelSize", TextMapHelper.GetNullableFloatFromMap(map, 6));

            if (TextMapHelper.IsValue(map, 7))
                map.Add("ellipsis", TextMapHelper.GetBoolFromMap(map, 7, false));

            return map;
        }

        /// <summary>
        /// Set Placeholder to TextField. <br />
        /// </summary>
        /// <param name="placeholder">The Placeholder</param>
        /// <remarks>
        /// SetPlaceholder specifies the attributes of the placeholder property through <see cref="Tizen.NUI.Text.Placeholder"/>. <br />
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetPlaceholder method.
        /// <code>
        /// var placeholder = new Tizen.NUI.Text.Placeholder();
        /// placeholder.Text = "placeholder text";
        /// placeholder.TextFocused = "placeholder textFocused";
        /// placeholder.Color = new Color("#45B39D");
        /// placeholder.FontFamily = "BreezeSans";
        /// placeholder.FontStyle = new Tizen.NUI.Text.FontStyle()
        /// {
        ///     Width = FontWidthType.Expanded,
        ///     Weight = FontWeightType.ExtraLight,
        ///     Slant = FontSlantType.Italic,
        /// };
        /// placeholder.PointSize = 25.0f;
        /// //placeholder.PixelSize = 50.0f;
        /// placeholder.Ellipsis = true;
        /// field.SetPlaceholder(placeholder);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetPlaceholder(Placeholder placeholder)
        {
            using (var placeholderMap = TextMapHelper.GetPlaceholderMap(placeholder))
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(PlaceholderProperty, placeholderMap);
                }
                else
                {
                    SetInternalPlaceholder(placeholderMap);
                }
            }
        }

        /// <summary>
        /// Get Placeholder from TextField. <br />
        /// </summary>
        /// <returns>The Placeholder</returns>
        /// <remarks>
        /// <see cref="Tizen.NUI.Text.Placeholder"/>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Placeholder GetPlaceholder()
        {
            Placeholder placeholder;
            if (NUIApplication.IsUsingXaml)
            {
                var placeholderMap = (PropertyMap)GetValue(PlaceholderProperty);
                placeholder = TextMapHelper.GetPlaceholderStruct(placeholderMap);
            }
            else
            {
                using var placeholderMap = GetInternalPlaceholder();
                placeholder = TextMapHelper.GetPlaceholderStruct(placeholderMap);
            }
            return placeholder;
        }

        /// <summary>
        /// The Ellipsis property.<br />
        /// Enable or disable the ellipsis.<br />
        /// Placeholder PropertyMap is used to add ellipsis to placeholder text.
        /// </summary>
        /// <since_tizen> 4 </since_tizen>
        public bool Ellipsis
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EllipsisProperty);
                }
                else
                {
                    return GetInternalEllipsis();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EllipsisProperty, value);
                }
                else
                {
                    SetInternalEllipsis(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEllipsis(bool ellipsis)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.ELLIPSIS, ellipsis);
        }

        private bool GetInternalEllipsis()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.ELLIPSIS);
        }

        /// <summary>
        /// Enables selection of the text using the Shift key.
        /// </summary>
        /// This will be public opened after ACR done. Before ACR, it is used as HiddenAPI (InhouseAPI).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableShiftSelection
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableShiftSelectionProperty);
                }
                else
                {
                    return GetInternalEnableShiftSelection();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableShiftSelectionProperty, value);
                }
                else
                {
                    SetInternalEnableShiftSelection(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableShiftSelection(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableShiftSelection, newValue);
        }

        private bool GetInternalEnableShiftSelection()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableShiftSelection);
        }

        /// <summary>
        /// The text alignment to match the direction of the system language.<br />
        /// The default value is true.<br />
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public bool MatchSystemLanguageDirection
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(MatchSystemLanguageDirectionProperty);
                }
                else
                {
                    return GetInternalMatchSystemLanguageDirection();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(MatchSystemLanguageDirectionProperty, value);
                }
                else
                {
                    SetInternalMatchSystemLanguageDirection(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalMatchSystemLanguageDirection(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.MatchSystemLanguageDirection, newValue);
        }

        private bool GetInternalMatchSystemLanguageDirection()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.MatchSystemLanguageDirection);
        }

        /// <summary>
        /// The FontSizeScale property. <br />
        /// The default value is 1.0. <br />
        /// The given font size scale value is used for multiplying the specified font size before querying fonts. <br />
        /// If FontSizeScale.UseSystemSetting, will use the SystemSettings.FontSize internally. <br />
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public float FontSizeScale
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (float)GetValue(FontSizeScaleProperty);
                }
                else
                {
                    return InternalFontSizeScale;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(FontSizeScaleProperty, value);
                }
                else
                {
                    InternalFontSizeScale = value;
                }
                NotifyPropertyChanged();
            }
        }

        private float InternalFontSizeScale
        {
            get
            {
                return fontSizeScale;
            }
            set
            {
                if (fontSizeScale == value) return;
                fontSizeScale = value;

                float newFontSizeScale;
                if (fontSizeScale == Tizen.NUI.FontSizeScale.UseSystemSetting)
                {
                    var systemSettingsFontSize = SystemFontSizeChangedManager.FontSize;
                    newFontSizeScale = TextUtils.GetFontSizeScale(systemSettingsFontSize);
                    AddSystemSettingsFontSizeChanged();
                }
                else
                {
                    newFontSizeScale = fontSizeScale;
                    RemoveSystemSettingsFontSizeChanged();
                }

                SetInternalFontSizeScale(newFontSizeScale);
            }
        }

        private void SetInternalFontSizeScale(float fontSizeScale)
        {

            Object.InternalSetPropertyFloat(this.SwigCPtr, TextField.Property.FontSizeScale, (float)fontSizeScale);
        }

        /// <summary>
        /// The EnableFontSizeScale property.<br />
        /// Whether the font size scale is enabled. (The default value is true)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool EnableFontSizeScale
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(EnableFontSizeScaleProperty);
                }
                else
                {
                    return GetInternalEnableFontSizeScale();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(EnableFontSizeScaleProperty, value);
                }
                else
                {
                    SetInternalEnableFontSizeScale(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalEnableFontSizeScale(bool enable)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.EnableFontSizeScale, enable);
        }

        private bool GetInternalEnableFontSizeScale()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.EnableFontSizeScale);
        }

        /// <summary>
        /// Get the InputMethodContext instance.
        /// </summary>
        /// <returns>The InputMethodContext instance.</returns>
        /// <since_tizen> 5 </since_tizen>
        public InputMethodContext GetInputMethodContext()
        {
            if (inputMethodCotext == null)
            {
                /*Avoid raising InputMethodContext reference count.*/
                inputMethodCotext = new InputMethodContext(Interop.TextField.GetInputMethodContext(SwigCPtr), true);
                if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            }
            return inputMethodCotext;
        }

        /// <summary>
        /// Selects the entire text within the TextField.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SelectWholeText()
        {
            Interop.TextField.SelectWholeText(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Select text from start to end index. <br />
        /// The index is valid when 0 or positive.
        /// </summary>
        /// <param name="start">The start index for selection.</param>
        /// <param name="end">The end index for selection.</param>
        /// <remarks>
        /// If the end index exceeds the maximum value, it is set to the length of the text.
        /// </remarks>
        /// <since_tizen> 9 </since_tizen>
        public void SelectText(int start, int end)
        {
            if (start < 0)
                throw new global::System.ArgumentOutOfRangeException(nameof(start), "Value is less than zero");
            if (end < 0)
                throw new global::System.ArgumentOutOfRangeException(nameof(end), "Value is less than zero");

            Interop.TextField.SelectText(SwigCPtr, (uint)start, (uint)end);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// Clear selection of the text. <br />
        /// Valid when selection is activate.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public void SelectNone()
        {
            _ = Interop.TextField.SelectNone(SwigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        /// <summary>
        /// The RemoveFrontInset property.<br />
        /// This property is used when the xBearing of first glyph must not be trimmed.<br />
        /// When set to false, The gap between (0, 0) from the first glyph's leftmost pixel is included in the width of text label.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool RemoveFrontInset
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(RemoveFrontInsetProperty);
                }
                else
                {
                    return GetInternalRemoveFrontInset();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(RemoveFrontInsetProperty, value);
                }
                else 
                {
                    SetInternalRemoveFrontInset(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalRemoveFrontInset(bool newValue)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.RemoveFrontInset, newValue);
        }

        private bool GetInternalRemoveFrontInset()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.RemoveFrontInset);
        }

        /// <summary>
        /// The RemoveBackInset property.<br />
        /// This property is used when the advance of last glyph must not be trimmed.<br />
        /// When set to false, The gap between the last glyph's rightmost pixel and X coordinate that next glyph will be placed is included in the width of text label.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool RemoveBackInset
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(RemoveBackInsetProperty);
                }
                else
                {
                    return GetInternalRemoveBackInset();
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(RemoveBackInsetProperty, value);
                }
                else 
                {
                    SetInternalRemoveBackInset(value);
                }
                NotifyPropertyChanged();
            }
        }

        private void SetInternalRemoveBackInset(bool remove)
        {
            Object.InternalSetPropertyBool(SwigCPtr, Property.RemoveBackInset, remove);
        }

        private bool GetInternalRemoveBackInset()
        {
            return Object.InternalGetPropertyBool(SwigCPtr, Property.RemoveBackInset);
        }

        /// <summary>
        /// Registers FontVariationsProperty with string tag.
        /// </summary>
        /// <param name="tag">The tag of font variation.</param>
        /// <returns>The index of the font variation.</returns>
        /// <remarks>
        /// The returned index can be used with setting property or animations.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the SetFontStyle method.
        /// <code>
        /// TextField field = new TextField();
        /// int index = field.RegisterFontVariationProperty("wght");
        /// Animation anim = new Animation(1000);
        /// anim.AnimateTo(field, "wght", 900.0f);
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int RegisterFontVariationProperty(string tag)
        {
            int index = Interop.TextField.RegisterFontVariationProperty(SwigCPtr, tag);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return index;
        }

        /// <summary>
        /// Sets Font Variation with string tag.
        /// </summary>
        /// <param name="tag">The tag of font variation.</param>
        /// <param name="value">The value of font variation.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetFontVariation(string tag, float value)
        {
            int index = RegisterFontVariationProperty(tag);
            Object.InternalSetPropertyFloat(SwigCPtr, index, value);
        }

        /// <summary>
        /// Sets Font Variation with index.
        /// </summary>
        /// <param name="index">The index of font variation property.</param>
        /// <param name="value">The value of font variation.</param>
        /// <remarks>
        /// To use the index, RegisterFontVariationProperty must precede it.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetFontVariation(int index, float value)
        {
            Object.InternalSetPropertyFloat(SwigCPtr, index, value);
        }

        /// <summary>
        /// Dispose.
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <remarks>
        /// When overriding this method, you need to distinguish between explicit and implicit conditions. For explicit conditions, release both managed and unmanaged resources. For implicit conditions, only release unmanaged resources.
        /// </remarks>
        /// <param name="type">Explicit to release both managed and unmanaged resources. Implicit to release only unmanaged resources.</param>
        /// <since_tizen> 3 </since_tizen>
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            internalPlaceholderTextColor?.Dispose();
            internalPrimaryCursorColor?.Dispose();
            internalSecondaryCursorColor?.Dispose();
            internalSelectionHighlightColor?.Dispose();
            internalInputColor?.Dispose();
            internalTextColor?.Dispose();
            internalGrabHandleColor?.Dispose();

            RemoveSystemSettingsLocaleLanguageChanged();
            RemoveSystemSettingsFontTypeChanged();
            RemoveSystemSettingsFontSizeChanged();

            if (type == DisposeTypes.Explicit)
            {
                //Called by User
                //Release your own managed resources here.
                //You should release all of your own disposable objects here.
            }

            //Release your own unmanaged resources here.
            //You should not access any managed member here except static instance.
            //because the execution order of Finalizes is non-deterministic.
            if (this.HasBody())
            {
                if (textFieldCursorPositionChangedCallbackDelegate != null)
                {
                    using var signal = CursorPositionChangedSignal();
                    signal.Disconnect(textFieldCursorPositionChangedCallbackDelegate);
                }

                if (textFieldMaxLengthReachedCallbackDelegate != null)
                {
                    using var signal = MaxLengthReachedSignal();
                    signal.Disconnect(textFieldMaxLengthReachedCallbackDelegate);
                }

                if (textFieldSelectionStartedCallbackDelegate != null)
                {
                    using var signal = SelectionStartedSignal();
                    signal.Disconnect(textFieldSelectionStartedCallbackDelegate);
                }

                if (textFieldSelectionClearedCallbackDelegate != null)
                {
                    using var signal = SelectionClearedSignal();
                    signal.Disconnect(textFieldSelectionClearedCallbackDelegate);
                }

                if (textFieldSelectionChangedCallbackDelegate != null)
                {
                    using var signal = SelectionChangedSignal();
                    signal.Disconnect(textFieldSelectionChangedCallbackDelegate);
                }

                if (textFieldTextChangedCallbackDelegate != null)
                {
                    using var signal = TextChangedSignal();
                    signal.Disconnect(textFieldTextChangedCallbackDelegate);
                }
            }

            TextChanged -= TextFieldTextChanged;
            GetInputMethodContext()?.DestroyContext();

            base.Dispose(type);
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.TextField.DeleteTextField(swigCPtr);
        }

        internal override LayoutItem CreateDefaultLayout()
        {
            return new TextFieldLayout();
        }

        internal void SetTextWithoutTextChanged(string text)
        {
            invokeTextChanged = false;
            using var pv = new Tizen.NUI.PropertyValue(text);
            Tizen.NUI.Object.SetProperty((System.Runtime.InteropServices.HandleRef)SwigCPtr, TextField.Property.TEXT, pv);
            invokeTextChanged = true;
        }

        private string SetTranslatable(string textFieldSid)
        {
            string translatableText = null;
            translatableText = NUIApplication.MultilingualResourceManager?.GetString(textFieldSid, new CultureInfo(SystemLocaleLanguageChangedManager.LocaleLanguage.Replace("_", "-")));
            if (translatableText != null)
            {
                AddSystemSettingsLocaleLanguageChanged();
                return translatableText;
            }
            else
            {
                translatableText = "";
                RemoveSystemSettingsLocaleLanguageChanged();
                return translatableText;
            }
        }

        private void SystemSettingsLocaleLanguageChanged(object sender, LocaleLanguageChangedEventArgs e)
        {
            if (textFieldTextSid != null)
            {
                Text = NUIApplication.MultilingualResourceManager?.GetString(textFieldTextSid, new CultureInfo(e.Value.Replace("_", "-")));
            }
            if (textFieldPlaceHolderTextSid != null)
            {
                PlaceholderText = NUIApplication.MultilingualResourceManager?.GetString(textFieldPlaceHolderTextSid, new CultureInfo(e.Value.Replace("_", "-")));
            }
            if (textFieldPlaceHolderTextFocusedSid != null)
            {
                PlaceholderTextFocused = NUIApplication.MultilingualResourceManager?.GetString(textFieldPlaceHolderTextFocusedSid, new CultureInfo(e.Value.Replace("_", "-")));
            }
        }

        private void AddSystemSettingsLocaleLanguageChanged()
        {
            if (!hasSystemLanguageChanged)
            {
                SystemLocaleLanguageChangedManager.Add(SystemSettingsLocaleLanguageChanged);
                hasSystemLanguageChanged = true;
            }
        }
        
        private void RemoveSystemSettingsLocaleLanguageChanged()
        {
            if (hasSystemLanguageChanged)
            {
                SystemLocaleLanguageChangedManager.Remove(SystemSettingsLocaleLanguageChanged);
                hasSystemLanguageChanged = false;
            }
        }

        private void SystemSettingsFontSizeChanged(object sender, FontSizeChangedEventArgs e)
        {
            float newFontSizeScale = TextUtils.GetFontSizeScale(e.Value);
            SetInternalFontSizeScale(newFontSizeScale);
        }

        private void AddSystemSettingsFontSizeChanged()
        {
            if (!hasSystemFontSizeChanged)
            {
                SystemFontSizeChangedManager.Add(SystemSettingsFontSizeChanged);
                hasSystemFontSizeChanged = true;
            }
        }

        private void RemoveSystemSettingsFontSizeChanged()
        {
            if (hasSystemFontSizeChanged)
            {
                SystemFontSizeChangedManager.Remove(SystemSettingsFontSizeChanged);
                hasSystemFontSizeChanged = false;
            }
        }

        private void SystemSettingsFontTypeChanged(object sender, FontTypeChangedEventArgs e)
        {
            SetInternalFontFamily(e.Value);
        }

        private void AddSystemSettingsFontTypeChanged()
        {
            if (HasStyle() && !hasSystemFontTypeChanged)
            {
                SystemFontTypeChangedManager.Add(SystemSettingsFontTypeChanged);
                hasSystemFontTypeChanged = true;
            }
        }
        
        private void RemoveSystemSettingsFontTypeChanged()
        {
            if (hasSystemFontTypeChanged)
            {
                SystemFontTypeChangedManager.Remove(SystemSettingsFontTypeChanged);
                hasSystemFontTypeChanged = false;
            }
        }

        private void TextFieldTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isSettingTextInCSharp)
            {
                EnforceNotifyBindedInstance(TextProperty);
            }
        }

        internal new class Property
        {
            internal static readonly int TEXT = Interop.TextField.TextGet();
            internal static readonly int PlaceholderText = Interop.TextField.PlaceholderTextGet();
            internal static readonly int PlaceholderTextFocused = Interop.TextField.PlaceholderTextFocusedGet();
            internal static readonly int FontFamily = Interop.TextField.FontFamilyGet();
            internal static readonly int FontStyle = Interop.TextField.FontStyleGet();
            internal static readonly int PointSize = Interop.TextField.PointSizeGet();
            internal static readonly int MaxLength = Interop.TextField.MaxLengthGet();
            internal static readonly int ExceedPolicy = Interop.TextField.ExceedPolicyGet();
            internal static readonly int HorizontalAlignment = Interop.TextField.HorizontalAlignmentGet();
            internal static readonly int VerticalAlignment = Interop.TextField.VerticalAlignmentGet();
            internal static readonly int TextColor = Interop.TextField.TextColorGet();
            internal static readonly int PlaceholderTextColor = Interop.TextField.PlaceholderTextColorGet();
            internal static readonly int PrimaryCursorColor = Interop.TextField.PrimaryCursorColorGet();
            internal static readonly int SecondaryCursorColor = Interop.TextField.SecondaryCursorColorGet();
            internal static readonly int EnableCursorBlink = Interop.TextField.EnableCursorBlinkGet();
            internal static readonly int CursorBlinkInterval = Interop.TextField.CursorBlinkIntervalGet();
            internal static readonly int CursorBlinkDuration = Interop.TextField.CursorBlinkDurationGet();
            internal static readonly int CursorWidth = Interop.TextField.CursorWidthGet();
            internal static readonly int GrabHandleImage = Interop.TextField.GrabHandleImageGet();
            internal static readonly int GrabHandlePressedImage = Interop.TextField.GrabHandlePressedImageGet();
            internal static readonly int ScrollThreshold = Interop.TextField.ScrollThresholdGet();
            internal static readonly int ScrollSpeed = Interop.TextField.ScrollSpeedGet();
            internal static readonly int SelectionPopupStyle = Interop.TextField.SelectionPopupStyleGet();
            internal static readonly int SelectionHandleImageLeft = Interop.TextField.SelectionHandleImageLeftGet();
            internal static readonly int SelectionHandleImageRight = Interop.TextField.SelectionHandleImageRightGet();
            internal static readonly int SelectionHandlePressedImageLeft = Interop.TextField.SelectionHandlePressedImageLeftGet();
            internal static readonly int SelectionHandlePressedImageRight = Interop.TextField.SelectionHandlePressedImageRightGet();
            internal static readonly int SelectionHandleMarkerImageLeft = Interop.TextField.SelectionHandleMarkerImageLeftGet();
            internal static readonly int SelectionHandleMarkerImageRight = Interop.TextField.SelectionHandleMarkerImageRightGet();
            internal static readonly int SelectionHighlightColor = Interop.TextField.SelectionHighlightColorGet();
            internal static readonly int DecorationBoundingBox = Interop.TextField.DecorationBoundingBoxGet();
            internal static readonly int InputMethodSettings = Interop.TextField.InputMethodSettingsGet();
            internal static readonly int InputColor = Interop.TextField.InputColorGet();
            internal static readonly int EnableMarkup = Interop.TextField.EnableMarkupGet();
            internal static readonly int InputFontFamily = Interop.TextField.InputFontFamilyGet();
            internal static readonly int InputFontStyle = Interop.TextField.InputFontStyleGet();
            internal static readonly int InputPointSize = Interop.TextField.InputPointSizeGet();
            internal static readonly int UNDERLINE = Interop.TextField.UnderlineGet();
            internal static readonly int InputUnderline = Interop.TextField.InputUnderlineGet();
            internal static readonly int SHADOW = Interop.TextField.ShadowGet();
            internal static readonly int InputShadow = Interop.TextField.InputShadowGet();
            internal static readonly int EMBOSS = Interop.TextField.EmbossGet();
            internal static readonly int InputEmboss = Interop.TextField.InputEmbossGet();
            internal static readonly int OUTLINE = Interop.TextField.OutlineGet();
            internal static readonly int InputOutline = Interop.TextField.InputOutlineGet();
            internal static readonly int HiddenInputSettings = Interop.TextField.HiddenInputSettingsGet();
            internal static readonly int PixelSize = Interop.TextField.PixelSizeGet();
            internal static readonly int EnableSelection = Interop.TextField.EnableSelectionGet();
            internal static readonly int PLACEHOLDER = Interop.TextField.PlaceholderGet();
            internal static readonly int ELLIPSIS = Interop.TextField.EllipsisGet();
            internal static readonly int EnableShiftSelection = Interop.TextField.EnableShiftSelectionGet();
            internal static readonly int MatchSystemLanguageDirection = Interop.TextField.MatchSystemLanguageDirectionGet();
            internal static readonly int EnableGrabHandle = Interop.TextField.EnableGrabHandleGet();
            internal static readonly int EnableGrabHandlePopup = Interop.TextField.EnableGrabHandlePopupGet();
            internal static readonly int SelectedText = Interop.TextField.SelectedTextGet();
            internal static readonly int SelectedTextStart = Interop.TextField.SelectedTextStartGet();
            internal static readonly int SelectedTextEnd = Interop.TextField.SelectedTextEndGet();
            internal static readonly int EnableEditing = Interop.TextField.EnableEditingGet();
            internal static readonly int PrimaryCursorPosition = Interop.TextField.PrimaryCursorPositionGet();
            internal static readonly int FontSizeScale = Interop.TextField.FontSizeScaleGet();
            internal static readonly int EnableFontSizeScale = Interop.TextField.EnableFontSizeScaleGet();
            internal static readonly int GrabHandleColor = Interop.TextField.GrabHandleColorGet();
            internal static readonly int EllipsisPosition = Interop.TextField.EllipsisPositionGet();
            internal static readonly int InputFilter = Interop.TextField.InputFilterGet();
            internal static readonly int Strikethrough = Interop.TextField.StrikethroughGet();
            internal static readonly int CharacterSpacing = Interop.TextField.CharacterSpacingGet();
            internal static readonly int RemoveFrontInset = Interop.TextField.RemoveFrontInsetGet();
            internal static readonly int RemoveBackInset = Interop.TextField.RemoveBackInsetGet();

            internal static void Preload()
            {
                // Do nothing. Just call for load static values.
            }
        }

        internal class InputStyle
        {
            internal enum Mask
            {
                None = 0x0000,
                Color = 0x0001,
                FontFamily = 0x0002,
                PointSize = 0x0004,
                FontStyle = 0x0008,
                Underline = 0x0010,
                Shadow = 0x0020,
                Emboss = 0x0040,
                Outline = 0x0080
            }
        }

        private void OnDecorationBoundingBoxChanged(int x, int y, int width, int height)
        {
            DecorationBoundingBox = new Rectangle(x, y, width, height);
        }
        private void OnInputColorChanged(float x, float y, float z, float w)
        {
            InputColor = new Vector4(x, y, z, w);
        }
        private void OnPlaceholderTextColorChanged(float r, float g, float b, float a)
        {
            PlaceholderTextColor = new Vector4(r, g, b, a);
        }
        private void OnPrimaryCursorColorChanged(float x, float y, float z, float w)
        {
            PrimaryCursorColor = new Vector4(x, y, z, w);
        }
        private void OnSecondaryCursorColorChanged(float x, float y, float z, float w)
        {
            SecondaryCursorColor = new Vector4(x, y, z, w);
        }
        private void OnSelectionHighlightColorChanged(float x, float y, float z, float w)
        {
            SelectionHighlightColor = new Vector4(x, y, z, w);
        }
        private void OnShadowColorChanged(float x, float y, float z, float w)
        {
            ShadowColor = new Vector4(x, y, z, w);
        }
        private void OnShadowOffsetChanged(float x, float y)
        {
            ShadowOffset = new Vector2(x, y);
        }
        private void OnTextColorChanged(float r, float g, float b, float a)
        {
            TextColor = new Color(r, g, b, a);
        }
        private void OnGrabHandleColorChanged(float r, float g, float b, float a)
        {
            GrabHandleColor = new Color(r, g, b, a);
        }

        internal class TextFieldLayout : LayoutItem
        {
            protected override void OnMeasure(MeasureSpecification widthMeasureSpec, MeasureSpecification heightMeasureSpec)
            {
                // Padding will be automatically applied by DALi TextField.
                var totalWidth = widthMeasureSpec.Size.AsDecimal();
                var totalHeight = heightMeasureSpec.Size.AsDecimal();
                var minWidth = Owner.GetMinimumWidth();
                var minHeight = Owner.GetMinimumHeight();
                var maxWidth = Owner.GetMaximumWidth();
                var maxHeight = Owner.GetMaximumHeight();
                var naturalSize = Owner.GetNaturalSize();

                if (((TextField)Owner).Text.Length == 0)
                {
                    // Calculate height of TextField by setting Text with " ".
                    // By calling SetTextWithoutTextChanged, TextChanged callback is not called for this.
                    ((TextField)Owner).SetTextWithoutTextChanged(" ");

                    // Store original WidthSpecification to restore it after setting ResizePolicy.
                    var widthSpecification = Owner.WidthSpecification;

                    // In DALi's Size logic, if Width or Height is set to be 0, then
                    // ResizePolicy is not changed to Fixed.
                    // This causes Size changes after NUI Layout's OnMeasure is finished.
                    // e.g. TextField's Width fills to its parent although Text is null and
                    //      WidthSpecification is WrapContent.
                    // To prevent the Size changes, WidthResizePolicy is set to be Fixed
                    // in advance if Text is null.
                    Owner.WidthResizePolicy = ResizePolicyType.Fixed;

                    // Restore WidthSpecification because ResizePolicy changes WidthSpecification.
                    Owner.WidthSpecification = widthSpecification;

                    naturalSize = Owner.GetNaturalSize();

                    // Restore TextField's Text after calculating height of TextField.
                    // By calling SetTextWithoutTextChanged, TextChanged callback is not called for this.
                    ((TextField)Owner).SetTextWithoutTextChanged("");
                }

                if (widthMeasureSpec.Mode != MeasureSpecification.ModeType.Exactly)
                {
                    float width = naturalSize != null ? naturalSize.Width : 0;
                    // Since priority of MinimumSize is higher than MaximumSize in DALi, here follows it.
                    totalWidth = Math.Max(Math.Min(width, maxWidth), minWidth);
                }

                if (heightMeasureSpec.Mode != MeasureSpecification.ModeType.Exactly)
                {
                    float height = naturalSize != null ? naturalSize.Height : 0;
                    // Since priority of MinimumSize is higher than MaximumSize in DALi, here follows it.
                    totalHeight = Math.Max(Math.Min(height, maxHeight), minHeight);
                }

                widthMeasureSpec = new MeasureSpecification(new LayoutLength(totalWidth), MeasureSpecification.ModeType.Exactly);
                heightMeasureSpec = new MeasureSpecification(new LayoutLength(totalHeight), MeasureSpecification.ModeType.Exactly);

                MeasuredSize.StateType childWidthState = MeasuredSize.StateType.MeasuredSizeOK;
                MeasuredSize.StateType childHeightState = MeasuredSize.StateType.MeasuredSizeOK;

                SetMeasuredDimensions(ResolveSizeAndState(new LayoutLength(totalWidth), widthMeasureSpec, childWidthState),
                                      ResolveSizeAndState(new LayoutLength(totalHeight), heightMeasureSpec, childHeightState));
            }

            /// <inheritdoc/>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public override bool IsPaddingHandledByNative()
            {
                return true;
            }
        }
    }
}
