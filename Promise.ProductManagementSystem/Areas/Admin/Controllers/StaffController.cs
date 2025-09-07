using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Promise.ProductManagementSystem.Areas.Admin.ViewModels;
using Promise.ProductManagementSystem.Helpers;
using Promise.ProductManagementSystem.Services;
using System.Text;
using System.Text.Encodings.Web;

namespace Promise.ProductManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly EmailQueue _emailQueue;

        public StaffController(ILogger<StaffController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, EmailQueue emailQueue)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _emailQueue = emailQueue;
        }
        // GET: StaffController
        public ActionResult Index()
        {
            // return a list of staff users
            var staffRole = _roleManager.FindByNameAsync("Staff").Result;
            var staffUsers = _userManager.GetUsersInRoleAsync(staffRole.Name).Result;
            _logger.LogInformation("--> Total staff users retrieved: {count}", staffUsers.Count());
            var result = new StaffViewModel { StaffUsers = staffUsers.ToList() };

            return View(result);
        }

        // GET: StaffController/Details/5
        public ActionResult Details(string id)
        {
            var staffUser = _userManager.FindByIdAsync(id).Result;
            if (staffUser == null)
            {
                return RedirectToAction("Index");
            }
            return View(staffUser);
        }

        // GET: StaffController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStaff createStaff)
        {
            if(!ModelState.IsValid)
            {
                return View(createStaff);
            }
            var passwordHasher = new PasswordHasher<IdentityUser>();    
            var staffUser = new IdentityUser
            {
                UserName = createStaff.Email,
                Email = createStaff.Email,
                EmailConfirmed = false,
                NormalizedEmail = createStaff.Email.ToUpper(),
                NormalizedUserName = createStaff.Email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = passwordHasher.HashPassword(null, createStaff.Password),
            };
            var result = _userManager.CreateAsync(staffUser).Result;
            if (result.Succeeded) {
                var roleResult = _userManager.AddToRoleAsync(staffUser, "Staff").Result;
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("New staff user created: {email}", createStaff.Email);
                    
                    // Generate email confirmation token
                    var userId =  _userManager.GetUserIdAsync(staffUser).Result;
                    var code = _userManager.GenerateEmailConfirmationTokenAsync(staffUser).Result;
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);

                    // Queue confirmation email
                    var confirmationSubject = "Confirm your email - Product Management System";
                    var confirmationBody = $@"
                        <h2>Welcome to Product Management System</h2>
                        <p>Your staff account has been created by an administrator.</p>
                        <p>Please confirm your email address by clicking the link below:</p>
                        <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email</a></p>
                        <p>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p>{HtmlEncoder.Default.Encode(callbackUrl)}</p>
                        <p>This link will expire in 24 hours.</p>
                        <hr>
                        <p><small>This is an automated message. Please do not reply to this email.</small></p>";

                    _emailQueue.EnqueueEmail(createStaff.Email, confirmationSubject, confirmationBody);

                    // Queue welcome email with login credentials
                    var welcomeSubject = "Welcome to Product Management System - Your Account Details";
                    var welcomeBody = $@"
                        <h2>Welcome to Product Management System</h2>
                        <p>Your staff account has been successfully created by an administrator.</p>
                        <h3>Your Login Details:</h3>
                        <p><strong>Email:</strong> {createStaff.Email}</p>
                        <p><strong>Temporary Password:</strong> {createStaff.Password}</p>
                        <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                            <p style='color: #856404; margin: 0;'><strong>Important Security Notice:</strong></p>
                            <p style='color: #856404; margin: 5px 0 0 0;'>Please change your password after your first login for security reasons.</p>
                        </div>
                        <p>You can log in at: <a href='{Request.Scheme}://{Request.Host}/Identity/Account/Login' style='color: #007bff;'>Login Here</a></p>
                        <p><strong>Note:</strong> You must confirm your email address before you can log in. Please check your email for a confirmation link.</p>
                        <hr>
                        <p><small>This is an automated message. Please do not reply to this email.</small></p>";

                    _emailQueue.EnqueueEmail(createStaff.Email, welcomeSubject, welcomeBody);

                    _logger.LogInformation("Confirmation and welcome emails queued for staff user: {email}", createStaff.Email);

                    TempData["SuccessMessage"] = $"Staff account created successfully. Confirmation and welcome emails have been sent to {createStaff.Email}.";


                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(createStaff);
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(createStaff);
            }
        }

        // GET: StaffController/Edit/5
        public ActionResult Edit(string id)
        {
            var staffUser = _userManager.FindByIdAsync(id).Result;
            if (staffUser == null)
            {
                return RedirectToAction("Index");
            }
            var editModel = new EditStaff
            {
                UserId = staffUser.Id,
                UserName = staffUser.UserName,
                Email = staffUser.Email,
                EmailConfirmed = staffUser.EmailConfirmed
            };
            return View(editModel);
        }

        // POST: StaffController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, EditStaff editStaff)
        {
            if (!ModelState.IsValid) {
                return View(editStaff);
            }
            var staffUser = _userManager.FindByIdAsync(id).Result;
            if (staffUser == null)
            {
                return RedirectToAction("Index");
            }
            var originalEmail = staffUser.Email;

            staffUser.Email = editStaff.Email;
            staffUser.UserName = editStaff.UserName;   
            staffUser.EmailConfirmed = editStaff.EmailConfirmed;
            staffUser.NormalizedEmail = editStaff.Email.ToUpper();
            staffUser.NormalizedUserName = editStaff.UserName.ToUpper();

            var result = _userManager.UpdateAsync(staffUser).Result;
            if (result.Succeeded)
            {
                _logger.LogInformation("Staff user updated: {email}", editStaff.Email);

                // Queue email notification about account update
                var updateSubject = "Account Updated - Product Management System";
                var updateBody = $@"
                    <h2>Account Updated</h2>
                    <p>Your account information has been updated by an administrator.</p>
                    <h3>Updated Information:</h3>
                    <p><strong>Username:</strong> {editStaff.UserName}</p>
                    <p><strong>Email:</strong> {editStaff.Email}</p>
                    <p><strong>Email Confirmed:</strong> {(editStaff.EmailConfirmed ? "Yes" : "No")}</p>
                    {(originalEmail != editStaff.Email ? $"<p style='color: #856404;'><strong>Note:</strong> Your email address has been changed from {originalEmail} to {editStaff.Email}</p>" : "")}
                    <p>If you have any questions about these changes, please contact your administrator.</p>
                    <hr>
                    <p><small>This is an automated message. Please do not reply to this email.</small></p>";

                _emailQueue.EnqueueEmail(editStaff.Email, updateSubject, updateBody);
                _logger.LogInformation("Account update email queued for staff user: {email}", editStaff.Email);

                TempData["SuccessMessage"] = "Staff account updated successfully. Notification email sent.";

                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(editStaff);
            }
        } 

        // POST: StaffController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var staffUser = _userManager.FindByIdAsync(id).Result;
            var userEmail = staffUser?.Email;
            if (staffUser != null) {
                var result = _userManager.DeleteAsync(staffUser).Result;
                if (result.Succeeded)
                {
                    _logger.LogInformation("Staff user deleted: {email}", staffUser.Email);

                    // Queue account deletion notification email
                    var deleteSubject = "Account Deleted - Product Management System";
                    var deleteBody = $@"
                        <h2>Account Deleted</h2>
                        <p>Your account has been deleted by an administrator.</p>
                        <p>You will no longer have access to the Product Management System.</p>
                        <p>If you believe this was done in error, please contact your administrator.</p>
                        <hr>
                        <p><small>This is an automated message. Please do not reply to this email.</small></p>";

                    _emailQueue.EnqueueEmail(userEmail, deleteSubject, deleteBody);
                    _logger.LogInformation("Account deletion email queued for: {email}", userEmail);

                    TempData["SuccessMessage"] = $"Staff account deleted successfully. Notification email sent to {userEmail}.";

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return RedirectToAction("Index");
            }
        }
        // GET: Admin/Staff/ChangePassword/5
        public ActionResult ChangePassword(string id)
        {
            var staffUser = _userManager.FindByIdAsync(id).Result;
            if (staffUser == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ChangeStaffPasswordViewModel
            {
                UserId = id,
                Email = staffUser.Email
            };

            return View(model);
        }

        // POST: Admin/Staff/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangeStaffPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var staffUser = await _userManager.FindByIdAsync(model.UserId);
            if (staffUser == null)
            {
                return RedirectToAction("Index");
            }

            // Remove current password and set new one
            var removePasswordResult = await _userManager.RemovePasswordAsync(staffUser);
            if (removePasswordResult.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(staffUser, model.NewPassword);
                if (addPasswordResult.Succeeded)
                {
                    _logger.LogInformation("Password changed for staff user: {email}", staffUser.Email);
                    // Queue password change notification email
                    var passwordChangeSubject = "Password Changed - Product Management System";
                    var passwordChangeBody = $@"
                        <h2>Password Changed</h2>
                        <p>Your password has been successfully changed by an administrator.</p>
                        <div style='background-color: #d1ecf1; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                            <p style='color: #0c5460; margin: 0;'><strong>Security Notice:</strong></p>
                            <p style='color: #0c5460; margin: 5px 0 0 0;'>If you didn't request this change, please contact your administrator immediately.</p>
                        </div>
                        <p>You can log in with your new password at: <a href='{Request.Scheme}://{Request.Host}/Identity/Account/Login' style='color: #007bff;'>Login Here</a></p>
                        <hr>
                        <p><small>This is an automated message. Please do not reply to this email.</small></p>";

                    _emailQueue.EnqueueEmail(staffUser.Email, passwordChangeSubject, passwordChangeBody);
                    _logger.LogInformation("Password change email queued for staff user: {email}", staffUser.Email);

                    TempData["SuccessMessage"] = "Password updated successfully. Notification email sent.";
                    return RedirectToAction("Details", new { id = model.UserId });
                }
                else
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
