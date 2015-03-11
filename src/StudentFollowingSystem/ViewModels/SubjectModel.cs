using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.ViewModels
{
    public class SubjectModel : IValidatableObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                // Start date is after end date.
                yield return new ValidationResult("De start datum/tijd kan niet na de eind datum/tijd liggen.");
            }
        }
    }
}
