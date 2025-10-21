namespace qa_dotnet_cucumber.Config
{
    public class TestSettings
    {
        public BrowserSettings Browser { get; set; }
        public ReportSettings Report { get; set; }
        public EnvironmentSettings Environment { get; set; }

        // New properties for dynamic test users
        public string TestUserEmail { get; set; } = "autotest_user@test.com";
        public string TestUserPassword { get; set; } = "Pass123!";
    }

    public class BrowserSettings
    {
        public string Type { get; set; }
        public bool Headless { get; set; }
        public int TimeoutSeconds { get; set; }
    }

    public class ReportSettings
    {
        public string Path { get; set; }
        public string Title { get; set; }
    }

    public class EnvironmentSettings
    {
        public string BaseUrl { get; set; }
    }
}