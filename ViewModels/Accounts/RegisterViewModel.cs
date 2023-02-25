﻿using System.ComponentModel.DataAnnotations;

namespace BlogAspNet_Env.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email é inválido.")]
    public string Email { get; set; }
}
