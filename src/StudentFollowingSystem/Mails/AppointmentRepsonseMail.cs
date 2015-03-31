using System.Text;
using StudentFollowingSystem.Models;

namespace StudentFollowingSystem.Mails
{
    /// <summary>
    /// The mail for an appointment response.
    /// </summary>
    public class AppointmentRepsonseMail
    {
        public Appointment Appointment { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the response came from a counseler.
        /// </summary>
        public bool FromCounseler { get; set; }

        /// <summary>
        /// Merge data in the model in a mail message.
        /// </summary>
        public string MergeMessage()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Beste {0},",
                FromCounseler
                    ? Appointment.Counseler.GetFullName()
                    : Appointment.Student.GetFullName())
              .AppendLine()
              .AppendLine()
              .AppendFormat("{0} heeft je uitnodiging voor een SLB gesprek {1}geaccepteerd.",
                  FromCounseler
                      ? Appointment.Student.GetFullName()
                      : Appointment.Counseler.GetFullName(),
                  !Appointment.Accepted
                      ? "niet "
                      : string.Empty);

            if (!string.IsNullOrEmpty(Notes))
            {
                sb.AppendLine()
                  .AppendLine()
                  .AppendLine("De volgende opmerkingen werden bij de reactie geplaatst:")
                  .Append(Notes);
            }

            if (Appointment.Accepted)
            {
                sb.AppendLine()
                  .AppendLine()
                  .AppendFormat("De afspraak zal plaatsvinden op {0:dddd d MMMM} om {0:HH:mm}u", Appointment.DateTime)
                  .AppendLine()
                  .AppendFormat("Locatie: {0}", Appointment.Location);
            }
            else
            {
                sb.AppendLine()
                  .AppendLine()
                  .Append("De afspraak is geannuleerd.");
            }

            sb.AppendLine()
              .AppendLine()
              .AppendLine("Met vriendelijke groet,")
              .Append("NHL Hogeschool");

            return sb.ToString();
        }
    }
}
