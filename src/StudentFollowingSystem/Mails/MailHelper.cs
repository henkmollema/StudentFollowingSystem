using Mandrill;
using StudentFollowingSystem.Services;

namespace StudentFollowingSystem.Mails
{
    public class MailHelper
    {
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        public void SendAppointmentByCounseler(AppointmentMail appointment)
        {
            string body = appointment.MergeMessage();

            var email = new EmailMessage
                          {
                              text = body,
                              to = new[] { new EmailAddress(appointment.Student.Email, appointment.Student.GetFullName()) }
                          };

            _mailEngine.Send(email);
        }
    }
}
