using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Collections.Generic;

public class GoogleSheetsIntegration
{
    private SheetsService _sheetsService;

    public GoogleSheetsIntegration()
    {
        // Путь к JSON-файлу с ключами доступа
        string keyFilePath = "Integration/secrets.json";

        // Создание учетных данных и аутентификация
        var credential = GoogleCredential.FromFile(keyFilePath)
            .CreateScoped(SheetsService.Scope.Spreadsheets);

        // Создание клиента Google Sheets API
        _sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });
    }

    public IList<IList<object>> ReadData(string spreadsheetId, string range)
    {
        // Выполнение запроса на чтение данных
        SpreadsheetsResource.ValuesResource.GetRequest request =
            _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        return values;
    }
}
