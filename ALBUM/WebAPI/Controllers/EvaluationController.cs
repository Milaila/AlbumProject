using System.Net;
using System.Web.Http;
using BLL.Interfaces;
using BLL.DTO;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/evaluations")]
    [Authorize(Roles = Roles.UserRole)]
    public class EvaluationsController : ApiController
    {
        private readonly IEvaluationService _evaluationService;
        private readonly IProfileService _profileService;
        private readonly IImageService _imageService;

        public EvaluationsController(IEvaluationService evaluationService, 
            IProfileService profileService, IImageService imageService)
        {
            _evaluationService = evaluationService;
            _profileService = profileService;
            _imageService = imageService;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetEvaluation(int id)
        {
            return Ok(_evaluationService.GetEvaluationById(id)); 
        }

        /// <summary>
        /// Get all evaluations by this image
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("images/{id}")]
        [ResponseType(typeof(EvaluationDTO[]))]
        public IHttpActionResult GetByImage(int id)
        {
            return Ok(_evaluationService.GetEvaluationsByImage(id));
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(int))]
        public IHttpActionResult AddEvaluation([FromBody]EvaluationModel evaluation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            EvaluationDTO dto = new EvaluationDTO
            {
                ProfileId = profileId.Value,
                ImageId = evaluation.ImageId,
                Mark = evaluation.Mark
            };
            int id = _evaluationService.MakeEvaluation(dto);
            return Ok(id);
        }

        /// <summary>
        /// Make evaluation (rate the picture)
        /// </summary>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ResponseType(typeof(int))]
        public IHttpActionResult PutEvaluation([FromBody]EvaluationModel evaluation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int? profileId = GetProfileId();
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            EvaluationDTO dto = new EvaluationDTO
            {
                ProfileId = profileId.Value,
                ImageId = evaluation.ImageId,
                Mark = evaluation.Mark
            };
            int id = _evaluationService.UpdateOrCreateEvaluation(dto);
            return Ok(id);
        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public IHttpActionResult DeleteEvaluation(int id)
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