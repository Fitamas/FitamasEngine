using Fitamas.Gameplay.Characters;
using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public abstract class GUIComponent
    {
        [SerializableField] private GUIComponent parent;
        [SerializableField] private List<GUIComponent> childrensComponent = new List<GUIComponent>();
        [SerializableField] private Rectangle localRectangle;
        [SerializableField] private GUIStretch stretch;
        [SerializableField] private Vector2 anchor;
        [SerializableField] private Vector2 pivot;
        [SerializableField] private int layer;
        [SerializableField] private bool enable = true;

        private GUISystem system;
        private GUIContextRender context;
        private Rectangle absoluteRectangle;
        private Rectangle visibleRectangle;
        private bool isDirty = true;
        private bool isVisible;

        public GUISystem System => system;
        public GUIRenderBatch GUIRender => System.GUIRender;
        public bool IsInHierarchy => system != null;
        public bool IsVisible => isVisible;
        public bool IsVisibleAndEnable => isVisible && enable;

        public string Id;
        public bool AutoSortingLayer;
        public bool Interecteble;
        public bool RaycastTarget;
        public bool IsMask;

        public bool OnMouse 
        { 
            get
            {
                return system != null ? system.onMouse == this : false;
            }
        }

        public int Layer
        {
            get
            {
                if (AutoSortingLayer)
                {
                    if (parent != null)
                    {
                        int index = parent.childrensComponent.Count - parent.childrensComponent.IndexOf(this);
                        return parent.Layer + index + 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return layer;
                }
            }
            set
            {
                layer = value;
            }
        }

        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                if (enable != value)
                {
                    enable = value;

                    if (enable)
                    {
                        system?.SubscribeInput(this);
                    }
                    else
                    {
                        system?.UnsubscribeInput(this);
                    }

                    foreach (var child in childrensComponent)
                    {
                        child.Enable = enable;
                    }
                }
            }
        }

        public GUIStretch Stretch
        {
            get
            {
                return stretch;
            }
            set
            {
                SetDirty();
                stretch = value;
            }
        }

        public Vector2 Anchor
        {
            get
            {
                return anchor;
            }
            set
            {
                SetDirty();
                anchor = new Vector2(MathV.Clamp01(value.X), MathV.Clamp01(value.Y));
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
                pivot = new Vector2(MathV.Clamp01(value.X), MathV.Clamp01(value.Y));
            }
        }

        public GUIAlignment Alignment
        {
            set
            {
                switch (value)
                {
                    case GUIAlignment.Center:
                        anchor = new Vector2(0.5f, 0.5f);
                        break;
                    case GUIAlignment.LeftUp:
                        anchor = new Vector2(0, 0);
                        break;
                    case GUIAlignment.LeftDown:
                        anchor = new Vector2(0, 1);
                        break;
                    case GUIAlignment.RightUp:
                        anchor = new Vector2(1, 0);
                        break;
                    case GUIAlignment.RightDown:
                        anchor = new Vector2(1, 1);
                        break;
                }
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
                if (value != parent)
                {
                    parent?.RemoveChild(this);

                    SetDirty();
                    parent = value;

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
                        Point size = new Point();

                        if (stretch == GUIStretch.Horizontal || stretch == GUIStretch.HorizontalAndVertical)
                        {
                            position.X = parentPosition.X + LocalPosition.X;
                            size.X = parentSize.X - LocalScale.X - LocalPosition.X;
                        }
                        else
                        {
                            float pivotX = LocalScale.X * pivot.X;
                            float anchorX = parentSize.X * anchor.X;
                            position.X = (int)(parentPosition.X + LocalPosition.X + anchorX - pivotX);
                            size.X = LocalScale.X;
                        }

                        if (stretch == GUIStretch.Vertical || stretch == GUIStretch.HorizontalAndVertical)
                        {
                            position.Y = parentPosition.Y + LocalPosition.Y;
                            size.Y = parentSize.Y - LocalScale.Y - LocalPosition.Y;
                        }
                        else
                        {
                            float pivotY = LocalScale.Y * pivot.Y;
                            float anchorY = parentSize.Y * anchor.Y;
                            position.Y = (int)(parentPosition.Y - LocalPosition.Y + anchorY - pivotY);
                            size.Y = LocalScale.Y;
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

        public GUIComponent(Rectangle rectangle = new Rectangle(), GUIAlignment aligment = GUIAlignment.LeftUp)
        {
            Alignment = aligment;
            pivot = new Vector2(0.5f, 0.5f);
            Id = "Component";
            AutoSortingLayer = true;
            Interecteble = true;
            RaycastTarget = false;
            localRectangle = rectangle;
            isDirty = true;
        }

        public GUIComponent(Point positon, GUIAlignment aligment = GUIAlignment.LeftUp) 
            : this(new Rectangle(positon, Point.Zero), aligment)
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
            if (!childrensComponent.Contains(component))
            {
                childrensComponent.Add(component);

                component.parent?.RemoveChild(component);

                component.Init(System);
                component.SetDirty();
                component.parent = this;

                OnAddChild(component);
            }
        }

        public void RemoveChild(GUIComponent component)
        {
            if (childrensComponent.Contains(component))
            {
                childrensComponent.Remove(component);

                component.SetDirty();
                component.parent = IsInHierarchy ? system.CurrentLayout.Canvas : null;

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

        public Point ToLocalPosition(Point screenPoint)
        {
            Rectangle rectangle = Rectangle;
            Point leftUp = new Point(rectangle.X, rectangle.Y);

            return new Point(screenPoint.X - leftUp.X, leftUp.Y - screenPoint.Y);
        }

        public Point ToScreenPosition(Point localPoint)
        {
            Rectangle rectangle = Rectangle;
            Point leftUp = new Point(rectangle.X, rectangle.Y);

            return new Point(localPoint.X + leftUp.X, leftUp.Y -localPoint.Y) ;
        }

        public GUIComponent GetComponentFromId(string id)
        {
            if (Id == id)
            {
                return this;
            }

            foreach (var component in childrensComponent)
            {
                GUIComponent result = component.GetComponentFromId(id);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
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
            if (enable)
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
            context = new GUIContextRender(Rectangle);

            DoDraw(gameTime);
        }

        private void DoDraw(GameTime gameTime)
        {
            if (enable)
            {
                GUIRender.ContextRender = context;

                visibleRectangle = Rectangle.Intersect(Rectangle, context.Mask);
                isVisible = visibleRectangle.Size != Point.Zero;

                if (isVisible)
                {
                    OnDraw(gameTime);
                }

                GUIContextRender contextRender = context;

                if (IsMask)
                {
                    contextRender.SetMask(Rectangle);
                }

                foreach (var component in childrensComponent)
                {
                    component.context = contextRender;
                    component.DoDraw(gameTime);
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

        protected virtual void OnDraw(GameTime gameTime) { }

        protected virtual void OnDestroy() { }
    }

    public struct GUIContextRender
    {
        public Rectangle Mask { get; set; }
        public float Layer { get; set; }

        public GUIContextRender(Rectangle rectangle, float layer = 0)
        {
            Mask = rectangle;
            Layer = layer;
        }

        public void SetMask(Rectangle rectangle)
        {
            Mask = Rectangle.Intersect(rectangle, Mask);
        }
    }
}
