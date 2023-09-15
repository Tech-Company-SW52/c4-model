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

            // 3. Diagrama de Componentes

            // Search

            Component domainLayerS =
                search.AddComponent("Domain Layer", "", "Angular");
            Component searchController = search.AddComponent(
                "SearchController",
                "REST API endpoints de búsqueda de servicios.",
                "REST Controller");
            Component searchService = search.AddComponent(
                "SearchApplicationService",
                "Provee métodos para la búsqueda de servicios",
                "Component");
            Component transportServiceRepository = search.AddComponent(
                "TransportServiceRepository",
                "Información de los servicios de transporte",
                "Component");

            apiGateway.Uses(searchController, "", "JSON/HTTPS");
            searchController.Uses(searchService, "Invoca métodos de búsqueda");

            searchService.Uses(domainLayerS, "Usa", "");
            searchService.Uses(transportServiceRepository, "", "");

            transportServiceRepository.Uses(databaseS, "", "");

            // Tags
            domainLayerS.AddTags("DomainLayerS");
            searchController.AddTags("SearchController");
            searchService.AddTags("SearchApplicationService");
            transportServiceRepository.AddTags("TransportServiceRepository");

            styles.Add(new ElementStyle("DomainLayerS")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("SearchController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("SearchApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("TransportServiceRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewS = viewSet.CreateComponentView(search, "Search", "Component Diagram");
            componentViewS.PaperSize = PaperSize.A4_Landscape;
            componentViewS.Add(webApplication);
            componentViewS.Add(apiGateway);
            componentViewS.Add(databaseS);
            componentViewS.AddAllComponents();

            // Hiring

            Component domainLayerH =
                hiring.AddComponent("Domain Layer", "", "Angular");
            Component hiringController = hiring.AddComponent(
                "HiringController",
                "REST API endpoints de contratación y pagos de servicios.",
                "REST Controller");
            Component hiringApplicationService = hiring.AddComponent(
                "HiringApplicationService",
                "Provee métodos para la contración de servicios de transporte.",
                "Component");
            Component hiringRepository = hiring.AddComponent(
                "HiringRepository",
                "Información de los contratos",
                "Component");

            apiGateway.Uses(hiringController, "", "JSON/HTTPS");

            hiringController.Uses(hiringApplicationService, "Invoca métodos de contratación de servicios de trasnporte", "");

            hiringApplicationService.Uses(domainLayerH, "Usa", "");
            hiringApplicationService.Uses(hiringRepository, "Usa", "");

            hiringRepository.Uses(databaseH, "", "");

            // Tags
            domainLayerH.AddTags("DomainLayerH");
            hiringController.AddTags("HiringController");
            hiringApplicationService.AddTags("HiringApplicationService");
            hiringRepository.AddTags("HiringRepository");

            styles.Add(new ElementStyle("DomainLayerH")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("HiringController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("HiringApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("HiringRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewH =
                viewSet.CreateComponentView(hiring, "Hiring", "Component Diagram");
            componentViewH.PaperSize = PaperSize.A4_Landscape;
            componentViewH.Add(webApplication);
            componentViewH.Add(apiGateway);
            componentViewH.Add(databaseH);
            componentViewH.AddAllComponents();

            // PaymentService

            Component domainLayerPS =
                paymentService.AddComponent("Domain Layer", "", "Angular");
            Component paymentServiceController = paymentService.AddComponent(
                "PaymentServiceController",
                "REST API endpoints de contratación y pagos de servicios.",
                "REST Controller");
            Component paymentServiceApplicationService = paymentService.AddComponent(
                "PaymentServiceApplicationService",
                "Provee métodos para el pago de contratos",
                "Component");
            Component paymentServiceRepository = paymentService.AddComponent(
                "PaymentServiceRepository",
                "Información de los contratos",
                "Component");
            Component paymentFacade =
                paymentService.AddComponent("PaymentFacade", "", "Component");

            apiGateway.Uses(paymentServiceController, "", "JSON/HTTPS");

            paymentServiceController.Uses(paymentServiceApplicationService, "Invoca métodos de pago de contratos", "");
            paymentServiceController.Uses(paymentServiceApplicationService, "Invoca métodos de contratación de servicios de trasnporte", "");

            paymentServiceApplicationService.Uses(domainLayerPS, "Usa", "");

            paymentServiceApplicationService.Uses(domainLayerPS, "Usa", "");
            paymentServiceApplicationService.Uses(paymentFacade, "Usa");
            paymentServiceApplicationService.Uses(paymentServiceRepository, "", "");

            paymentServiceRepository.Uses(databasePS, "", "");

            paymentFacade.Uses(payment, "JSON/HTTPS");

            // Tags
            domainLayerPS.AddTags("DomainLayerPS");
            paymentServiceController.AddTags("PaymentServiceController");
            paymentServiceApplicationService.AddTags("PaymentServiceApplicationService");
            paymentServiceRepository.AddTags("PaymentServiceRepository");
            paymentFacade.AddTags("PaymentFacade");

            styles.Add(new ElementStyle("DomainLayerPS")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentServiceController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentServiceApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentServiceRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentFacade")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewPS =
                viewSet.CreateComponentView(paymentService, "PaymentService", "Component Diagram");
            componentViewPS.PaperSize = PaperSize.A4_Landscape;
            componentViewPS.Add(webApplication);
            componentViewPS.Add(apiGateway);
            componentViewPS.Add(databasePS);
            componentViewPS.Add(payment);
            componentViewPS.AddAllComponents();

            // Location

            Component domainLayerL = location.AddComponent("Domain Layer", "", "");
            Component locationController = location.AddComponent(
                "SeguridadController",
                "REST API endpoints de ubicación.",
                "REST Controller");
            Component locationService = location.AddComponent(
                "LocationApplicationService",
                "Provee métodos para la localización del usuario transportista.",
                "Component");
            Component carrierRepository = location.AddComponent(
                "CarrierRepository",
                "Información de los usuarios transportistas",
                "Component");
            Component locationFacade =
                location.AddComponent("LocationFacade", "", "Component");

            apiGateway.Uses(locationController, "", "JSON/HTTPS");
            locationController.Uses(locationService, "Invoca métodos de localización");

            locationService.Uses(domainLayerL, "Usa", "");
            locationService.Uses(carrierRepository, "", "");
            locationService.Uses(locationFacade, "Usa", "");

            carrierRepository.Uses(databaseL, "", "");

            locationFacade.Uses(googleMaps, "", "JSON/HTTPS");

            // Tags
            domainLayerL.AddTags("DomainLayerL");
            locationController.AddTags("LocationController");
            locationService.AddTags("LocationApplicationService");
            carrierRepository.AddTags("CarrierRepository");
            locationFacade.AddTags("LocationFacade");

            styles.Add(new ElementStyle("DomainLayerL")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("LocationController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("LocationApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("LocationFacade")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("CarrierRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewL =
                viewSet.CreateComponentView(location, "Location", "Component Diagram");
            componentViewL.PaperSize = PaperSize.A4_Landscape;
            componentViewL.Add(webApplication);
            componentViewL.Add(apiGateway);
            componentViewL.Add(databaseL);
            componentViewL.Add(googleMaps);
            componentViewL.AddAllComponents();

            // Personal Data

            Component domainLayerPD = personalData.AddComponent("Domain Layer", "", "");
            Component personalDataController = personalData.AddComponent(
                "PersonalDataController",
                "REST API endpoints de información personal del usuario.",
                "REST Controller");
            Component personalDataService = personalData.AddComponent(
                "PersonalDatApplicationService",
                "Provee métodos para el apartado de información personal del usuario",
                "Component");
            Component carrierPDRepository = personalData.AddComponent(
                "CarrierRepository",
                "Información personal de los usuarios transportistas",
                "Component");
            Component clientPDRepository = personalData.AddComponent(
                "ClientRepository",
                "Información personal de los usuarios clientes",
                "Component");

            apiGateway.Uses(personalDataController, "", "JSON/HTTPS");
            personalDataController.Uses(personalDataService, "Invoca métodos de información personal");

            personalDataService.Uses(domainLayerPD, "Usa", "");
            personalDataService.Uses(carrierPDRepository, "", "");
            personalDataService.Uses(clientPDRepository, "", "");

            carrierPDRepository.Uses(databasePD, "", "");
            clientPDRepository.Uses(databasePD, "", "");

            // Tags
            domainLayerPD.AddTags("DomainLayerPD");
            personalDataController.AddTags("PersonalDataController");
            personalDataService.AddTags("PersonalDataApplicationService");
            carrierPDRepository.AddTags("CarrierPDRepository");
            clientPDRepository.AddTags("ClientPDRepository");

            styles.Add(new ElementStyle("DomainLayerPD")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PersonalDataController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PersonalDataApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("CarrierPDRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("ClientPDRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewPD = viewSet.CreateComponentView(
                personalData, "Información personal", "Component Diagram");
            componentViewPD.PaperSize = PaperSize.A4_Landscape;
            componentViewPD.Add(webApplication);
            componentViewPD.Add(apiGateway);
            componentViewPD.Add(databasePD);
            componentViewPD.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
