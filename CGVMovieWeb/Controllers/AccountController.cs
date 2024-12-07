using System;
using System.Linq;
using System.Web.Mvc;
using CGVMovieWeb.Models;
using System.Web.Security;

public class AccountController : Controller
{
    private readonly CGV_BinhDuong_SquareEntities _context; // Replace with your actual DbContext

    public AccountController()
    {
        _context = new CGV_BinhDuong_SquareEntities(); // Initialize your DbContext
    }


    // GET: /Account/Register
    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                return View(model);
            }

            // Check if the username already exists
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            // Create a new user
            var user = new User
            {
                Username = model.Username,
                PasswordHash = HashPassword(model.Password), // Hash the password
                FullName = model.FullName,
                Role = "Customer" // Default role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Optionally, log the user in after registration
            FormsAuthentication.SetAuthCookie(user.Username, false);
            return RedirectToAction("Index", "Home"); // Redirect to home page
        }

        return View(model);
    }

    // GET: /Account/Login
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if the user exists and the password is correct
            var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);
            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                Session["Username"] = user.FullName;
                Session["UserID"] = user.UserID;
                Session["Role"] = user.Role;
                if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
                {
                    // Nếu là Staff, chuyển hướng đến phương thức Index của AdminController
                    return RedirectToAction("MoviesList", "Admin");
                }
                return RedirectToAction("Index", "Home"); // Redirect to home page
            }

            ModelState.AddModelError("", "Invalid username or password.");
        }

        return View(model);
    }

    // Log out user
    public ActionResult LogOut()
    {
        Session.Abandon();
        FormsAuthentication.SignOut();
        return RedirectToAction("Index", "Home"); // Redirect to home page
    }

    private string HashPassword(string password)
    {
        // Implement your password hashing logic here
        return password; // Placeholder; replace with actual hashing
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        // Implement your password verification logic here
        return password == storedHash; // Placeholder; replace with actual verification
    }
}
