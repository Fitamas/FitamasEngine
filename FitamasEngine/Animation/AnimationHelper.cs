
namespace Fitamas.Animation
{
    public static class AnimationHelper
    {
        public static KeyFrame<int>[] CreateFrames(int count)
        {
            KeyFrame<int>[] result = new KeyFrame<int>[count];
            float normolizeDelta = 1f / count;
            float normolizeTime = normolizeDelta;
            for (int i = 0; i < count; i++) 
            {
                result[i] = new KeyFrame<int>(normolizeTime, i);
                normolizeTime += normolizeDelta;
            }
            return result;
        }
    }
}
