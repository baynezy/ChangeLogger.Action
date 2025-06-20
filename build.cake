var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");
var versionNumber = Argument("versionNumber", "0.1.0");
var testFilter = Argument("testFilter", "");
var solutionFolder = "./";
var appProject = "./src/ChangeLogger.Action/ChangeLogger.Action.csproj";

Task("Clean")
    .Does(() =>
    {
        // Clean solution
        DotNetClean(solutionFolder);
    });

Task("Restore")
	.Does(() =>
	{
		// Restore NuGet packages
		DotNetRestore(solutionFolder);
	});

Task("Build")
	.Does(() =>
	{
		// Build solution
		DotNetBuild(solutionFolder, new DotNetBuildSettings
		{
			NoRestore = true,
			Configuration = configuration,
            ArgumentCustomization = args => args.Append("/p:Version=" + versionNumber)
		});
	});

Task("Test")
	.Does(() =>
	{
	var settings = new DotNetTestSettings
                   		{
                   			NoRestore = true,
                   			Configuration = configuration,
                               Loggers = new string[] { "junit;LogFileName=results.xml" }
                   		};
                   		
        if (!string.IsNullOrEmpty(testFilter))
        {
            settings.Filter = testFilter;
        }
		// Run tests
		DotNetTest(solutionFolder, settings);
	});

RunTarget(target);