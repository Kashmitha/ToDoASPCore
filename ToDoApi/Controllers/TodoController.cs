using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext todoContext;

        public TodoController(TodoContext todoContext)
        {
            this.todoContext = todoContext;
        }

        // GET : api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await todoContext.TodoItems.ToListAsync();
        }
        // GET : api/todo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todo = await todoContext.TodoItems.FindAsync(id);
            if (todo == null)
                return NotFound();

            return todo;
        }

        // POST : api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todo)
        {
            todoContext.TodoItems.Add(todo);
            await todoContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = todo.Id }, todo);
        }

        // PUT : api/todo/id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoItem(int id, TodoItem todo)
        {
            if (id != todo.Id)
                return BadRequest();

            todoContext.Entry(todo).State = EntityState.Modified;

            try
            {
                await todoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!todoContext.TodoItems.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE : api/todo/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            var todo = await todoContext.TodoItems.FindAsync(id);
            if (todo == null)
                return NotFound();

            todoContext.TodoItems.Remove(todo);
            await todoContext.SaveChangesAsync();

            return NoContent();
        }
    }
}