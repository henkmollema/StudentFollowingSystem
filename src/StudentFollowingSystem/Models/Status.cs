using System.ComponentModel.DataAnnotations;

namespace StudentFollowingSystem.Models
{
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
