using Fitamas.Math2D;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Fitamas.UserInterface
{
    public abstract class GUIComponent : DependencyObject
    {
        public static readonly DependencyProperty<GUIStyle> StyleProperty = new DependencyProperty<GUIStyle>(StyleChangedCallback);

        public static readonly DependencyProperty<GUIHorizontalAlignment> HorizontalAlignmentProperty = new DependencyProperty<GUIHorizontalAlignment>(GUIHorizontalAlignment.Left, false);

        public static readonly DependencyProperty<GUIVerticalAlignment> VerticalAlignmentProperty = new DependencyProperty<GUIVerticalAlignment>(GUIVerticalAlignment.Top, false);

        public static readonly DependencyProperty<bool> IsMouseOverProperty = new DependencyProperty<bool>(false, false);

        public static readonly DependencyProperty<bool> InterectebleProperty = new DependencyProperty<bool>(true, false);

        public static readonly DependencyProperty<bool> EnableProperty = new DependencyProperty<bool>(true, false);

        private GUISystem system;
        private GUIComponent parent;
        private List<GUIComponent> childrensComponent;
        private Vector2 pivot;
        private Rectangle localRectangle;
        private Rectangle absoluteRectangle;
        private Rectangle visibleRectangle;
        private bool isDirty = true;
        private bool isVisible;

        protected EventHandlersStore eventHandlersStore;

        public GUISystem System => system;
        public GUIRenderBatch Render => System.Render;
        public GUIRoot Root => System.Root;
        public bool IsInHierarchy => system != null;
        public bool IsVisible => isVisible;
        public bool IsVisibleAndEnable => isVisible && Enable;

        public string Name;
        public bool RaycastTarget;
        public bool IsMask;

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
                SetDirty();
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
                SetDirty();
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

        public bool Interecteble
        {
            get
            {
                return GetValue(InterectebleProperty);
            }
            set
            {
                SetValue(InterectebleProperty, value);
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

        public Vector2 Pivot
        {
            get
            {
                return pivot;
            }
            set
            {
                SetDirty();
                pivot = value;
            }
        }

        public Rectangle LocalRectangle 
        { 
            get
            {
                return localRectangle;
            }
            set
            {
                if (localRectangle != value)
                {
                    SetDirty();
                    localRectangle = value;
                }
            }
        }
        public Point LocalPosition
        {
            get
            {
                return localRectangle.Location;
            }
            set
            {
                if (localRectangle.Location != value)
                {
                    SetDirty();
                    localRectangle.Location = value;
                }
            }
        }
        public Point LocalScale
        {
            get
            {
                return localRectangle.Size;
            }
            set
            {
                if (localRectangle.Size != value)
                {
                    SetDirty();
                    localRectangle.Size = value;
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
                return childrensComponent.ToArray();
            }
        }
        public Rectangle Rectangle 
        {
            get
            {
                if (isDirty)
                {
                    if (parent != null)
                    {
                        Rectangle parentRectangle = Parent.Rectangle;
                        Point parentPosition = parentRectangle.Location;
                        Point parentSize = parentRectangle.Size;    
                        Point position = new Point();
                        Point size = LocalScale;
                        int pivotX = (int)(LocalScale.X * pivot.X);
                        int pivotY = (int)(LocalScale.Y * pivot.Y);

                        GUIHorizontalAlignment horizontalAlignment = HorizontalAlignment;
                        if (horizontalAlignment == GUIHorizontalAlignment.Left)
                        {                            
                            position.X = parentPosition.X + LocalPosition.X - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Center)
                        {
                            position.X = parentPosition.X + LocalPosition.X + parentSize.X / 2 - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Right)
                        {
                            position.X = parentPosition.X + LocalPosition.X + parentSize.X - pivotX;
                        }
                        else if (horizontalAlignment == GUIHorizontalAlignment.Stretch)
                        {
                            position.X = parentPosition.X + LocalPosition.X;
                            size.X = parentSize.X - LocalScale.X;
                        }

                        GUIVerticalAlignment verticalAlignment = VerticalAlignment;
                        if (verticalAlignment == GUIVerticalAlignment.Top)
                        {
                            position.Y = parentPosition.Y - LocalPosition.Y - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Center)
                        {
                            position.Y = parentPosition.Y - LocalPosition.Y + parentSize.Y / 2 - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Bottom)
                        {
                            position.Y = parentPosition.Y - LocalPosition.Y + parentSize.Y - pivotY;
                        }
                        else if (verticalAlignment == GUIVerticalAlignment.Stretch)
                        {
                            position.Y = parentPosition.Y + LocalPosition.Y;
                            size.Y = parentSize.Y - LocalScale.Y;
                        }

                        absoluteRectangle = new Rectangle(position, size);
                    }
                    else
                    {
                        absoluteRectangle = localRectangle;
                    }

                    isDirty = false;                   
                }

                return absoluteRectangle;
            }
        }

        public GUIComponent(Rectangle rectangle = new Rectangle())
        {
            childrensComponent = new List<GUIComponent>();
            eventHandlersStore = new EventHandlersStore();
            pivot = new Vector2(0.5f, 0.5f);
            Name = "Component";
            RaycastTarget = false;
            localRectangle = rectangle;
            isDirty = true;
        }

        public GUIComponent(Point positon) : this(new Rectangle(positon, Point.Zero))
        {

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

        public virtual bool Contain(Point mousePosition) 
        {
            return visibleRectangle.Contains(mousePosition);
        }

        public void RaycastAll(Point point, List<GUIComponent> result)
        {
            if (IsVisibleAndEnable && RaycastTarget && Contain(point))
            {
                result.Add(this);
            }

            foreach (var component in childrensComponent)
            {
                component.RaycastAll(point, result);
            }
        }

        public Point ScreenToLocal(Point screenPoint)
        {
            Point leftUp = Rectangle.Location;
            return new Point(screenPoint.X - leftUp.X, leftUp.Y - screenPoint.Y);
        }

        public Point LocalToScreen(Point localPoint)
        {
            Point leftUp = Rectangle.Location;
            return new Point(localPoint.X + leftUp.X, leftUp.Y - localPoint.Y) ;
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

        public void Draw(GameTime gameTime)
        {
            Rectangle rectangle = new Rectangle(Point.Zero, Render.GetViewportSize());
            Draw(gameTime, new GUIContextRender(rectangle));
        }

        private void Draw(GameTime gameTime, GUIContextRender context)
        {
            if (Enable)
            {
                visibleRectangle = Rectangle.Intersect(Rectangle, context.Mask);
                isVisible = visibleRectangle.Size != Point.Zero;

                OnDraw(gameTime, context);

                if (IsMask)
                {
                    context.SetMask(Rectangle);
                }

                //for (int i = 0; i < childrensComponent.Count; i++)

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
