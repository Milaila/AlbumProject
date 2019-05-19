using System;
using System.Web.Http;
using BLL.Interfaces;
using BLL.DTO;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;
using System.Web.Http.Description;
using System.Net;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/imageFiles")]
    [Authorize(Roles = Roles.UserRole)]
    public class ImageFilesController : ApiController
    {
        private readonly IImageFileService _imageFileService;
        private readonly IProfileService _profileService;
        private const string _imagesFolder = "~/Images";

        public ImageFilesController(IImageFileService imageFileService, IProfileService profileService)
        {
            _profileService = profileService;
            _imageFileService = imageFileService;
        }

        [HttpGet]
        [ResponseType(typeof(ImageFileDTO[]))]
        [Route("")]
        [Authorize(Roles = Roles.AdminRole)]
        public IHttpActionResult GetAllFiles()
        {
            var files = _imageFileService.GetAllImageFiles();
            if (files?.Count > 0)
                return Ok(files);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(ImageFileDTO))]
        [Authorize(Roles = Roles.AdminRole)]
        [Route("byImage")]
        public IHttpActionResult GetFileByImage(int imageId)
        {
            var file = _imageFileService.GetImageFileByImageId(imageId);
            if (file != null)
                return Ok(file);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get profile avatar
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ImageFileDTO))]
        [Route("byProfile")]
        public IHttpActionResult GetFileByProfile(int profileId)
        {
            var file = _imageFileService.GetImageFileByProfileId(profileId);
            if (file != null)
                return Ok(file);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(ImageFileDTO))]
        [Route("myAvatar")]
        public IHttpActionResult GetFileByCurrProfile()
        {
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            var file = _imageFileService.GetImageFileByProfileId(profileId.Value);
            if (file != null)
                return Ok(file);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(ImageFileDTO))]
        [Route("{id}")]
        public IHttpActionResult GetFileById(int id)
        {
            var file = _imageFileService.GetImageFileById(id);
            if (file != null)
                return Ok(file);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("{id}/path")]
        public IHttpActionResult GetPathByFileId(int id)
        {
            string path = MakePath(id);
            if (path == null)
                return new HttpActionResult(HttpStatusCode.NotFound, request: Request);
            return Ok();
        }

        /// <summary>
        /// Upload image to application store
        /// </summary>
        /// <returns>Id of file location</returns>
        [HttpPut]
        [ResponseType(typeof(int))]
        [Route("upload")]
        public IHttpActionResult UploadImage()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Files.Count <= 0)
                return new HttpActionResult(HttpStatusCode.NotFound, 
                    "File was not found. Please upload it.", Request);
            var file = request.Files[0];
            if (!(file?.ContentLength > 0))
                return new HttpActionResult(HttpStatusCode.UnsupportedMediaType, request: Request);
            int fileId = _imageFileService.GenerateImageFile(file.FileName, _imagesFolder);
            file.SaveAs(MakePath(fileId));
            return Ok(fileId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public IHttpActionResult DeleteFile(int id)
        {
            string path = MakePath(id);
            if (path != null)
                File.Delete(path);
            bool isRemoved = _imageFileService.DeleteImageFileById(id);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult PutImageFile(ImageFileDTO file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_imageFileService.UpdateOrCreateImageFile(file));
        }

        private string MakePath(int fileId)
        {
            try
            {
                return HttpContext.Current.Server.MapPath
                    (_imageFileService.GetImagePath(fileId));
            }
            catch(NotSupportedException)
            {
                return null;
            }
        }
        private int? GetProfileId()
        {
            string userId = User.Identity.GetUserId();
            if (userId == null)
                return null;
            return _profileService.GetProfileByUser(userId)?.Id;
        }
    }
}