using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace HarvestValley.Extenstions
{
    public static class Extension
    {
        public static int GetScoreValue(this Enum Value)
        {
            Type Type = Value.GetType();

            FieldInfo FieldInfo = Type.GetField(Value.ToString());

            ScoreAttribute Attribute = FieldInfo.GetCustomAttribute(
                typeof(ScoreAttribute)
            ) as ScoreAttribute;

            return Attribute.Score;
        }

        public static Rectangle GetSpriteRectangle(this Enum Value)
        {
            Type Type = Value.GetType();

            FieldInfo FieldInfo = Type.GetField(Value.ToString());

            SpriteRectangleAttribute Attribute = FieldInfo.GetCustomAttribute(
                typeof(SpriteRectangleAttribute)
            ) as SpriteRectangleAttribute;

            return Attribute.SpriteRectangle;
        }
    }
}
