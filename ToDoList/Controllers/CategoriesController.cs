using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
public class CategoriesController : Controller
{
[HttpGet("/categories")]
public ActionResult Index()
{
	List<Category> allCategories = Category.GetAll();
	return View(allCategories);
}

[HttpPost("/categories")]
public ActionResult Create(string categoryName)
{
	Category newCategory = new Category(categoryName);
	newCategory.Save();
	List<Category> allCategories = Category.GetAll();
	return View("Index", allCategories);
}

[HttpGet("/categories/{categoryId}/edit")]
public ActionResult Edit(int categoryId)

{
	Category foundCategory = Category.Find(categoryId);
	return View("Edit",foundCategory );
}

[HttpPost("/categories/{id}/editedCategory")]
public ActionResult Edit(int id, string categoryName)
{
	Category newCategory = Category.Find(id);
	newCategory.Edit(categoryName);
	List<Category> allCategories = Category.GetAll();
	return View("Index", allCategories);
}

[HttpGet("/categories/new")]
public ActionResult New()
{
	return View();
}

[HttpGet("/categories/{id}")]
public ActionResult Show(int id)
{
	Dictionary<string, object> model = new Dictionary<string, object>();
	Category selectedCategory = Category.Find(id);
	List<Item> categoryItems = selectedCategory.GetItems();
	model.Add("category", selectedCategory);
	model.Add("items", categoryItems);
	return View(model);
}

[HttpPost("/categories/{categoryId}/items")]
public ActionResult Create(int categoryId, string itemDescription, DateTime dueDate, bool status)
{
	Dictionary<string, object> model = new Dictionary<string, object>();
	Category foundCategory = Category.Find(categoryId);
	Item newItem = new Item(itemDescription, dueDate, status, categoryId);
	newItem.Save();
	// foundCategory.AddItem(newItem);
	List<Item> categoryItems = foundCategory.GetItems();
	model.Add("items", categoryItems);
	model.Add("category", foundCategory);
	return View("Show", model);
}


[HttpGet("/categories/{categoryId}/itemssort")]
public ActionResult Sort(int categoryId, string itemDescription, DateTime dueDate  )
{
	Dictionary<string, object> model = new Dictionary<string, object>();
	Category foundCategory = Category.Find(categoryId);
	// Item newItem = new Item(itemDescription, dueDate);
	// newItem.Save();
	// foundCategory.AddItem(newItem);
	List<Item> sortedItems = Item.Sort();
	model.Add("items", sortedItems);
	model.Add("category", foundCategory);
	return View("Show", model);
}



[HttpGet("/search_by_category")]
public ActionResult SearchByCategory()
{
	return View();
}


[HttpPost("/search_by_category")]
public ActionResult Index(string artist)
{
	List<Category> filteredcategories = Category.FilterAll(artist);
	return View(filteredcategories);
}

[HttpGet("/categories/{categoryId}/delete")]
public ActionResult Delete(int categoryId)

{
	Category foundCategory = Category.Find(categoryId);
	foundCategory.Delete();
	return RedirectToAction("Index");
}



// [HttpGet("/categories/{categoryId}/itemssort")]
// public ActionResult SortItem(int categoryId)
// {
//   Dictionary<string, object> model = new Dictionary<string, object>();
//   Category foundCategory = Category.Find(categoryId);
//   List<Item> sortedItems= Item.Sort();
//   model.Add("items", sortedItems);
//   model.Add("category", foundCategory);
//   return View("Show", model);
// }

[HttpPost("/categories/{categoryId}/items/{itemId}/edititem")]
public ActionResult Update(int categoryId, int itemId, string newDescription, DateTime newDueDate, bool newStatus)
{
	Item item = Item.Find(itemId);
	item.Edit(newDescription, newDueDate, newStatus);

	Dictionary<string, object> model = new Dictionary<string, object>();
	Category category = Category.Find(categoryId);
	List<Item> categoryItems = category.GetItems();
	model.Add("category", category);
	model.Add("items", categoryItems);
	return View("Show", model);
}

[HttpGet("/categories/{categoryId}/items/{itemId}/done")]
public ActionResult SetStatus(int categoryId, int itemId)
{
	Item item = Item.Find(itemId);
	item.Edit(item.GetDescription(), item.GetDueDate(), true);

	Dictionary<string, object> model = new Dictionary<string, object>();
	Category category = Category.Find(categoryId);
	List<Item> categoryItems = category.GetItems();
	model.Add("category", category);
	model.Add("items", categoryItems);
	return View("Show", model);
}

[HttpPost("/categories/{categoryId}/items/{itemId}/deleteitem")]
public ActionResult DeleteItem(int categoryId, int itemId)
{
	Item item = Item.Find(itemId);
	item.Delete();
	Dictionary<string, object> model = new Dictionary<string, object>();
	Category category = Category.Find(categoryId);
	List<Item> categoryItems = category.GetItems();
	model.Add("category", category);
	model.Add("items", categoryItems);
	return View("Show", model);
}
}
}
