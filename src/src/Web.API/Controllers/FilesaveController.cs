
using System.Net;
using Application.Business.PatientFiles.Commands.CreatePatientFile;
using Application.Business.PatientFiles.Queries;
using Application.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using Web.API.Helpers;
namespace Web.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FileSaveController : ApiControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<FileSaveController> logger;
    private readonly IHubContext<PatientFileHub, IPatientFileHub> _hubContext;
    public FileSaveController(IWebHostEnvironment env,
        ILogger<FileSaveController> logger,
        IHubContext<PatientFileHub, IPatientFileHub> hubContext)
    {
        this.env = env;
        this.logger = logger;
        _hubContext=hubContext;
    }

    [HttpPost]
    public async Task<ActionResult<UploadResult>> PostFile(
        [FromForm] List<IFormFile>  files)
    {
        long maxFileSize = 1024*1024*20;
        var uploadResult = new UploadResult();
        var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
        
        if (files.Count <= 0)
        {
            return new CreatedResult(resourcePath, uploadResult);
        }
        var file = files.FirstOrDefault();
        if (file == null)
        {
            return new CreatedResult(resourcePath, uploadResult);
        }
      

        var trustedFileNameForFileStorage=file.FileName.Replace(".pdf","").Trim() + Guid.NewGuid().ToString() +".pdf";
        uploadResult.FileName = file.FileName;
        var trustedFileNameForDisplay =
            WebUtility.HtmlEncode(file.FileName);
        if (file.Length>maxFileSize)
        {
            logger.LogInformation($"{trustedFileNameForFileStorage} length is larger than the limit{maxFileSize}",
                          trustedFileNameForDisplay);
        }
        if (file.Length == 0)
        {
            logger.LogInformation("{FileName} length is 0 (Err: 1)",
                trustedFileNameForDisplay);
            // uploadResult.ErrorCode = 1;
        }
        else
        {
            try
            {
                var path = Path.Combine(env.ContentRootPath,
                    "wwwroot", "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                await using FileStream fs = new( Path.Combine(path,trustedFileNameForFileStorage) , FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fs);
                logger.LogInformation("{FileName} saved at {Path}",
                    trustedFileNameForDisplay, path);
                uploadResult.Uploaded = true;
                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResult.ContentType = file.ContentType;
            }
            catch (IOException ex)
            {
                logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                    trustedFileNameForDisplay, ex.Message);
               // uploadResult.ErrorCode = 3;
            }
        }

        return new CreatedResult(resourcePath, uploadResult);

    }
    [HttpPost("patientLatestFiles")]
    public async Task Post([FromBody] UploadResult[] files)
    {
        var path = Path.Combine(env.ContentRootPath,
                     "wwwroot", "TemporaryPatientFiles");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        foreach (var file in files)
        {
                var info = new System.IO.FileInfo(file.FileName);
                var storedFileName = file.FileName.Replace(info.Extension, "").Trim() + Guid.NewGuid().ToString()+info.Extension;
                var command = new CreatePatientFileCommand()
                {
                    Attachments= Convert.FromBase64String(file.Base64data),
                    ContentType=file.ContentType,
                    CreatedBy= file.CreatedBy,
                    FileName= file.FileName,
                    Latitude= file.Latitude,
                    Longitude=file.Longitude,
                    StoredFileName=storedFileName,
                };
            var buf = Convert.FromBase64String(file.Base64data);
            IFormFile formFile = null;
            using (var stream = new MemoryStream(buf))
            {
                formFile = new FormFile(stream, 0, buf.Length, file.FileName, storedFileName);
                await using FileStream fs = new(Path.Combine(path, storedFileName), FileMode.Create);
                await formFile.OpenReadStream().CopyToAsync(fs);
            };
       
            //await System.IO.File.WriteAllBytesAsync(path +file.StoredFileName, buf);
            await Mediator.Send(command);

        }
        var data = await Mediator.Send(new GetPatientFilesByPatientId { PatientId = files.FirstOrDefault()?.CreatedBy });
        await _hubContext.Clients.All.BroadCastPatientData(data);
    }
    [HttpPost("patientFiles")]
    public async Task<ActionResult<IList<UploadResult>>> PostTemporaryFile(
        [FromForm] List<IFormFile>  files)
    {
        var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
        var uploadResults = new List<UploadResult>();
        if (files.Count <= 0)
        {
            return new CreatedResult(resourcePath, uploadResults);
        }
        foreach (var file in files)
        {
            var info = new FileInfo(file.FileName);
            var uploadResult = new UploadResult();
            var trustedFileNameForFileStorage=file.FileName.Replace(info.Extension,"").Trim() + Guid.NewGuid().ToString()+info.Extension;
            uploadResult.FileName = file.FileName;
            var trustedFileNameForDisplay =
                WebUtility.HtmlEncode(file.FileName);
            if (file.Length == 0)
            {
                logger.LogInformation("{FileName} length is 0 (Err: 1)",
                    trustedFileNameForDisplay);
            }
            else
            {
                try
                {
                    var path = Path.Combine(env.ContentRootPath,
                        "wwwroot", "TemporaryPatientFiles");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    await using FileStream fs = new( Path.Combine(path,trustedFileNameForFileStorage) , FileMode.Create);
                    await file.OpenReadStream().CopyToAsync(fs);
                    logger.LogInformation("{FileName} saved at {Path}",
                        trustedFileNameForDisplay, path);
                    uploadResult.Uploaded = true;
                    uploadResult.StoredFileName = trustedFileNameForFileStorage;
                    uploadResult.ContentType = file.ContentType;
                    uploadResults.Add(uploadResult);
                }
                catch (IOException ex)
                {
                    logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                        trustedFileNameForDisplay, ex.Message);
                    // uploadResult.ErrorCode = 3;
                }
            }
        }
        return new CreatedResult(resourcePath, uploadResults);

    }
    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
       // var uploadResult = await _context.Uploads.FirstOrDefaultAsync(u => u.StoredFileName.Equals(fileName));

        var path = Path.Combine(env.ContentRootPath,  "wwwroot", "Files", fileName);

        var memory = new MemoryStream();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, "application/pdf", Path.GetFileName(path));
    }
    [HttpGet]
    [Route("pdf/{fileName}")]
    public async Task<IActionResult> ExternlDomainPdf(string fileName)
    {
        string physicalPath  = Path.Combine(env.ContentRootPath,  "wwwroot", "Files", fileName);;
        byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
        var contentType = "";
        new FileExtensionContentTypeProvider().TryGetContentType(physicalPath, out contentType);
        MemoryStream ms = new MemoryStream(pdfBytes);
        return new FileStreamResult(ms, contentType);
    }
    [HttpGet]
    [Route("TemporaryPatient/{fileName}")]
    public async Task<IActionResult> GetTemporaryPatientFiles(string fileName)
    {
        string physicalPath  = Path.Combine(env.ContentRootPath,  "wwwroot", "TemporaryPatientFiles", fileName);;
        byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
        new FileExtensionContentTypeProvider().TryGetContentType(physicalPath, out var contentType);
        MemoryStream ms = new MemoryStream(pdfBytes);
        return new FileStreamResult(ms, contentType);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        string physicalPath  = Path.Combine(env.ContentRootPath,  "wwwroot", "Files", fileName);;
        if (System.IO.File.Exists(physicalPath))
        {
            System.IO.File.Delete(Path.Combine(physicalPath));
        }
        return Ok();
    }
    [HttpDelete("TemporaryPatientFiles/{fileName}")]
    public async Task<IActionResult> DeleteTemporary(string fileName)
    {
        string physicalPath  = Path.Combine(env.ContentRootPath,  "wwwroot", "TemporaryPatientFiles", fileName);;
        if (System.IO.File.Exists(physicalPath))
        {
            System.IO.File.Delete(Path.Combine(physicalPath));
        }
        return Ok();
    }
}
public class UploadResult
{
    public bool Uploaded { get; set; }
    public string? FileName { get; set; }
    public string? StoredFileName { get; set; }
    public string?  ContentType { get; set; }
    public string? Base64data { get; set; }
    public string? CreatedBy { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}