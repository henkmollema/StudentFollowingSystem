using System;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Mails
{
    public class AppointmentMail
    {
        public Student Student { get; set; }

        public Counseler Counseler { get; set; }

        public DateTime DateTime { get; set; }

        public string AcceptUrl { get; set; }

        public string Location { get; set; }

        public string MergeMessage()
        {
            return string.Format(@"Beste {0},

{1} heeft je uitgenodigd voor een gesprek op {2:dddd d MMMM} om {2:HH:mm}u.
Locatie: {3}.

Klik op ondertaande link om aan te geven of je hiermee akkoord gaat of niet:
{4}

Met vriendelijke groet,

NHL Hogeschool
",
                Student.GetFullName(),
                Counseler.GetFullName(),
                DateTime,
                Location,
                AcceptUrl);
        }
    }
}
