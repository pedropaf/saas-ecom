namespace $rootnamespace$.Views.SaasEcom.ViewModels
{
    public abstract class FlashViewModel
    {
        protected FlashViewModel(string message, string css)
        {
            this.Message = message;
            this.CSS = css;
        }

        public string CSS { get; set; }

        public string Message { get; set; }
    }

    public class FlashSuccessViewModel : FlashViewModel
    {
        public FlashSuccessViewModel(string message)
            : base(message, "success")
        {
        }
    }

    public class FlashInfoViewModel : FlashViewModel
    {
        public FlashInfoViewModel(string message)
            : base(message, "info")
        {
        }
    }

    public class FlashWarningViewModel : FlashViewModel
    {
        public FlashWarningViewModel(string message)
            : base(message, "warning")
        {
        }
    }

    public class FlashDangerViewModel : FlashViewModel
    {
        public FlashDangerViewModel(string message)
            : base(message, "danger")
        {
        }
    }
}