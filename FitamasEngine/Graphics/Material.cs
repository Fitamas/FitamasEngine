using Fitamas.Core;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.Graphics
{
    public class Material : MonoContentObject
    {
        public static Material DefaultMaterial = new Material(new AlphaTestEffect(GameEngine.Instance.GraphicsDevice) 
        { 
            VertexColorEnabled = true,
        });

        public static Material DefaultOpaqueMaterial = new Material(new AlphaTestEffect(GameEngine.Instance.GraphicsDevice)
        {
            VertexColorEnabled = true,
        })
        {
            BlendState = BlendState.Opaque
        };

        public static Material BlitMaterial = new Material(new SpriteEffect(GameEngine.Instance.GraphicsDevice)) 
        { 
            BlendState = BlendState.Opaque 
        };

        [SerializeField] private Effect effect;

        private EffectParameter textureParam;
        private EffectParameter diffuseColorParam;
        private EffectParameter worldViewProjParam;

        public BlendState BlendState = BlendState.NonPremultiplied;
        public DepthStencilState DepthStencilState = DepthStencilState.None;
        public SamplerState SamplerState = SamplerState.PointWrap;

        public Effect Effect => effect;

        public Texture2D MainTexture
        {
            get
            {
                return textureParam?.GetValueTexture2D();
            }
            set
            {
                textureParam?.SetValue(value);
            }
        }

        public Material(Effect effect)
        {
            this.effect = effect;
            CacheEffectParameters();
        }

        private void CacheEffectParameters()
        {
            textureParam = effect.Parameters["Texture"];
            diffuseColorParam = effect.Parameters["DiffuseColor"];
            worldViewProjParam = effect.Parameters["WorldViewProj"];
        }

        public Material Clone()
        {
            return new Material(Effect.Clone())
            {
                BlendState = BlendState,
                DepthStencilState = DepthStencilState,
                SamplerState = SamplerState,
            };
        }
    }
}
