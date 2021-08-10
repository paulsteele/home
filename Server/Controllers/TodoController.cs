﻿using System.Linq;
using System.Threading.Tasks;
using hub.Server.Database;
using hub.Shared.Models.Todo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hub.Server.Controllers
{
	[ApiController]
	[Route("todos")]
    public class TodoController : ControllerBase
    {
	    private readonly UserManager<IdentityUser> _userManager;
	    private readonly IDb _db;

	    public TodoController(
		    UserManager<IdentityUser> userManager,
		    IDb db
		)
	    {
		    _userManager = userManager;
		    _db = db;
	    }
	    
	    [HttpGet]
	    public async Task<IActionResult> GetAllTodos()
	    {
		    var user = await _userManager.GetUserAsync(HttpContext.User);

		    var todos = _db.DatabaseContext.Todos.Where(t => t.User == user).ToArray();
		    return Ok(todos);
	    }
	    
	    [HttpPut]
	    public async Task<IActionResult> UpdateTodo(TodoModel todoModel)
	    {
		    var user = await _userManager.GetUserAsync(HttpContext.User);
		    
		    // sanitization
		    todoModel.User = user;

		    var savedTodo = _db.DatabaseContext.Todos.Update(todoModel);

		    await _db.DatabaseContext.SaveChangesAsync();
		    return Ok(savedTodo.Entity);
	    }
	    
	    [HttpDelete]
	    public async Task<IActionResult> DeleteTodo(TodoModel todoModel)
	    {
		    var user = await _userManager.GetUserAsync(HttpContext.User);
		    
		    // sanitization
		    todoModel.User = user;

		    _db.DatabaseContext.Todos.Remove(todoModel);

		    await _db.DatabaseContext.SaveChangesAsync();
		    return Ok();
	    }
    }
}