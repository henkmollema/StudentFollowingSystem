﻿using Mandrill;
using StudentFollowingSystem.Services;

namespace StudentFollowingSystem.Mails
{
    public class MailHelper
    {
        private readonly MandrillMailEngine _mailEngine = new MandrillMailEngine();

        public void SendAppointmentMail(AppointmentMail appointment)
        {
            string body = appointment.MergeMessage();

            var email = new EmailMessage
                            {
                                text = body,
                                to = new[]
                                         {
                                             appointment.FromCounseler
                                                 ? new EmailAddress(appointment.Student.Email, appointment.Student.GetFullName())
                                                 : new EmailAddress("henkmollema@gmail.com", appointment.Counseler.GetFullName())
                                         }
                            };

            _mailEngine.Send(email);
        }

        public void SendAppointmentResponseMail(AppointmentRepsonseMail mail)
        {
            string body = mail.MergeMessage();

            var email = new EmailMessage
            {
                text = body,
                to = new[] { new EmailAddress(mail.Appointment.Student.Email, mail.Appointment.Student.GetFullName()) }
            };

            _mailEngine.Send(email);
        }
    }
}
