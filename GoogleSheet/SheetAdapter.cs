using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace GoogleSheet
{
    public class SheetAdapter : ISheetAdapter
    {
        SheetsService _service { get; set; }
        const string _applicationName = "DataSphere";
        string[] _scopes = { SheetsService.Scope.Spreadsheets };

        public SheetAdapter()
        {
            InitializeService();
        }

        public async Task ClearItems(string spreadSheetId, IGoogleSheetRange range)
        {
            var requestBody = new ClearValuesRequest();
            var deleteRequest = _service.Spreadsheets.Values.Clear(requestBody, spreadSheetId, range.ToString());
            await deleteRequest.ExecuteAsync();
        }

        public async Task Post(List<IList<object>> data, string spreadSheetId, IGoogleSheetRange range)
        {
            var valueRange = new ValueRange
            {
                Values = data
            };
            var appendRequest = _service.Spreadsheets.Values.Append(valueRange, spreadSheetId, range.ToString());
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            valueRange.Range = range.ToString();
            var response = await appendRequest.ExecuteAsync();
        }

        public IList<IList<object>> GetDataFromRange(string spreadSheetId, IGoogleSheetRange range)
        {
            return _service.Spreadsheets.Values.Get(spreadSheetId, range.ToString()).Execute().Values;
        }

        [MemberNotNull(nameof(_service))]
        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
        }

        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("google.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }
            return credential;
        }
    }
}
