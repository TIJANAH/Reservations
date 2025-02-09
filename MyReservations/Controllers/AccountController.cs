﻿using Microsoft.AspNetCore.Mvc;
using MyReservations.Models;
using System.Linq;
using BCrypt.Net;

public class AccountController : Controller
{
    private readonly ReservationsContext _context;

    public AccountController(ReservationsContext context)
    {
        _context = context;
    }

 
    public ActionResult Login()
    {
        return View();
    }
 
    [HttpPost]
    public ActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return RedirectToAction("Index", "Home"); 
        }
        else
        {
            ModelState.AddModelError(nameof(LoginViewModel.Username), "Invalid username or password");
            return View();
        }
    }

    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }


    public ActionResult Edit(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound(); 
        }
        return View(user);
    }

    [HttpPost]
    public ActionResult Edit(int id, User user)
    {
        if (id != user.UserId)
        {
            return BadRequest(); 
        }

        if (ModelState.IsValid)
        {
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home"); 
        }

        return View(user);
    }

 
    public ActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }
  

}
