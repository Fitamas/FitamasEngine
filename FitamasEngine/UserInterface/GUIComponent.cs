using Fitamas.Math2D;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Win32;
using System.Linq;

namespace Fitamas.UserInterface
{
    public abstract class GUIComponent : DependencyObject
    {
        public static readonly DependencyProperty<Thickness> MarginProperty = new DependencyProperty<Thickness>(MarginChangedCallback, Thickness.Zero, false);

        public static readonly DependencyProperty<Vector2> PivotProperty = new DependencyProperty<Vector2>(PivotChangedCallback, new Vector2(0.5f, 0.5f), false);

        public static readonly DependencyProperty<GUIStyle> StyleProperty = new DependencyProperty<GUIStyle>(StyleChangedCallback);

        public static readonly DependencyProperty<GUIHorizontalAlignment> HorizontalAlignmentProperty = new DependencyProperty<GUIHorizontalAlignment>(AlignmentChangedCallback, GUIHorizontalAlignment.Left, false);

        public static readonly DependencyProperty<GUIVerticalAlignment> VerticalAlignmentProperty = new DependencyProperty<GUIVerticalAlignment>(AlignmentChangedCallback, GUIVerticalAlignment.Bottom, false);

        public static readonly DependencyProperty<bool> IsMouseOverProperty = new DependencyProperty<bool>(false, false);

        public static readonly DependencyProperty<bool> InteractebleProperty = new DependencyProperty<bool>(true, false);

        public static readonly DependencyProperty<bool> EnableProperty = new DependencyProperty<bool>(true, false);

        private GUISystem system;
        private GUIComponent parent;
        private List<GUIComponent> childrensComponent;
        private Rectangle absoluteRectangle;
        private Rectangle visibleRectangle;
        private bool isDirty = true;

        protected EventHandlersStore eventHandlersStore;

        public GUISystem System => system;
        public GUIRenderBatch Render => System.Render;
        public GUIRoot Root => System.Root;
        public bool IsInHierarchy => system != null;
        public Rectangle VisibleRectangle => visibleRectangle;
        public bool IsVisible => visibleRectangle.Size != Point.Zero;
        public bool IsVisibleAndEnable => IsVisible && Enable;

        public string Name { get; set; }

        public bool RaycastTarget { get; set; }

        public bool IsMask { get; set; }

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
                if (Enable != value)
                {
                    SetValue(EnableProperty, value);
 
                    if (value)
                    {
                        system?.SubscribeInput(this);
                    }
                    else
                    {
                        system?.UnsubscribeInput(this);
                    }

                    foreach (var child in childrensComponent)
                    {
                        child.Enable = value;
                    }
                }
            }
        }

        public Point LocalPosition
        {
            get
            {
                Thickness thickness = Margin;
                return new Point(thickness.Left, thickness.Bottom);
            }
            set
            {
                Thickness thickness = Margin;
                if (thickness.Left != value.X || thickness.Bottom != value.Y)
                {
                    thickness.Left = value.X;
                    thickness.Bottom = value.Y;
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
                size.Y = VerticalAlignment != GUIVerticalAlignment.Stretch ? thickness.Top : 0;
                return size;
            }
            set
            {
                Thickness thickness = Margin;

                if (thickness.Right != value.X || thickness.Top != value.Y)
                {
                    thickness.Right = HorizontalAlignment != GUIHorizontalAlignment.Stretch ? value.X : 0;
                    thickness.Top = VerticalAlignment != GUIVerticalAlignment.Stretch ?  value.Y : 0;
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
                        Rectangle parentRectangle = parent.Rectangle;
                        Point parentPosition = parentRectangle.Location;
                        Point parentSize = parentRectangle.Size;    
                        Point position = new Point();
                        Point size = new Point(thickness.Right, thickness.Top);
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
                            position.Y = parentPosition.Y + thickness.Bottom - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Center)
                        {
                            position.Y = parentPosition.Y + thickness.Bottom + parentSize.Y / 2 - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Top)
                        {
                            position.Y = parentPosition.Y + thickness.Bottom + parentSize.Y - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Stretch)
                        {
                            position.Y = parentPosition.Y + thickness.Bottom;
                            size.Y = parentSize.Y - thickness.Bottom - thickness.Top;
                        }

                        absoluteRectangle = new Rectangle(position, size);
                    }
                    else
                    {
                        absoluteRectangle = new Rectangle(thickness.Left, thickness.Bottom, thickness.Right, thickness.Top);
                    }

                    isDirty = false;                   
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
            if (component == null)
            {
                return;
            }

            if (!childrensComponent.Contains(component))
            {
                component.Parent?.RemoveChild(component);

                childrensComponent.Add(component);
                                
                component.Init(System);
                component.Parent = this;

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

        public void RaycastAll(Point point, List<GUIComponent> result)
        {
            if (IsVisibleAndEnable && RaycastTarget && Contains(point))
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

        public GUIComponent GetComponentFromName(string name, bool recursion = true)
        {
            foreach (var component in childrensComponent)
            {
                if (component.Name == name)
                {
                    return component;
                }

                if (recursion)
                {
                    return component.GetComponentFromName(name, recursion);
                }
            }

            return null;
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
                case GUIAlignment.LeftBottom:
                    HorizontalAlignment = GUIHorizontalAlignment.Left;
                    VerticalAlignment = GUIVerticalAlignment.Bottom;
                    break;
                case GUIAlignment.RightTop:
                    HorizontalAlignment = GUIHorizontalAlignment.Right;
                    VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
                case GUIAlignment.RightBottom:
                    HorizontalAlignment = GUIHorizontalAlignment.Right;
                    VerticalAlignment = GUIVerticalAlignment.Bottom;
                    break;
                case GUIAlignment.Stretch:
                    HorizontalAlignment = GUIHorizontalAlignment.Stretch;
                    VerticalAlignment = GUIVerticalAlignment.Stretch;
                    break;
            }
        }

        public void Init(GUISystem system)
        {
            if (this.system != system && system != null)
            {
                this.system = system;

                System?.SubscribeInput(this);
                OnInit();
            }

            foreach (var child in childrensComponent)
            {
                child.parent = this;
                child.Init(System);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Enable)
            {
                OnUpdate(gameTime);

                foreach (var component in childrensComponent)
                {
                    component.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime, GUIContextRender context)
        {
            if (Enable)
            {
                visibleRectangle = Rectangle.Intersect(Rectangle, context.Mask);

                OnDraw(gameTime, context);

                if (IsMask)
                {
                    context.SetMask(visibleRectangle);
                }

                foreach (var component in childrensComponent)
                {
                    component.Draw(gameTime, context);
                }
            }
        }

        public void Destroy()
        {
            if (IsInHierarchy)
            {
                OnDestroy();
                system.UnsubscribeInput(this);
                system = null;

                foreach (var component in childrensComponent.ToArray())
                {
                    component.Destroy();
                }

                if (parent != null)
                {
                    parent.RemoveChild(this);
                }
            }
        }

        protected virtual void OnInit() { }

        protected virtual void OnAddChild(GUIComponent component) { }

        protected virtual void OnRemoveChild(GUIComponent component) { }

        protected virtual void OnChildPositionChanged(GUIComponent component) { }

        protected virtual void OnChildSizeChanged(GUIComponent component) { }

        protected virtual void OnUpdate(GameTime gameTime) { }

        protected virtual void OnDraw(GameTime gameTime, GUIContextRender context) { }

        protected virtual void OnDestroy() { }

        public override void OnPropertyChanged<T>(DependencyProperty<T> property)
        {
            if (property.Id != StyleProperty.Id)
            {
                GUIStyle style = GetValue(StyleProperty);
                style?.ProcessTriggers(this, property);
            }
        }

        private static void MarginChangedCallback(DependencyObject dependencyObject, DependencyProperty<Thickness> property, Thickness oldValue, Thickness newValue)
        {
            if (dependencyObject is GUIComponent component)
            {
                component.SetDirty();

                if (oldValue.Left != newValue.Left || oldValue.Bottom != newValue.Bottom)
                {
                    component.parent?.OnChildPositionChanged(component);
                }

                if (oldValue.Right != newValue.Right || oldValue.Top != newValue.Top)
                {
                    component.parent?.OnChildSizeChanged(component);
                }
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
    }

    public struct GUIContextRender
    {
        public Rectangle Mask { get; set; }
        public float Opacity { get; set; }

        public GUIContextRender(Rectangle rectangle, float opacity = 1)
        {
            Mask = rectangle;
            Opacity = opacity;
        }

        public void SetMask(Rectangle rectangle)
        {
            Mask = Rectangle.Intersect(rectangle, Mask);
        }
    }
}
