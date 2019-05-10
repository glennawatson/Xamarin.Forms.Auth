#load nuget:https://www.myget.org/F/reactiveui/api/v2?package=ReactiveUI.Cake.Recipe&prerelease

Environment.SetVariableNames();

// Whitelisted Packages
var packageWhitelist = new[] 
{ 
    MakeAbsolute(File("./src/Xamarin.Forms.Auth/Xamarin.Forms.Auth.csproj")),
};

var packageTestWhitelist = new FilePath[]
{
};

BuildParameters.SetParameters(context: Context, 
                            buildSystem: BuildSystem,
                            title: "Xamarin.Forms.Auth",
                            whitelistPackages: packageWhitelist,
                            whitelistTestPackages: packageTestWhitelist,
                            artifactsDirectory: "./artifacts",
                            sourceDirectory: "./src");

ToolSettings.SetToolSettings(context: Context);

Build.Run();
