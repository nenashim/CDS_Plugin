using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System;

namespace RequestNevisFile
{
    public class TokenValue
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
    public class DynamicsControlerFileModel
    {
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public object Lines { get; set; }
        public string fileBase64 { get; set; }
        public string fileName { get; set; }
    }
    public class RequestValue
    {
        public Boolean IsError { get; set; }
        public string MessageText { get; set; }
    }

    public class RequestNevisFileControler
    {
        private string fileBase64 = "";
        private string fileNameValue = "";
        private TokenValue tokenValue;
        public RequestValue loadFile(string _path)
        {
            RequestValue value = new RequestValue();
            try
            {
                Byte[] bytes = File.ReadAllBytes(_path);
                fileBase64 = Convert.ToBase64String(bytes);
                fileNameValue = Path.GetFileName(_path);
            }
            catch (Exception ex)
            {
                value.IsError = true;
                value.MessageText = ex.ToString();
                return value;
            }
            value.IsError = false;
            value.MessageText = "Файл добавлен";
            return value;

        }

        public RequestValue loadToken(string _httpSend,
                                      string _login,
                                      string _password)
        {

            RequestValue value = new RequestValue();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var requestToken = (HttpWebRequest)WebRequest.Create(_httpSend + "/api/Account/token");

                var postData = "grant_type=" + Uri.EscapeDataString("password");
                postData += "&username=" + Uri.EscapeDataString(_login);
                postData += "&password=" + Uri.EscapeDataString(_password);
                var dataToken = Encoding.ASCII.GetBytes(postData);

                requestToken.Method = "POST";
                requestToken.ContentType = "application/x-www-form-urlencoded";
                requestToken.ContentLength = dataToken.Length;


                using (var stream = requestToken.GetRequestStream())
                {
                    stream.Write(dataToken, 0, dataToken.Length);
                }

                var responseToken = (HttpWebResponse)requestToken.GetResponse();

                var responsTokenString = new StreamReader(responseToken.GetResponseStream()).ReadToEnd();

                tokenValue = JsonConvert.DeserializeObject<TokenValue>(responsTokenString);
            }
            catch (Exception ex)
            {
                value.IsError = true;
                value.MessageText = ex.ToString();
                return value;
            }

            value.IsError = false;
            value.MessageText = "Токен получен";
            return value;

        }

        public RequestValue sendFile(string _httpSend,
                                          string _ClassName,
                                          string _MethodName)
        {

            RequestValue value = new RequestValue();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                DynamicsControlerFileModel model = new DynamicsControlerFileModel();
                model.MethodName = _MethodName;
                model.ClassName = _ClassName;
                //model.Lines = [];
                model.fileBase64 = this.fileBase64;
                model.fileName = fileNameValue;

                string valueJson = JsonConvert.SerializeObject(model);

               
                var request = (HttpWebRequest)WebRequest.Create(_httpSend + "/api/AddFileDynamicsPage");



                var data = Encoding.ASCII.GetBytes(valueJson);

                request.Method = "POST";
                request.ContentType = "application/JSON; charset=utf-8";
                request.ContentLength = data.Length;

                request.Headers.Add("Authorization", tokenValue.token_type + " " + tokenValue.access_token);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                RequestValue requestValue = JsonConvert.DeserializeObject<RequestValue>(responseString);

                return requestValue;
            }
            catch (Exception ex)
            {
                value.IsError = true;
                value.MessageText = ex.ToString();
                return value;
            }



        }

    }
}