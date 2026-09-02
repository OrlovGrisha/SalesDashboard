namespace SalesDashboard.Infrastructure.Seed.Data;

public static class SeedNames
{
    public static readonly string[] FirstNames =
    {
        "Александр", "Дмитрий", "Иван", "Сергей", "Андрей", "Максим", "Никита", "Артём",
        "Елена", "Ольга", "Мария", "Анна", "Наталья", "Екатерина", "Ирина", "Татьяна",
        "Павел", "Роман", "Кирилл", "Егор", "Виктория", "Юлия", "Дарья", "Светлана"
    };

    public static readonly string[] LastNames =
    {
        "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов", "Соколов", "Лебедев",
        "Козлов", "Новиков", "Морозов", "Волков", "Соловьёв", "Васильев", "Зайцев", "Павлов"
    };

    public static readonly string[] Teams =
    {
        "Enterprise Sales", "SMB Sales", "Key Accounts", "Regional Sales", "New Business"
    };

    public static readonly string[] CompanySuffixes =
    {
        "ООО", "АО", "ИП", "ЗАО"
    };

    public static readonly string[] CompanyNames =
    {
        "ТехноСтрой", "МеталлИнвест", "АгроПром", "СтройГрупп", "ЛогистикЦентр",
        "ИнфоСистемы", "ПромТрейд", "ЭнергоСервис", "ФинансГрупп", "МедТехника",
        "АвтоПарк", "СтройМатериалы", "ХимПром", "ПищеПром", "ТоргДом",
        "ИнжинирингЦентр", "МашСтрой", "ЭлектроСнаб", "ТрансКомпани", "РесурсГрупп"
    };

    public static readonly string[] CustomerSegments = { "SMB", "Mid-Market", "Enterprise" };

    public static readonly (string Category, string[] Products)[] Catalog =
    {
        ("Ноутбуки", new[] { "ProBook 450", "UltraSlim 14", "GameMaster X15", "OfficeLite 13", "WorkStation Pro 17" }),
        ("Мониторы", new[] { "ViewMax 27", "CurveDisplay 34", "ProScreen 24", "UltraWide 32" }),
        ("Периферия", new[] { "MechKey RGB", "SilentMouse Pro", "WebCam HD", "AudioSet Studio" }),
        ("Серверное оборудование", new[] { "RackServer X200", "StoragePro 8TB", "NetSwitch 24P" }),
        ("Сетевое оборудование", new[] { "RouterPro AX6000", "AccessPoint Mesh", "SwitchLite 16P" }),
        ("Программное обеспечение", new[] { "OfficeSuite Pro", "SecurityShield", "BackupCloud", "DevToolsBundle" })
    };
}