using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SharedComponent.Server.Api.Impl
{
    public class FileSaverService : IFileSaverService
    {
        private readonly HttpClient _httpClient;

        public FileSaverService(HttpClient httpClient)
        {
            _httpClient=httpClient;
        }

        public async Task<string> Delete(string storedFileName)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"/api/FileSave/TemporaryPatientFiles/{storedFileName}");
                if (result.IsSuccessStatusCode)
                {
                    return "Successfully Deleted";
                }
                return "Failed to delete";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public async Task<string> DeleteResultFile(string storedFileName)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"/api/FileSave/{storedFileName}");
                if (result.IsSuccessStatusCode)
                {
                    return "Successfully Deleted";
                }
                return "Failed to delete";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<FileClass?> PostResultFiles(MultipartFormDataContent files)
        {
            try
            {
                var postedResponse = await _httpClient.PostAsync("/api/FileSave", files);
                var uploadedResult = await postedResponse.Content.ReadFromJsonAsync<FileClass>();
                return uploadedResult;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<FileClass>?> PostFiles(MultipartFormDataContent files)
        {
            try
            {
                var postedResponse = await _httpClient.PostAsync("/api/FileSave/patientFiles", files);
                var uploadedResult = await postedResponse.Content.ReadFromJsonAsync<List<FileClass>>();
                return uploadedResult;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task<string> PostFiles(List<FileClass> files)
        {
            try
            {

               var response=  await _httpClient.PostAsJsonAsync("/api/FileSave/patientLatestFiles", files, System.Threading.CancellationToken.None); 
                if(response.IsSuccessStatusCode)
                {
                    return "SuccessFully Uploaded";
                }
                return "Failed to Upload";
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
