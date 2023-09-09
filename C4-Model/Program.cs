using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 85947;
            const string apiKey = "b57c77e3-b8c6-4611-97f7-658da42d0a69";
            const string apiSecret = "1da66943-8509-48d0-9d34-5efe249f2dd3";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);

            Workspace workspace = new Workspace("C4 Model", "FastPorte");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem fastPorte = model.AddSoftwareSystem(
                "FastPorte",
                "Software que conectará transportistas de diferentes rubros (independientes o empresas) con personas (clientes o personas naturales) que requieren de sus servicios.");
            SoftwareSystem googleMaps = model.AddSoftwareSystem(
                "Google Maps", "Plataforma que ofrece una REST API de información geo referencial.");
            SoftwareSystem reniec = model.AddSoftwareSystem(
                "RENIEC System",
                "Organización autónoma del Estado Peruano encargada de la validación de datos.");
            SoftwareSystem email = model.AddSoftwareSystem(
                "E-mail System",
                "Sistema de confirmación de servicios mediante correo.");
            SoftwareSystem payment = model.AddSoftwareSystem(
                "Payment System",
                "Intermediario de pagos.");

            Person client =
                model.AddPerson("Client", "Persona que requiere de algún servicio de transporte.");
            Person carrier =
                model.AddPerson("Carrier", "Trabajadores el sector transporte que quieran brindar sus servicios.");
            Person admin = model.AddPerson("Admin", "Usuario administrador.");

            client.Uses(fastPorte, "Contacta con transportistas.");
            carrier.Uses(fastPorte, "Promociona sus servicios de transporte.");
            admin.Uses(fastPorte, "Usuario administrador del sistema.");

            fastPorte.Uses(googleMaps, "Usa la API de google maps");
            fastPorte.Uses(reniec, "Verifica datos de los usuarios");
            fastPorte.Uses(email, "Envia correos de confirmación de servicios");
            fastPorte.Uses(payment, "Realiza pagos mediante la aplicación");

            // Tags
            client.AddTags("Client");
            carrier.AddTags("Carrier");
            admin.AddTags("Admin");
            fastPorte.AddTags("FastPorte");
            googleMaps.AddTags("GoogleMaps");
            reniec.AddTags("RENIEC System");
            email.AddTags("E-mail System");
            payment.AddTags("Payment System");

            Styles styles = viewSet.Configuration.Styles;

            styles.Add(new ElementStyle("Client")
            {
                Background = "#0a60ff",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("Carrier")
            {
                Background = "#0a60ff",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("Admin")
            {
                Background = "#aa60af",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("FastPorte")
            {
                Background = "#008f39",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("GoogleMaps")
            {
                Background = "#90714c",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("RENIEC System")
            {
                Background = "#7971eb",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("E-mail System")
            {
                Background = "#2f95c7",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("Payment System")
            {
                Background = "#f29549",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });

            SystemContextView contextView =
                viewSet.CreateSystemContextView(fastPorte, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // 2. Diagrama de Contenedores

            Container mobileApplication = fastPorte.AddContainer(
                "Mobile App",
                "los clientes podrán contratar los servicios de transporte desde su celular",
                "Flutter/Dart");
            Container webApplication = fastPorte.AddContainer(
                "Web App",
                "los clientes podrán contratar los servicios de transporte desde la web",
                "Angular");
            Container landingPage = fastPorte.AddContainer(
                "Landing Page",
                "los usuarios podrán visualizar las características de FastPorte",
                "Bootstrap");
            Container apiGateway = fastPorte.AddContainer(
                "API Gateway",
                "API Gateway",
                "Java - SpringBoot");

            Container personalData = fastPorte.AddContainer(
                "Personal Data Context",
                "Informacion de usuario (mombre, edad, fotos, etc.)",
                "Java - SpringBoot");
            Container search = fastPorte.AddContainer(
                "Search Context",
                "Búsqueda de transportistas según las características requeridas por el cliente",
                "Java - SpringBoot");
            Container hiring = fastPorte.AddContainer(
                "Hiring Context",
                "Contratación del servicio",
                "Java - SpringBoot");
            Container paymentService = fastPorte.AddContainer(
                "Payment Context",
                "Pago del servicio",
                "Java - SpringBoot");
            Container location = fastPorte.AddContainer(
                "Location Context",
                "Ubicación del transportista en tiempo real",
                "Java - SpringBoot");

            Container databasePD = fastPorte.AddContainer("Database Personal Data", "Información personal", "MySQL");
            Container databaseS = fastPorte.AddContainer("Database Search", "Búsqueda de transportistas", "MySQL");
            Container databaseH = fastPorte.AddContainer("Database Hiring", "Contratación del servicio", "MySQL");
            Container databasePS = fastPorte.AddContainer("Database Payment", "Pago del servicio", "MySQL");
            Container databaseL = fastPorte.AddContainer("Database Location", "Ubicación del transportista", "MySQL");

            client.Uses(mobileApplication, "Consulta");
            client.Uses(webApplication, "Consulta");
            client.Uses(landingPage, "Consulta");

            admin.Uses(mobileApplication, "Consulta");
            admin.Uses(webApplication, "Consulta");
            admin.Uses(landingPage, "Consulta");

            carrier.Uses(mobileApplication, "Consulta");
            carrier.Uses(webApplication, "Consulta");
            carrier.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            apiGateway.Uses(personalData, "", "");
            apiGateway.Uses(search, "", "");
            apiGateway.Uses(hiring, "", "");
            apiGateway.Uses(paymentService, "", "");
            apiGateway.Uses(location, "", "");

            personalData.Uses(databasePD, "Writes and Read from", "");
            personalData.Uses(reniec, "API Request", "JSON/HTTPS");

            search.Uses(databaseS, "Writes and Read from", "");

            hiring.Uses(databaseH, "Writes and Read from", "");
            hiring.Uses(email, "API Request", "JSON/HTTPS");

            paymentService.Uses(databasePS, "Writes and Read from", "");
            paymentService.Uses(payment, "API Request", "JSON/HTTPS");

            location.Uses(databaseL, "Writes and Read from", "");
            location.Uses(googleMaps, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");

            string databaseTag = "Database";

            databasePD.AddTags(databaseTag);
            databaseS.AddTags(databaseTag);
            databaseH.AddTags(databaseTag);
            databasePS.AddTags(databaseTag);
            databaseL.AddTags(databaseTag);

            string contextTag = "Context";

            personalData.AddTags(contextTag);
            search.AddTags(contextTag);
            hiring.AddTags(contextTag);
            paymentService.AddTags(contextTag);
            location.AddTags(contextTag);

            styles.Add(new ElementStyle("MobileApp")
            {
                Background = "#9d33d6",
                Color = "#ffffff",
                Shape = Shape.MobileDevicePortrait,
                Icon = ""
            });
            styles.Add(new ElementStyle("WebApp")
            {
                Background = "#7be3af",
                Color = "#ffffff",
                Shape = Shape.WebBrowser,
                Icon = ""
            });
            styles.Add(new ElementStyle("LandingPage")
            {
                Background = "#929000",
                Color = "#ffffff",
                Shape = Shape.WebBrowser,
                Icon = ""
            });
            styles.Add(new ElementStyle("APIGateway")
            {
                Background = "#0000ff",
                Color = "#ffffff",
                Shape = Shape.RoundedBox,
                Icon = ""
            });
            styles.Add(new ElementStyle(contextTag)
            {
                Background = "#facc2e",
                Shape = Shape.Hexagon,
                Icon = ""
            });
            styles.Add(new ElementStyle(databaseTag)
            {
                Background = "#ff0000",
                Color = "#ffffff",
                Shape = Shape.Cylinder,
                Icon = ""
            });

            ContainerView containerView =
                viewSet.CreateContainerView(fastPorte, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
