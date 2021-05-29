using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    [DefOf]
    public static class RecipeDefOf
    {
        public static RecipeDef Anesthetize;
        public static RecipeDef Euthanize;

        static RecipeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
        }
    }
}
