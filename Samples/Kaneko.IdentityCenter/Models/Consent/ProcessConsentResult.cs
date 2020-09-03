namespace Kaneko.IdentityCenter.Models.Consent
{
    public class ProcessConsentResult
    {
        public string RedirectUrl { get; set; }
        public bool IsRedirect => RedirectUrl != null;
        public ConsentViewModel ViewModel { get; set; }

        public string ValidationError { get; set; }
    }
}
