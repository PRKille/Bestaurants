using Microsoft.AspNetCore.Mvc;
using Bestaurant.Models;
using System.Collections.Generic;
using System.Linq;
// using System.Collections.IEnumerable;
using Microsoft.EntityFrameworkCore;

namespace Bestaurant.Controllers
{
  public class CuisineController : Controller 
  {
    private readonly BestaurantContext _db;

    public CuisineController(BestaurantContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      try
      {
        List<Cuisine> model = _db.Cuisines.ToList();
        return View(model);
      }
      catch
      {
        return RedirectToAction("Index", "Home");
      }
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Cuisine cuisine)
    {
      try
      {
        _db.Cuisines.Add(cuisine);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      try
      {
        Cuisine currentCuisine = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
        currentCuisine.Restaurants = _db.Restaurants.Where(restaurant => restaurant.CuisineId == id).ToList();
        return View(currentCuisine);
      }
      catch
      {
        return RedirectToAction("Index");
      }
    }

    public ActionResult Edit(int id)
    {
      try
      {
        Cuisine thisCuisine = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
        return View(thisCuisine);
      }
      catch
      {
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public ActionResult Edit(Cuisine cuisine)
    {
      try
      {
        _db.Entry(cuisine).State = EntityState.Modified; //find and update all the entries for the properties of this specific instance
        //state (state of instance), is now equal to MODIFIED state..
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = cuisine.CuisineId } );
      }
      catch
      {
        return RedirectToAction("Index");    
      }
    }

    public ActionResult Delete(int id)
    {
      try
      {
        Cuisine cuisineToDelete = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
        return View(cuisineToDelete);
      }
      catch
      {
        return RedirectToAction("Index");
      }
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      try
      {

        // Alternative solution to removing child objects from DB - raw SQL 
        // var numOfDeletions = _db.Database.ExecuteSqlCommand($"DELETE FROM restaurants WHERE CuisineId={id}");
        // var cuisineOfDeletion = _db.Database.ExecuteSqlCommand($"DELETE FROM cuisines WHERE CuisineId={id}");

        //remove child restaurants of the chosen cuisine
        Cuisine cuisineToDelete = _db.Cuisines.FirstOrDefault(cuisine => cuisine.CuisineId == id);
        cuisineToDelete.Restaurants = _db.Restaurants.Where(restaurant => restaurant.CuisineId == id).ToList();
        foreach(Restaurant restaurant in cuisineToDelete.Restaurants)
        {
          _db.Restaurants.Remove(restaurant);
        }
        _db.Cuisines.Remove(cuisineToDelete);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return RedirectToAction("Index");
      }
    }
  }
}