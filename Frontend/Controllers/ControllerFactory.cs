using Frontend.Controllers;
using IntroSE.Kanban.Backend.ServiceLayer;

public class ControllerFactory
{
    public static ControllerFactory Instance { get; } = new ControllerFactory();

    public readonly UserController UserController;

    private ControllerFactory()
    {
        ServiceFactory serviceFactory = new ServiceFactory();
        UserController = new UserController(serviceFactory.Us);  
    }
}