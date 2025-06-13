using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Allure.Commons;

[SetUpFixture]
public class AllureSetup
{
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        var asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        Environment.CurrentDirectory = asmPath;

        AllureLifecycle.Instance.CleanupResultDirectory();
    }
}