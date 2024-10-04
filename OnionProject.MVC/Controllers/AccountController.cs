using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    // Yetkisiz erişim sayfası
    public IActionResult AccessDenied()
    {
        return View();
    }
}
