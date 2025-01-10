using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "İsim Soyisim alanı zorunludur.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(20, ErrorMessage = "Şifre en az {2} ve en fazla {1} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifre ve şifre tekrar alanı eşleşmiyor.")]
        public string? ConfirmPassword { get; set; }
    }
}
