using Frontend.Controllers;
using IntroSE.Kanban.Backend.ServiceLayer;

public class ControllerFactory
{
    private static ControllerFactory instance = new ControllerFactory(); //singelton class

    public readonly UserController UserController;
    public readonly BoardController boardController;
    public readonly TaskController taskController;

    private ControllerFactory()
    {
        ServiceFactory serviceFactory = new ServiceFactory();
        UserController = new UserController(serviceFactory.us);
        boardController = new BoardController(serviceFactory.bs);
        taskController = new TaskController(serviceFactory.ts); //constructs the controllers using the services.

    }

    public static ControllerFactory Instance { get => instance; }
}