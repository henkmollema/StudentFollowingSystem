using Mandrill;

namespace StudentFollowingSystem.Services
{
    public class MandrillMailEngine
    {
        private readonly IMandrillApi _api;

        public MandrillMailEngine()
        {
            _api = new MandrillApi("WzIKKJSUZAXYaRLWVqPyVA");
        }

        public void Send(EmailMessage mailMessage)
        {
            mailMessage.from_email = "moll1400@nhl.nl";
            mailMessage.from_name = "Student Volg Systeem";
            var results = _api.SendMessage(mailMessage);
        }
    }
}
