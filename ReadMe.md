## Delivery Calculator Demo App

In order to run this application, you will need Visual Studio 2017 and .Net Core 2.0 SDK.

Download the solution, build and run.  You will be able to access the site on http://localhost:5000.  This is basically a single page app with directions for use on the index page.

The site should display properly in either IE10+, FireFox or Chrome.

If you have any issues compiling the site, please open a command prompt, move to the solution directory and type "dotnet restore".  While a rebuild __should__ download all the proper nuget packages, it's been my experience that it doesn't always work that way in practice.  Also, if there are any issues beyond just missing packages, the dotnet restore command line almost always provides more information.

Please ignore the warning about the ExpressMapper package not being available for .Net Core.  I have my own branch of ExpressMapper that is .Net Core compliant, but after thorough testing, I've found that this code does work correctly other than the annoying warnings on compile.
