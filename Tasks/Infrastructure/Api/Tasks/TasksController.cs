using Intaker.Application.Tasks.Commands;
using Intaker.Application.Tasks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Intaker.Infrastructure.Api.Tasks;

[ApiController]
[Route("[controller]/[action]")]
public class TasksController(ILogger<TasksController> logger,
    TaskStatusChangeCommand taskStatusChangeCommand,
    TaskCreateCommand taskCreateCommand,
    GetTaskQuery getTaskQuery,
    GetAllTasksQuery getAllTasksQuery) : ControllerBase
{
    private readonly ILogger<TasksController> _logger = logger;
    private readonly TaskStatusChangeCommand taskStatusChangeCommand = taskStatusChangeCommand;
    private readonly TaskCreateCommand taskCreateCommand = taskCreateCommand;
    private readonly GetTaskQuery getTaskQuery = getTaskQuery;
    private readonly GetAllTasksQuery getAllTasksQuery = getAllTasksQuery;

    [HttpGet(Name = nameof(Get))]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var queryInput = new GetTaskQueryInput { TaskId = id };
        var task = await getTaskQuery.ExecuteAsync(queryInput);
        if (task == null)
            return BadRequest();

        var viewModel = new TaskViewModel(task);
        return Ok(viewModel);
    }

    [HttpGet(Name = nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var queryInput = new GetAllTasksQueryInput();
        var tasks = await getAllTasksQuery.ExecuteAsync(queryInput);
        var viewModels = tasks.Select(t => new TaskViewModel(t));
        return Ok(viewModels);
    }

    [HttpPost(Name = nameof(StatusChange))]
    public async Task<IActionResult> StatusChange([FromBody] TaskStatusChangeRequest request)
    {
        var input = new TaskStatusChangeCommandInput(request.TaskId, request.TaskStatusId);
        await taskStatusChangeCommand.ExecuteAsync(input);
        return Ok();
    }

    [HttpPost(Name = nameof(Create))]
    public async Task<IActionResult> Create([FromBody] TaskCreateRequest request)
    {
        var input = new TaskCreateCommandInput
            (request.TaskStatusId, request.Name, request.Description, request.AssignedTo);
        var id = await taskCreateCommand.ExecuteAsync(input);
        return Ok(id);
    }
}
