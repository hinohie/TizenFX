﻿/*
 * Copyright(c) 2022 Samsung Electronics Co., Ltd.
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

using System.ComponentModel;
using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// The AppBar class is a class which shows title text and provides navigation
    /// and action functions on Page.
    /// </summary>
    /// <since_tizen> 9 </since_tizen>
    public partial class AppBar : Control
    {
        private bool autoNavigationContent = true;

        private string title = null;

        private View navigationContent = null;
        private View titleContent = null;
        private View actionContent = null;
        private IEnumerable<View> actionContentViews = null;

        private View defaultNavigationContent = null;
        private View defaultTitleContent = null;
        private View defaultActionContent = null;

        private Extents navigationPadding;
        private Extents actionPadding;
        private ViewStyle actionViewStyle;
        private ButtonStyle actionButtonStyle;

        private bool styleApplied = false;

        static AppBar()
        {
            if (NUIApplication.IsUsingXaml)
            {
                NavigationContentProperty = BindableProperty.Create(nameof(NavigationContent), typeof(View), typeof(AppBar), null,
                    propertyChanged: SetInternalNavigationContentProperty, defaultValueCreator: GetInternalNavigationContentProperty);
                TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AppBar), default(string),
                    propertyChanged: SetInternalTitleProperty, defaultValueCreator: GetInternalTitleProperty);
                TitleContentProperty = BindableProperty.Create(nameof(TitleContent), typeof(View), typeof(AppBar), null,
                    propertyChanged: SetInternalTitleContentProperty, defaultValueCreator: GetInternalTitleContentProperty);
                ActionContentProperty = BindableProperty.Create(nameof(ActionContent), typeof(View), typeof(AppBar), null,
                    propertyChanged: SetInternalActionContentProperty, defaultValueCreator: GetInternalActionContentProperty);
                AutoNavigationContentProperty = BindableProperty.Create(nameof(AutoNavigationContent), typeof(bool), typeof(AppBar), default(bool),
                    propertyChanged: SetInternalAutoNavigationContentProperty, defaultValueCreator: GetInternalAutoNavigationContentProperty);
            }
        }

        /// <summary>
        /// Creates a new instance of AppBar.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public AppBar() : base()
        {
            Initialize();
            //to use GetNextFocusableView
            SetKeyboardNavigationSupport(true);
        }

        /// <summary>
        /// Creates a new instance of AppBar.
        /// </summary>
        /// <param name="style">Creates AppBar by special style defined in UX.</param>
        /// <since_tizen> 9 </since_tizen>
        public AppBar(string style) : base(style)
        {
            Initialize();
            //to use GetNextFocusableView
            SetKeyboardNavigationSupport(true);
        }

        /// <summary>
        /// Creates a new instance of AppBar.
        /// </summary>
        /// <param name="appBarStyle">Creates AppBar by style customized by user.</param>
        /// <since_tizen> 9 </since_tizen>
        public AppBar(AppBarStyle appBarStyle) : base(appBarStyle)
        {
            Initialize();
            //to use GetNextFocusableView
            SetKeyboardNavigationSupport(true);
        }

        /// <summary>
        /// Disposes AppBar and all children on it.
        /// </summary>
        /// <param name="type">Dispose type.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                if (navigationContent == defaultNavigationContent)
                {
                    if (navigationContent != null)
                    {
                        Utility.Dispose(navigationContent);
                    }
                }
                else
                {
                    if (navigationContent != null)
                    {
                        Utility.Dispose(navigationContent);
                    }

                    if (defaultNavigationContent != null)
                    {
                        Utility.Dispose(defaultNavigationContent);
                    }
                }

                if (titleContent == defaultTitleContent)
                {
                    if (titleContent != null)
                    {
                        Utility.Dispose(titleContent);
                    }
                }
                else
                {
                    if (titleContent != null)
                    {
                        Utility.Dispose(titleContent);
                    }

                    if (defaultTitleContent != null)
                    {
                        Utility.Dispose(defaultTitleContent);
                    }
                }

                if (actionContent == defaultActionContent)
                {
                    if (actionContent != null)
                    {
                        Utility.Dispose(actionContent);
                    }
                }
                else
                {
                    if (actionContent != null)
                    {
                        Utility.Dispose(actionContent);
                    }

                    if (defaultActionContent != null)
                    {
                        Utility.Dispose(defaultActionContent);
                    }
                }
            }

            base.Dispose(type);
        }

        /// <summary>
        /// Navigation content of AppBar.
        /// NavigationContent is added as a child of AppBar automatically.
        /// If AutoNavigationContent is set to be true and NavigationContent is not set,
        /// then default navigation content is automatically displayed.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public View NavigationContent
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(NavigationContentProperty) as View;
                }
                else
                {
                    return InternalNavigationContent;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(NavigationContentProperty, value);
                }
                else
                {
                    InternalNavigationContent = value;
                }
                NotifyPropertyChanged();
            }
        }
        private View InternalNavigationContent
        {
            get
            {
                return navigationContent;
            }
            set
            {
                if (navigationContent == value)
                {
                    return;
                }

                if (navigationContent != null)
                {
                    Remove(navigationContent);
                }

                navigationContent = value;
                if (navigationContent == null)
                {
                    return;
                }
                navigationContent.Focusable = true;
                ResetContent();
            }
        }

        /// <summary>
        /// Title text of AppBar.
        /// Title sets title text to the default title content.
        /// If TitleContent is not TextLabel, then Title does not set title text of the TitleContent.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public string Title
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(TitleProperty) as string;
                }
                else
                {
                    return InternalTitle;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TitleProperty, value);
                }
                else
                {
                    InternalTitle = value;
                }
                NotifyPropertyChanged();
            }
        }
        private string InternalTitle
        {
            get
            {
                return title;
            }
            set
            {
                if (title == value)
                {
                    return;
                }

                title = value;

                if (TitleContent is TextLabel textLabel)
                {
                    textLabel.Text = title;
                }
            }
        }

        /// <summary>
        /// Title content of AppBar.
        /// TitleContent is added as a child of AppBar automatically.
        /// If TitleContent is not TextLabel, then Title does not set title text of the TitleContent.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public View TitleContent
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(TitleContentProperty) as View;
                }
                else
                {
                    return InternalTitleContent;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(TitleContentProperty, value);
                }
                else
                {
                    InternalTitleContent = value;
                }
                NotifyPropertyChanged();
            }
        }
        private View InternalTitleContent
        {
            get
            {
                return titleContent;
            }
            set
            {
                if (titleContent == value)
                {
                    return;
                }

                if (titleContent != null)
                {
                    Remove(titleContent);
                }

                titleContent = value;
                if (titleContent == null)
                {
                    return;
                }

                if (titleContent is TextLabel textLabel)
                {
                    textLabel.Text = Title;
                }
                else
                {
                    titleContent.Focusable = true;
                }

                ResetContent();
            }
        }

        /// <summary>
        /// Action views of AppBar.
        /// Action views are added to ActionContent of AppBar.
        /// If action views and action buttons are set to Actions, then proper style look for action views and action buttons are automatically applied to action views and action buttons.
        /// e.g. size, button icon color, etc.
        /// If you do not want to apply framework's style look for action views and action buttons, then please use ActionContent.Add(actionView) instead of setting Actions.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public IEnumerable<View> Actions
        {
            get
            {
                return actionContentViews;
            }
            set
            {
                if (ActionContent == null)
                {
                    actionContentViews = value;
                    return;
                }

                if (actionContentViews != null)
                {
                    foreach (var oldAction in actionContentViews)
                    {
                        if (ActionContent.Children?.Contains(oldAction) == true)
                        {
                            ActionContent.Children.Remove(oldAction);
                        }
                    }
                }

                actionContentViews = value;

                if (actionContentViews == null)
                {
                    return;
                }

                foreach (var action in actionContentViews)
                {
                    // Apply Action and ActionButton styles.
                    if ((action is Button) && (actionButtonStyle != null))
                    {
                        action.ApplyStyle(actionButtonStyle);
                    }
                    else if (actionViewStyle != null)
                    {
                        action.ApplyStyle(actionViewStyle);
                    }

                    ActionContent.Add(action);
                }
            }
        }

        /// <summary>
        /// Action content of AppBar.
        /// ActionContent is added as a child of AppBar automatically.
        /// Action content contains action views and action buttons by Actions.
        /// The Action and ActionButton styles of AppBarStyle are applied to actions only by setting Actions.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public View ActionContent
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return GetValue(ActionContentProperty) as View;
                }
                else
                {
                    return InternalActionContent;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(ActionContentProperty, value);
                }
                else
                {
                    InternalActionContent = value;
                }
                NotifyPropertyChanged();
            }
        }
        private View InternalActionContent
        {
            get
            {
                return actionContent;
            }
            set
            {
                if (actionContent == value)
                {
                    return;
                }

                var oldActionContent = actionContent;
                actionContent = value;

                // Add views first before remove previous action content
                // not to cause Garbage Collector collects views.
                if ((actionContent != null) && (Actions != null))
                {
                    foreach (var action in Actions)
                    {
                        // Apply Action and ActionButton styles.
                        if ((action is Button) && (actionButtonStyle != null))
                        {
                            action.ApplyStyle(actionButtonStyle);
                        }
                        else if (actionViewStyle != null)
                        {
                            action.ApplyStyle(actionViewStyle);
                        }

                        actionContent.Add(action);
                    }
                }

                if (oldActionContent != null)
                {
                    Remove(oldActionContent);
                }

                if (actionContent == null)
                {
                    return;
                }

                ResetContent();
            }
        }

        /// <summary>
        /// Flag to indicate if default navigation content is automatically set or not.
        /// The default value is true.
        /// If AutoNavigationContent is set to be true and NavigationContent is not set,
        /// then default navigation content is automatically displayed.
        /// If default navigation content is clicked, the back navigation proceeds.
        /// </summary>
        /// <since_tizen> 9 </since_tizen>
        public bool AutoNavigationContent
        {
            get
            {
                if (NUIApplication.IsUsingXaml)
                {
                    return (bool)GetValue(AutoNavigationContentProperty);
                }
                else
                {
                    return InternalAutoNavigationContent;
                }
            }
            set
            {
                if (NUIApplication.IsUsingXaml)
                {
                    SetValue(AutoNavigationContentProperty, value);
                }
                else
                {
                    InternalAutoNavigationContent = value;
                }
                NotifyPropertyChanged();
            }
        }
        private bool InternalAutoNavigationContent
        {
            get
            {
                return autoNavigationContent;
            }

            set
            {
                if (autoNavigationContent == value)
                {
                    return;
                }

                autoNavigationContent = value;

                if (autoNavigationContent == true)
                {
                    if (NavigationContent == null)
                    {
                        NavigationContent = DefaultNavigationContent;
                    }
                }
                else if (NavigationContent == DefaultNavigationContent)
                {
                    NavigationContent = null;
                }
            }
        }

        /// <summary>
        /// Default navigation content of AppBar set automatically by default.
        /// If AutoNavigationContent is set to be true and NavigationContent is not set,
        /// then default navigation content is automatically displayed.
        /// If default navigation content is clicked, it calls navigator pop operation.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected View DefaultNavigationContent
        {
            get
            {
                // TODO: Do not set default navigation content if there is no previous page.
                if (defaultNavigationContent == null)
                {
                    defaultNavigationContent = CreateDefaultNavigationContent();
                }

                return defaultNavigationContent;
            }
        }

        /// <summary>
        /// Default title content of AppBar set automatically by default.
        /// If TitleContent is not set by user, then default title content is
        /// automatically displayed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected View DefaultTitleContent
        {
            get
            {
                if (defaultTitleContent == null)
                {
                    defaultTitleContent = CreateDefaultTitleContent();
                }

                return defaultTitleContent;
            }
        }

        /// <summary>
        /// Default action content of AppBar set automatically by default.
        /// If ActionContent is not set by user, then default action content is
        /// automatically displayed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected View DefaultActionContent
        {
            get
            {
                if (defaultActionContent == null)
                {
                    defaultActionContent = CreateDefaultActionContent();
                }

                return defaultActionContent;
            }
        }

        /// <summary>
        /// Gets accessibility name.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string AccessibilityGetName()
        {
            return Title;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnInitialize()
        {
            base.OnInitialize();

            AccessibilityRole = Role.TitleBar;
        }

        /// <summary>
        /// Applies style to AppBar.
        /// </summary>
        /// <param name="viewStyle">The style to apply.</param>
        /// <since_tizen> 9 </since_tizen>
        public override void ApplyStyle(ViewStyle viewStyle)
        {
            styleApplied = false;

            base.ApplyStyle(viewStyle);

            var appBarStyle = viewStyle as AppBarStyle;

            if (appBarStyle == null)
            {
                return;
            }

            if (appBarStyle.NavigationPadding != null)
            {
                navigationPadding = new Extents(appBarStyle.NavigationPadding);
            }

            if (appBarStyle.ActionPadding != null)
            {
                actionPadding = new Extents(appBarStyle.ActionPadding);
            }

            // Apply Back Button style.
            if ((appBarStyle.BackButton != null) && (DefaultNavigationContent is Button button))
            {
                button.ApplyStyle(appBarStyle.BackButton);
            }

            // Apply Title style.
            if ((appBarStyle.TitleTextLabel != null) && (DefaultTitleContent is TextLabel textLabel))
            {
                textLabel.ApplyStyle(appBarStyle.TitleTextLabel);
            }

            // Apply ActionCellPadding style.
            if (DefaultActionContent?.Layout is LinearLayout linearLayout && appBarStyle.ActionCellPadding != null)
            {
                linearLayout.CellPadding = new Size2D(appBarStyle.ActionCellPadding.Width, appBarStyle.ActionCellPadding.Height);
            }

            // Apply Action and ActionButton styles.
            if (DefaultActionContent?.Children != null)
            {
                foreach (var action in DefaultActionContent?.Children)
                {
                    if ((action is Button) && (appBarStyle.ActionButton != null))
                    {
                        action.ApplyStyle(appBarStyle.ActionButton);
                    }
                    else if (appBarStyle.ActionView != null)
                    {
                        action.ApplyStyle(appBarStyle.ActionView);
                    }
                }
            }

            if (actionButtonStyle == null) actionButtonStyle = (ButtonStyle)appBarStyle.ActionButton?.Clone();
            else actionButtonStyle.MergeDirectly(appBarStyle.ActionButton);

            if (actionViewStyle == null) actionViewStyle = (ViewStyle)appBarStyle.ActionView?.Clone();
            else actionViewStyle.MergeDirectly(appBarStyle.ActionView);


            styleApplied = true;

            // Calculate children's positions based on padding sizes.
            CalculatePosition();
        }

        /// <summary>
        /// Gets AppBar style.
        /// </summary>
        /// <returns>The default AppBar style.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ViewStyle CreateViewStyle()
        {
            return new AppBarStyle();
        }

        private void Initialize()
        {
            // Navigation, Title and Action are located horizontally.
            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                LinearAlignment = LinearLayout.Alignment.CenterVertical,
            };

            WidthSpecification = LayoutParamPolicies.MatchParent;

            if (AutoNavigationContent == true)
            {
                NavigationContent = DefaultNavigationContent;
            }

            TitleContent = DefaultTitleContent;

            ActionContent = DefaultActionContent;
        }

        private View CreateDefaultNavigationContent()
        {
            var backButton = new Button();

            backButton.Clicked += (object sender, ClickedEventArgs args) =>
            {
                // The page of app bar is popped when default back button is clicked.
                var page = GetParent() as Page;
                if (page != null)
                {
                    var navigator = page.Navigator;
                    if ((navigator != null) && (navigator.EnableBackNavigation))
                    {
                        navigator.NavigateBack();
                    }
                }
            };

            return backButton;
        }

        private View CreateDefaultTitleContent()
        {
            return new TextLabel()
            {
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
        }

        private View CreateDefaultActionContent()
        {
            return new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.End,
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
        }

        private void ResetContent()
        {
            // To keep the order of NavigationContent, TitleContent and ActionContent,
            // the existing contents are removed and added again.
            if ((navigationContent != null) && Children.Contains(navigationContent))
            {
                Remove(navigationContent);
            }

            if ((titleContent != null) && Children.Contains(titleContent))
            {
                Remove(titleContent);
            }

            if ((actionContent != null) && Children.Contains(actionContent))
            {
                Remove(actionContent);
            }

            if (navigationContent != null)
            {
                Add(navigationContent);
            }

            if (titleContent != null)
            {
                Add(titleContent);
            }

            if (actionContent != null)
            {
                Add(actionContent);
            }

            // Calculate children's positions based on padding sizes.
            CalculatePosition();
        }

        private void CalculatePosition()
        {
            if (styleApplied == false)
            {
                return;
            }

            // Apply NavigationPadding style.
            if ((NavigationContent != null) && (navigationPadding != null))
            {
                if (NavigationContent.Margin.NotEqualTo(navigationPadding))
                {
                    NavigationContent.Margin.CopyFrom(navigationPadding);
                }
            }

            // Apply ActionPadding style.
            if ((ActionContent != null) && (actionPadding != null))
            {
                if (ActionContent.Margin.NotEqualTo(actionPadding))
                {
                    ActionContent.Margin.CopyFrom(actionPadding);
                }
            }
        }

        /// <summary>
        /// ToDo : only key navigation is enabled, but value editing is not yet added. for example, after enter key and up/down key the value need be changed.
        /// </summary>
        /// <param name="currentFocusedView"></param>
        /// <param name="direction"></param>
        /// <param name="loopEnabled"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override View GetNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            if (currentFocusedView == null)
            {
                if (navigationContent != null && navigationContent.Focusable)
                {
                    return navigationContent;
                }
                else if (titleContent != null && titleContent.Focusable)
                {
                    return titleContent;
                }
                else
                {
                    return null;
                }
            }
            else if (currentFocusedView == this)
            {
                return null;
            }
            else if (currentFocusedView == navigationContent)
            {
                if (direction == View.FocusDirection.Up)
                {
                    return null;
                }
                else if (direction == View.FocusDirection.Right)
                {
                    if (titleContent != null && titleContent.Focusable)
                    {
                        return titleContent;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (direction == View.FocusDirection.Down)
                {
                    return null;
                }
                else if (direction == View.FocusDirection.Left)
                {
                    return null;
                }
            }
            else if (currentFocusedView == titleContent)
            {
                if (direction == View.FocusDirection.Up)
                {
                    return null;
                }
                else if (direction == View.FocusDirection.Right)
                {
                    return null;
                }
                else if (direction == View.FocusDirection.Down)
                {
                    return null;
                }
                else if (direction == View.FocusDirection.Left)
                {
                    if (navigationContent != null && navigationContent.Focusable)
                    {
                        return navigationContent;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal override View PassFocusableViewInsideIfNeeded()
        {
            if (navigationContent != null && navigationContent.Focusable)
            {
                return navigationContent;
            }
            else if (titleContent != null && titleContent.Focusable)
            {
                return titleContent;
            }
            return this;
        }

    }
}
