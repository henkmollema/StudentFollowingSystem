using Mandrill;
using StudentFollowingSystem.Services;

namespace StudentFollowingSystem.Mails
{
    public class MailHelper
    {
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        /// <summary>
        /// Send a mail for an appointment.
        /// </summary>
        /// <param name="appointment">The appointment data.</param>
        public void SendAppointmentMail(AppointmentMail appointment)
        {
            string body = appointment.MergeMessage();

            var email = new EmailMessage
                            {
                                text = body,
                                subject = "Studenten Volg Systeem",
                                to =
                                    appointment.FromCounseler
                                        ? new[]
                                              {
                                                  new EmailAddress(appointment.Student.Email, appointment.Student.GetFullName()),
                                                  new EmailAddress(appointment.Student.SchoolEmail, appointment.Student.GetFullName(), "cc")
                                              }
                                        : new[] { new EmailAddress(appointment.Counseler.Email, appointment.Counseler.GetFullName()) }
                            };

            _mailEngine.Send(email);
        }

        /// <summary>
        /// Send an appointment response mail.
        /// </summary>
        /// <param name="mail">The appointment response data.</param>
        public void SendAppointmentResponseMail(AppointmentRepsonseMail mail)
        {
            string body = mail.MergeMessage();

            var email = new EmailMessage
                            {
                                text = body,
                                subject = "Studenten Volg Systeem",
                                to = new[]
                                         {
                                             new EmailAddress(mail.Appointment.Student.Email, mail.Appointment.Student.GetFullName()), 
                                             new EmailAddress(mail.Appointment.Student.SchoolEmail, mail.Appointment.Student.GetFullName(), "cc")
                                         }
                            };

            _mailEngine.Send(email);
        }
    }
}
