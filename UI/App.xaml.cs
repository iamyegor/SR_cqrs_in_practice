using UI.Api;

namespace UI
{
    public sealed partial class App
    {
        public App()
        {
            ApiClient.Init("http://192.168.0.10:52335/api/students");
        }
    }
}
