﻿namespace Construktion.Tests.SimpleBlueprints
{
    using System;
    using Blueprints.Simple;
    using Internal;
    using Shouldly;

    public class DateTimeBlueprintTests
    {
        public void should_construct()
        {
            var blueprint = new DateTimeBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(DateTime)),
                new DefaultConstruktionPipeline());

            result.ShouldNotBe(default(DateTime));
        }
    }
}