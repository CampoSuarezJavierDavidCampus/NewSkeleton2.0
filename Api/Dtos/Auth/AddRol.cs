using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Auth;
public class AddRol{

    [Required(ErrorMessage = "Role is required")]
    public string RolName { get; set; } = String.Empty;    
}