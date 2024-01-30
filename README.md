# Introduction 
ASP V2.0 

# Prerequisites
Before getting started, ensure you have the following installed on your machine: 

- [Node.js](https://nodejs.org/)
- [npm](https://www.npmjs.com/)


# Getting Started
To generate design system css/js/assets, you must run the below commands within `ASP.Web`
```
npm install
npm run build
```

# Acceptance tests using SpecFlow
The `ASP.AcceptanceTests` project needs the `SpecFlow for Visual Studio 2022` extension to edit and run the SpecFlow tests from the Visual Studio test runner. The version in the Visual Studio Marketplace doesn't support .NET 8 yet, but there is an out-of-band release that supports it, [available here](https://github.com/SpecFlowOSS/SpecFlow.VS/releases/tag/v2022.1.93-net8) (download and run the `.vsix` file.)