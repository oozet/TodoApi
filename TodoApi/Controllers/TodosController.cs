using System;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        // In-memory storage for simplicity
        private static readonly List<TodoItem> _todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Lär dig Docker", Description = "Genomför Docker-labb", IsCompleted = false },
            new TodoItem { Id = 2, Title = "Deploy till Azure", Description = "Deploya API till Azure VM", IsCompleted = false }
        };
        private static int _nextId = 3;

        // GET api/todos
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetTodos()
        {
            return Ok(_todos);
        }

        // GET api/todos/{id}
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodo(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound(new { message = $"Todo med ID {id} hittades inte" });
            }
            return Ok(todo);
        }

        // POST api/todos
        [HttpPost]
        public ActionResult<TodoItem> CreateTodo(CreateTodoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { message = "Titel krävs" });
            }

            var todo = new TodoItem
            {
                Id = _nextId++,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _todos.Add(todo);
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT api/todos/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, UpdateTodoRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new { message = "ID i URL matchar inte ID i body" });
            }

            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound(new { message = $"Todo med ID {id} hittades inte" });
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { message = "Titel krävs" });
            }

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.IsCompleted = request.IsCompleted;

            return Ok(todo);
        }

        // DELETE api/todos/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound(new { message = $"Todo med ID {id} hittades inte" });
            }

            _todos.Remove(todo);
            return Ok(new { message = $"Todo med ID {id} har tagits bort" });
        }
    }
}