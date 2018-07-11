#tool nuget:?package=MSBuild.SonarQube.Runner.Tool 
#addin "Cake.Sonar" 

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var project = "./src/TestClassLib.sln";

var appName = "TestClassLib";

///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("Restore-Nuget-Packages")
	.Does(() => {
		NuGetRestore(project);
	});

////////////////////////////////////////////////////////////////////////////// 
// Sonar 
////////////////////////////////////////////////////////////////////////////// 
 
Task("Sonar") 
  .IsDependentOn("SonarBegin") 
  .IsDependentOn("Build") 
  .IsDependentOn("SonarEnd"); 
 
Task("SonarBegin") 
  .Does(() => { 
    SonarBegin(new SonarBeginSettings{ 
      	Key = "Cake.Sonar"
      }); 
  }); 	
 
Task("SonarEnd") 
  .Does(() => { 
    SonarEnd(new SonarEndSettings{ 
	}); 
  }); 

//////////////////////////////////////////////////////////////////////////////
// Build
//////////////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Restore-Nuget-Packages")
	.Does(() => {
		MSBuild(project,new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		Configuration = configuration
    	});
	});

Task("AppVeyor")
	.IsDependentOn("Sonar");
	

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Sonar");

RunTarget(target);
