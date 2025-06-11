using Frontend.Controllers;
using IntroSE.Kanban.Backend.ServiceLayer;

public class ControllerFactory
{
    public static ControllerFactory Instance { get; } = new ControllerFactory();

    public readonly UserController UserController;
    public readonly BoardController boardController;
    public readonly TaskController taskController;

    private ControllerFactory()
    {
        ServiceFactory serviceFactory = new ServiceFactory();
        UserController = new UserController(serviceFactory.Us);
        boardController = new BoardController(serviceFactory.bs);
        taskController = new TaskController(serviceFactory.ts);

    }
}