using Fitamas.Math;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Linq;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components
{
    public abstract class GUIComponent : DependencyObject
    {
        public static readonly DependencyProperty<Thickness> MarginProperty = new DependencyProperty<Thickness>(MarginChangedCallback, Thickness.Zero, false);

        public static readonly DependencyProperty<Vector2> PivotProperty = new DependencyProperty<Vector2>(PivotChangedCallback, new Vector2(0.5f, 0.5f), false);

        public static readonly DependencyProperty<GUIStyle> StyleProperty = new DependencyProperty<GUIStyle>(StyleChangedCallback);

        public static readonly DependencyProperty<GUIHorizontalAlignment> HorizontalAlignmentProperty = new DependencyProperty<GUIHorizontalAlignment>(AlignmentChangedCallback, GUIHorizontalAlignment.Left, false);

        public static readonly DependencyProperty<GUIVerticalAlignment> VerticalAlignmentProperty = new DependencyProperty<GUIVerticalAlignment>(AlignmentChangedCallback, GUIVerticalAlignment.Top, false);

        public static readonly DependencyProperty<GUIComponent> FocusedComponentProperty = new DependencyProperty<GUIComponent>(FocusedComponentChangedCallback, null, false);

        public static readonly DependencyProperty<float> AlphaProperty = new DependencyProperty<float>(1f, false);

        public static readonly DependencyProperty<bool> IsFocusScopeProperty = new DependencyProperty<bool>(false, false);

        public static readonly DependencyProperty<bool> FocusableProperty = new DependencyProperty<bool>(true, false);

        public static readonly DependencyProperty<bool> IsFocusedProperty = new DependencyProperty<bool>(IsFocusedChangedCallback, false, false);

        public static readonly DependencyProperty<bool> IsMouseOverProperty = new DependencyProperty<bool>(IsMouseOverChangedCallback, false, false);

        public static readonly DependencyProperty<bool> InteractebleProperty = new DependencyProperty<bool>(true, false);

        public static readonly DependencyProperty<bool> EnableProperty = new DependencyProperty<bool>(EnableChangedCallback, true, false);

        private GUIManager manager;
        private GUIComponent parent;
        private List<GUIComponent> childrensComponent;
        private Rectangle absoluteRectangle;
        private Rectangle visibleRectangle;
        private bool isDirty = true;

        protected EventHandlersStore eventHandlersStore;

        public GUIManager Manager => manager;
        public GUIRenderBatch Render => Manager.RenderBatch;
        public bool IsInHierarchy => manager != null;
        public Rectangle VisibleRectangle => visibleRectangle;
        public bool IsVisible => visibleRectangle.Size != Point.Zero;
        public bool IsVisibleAndEnable => IsVisible && Enable;

        public string Name { get; set; }
        public bool RaycastTarget { get; set; }
        public bool IsMask { get; set; }
        public GUIControlTemplate ControlTemplate { get; set; }
        public Stack<TriggerBase> ActiveTriggers { get; }

        public Thickness Margin
        {
            get
            {
                return GetValue(MarginProperty);
            }
            set
            {
                SetValue(MarginProperty, value);
            }
        }

        public Vector2 Pivot
        {
            get
            {
                return GetValue(PivotProperty);
            }
            set
            {
                SetValue(PivotProperty, value);
            }
        }

        public GUIStyle Style
        {
            get 
            { 
                return GetValue(StyleProperty); 
            }
            set 
            { 
                SetValue(StyleProperty, value); 
            }
        }

        public GUIHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return GetValue(HorizontalAlignmentProperty);
            }
            set
            {
                SetValue(HorizontalAlignmentProperty, value);
            }
        }

        public GUIVerticalAlignment VerticalAlignment
        {
            get
            {
                return GetValue(VerticalAlignmentProperty);
            }
            set
            {
                SetValue(VerticalAlignmentProperty, value);
            }
        }

        public float Alpha
        {
            get
            {
                return GetValue(AlphaProperty);
            }
            set
            {
                SetValue(AlphaProperty, value);
            }
        }

        public bool IsFocusScope
        {
            get
            {
                return GetValue(IsFocusScopeProperty);
            }
            set
            {
                SetValue(IsFocusScopeProperty, value);
            }
        }

        public bool Focusable
        {
            get
            {
                return (bool)GetValue(FocusableProperty);
            }
            set
            {
                SetValue(FocusableProperty, value);
            }
        }

        public bool IsFocused
        {
            get
            {
                return GetValue(IsFocusedProperty);
            }
        }

        public bool IsMouseOver
        {
            get
            {
                return GetValue(IsMouseOverProperty);
            }
            internal set
            {
                SetValue(IsMouseOverProperty, value);
            }
        }

        public bool Interacteble
        {
            get
            {
                return GetValue(InteractebleProperty);
            }
            set
            {
                SetValue(InteractebleProperty, value);
            }
        }

        public bool Enable
        {
            get
            {
                return GetValue(EnableProperty);
            }
            set
            {
                SetValue(EnableProperty, value);
            }
        }

        public Point LocalPosition
        {
            get
            {
                Thickness thickness = Margin;
                return new Point(thickness.Left, thickness.Top);
            }
            set
            {
                Thickness thickness = Margin;
                if (thickness.Left != value.X || thickness.Top != value.Y)
                {
                    thickness.Left = value.X;
                    thickness.Top = value.Y;
                    Margin = thickness;
                }
            }
        }

        public Point LocalSize
        {
            get
            {
                Thickness thickness = Margin;
                Point size;
                size.X = HorizontalAlignment != GUIHorizontalAlignment.Stretch ? thickness.Right : 0;
                size.Y = VerticalAlignment != GUIVerticalAlignment.Stretch ? thickness.Bottom : 0;
                return size;
            }
            set
            {
                Thickness thickness = Margin;

                if (thickness.Right != value.X || thickness.Bottom != value.Y)
                {
                    thickness.Right = HorizontalAlignment != GUIHorizontalAlignment.Stretch ? value.X : 0;
                    thickness.Bottom = VerticalAlignment != GUIVerticalAlignment.Stretch ?  value.Y : 0;
                    Margin = thickness;
                }
            }
        }

        public GUIComponent Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (value != parent && this != value && value != null)
                {
                    SetDirty();
                    parent = value;
                    InheritanceParent = value;

                    parent?.AddChild(this);
                }
            }
        }

        public IEnumerable<GUIComponent> ChildrensComponent
        {
            get
            {
                return childrensComponent.AsEnumerable();
            }
        }

        public Rectangle Rectangle 
        {
            get
            {
                if (isDirty)
                {
                    Thickness thickness = Margin;

                    if (parent != null)
                    {
                        Rectangle parentRectangle = parent.AvailableRectangle(this);
                        Point parentPosition = parentRectangle.Location;
                        Point parentSize = parentRectangle.Size;    
                        Point position = new Point();
                        Point size = new Point(thickness.Right, thickness.Bottom);
                        Vector2 pivot = Pivot;
                        int pivotX = (int)(size.X * pivot.X);
                        int pivotY = (int)(size.Y * pivot.Y);

                        GUIHorizontalAlignment horizontalAlignment = HorizontalAlignment;
                        if (horizontalAlignment == GUIHorizontalAlignment.Left)
                        {                            
                            position.X = parentPosition.X + thickness.Left - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Center)
                        {
                            position.X = parentPosition.X + thickness.Left + parentSize.X / 2 - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Right)
                        {
                            position.X = parentPosition.X + thickness.Left + parentSize.X - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Stretch)
                        {
                            position.X = parentPosition.X + thickness.Left;
                            size.X = parentSize.X - thickness.Right - thickness.Left;
                        }

                        GUIVerticalAlignment verticalAlignment = VerticalAlignment;
                        if (verticalAlignment == GUIVerticalAlignment.Bottom)
                        {
                            position.Y = parentPosition.Y + thickness.Top + parentSize.Y - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Center)
                        {
                            position.Y = parentPosition.Y + thickness.Top + parentSize.Y / 2 - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Top)
                        {
                            position.Y = parentPosition.Y + thickness.Top - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Stretch)
                        {
                            position.Y = parentPosition.Y + thickness.Top;
                            size.Y = parentSize.Y - thickness.Bottom - thickness.Top;
                        }

                        absoluteRectangle = new Rectangle(position, size);
                    }
                    else
                    {
                        absoluteRectangle = new Rectangle(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
                    }

                    isDirty = false;

                    OnRecalculateRectangle(absoluteRectangle);
                }

                return absoluteRectangle;
            }
        }

        public GUIComponent()
        {
            childrensComponent = new List<GUIComponent>();
            eventHandlersStore = new EventHandlersStore();
            Name = "Component";
            RaycastTarget = false;
            isDirty = true;
            ActiveTriggers = new Stack<TriggerBase>();
        }

        public void Focus()
        {
            if (Focusable && Enable)
            {
                DependencyObject focusScope = GetFocusScope(parent);

                if (focusScope == null)
                {
                    throw new ArgumentNullException("element");
                }

                focusScope.SetValue(FocusedComponentProperty, this);
            }
        }

        private GUIComponent GetFocusScope(GUIComponent component)
        {
            if (component == null)
            {
                return null;
            }

            if (component.IsFocusScope)
            {
                return component;
            }

            return GetFocusScope(component.parent);
        }

        public void Unfocus()
        {
            DependencyObject focusScope = GetFocusScope(this);

            if (focusScope == null)
            {
                throw new ArgumentNullException("element");
            }

            if (focusScope.GetValue(FocusedComponentProperty) == this)
            {
                focusScope.SetValue(FocusedComponentProperty, null);
            }
        }

        protected virtual Rectangle AvailableRectangle(GUIComponent component)
        {
            return Rectangle;
        }

        protected void SetDirty()
        {
            isDirty = true;
            foreach (var item in childrensComponent)
            {
                item.SetDirty();
            }
        }

        public void AddChild(GUIComponent component)
        {
            if (component == null || component == this)
            {
                return;
            }

            if (!childrensComponent.Contains(component))
            {
                component.Parent?.RemoveChild(component);

                childrensComponent.Add(component);
                                
                component.Init(Manager);
                component.Parent = this;
                
                Style?.ApplySetters(this);

                OnAddChild(component);
            }
        }

        private void RemoveChild(GUIComponent component)
        {
            if (childrensComponent.Contains(component))
            {
                childrensComponent.Remove(component);

                OnRemoveChild(component);
            }
        }

        public void SetAsFirstSibling()
        {
            if (parent != null)
            {
                SetSiblingIndex(parent.childrensComponent.Count - 1);
            }
        }

        public void SetAsLastSibling()
        {
            SetSiblingIndex(0);
        }

        public void SetSiblingIndex(int index)
        {
            parent?.SetSiblingIndex(this, index);
        }

        private void SetSiblingIndex(GUIComponent child, int index)
        {
            if (index >= 0 && index < childrensComponent.Count)
            {
                if (childrensComponent.Remove(child))
                {
                    childrensComponent.Insert(index, child);
                }
            }
        }

        public virtual bool Contains(Point mousePosition)
        {
            return visibleRectangle.Contains(mousePosition);
        }

        public virtual void RaycastAll(Point point, List<GUIComponent> result)
        {
            if (!Enable || !Interacteble)
            {
                return;
            }

            if (RaycastTarget && Contains(point))
            {
                result.Add(this);
            }

            foreach (var component in childrensComponent)
            {
                component.RaycastAll(point, result);
            }
        }

        public Point ToLocal(Point screenPoint)
        {
            Point leftUp = Rectangle.Location;
            return new Point(screenPoint.X - leftUp.X, screenPoint.Y - leftUp.Y);
        }

        public Point FromLocal(Point localPoint)
        {
            Point leftUp = Rectangle.Location;
            return new Point(localPoint.X + leftUp.X, localPoint.Y + leftUp.Y) ;
        }

        public GUIComponent GetComponentFromName(string name)
        {
            if (Name == name)
            {
                return this;
            }

            foreach (var component in childrensComponent)
            {
                if (component.Name == name)
                {
                    return component;
                }

                return component.GetComponentFromName(name);
            }

            return null;
        }

        public void RaiseEvent(GUIEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            MonoEventBase eventBase = GUIEventManager.GetEvent(args.RoutedEvent);

            eventHandlersStore.Invoke(this, args.RoutedEvent, args);

            if (eventBase != null)
            {
                RaiseEvent(eventBase, args);
            }
        }

        private void RaiseEvent(MonoEventBase eventBase, GUIEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            eventBase.InvokeEvent(this, args);

            if (parent != null)
            {
                parent.RaiseEvent(eventBase, args);
            }
        }

        public void AddRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            eventHandlersStore.AddRoutedEventHandler(routedEvent, handler);
        }

        public void RemoveRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            eventHandlersStore.RemoveRoutedEventHandler(routedEvent, handler);
        }

        public void SetAlignment(GUIAlignment alignment)
        {
            switch (alignment)
            {
                case GUIAlignment.Center:
                    HorizontalAlignment = GUIHorizontalAlignment.Center;
                    VerticalAlignment = GUIVerticalAlignment.Center;
                    break;
                case GUIAlignment.LeftTop:
                    HorizontalAlignment = GUIHorizontalAlignment.Left;
                    VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
                case GUIAlignment.LeftCenter:
                    HorizontalAlignment = GUIHorizontalAlignment.Left;
                    VerticalAlignment = GUIVerticalAlignment.Center;
                    break;
                case GUIAlignment.LeftBottom:
                    HorizontalAlignment = GUIHorizontalAlignment.Left;
                    VerticalAlignment = GUIVerticalAlignment.Bottom;
                    break;
                case GUIAlignment.CenterBottom:
                    HorizontalAlignment = GUIHorizontalAlignment.Center;
                    VerticalAlignment = GUIVerticalAlignment.Bottom;
                    break;
                case GUIAlignment.RightBottom:
                    HorizontalAlignment = GUIHorizontalAlignment.Right;
                    VerticalAlignment = GUIVerticalAlignment.Bottom;
                    break;
                case GUIAlignment.RightCenter:
                    HorizontalAlignment = GUIHorizontalAlignment.Right;
                    VerticalAlignment = GUIVerticalAlignment.Center;
                    break;
                case GUIAlignment.RightTop:
                    HorizontalAlignment = GUIHorizontalAlignment.Right;
                    VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
                case GUIAlignment.CenterTop:
                    HorizontalAlignment = GUIHorizontalAlignment.Center;
                    VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
                case GUIAlignment.Stretch:
                    HorizontalAlignment = GUIHorizontalAlignment.Stretch;
                    VerticalAlignment = GUIVerticalAlignment.Stretch;
                    break;
            }
        }

        public void Init(GUIManager system)
        {
            if (this.manager != system && system != null)
            {
                this.manager = system;
                OnInit();
            }

            foreach (var child in childrensComponent)
            {
                child.parent = this;
                child.Init(Manager);
            }
        }

        public void Draw(GameTime gameTime, GUIContextRender context)
        {
            if (Enable)
            {
                visibleRectangle = Rectangle.Intersect(Rectangle, context.Mask);
                context.Alpha *= Alpha;

                if (IsMask)
                {
                    context.SetMask(visibleRectangle);
                }

                OnDraw(gameTime, context);
            }
        }

        protected virtual void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            foreach (var component in childrensComponent)
            {
                component.Draw(gameTime, context);
            }
        }

        public void Destroy()
        {
            if (IsInHierarchy)
            {
                OnDestroy();
                manager = null;
            }

            foreach (var component in childrensComponent.ToArray())
            {
                component.Destroy();
            }

            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        protected virtual void OnInit() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnRecalculateRectangle(Rectangle rectangle) { }

        protected virtual void OnAddChild(GUIComponent component) { }

        protected virtual void OnRemoveChild(GUIComponent component) { }

        protected virtual void OnFocus() { }

        protected virtual void OnUnfocus() { }

        protected virtual void OnMouseEntered() { }
        
        protected virtual void OnMouseExitted() { }

        protected virtual void OnDestroy() { }

        public override void OnPropertyChanged(DependencyProperty property)
        {
            if (property.Id != StyleProperty.Id)
            {
                GUIStyle style = GetValue(StyleProperty);
                style?.ProcessTriggers(this, property);
            }

            parent?.OnChildPropertyChanged(this, property);
        }

        protected virtual void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {

        }

        private static void MarginChangedCallback(DependencyObject dependencyObject, DependencyProperty<Thickness> property, Thickness oldValue, Thickness newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();
            }
        }

        private static void PivotChangedCallback(DependencyObject dependencyObject, DependencyProperty<Vector2> property, Vector2 oldValue, Vector2 newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();
            }
        }

        private static void AlignmentChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUIHorizontalAlignment> property, GUIHorizontalAlignment oldValue, GUIHorizontalAlignment newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();
            }
        }

        private static void AlignmentChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUIVerticalAlignment> property, GUIVerticalAlignment oldValue, GUIVerticalAlignment newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();
            }
        }

        private static void StyleChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUIStyle> property, GUIStyle oldValue, GUIStyle newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                if (oldValue != null)
                {
                    GUIStyleHelpers.ClearTriggerEvents(component, oldValue.TriggerEvents);
                }

                newValue.ApplyStyle(component);
            }
        }

        private static void IsMouseOverChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUIComponent component)
                {
                    if (newValue)
                    {
                        component.OnMouseEntered();
                    }
                    else
                    {
                        component.OnMouseExitted();
                    }
                }
            }
        }

        private static void FocusedComponentChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUIComponent> property, GUIComponent oldValue, GUIComponent newValue)
        {
            oldValue?.SetValue(IsFocusedProperty, false);
            newValue?.SetValue(IsFocusedProperty, true);
        }

        private static void IsFocusedChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUIComponent component)
                {
                    if (newValue)
                    {
                        component.OnFocus();
                    }
                    else
                    {
                        component.OnUnfocus();
                    }
                }
            }
        }

        private static void EnableChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();

                if (newValue)
                {
                    component.OnEnable();
                }
                else
                {
                    component.OnDisable();
                }
            }
        }
    }

    public struct GUIContextRender
    {
        public Rectangle Mask { get; set; }
        public float Alpha { get; set; }

        public GUIContextRender(Rectangle rectangle, float alpha)
        {
            Mask = rectangle;
            Alpha = alpha;
        }

        public void SetMask(Rectangle rectangle)
        {
            Mask = Rectangle.Intersect(rectangle, Mask);
        }
    }
}
