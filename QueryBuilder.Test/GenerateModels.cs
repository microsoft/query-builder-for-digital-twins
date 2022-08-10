// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.DigitalWorkplace.DigitalTwins.Models.Generator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public static class GenerateModels
    {
        [AssemblyInitialize]
        public static async Task GenerateCsharpModelsAsync(TestContext _)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var jsonDir = Path.Combine(currentDir, "..", "..", "..", "DtdlModels");
            var outDir = Path.Combine(currentDir, "..", "..", "..", "..", "QueryBuilder.Test.Generated");

            var options = new ModelGeneratorOptions
            {
                OutputDirectory = outDir,
                Namespace = "QueryBuilder.Test.Generated",
                JsonModelsDirectory = jsonDir,
                CopyrightHeader = "// Copyright (c) Microsoft Corporation.\n// Licensed under the MIT License."
            };

            var generator = new ModelGenerator(options);
            await generator.GenerateClassesAsync().ConfigureAwait(false);
        }
    }
}