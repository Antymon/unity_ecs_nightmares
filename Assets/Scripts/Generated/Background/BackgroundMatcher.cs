//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class BackgroundMatcher {

    public static Entitas.IAllOfMatcher<BackgroundEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<BackgroundEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<BackgroundEntity> AllOf(params Entitas.IMatcher<BackgroundEntity>[] matchers) {
          return Entitas.Matcher<BackgroundEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<BackgroundEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<BackgroundEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<BackgroundEntity> AnyOf(params Entitas.IMatcher<BackgroundEntity>[] matchers) {
          return Entitas.Matcher<BackgroundEntity>.AnyOf(matchers);
    }
}
