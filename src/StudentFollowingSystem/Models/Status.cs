using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.Models
{
    /// <summary>
    /// Represents the status of a student.
    /// </summary>
    public enum Status
    {
        [Display(Name = "Groen")]
        Green,

        [Display(Name = "Oranje")]
        Orange,

        [Display(Name = "Rood")]
        Red
    }
}
