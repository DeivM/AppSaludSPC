using App.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.WebServices
{
    public class WebService
    {
        public object Element { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public ParamType Type { get; set; }
        public WebService(string name, object value, ParamType type = ParamType.File, string fileName = "file.jpg")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("El parametro anme es null o esta vacion");
            }
            if (value is null)
            {
                throw new Exception("El parametro VALUES es null");
            }
            Element = value;
            Name = fileName;
            Type = type;
        }
        public HttpContent ToHttpContent()
        {
            if (Type == ParamType.File)
            {
                if (Element is byte[])
                {
                    var bytearray = Element as byte[];
                    return new ByteArrayContent(bytearray, 0, bytearray.Length);
                }
                else if (Element is Stream)
                {
                    var bytearray = ReadFully(Element as Stream);
                    return new ByteArrayContent(bytearray, 0, bytearray.Length);
                }
            }
            else if (Type == ParamType.String)
            {
                if (Element is string)
                {
                    var mystring = Element as string;
                    return new StringContent(mystring);
                }
            }
            return null;
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
    public enum ParamType
    {
        File, String
    }

    public class ResClient
    {
        public static bool CheckConnectivity { get; set; }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="simpleparams"></param>
        /// <param name="complexparams"></param>
        /// <returns></returns>
        public async Task<T> Post<T>(string url, Dictionary<string, string> simpleparams = null, List<WebService> complexparams = null)
        {
            if (complexparams != null && complexparams.Count > 0)
            {
                try
                {
                    var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                    var form = new MultipartFormDataContent();
                    foreach (var multipartparam in complexparams)
                    {
                        HttpContent content = multipartparam.ToHttpContent();
                        if (content != null)
                        {
                            switch (multipartparam.Type)
                            {
                                case ParamType.File:
                                    form.Add(content, multipartparam.Name, multipartparam.FileName);
                                    break;
                                case ParamType.String:
                                    form.Add(content, multipartparam.Name);
                                    break;
                            }
                        }
                    }
                    var response = await client.PostAsync(url, form);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var responsestring = await response.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                try
                {
                    var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                    var form = new FormUrlEncodedContent(simpleparams);
                    var response = await client.PostAsync(url, form);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var responsestring = await response.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="k"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectttosend"></param>
        /// <returns></returns>
        public async Task<T> Post<T, k>(string url, k objectttosend)
        {
            try
            {
               // Token tokenData = new Token();
              //  await tokenData.CheckTokenValidity();
                var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                var contentjson = Newtonsoft.Json.JsonConvert.SerializeObject(objectttosend);
                var buffer = Encoding.UTF8.GetBytes(contentjson);
                var rawcontent = new ByteArrayContent(buffer);
                var token = Token.GetAccesToken();
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.AccessToken);
                rawcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(url, rawcontent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return  Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);

                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
                else
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }

            }
            catch (Exception e)
            {

                throw e;
            }
            return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="k"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectttosend"></param>
        /// <returns></returns>
        public async Task<T> PostToken<T, k>(string url, k objectttosend)
        {
            try
            {
                var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                var contentjson = Newtonsoft.Json.JsonConvert.SerializeObject(objectttosend);
                var buffer = Encoding.UTF8.GetBytes(contentjson);
                var rawcontent = new ByteArrayContent(buffer);
                rawcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(url, rawcontent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
                else
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="k"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectttosend"></param>
        /// <returns></returns>
        public async Task<T> PostWithImagen<T, k>(string url, k objectttosend, MediaFile mediaFile)
        {

            var token = Token.GetAccesToken();

            try
            {
                var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                var contentjson = Newtonsoft.Json.JsonConvert.SerializeObject(objectttosend);
                var buffer = Encoding.UTF8.GetBytes(contentjson);
                var rawcontent = new ByteArrayContent(buffer);
                rawcontent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var content = new MultipartFormDataContent())
                {


                    var response = await client.PostAsync(url, content);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var responsestring = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                        return result;
                    }
                    else if (response != null && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var responsestring = await response.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                    }
                    else
                    {
                        var responsestring = await response.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return default(T);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="k"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectttosend"></param>
        /// <returns></returns>
        public async Task<T> Put<T, k>(string url, k objectttosend)
        {
            try
            {
                
                var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                var contentjson = Newtonsoft.Json.JsonConvert.SerializeObject(objectttosend);
                var buffer = Encoding.UTF8.GetBytes(contentjson);
                var rawcontent = new ByteArrayContent(buffer);
               // var token = Token.GetAccesToken();
                rawcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PutAsync(url, rawcontent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    var pruea = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                    return pruea;
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
                else
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<T> Get<T>(string url, IDictionary<string, string> formdata = null)
        {
            try
            {

 
                var token = Token.GetAccesToken();

                var client = new HttpClient(new System.Net.Http.HttpClientHandler());
                if (formdata != null)
                {
                    if (!url.EndsWith("?"))
                    {
                        url += "?";
                    }
                    foreach (var item in formdata)
                    {
                        url += item.Key + "=" + item.Value + "&";
                    }
                    if (url.EndsWith("&"))
                    {
                        url = url.Remove(url.Length - 1, 1);
                    }
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(url);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
                else
                {
                    var responsestring = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsestring);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }

}

