using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BLL.Interfaces;
using BLL.DTO;
using static WebAPI.Roles;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        private readonly IImageService _imageService;
        private readonly IProfileService _profileService;
        private readonly IHashTagService _hashTagService;

        public ImagesController(IImageService imageService,
            IHashTagService hashTagService, IProfileService profileService)
        {
            _imageService = imageService;
            _hashTagService = hashTagService;
            _profileService = profileService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = UserRole)]
        [ResponseType(typeof(int))]
        public IHttpActionResult PostImage([FromBody]ImagePostModel image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            HashSet<int> hashTags = _hashTagService.PutHashTagsByName
                (new HashSet<string>(image.HashTags));
            ImageDTO imageDTO = new ImageDTO
            {
                ImageFileId = image.FileId,
                Title = image.Title,
                ProfileId = profileId.Value
            };
            int id = _imageService.AddImage(imageDTO, hashTags);
            return Ok(id);
        }

        [HttpPut]
        [Route("")]
        [Authorize(Roles = UserRole)]
        [ResponseType(typeof(int))]
        public IHttpActionResult PutImage([FromBody]ImagePostModel image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            HashSet<int> hashTags = _hashTagService.PutHashTagsByName
                (new HashSet<string>(image.HashTags));
            ImageDTO imageDTO = new ImageDTO
            {
                ImageFileId = image.FileId,
                Title = image.Title,
                ProfileId = profileId.Value
            };
            int id = _imageService.AddImage(imageDTO, hashTags);
            return Ok(id);
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(ImageDTO[]))]
        public IHttpActionResult GetImages()
        {
            var images = _imageService.GetAllImages();
            if (images == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(images);
        }

        /// <summary>
        /// Get images that belong to current user
        /// </summary>
        /// <returns>Images without hashtags</returns>
        [HttpGet]
        [Route("current")]
        [ResponseType(typeof(ImageDTO[]))]
        [Authorize(Roles = UserRole)]
        public IHttpActionResult GetImagesByCurrentProfile()
        {
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            var images = _imageService.GetImagesByProfile(profileId.Value);
            if (images == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(images);
        }

        /// <summary>
        /// Get images that belong to choosen profile
        /// </summary>
        /// <returns>Images without hashtags</returns>
        [HttpGet]
        [Route("byProfile/{id}")]
        [ResponseType(typeof(ImageDTO[]))]
        public IHttpActionResult GetImagesByProfile(int id)
        {
            var images = _imageService.GetImagesByProfile(id);
            if (images == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(images);
        }

        [HttpGet]
        [Route("byTitle/{title}")]
        [ResponseType(typeof(ImageDTO[]))]
        public IHttpActionResult GetImagesByTitle(string title)
        {
            if (title == null)
                return BadRequest("Title must be not empty.");
            var images = _imageService.GetImagesByTitle(title);
            if (images == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(images);
        }

        /// <summary>
        /// Get images that have such hashtag
        /// </summary>
        /// <param name="tag">Hashtag name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("byHashTagName/{tag}")]
        [ResponseType(typeof(ImageDTO[]))]
        public IHttpActionResult GetImagesByHashTag(string tag)
        {
            int? tagId = _hashTagService.GetHashTagByName(tag)?.Id;
            if (tagId == null)
                return BadRequest("Name must be not empty.");
            var images = _imageService.GetImagesByHashTag(tagId.Value);
            if (images == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(images);
        }

        /// <summary>
        /// Get image with tags by its id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("withTags/{id}")]
        [ResponseType(typeof(ImagePostModel))]
        public IHttpActionResult GetImageModelById(int id)
        {
            ImageDTO dto = _imageService.GetImageById(id);
            if (dto == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(new ImagePostModel
            {
                Id = dto.Id,
                Title = dto.Title,
                FileId = dto.ImageFileId,
                ProfileId = dto.ProfileId,
                HashTags = _hashTagService.GetHashTagsByImage(dto.Id)
                    .Select(x => x.Name).ToList(),
                AvgMark = _imageService.GetAverageMarkByImage(dto.Id)
            });
        }

        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(ImageDTO))]
        public IHttpActionResult GetImageById(int id)
        {
            ImageDTO image = _imageService.GetImageById(id);
            if (image == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            return Ok(image);
        }

        /// <summary>
        /// Get all images with tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("withTags")]
        [ResponseType(typeof(ImagePostModel[]))]
        public IHttpActionResult GetImageModels()
        {
            HashSet<ImageDTO> imagesDTO = _imageService.GetAllImages();
            if (imagesDTO == null)
                return new HttpActionResult(HttpStatusCode.NoContent);
            ICollection<ImagePostModel> images = new List<ImagePostModel>();
            foreach (var dto in imagesDTO)
            {
                images.Add(new ImagePostModel
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    FileId = dto.ImageFileId,
                    ProfileId = dto.ProfileId,
                    HashTags = _hashTagService.GetHashTagsByImage(dto.Id)
                        .Select(x => x.Name).ToList(),
                    AvgMark = _imageService.GetAverageMarkByImage(dto.Id)
                });
            }
            return Ok(images);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = UserRole)]
        public IHttpActionResult DeleteImage(int id)
        {
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            ImageDTO image = _imageService.GetImageById(id);
            if (image == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "Image was not found");
            if (image.ProfileId != profileId)
                return new HttpActionResult(HttpStatusCode.Forbidden, 
                    "Only author can remove image!");
            bool isRemoved = _imageService.DeleteImageById(id);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
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